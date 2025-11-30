using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using InnoShop.IntegrationTests.ProductManagement.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Core.Features.CreateProduct;
using ProductManagement.DataAccess.Data;
using ProductManagement.Domain.Entities;

namespace InnoShop.IntegrationTests.ProductManagement;

public class ProductTests(PostgreSqlWebApplicationFactory factory) : IClassFixture<PostgreSqlWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly IServiceScopeFactory _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

    [Fact]
    public async Task CreateProduct_ShouldSaveToDatabase()
    {
        // Arrange
        var category = _fixture.Create<Category>();

        var command = _fixture.Build<CreateProductCommand>()
            .With(c => c.CategoryId, category.Id)
            .Create();

        using (var arrangeScope = _scopeFactory.CreateScope())
        {
            var context = arrangeScope.ServiceProvider.GetRequiredService<InnoShopContext>();
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        // Act
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "test-token");
        var response = await _client.PostAsJsonAsync("/api/product", command);

        // Assert API Response
        response.EnsureSuccessStatusCode();

        // Assert Database State
        using var assertScope = _scopeFactory.CreateScope();
        var assertContext = assertScope.ServiceProvider.GetRequiredService<InnoShopContext>();
        var product = await assertContext.Products.FirstOrDefaultAsync(p => p.Name == command.Name);
        product.Should().NotBeNull();
        product.Price.Should().Be(command.Price);
    }
    
    [Fact]
    public async Task SoftDelete_ShouldHideProducts_ButKeepInDb()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var productsNumber = 3;

        var products = _fixture.Build<Product>()
            .With(p => p.CreatedAt, DateTime.UtcNow)
            .With(p => p.IsDeleted, false)
            .With(p => p.UserId, userId)
            .CreateMany(productsNumber);

        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<InnoShopContext>();
            await context.Products.ExecuteDeleteAsync();
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        // Act
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "test-token");
        var response = await _client.PostAsync($"/api/internal/delete-products/{userId}", null);

        // Assert API Response
        response.EnsureSuccessStatusCode();

        // Assert
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<InnoShopContext>();
            
            var visibleProducts = await context.Products
                .Where(p => p.UserId == userId)
                .ToListAsync();
            visibleProducts.Should().BeEmpty();

            var deletedProducts = await context.Products
                .IgnoreQueryFilters()
                .Where(p => p.UserId == userId)
                .ToListAsync();

            deletedProducts.Should().HaveCount(productsNumber);
            deletedProducts.Should().AllSatisfy(p => p.IsDeleted.Should().BeTrue());
        }
    }

    [Fact]
    public async Task Restore_ShouldChangeIsDeletedFlag()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var productsNumber = 3;

        var products = _fixture.Build<Product>()
            .With(p => p.CreatedAt, DateTime.UtcNow)
            .With(p => p.IsDeleted, true)
            .With(p => p.UserId, userId)
            .CreateMany(productsNumber);

        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<InnoShopContext>();
            await context.Products.ExecuteDeleteAsync();
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        // Act
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "test-token");
        var response = await _client.PostAsync($"/api/internal/restore-products/{userId}", null);

        // Assert API Response
        response.EnsureSuccessStatusCode();

        // Assert
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<InnoShopContext>();
            
            var visibleProducts = await context.Products
                .Where(p => p.UserId == userId)
                .ToListAsync();
            visibleProducts.Should().HaveCount(productsNumber);
        }
    }
}

