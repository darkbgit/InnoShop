using AutoFixture;
using FluentAssertions;
using InnoShop.UnitTests.Helpers;
using Moq;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Enums;
using ProductManagement.Core.Features.GetPaginatedProduct;
using ProductManagement.Core.Interfaces;
using Shared.Core.Models;

namespace InnoShop.UnitTests.ProductManagement.Core.Features.GetPaginatedProduct;

public class GetPaginatedProductsHandlerTests
{
    private readonly Mock<IProductReadRepository> _repositoryMock;
    private readonly GetPaginatedProductsHandler _handler;

    public GetPaginatedProductsHandlerTests()
    {
        _repositoryMock = new Mock<IProductReadRepository>();
        _handler = new GetPaginatedProductsHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectParameters_WhenQueryIsValid()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var query = fixture.Create<GetPaginatedProductsQuery>();
        var paginatedList = fixture.Create<PaginatedList<ProductDto>>();

        _repositoryMock.Setup(r => r.GetPaginatedProductDtosAsync(
            query.PageNumber,
            query.PageSize,
            ProductsSortEnum.Name,
            SortOrderEnum.Ascending,
            query.SearchString,
            query.CreatedBy,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(paginatedList);
        _repositoryMock.Verify(r => r.GetPaginatedProductDtosAsync(
            query.PageNumber,
            query.PageSize,
            ProductsSortEnum.Name,
            SortOrderEnum.Ascending,
            query.SearchString,
            query.CreatedBy,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}