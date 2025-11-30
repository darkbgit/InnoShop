using AutoFixture;
using FluentAssertions;
using InnoShop.UnitTests.Helpers;
using Moq;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Features.GetCategories;
using ProductManagement.Core.Interfaces;

namespace InnoShop.UnitTests.ProductManagement.Core.Features.GetCategories;

public class GetCategoriesHandlerTests
{
    private readonly Mock<ICategoryReadRepository> _repositoryMock;
    private readonly GetCategoriesHandler _handler;

    public GetCategoriesHandlerTests()
    {
        _repositoryMock = new Mock<ICategoryReadRepository>();
        _handler = new GetCategoriesHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategories_WhenCalled()
    {
        // Arrange
        var query = new GetCategoriesQuery();
        var fixture = FixtureFactory.GetFixture();
        var categories = fixture.CreateMany<CategoryDto>().ToArray();

        _repositoryMock.Setup(r => r.GetCategoriesDtoAsync()).ReturnsAsync(categories);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(categories);
        _repositoryMock.Verify(r => r.GetCategoriesDtoAsync(), Times.Once);
    }
}
