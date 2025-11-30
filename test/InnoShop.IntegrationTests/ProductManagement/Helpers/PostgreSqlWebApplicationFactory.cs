using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.API.Clients;
using ProductManagement.DataAccess.Data;
using Testcontainers.PostgreSql;
using PM = ProductManagement.API.Program;

namespace InnoShop.IntegrationTests.ProductManagement.Helpers;

public class PostgreSqlWebApplicationFactory : WebApplicationFactory<PM>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("test_db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public Task InitializeAsync() => _dbContainer.StartAsync();

    public new Task DisposeAsync() => _dbContainer.StopAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<InnoShopContext>));

            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<InnoShopContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InnoShopContext>();
            dbContext.Database.EnsureCreated();

            var authDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IAuthClient));

            if (authDescriptor != null) services.Remove(authDescriptor);

            services.AddSingleton<IAuthClient, FakeAuthClient>();
        });
    }
}
