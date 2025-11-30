using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InnoShop.UnitTests.Helpers;
using Moq;
using ProductManagement.Core.Features.UpdateProduct;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using Shared.Core.Exceptions;

namespace InnoShop.UnitTests.ProductManagement.Core.Features.UpdateProduct;

public class UpdateProductHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateProductHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateProduct_WhenProductAndCategoryExist()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var productDb = fixture.Create<Product>();
        var product = fixture.Build<Product>()
            .With(p => p.Id, productDb.Id)
            .Create();
        var command = fixture.Build<UpdateProductCommand>()
                .With(p => p.Id, product.Id)
                .Create();
        
        var numberOfChangedRows = 1;

        _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(productDb);
        _unitOfWorkMock.Setup(u => u.Categories.ExistsAsync(command.CategoryId.Value, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mapperMock.Setup(m => m.Map(command, productDb)).Returns(productDb);
        _unitOfWorkMock.Setup(u => u.Products.Update(productDb));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(numberOfChangedRows);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(numberOfChangedRows);
        _unitOfWorkMock.Verify(u => u.Products.Update(productDb), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var command = fixture.Create<UpdateProductCommand>();

        _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.Products.Update(It.IsAny<Product>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var productDb = fixture.Create<Product>();
        var command = fixture.Build<UpdateProductCommand>()
                .With(p => p.Id, productDb.Id)
                .Create();

        _unitOfWorkMock.Setup(u => u.Products.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(productDb);
        _unitOfWorkMock.Setup(u => u.Categories.ExistsAsync(command.CategoryId.Value, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.Products.Update(It.IsAny<Product>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
