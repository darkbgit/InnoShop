using System.ComponentModel.DataAnnotations;
using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using InnoShop.UnitTests.Helpers;
using Moq;
using ProductManagement.Core.Features.CreateProduct;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace InnoShop.UnitTests.ProductManagement.Core.Features.CreateProduct;

public class CreateProductHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateProductHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProductAndReturnId_WhenCategoryExists()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var command = fixture.Create<CreateProductCommand>();
        var product = fixture.Build<Product>()
            .With(p => p.CategoryId, command.CategoryId)
            .Create();

        _mapperMock.Setup(m => m.Map<Product>(It.IsAny<CreateProductCommand>())).Returns(product);
        _unitOfWorkMock.Setup(u => u.Categories.ExistsAsync(command.CategoryId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.Products.AddAsync(product, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(product.Id);
        _unitOfWorkMock.Verify(u => u.Products.AddAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoData]
    public async Task Handle_ShouldThrowValidationException_WhenCategoryDoesNotExist(CreateProductCommand command, Product product)
    {
        // Arrange
        _mapperMock.Setup(m => m.Map<Product>(command)).Returns(product);
        _unitOfWorkMock.Setup(u => u.Categories.ExistsAsync(command.CategoryId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _unitOfWorkMock.Verify(u => u.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
