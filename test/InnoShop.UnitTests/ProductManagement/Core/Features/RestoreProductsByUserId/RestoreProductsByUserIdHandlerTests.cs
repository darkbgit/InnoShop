using AutoFixture;
using FluentAssertions;
using InnoShop.UnitTests.Helpers;
using Moq;
using ProductManagement.Core.Features.RestoreProductsByUserId;
using ProductManagement.Domain.Interfaces;

namespace InnoShop.UnitTests.ProductManagement.Core.Features.RestoreProductsByUserId;

public class RestoreProductsByUserIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RestoreProductsByUserIdHandler _handler;

    public RestoreProductsByUserIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RestoreProductsByUserIdHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallRestoreProductsByUserIdAndSaveChanges()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();
        var command = fixture.Create<RestoreProductsByUserIdCommand>();

        _unitOfWorkMock.Setup(u => u.Products.RestoreProductsByUserIdAsync(command.UserId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _unitOfWorkMock.Verify(u => u.Products.RestoreProductsByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
