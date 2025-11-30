using AutoFixture;
using FluentAssertions;
using InnoShop.UnitTests.Helpers;
using Moq;
using ProductManagement.Core.Features.DeleteProduct;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using Shared.Core.Exceptions;

namespace InnoShop.UnitTests.ProductManagement.Core.Features.DeleteProduct;

public class DeleteProductHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteProductHandler _handler;

    public DeleteProductHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteProductHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var product = fixture.Create<Product>();
        var command = fixture.Build<DeleteProductCommand>()
            .With(p => p.Id, product.Id)
            .Create();
        var numberOfChangedRows = 1;


        _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _unitOfWorkMock.Setup(u => u.Products.Remove(product));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(numberOfChangedRows);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(numberOfChangedRows);
        _unitOfWorkMock.Verify(u => u.Products.Remove(product), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var command = fixture.Create<DeleteProductCommand>();

        _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.Products.Remove(It.IsAny<Product>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
