using AutoFixture;
using FluentAssertions;
using InnoShop.UnitTests.Helpers;
using Moq;
using ProductManagement.Core.DTOs;
using ProductManagement.Core.Features.GetProductById;
using ProductManagement.Core.Interfaces;
using Shared.Core.Exceptions;

namespace InnoShop.UnitTests.ProductManagement.Core.Features.GetProductById;

public class GetProductByIdHandlerTests
{
    private readonly Mock<IProductReadRepository> _repositoryMock;
    private readonly GetProductByIdHandler _handler;

    public GetProductByIdHandlerTests()
    {
        _repositoryMock = new Mock<IProductReadRepository>();
        _handler = new GetProductByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var productDto = fixture.Create<ProductDto>();
        var query = fixture.Build<GetProductByIdQuery>()
            .With(p => p.Id, productDto.Id)
            .Create();

        _repositoryMock.Setup(r => r.GetProductDtoByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(productDto);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var query = fixture.Create<GetProductByIdQuery>();

        _repositoryMock.Setup(r => r.GetProductDtoByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductDto?)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
