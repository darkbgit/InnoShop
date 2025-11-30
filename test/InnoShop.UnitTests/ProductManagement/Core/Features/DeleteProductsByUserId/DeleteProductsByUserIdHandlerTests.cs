using AutoFixture;
using FluentAssertions;
using InnoShop.UnitTests.Helpers;
using Moq;
using ProductManagement.Core.Features.DeleteProductsByUserId;
using ProductManagement.Domain.Interfaces;

namespace InnoShop.UnitTests.ProductManagement.Core.Features.DeleteProductsByUserId;

public class DeleteProductsByUserIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteProductsByUserIdHandler _handler;

    public DeleteProductsByUserIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteProductsByUserIdHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteProductsByUserIdAndSaveChanges()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var command = fixture.Create<DeleteProductsByUserIdCommand>();

        _unitOfWorkMock.Setup(u => u.Products.DeleteProductsByUserIdAsync(command.UserId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _unitOfWorkMock.Verify(u => u.Products.DeleteProductsByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
