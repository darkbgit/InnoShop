using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Moq;
using Shared.Core.Enums;
using Shared.Core.Exceptions;
using Shared.Core.Models;
using UserManagement.Core.DTOs;
using UserManagement.Core.Exceptions;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Requests;
using UserManagement.Core.Services;
using UserManagement.Domain.Entities;

namespace InnoShop.UnitTests.UserManagement.Core.Services;

public class UserServiceTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UserService _userService;
    private readonly Fixture _fixture;

    public UserServiceTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _mapperMock = new Mock<IMapper>();
        _userService = new UserService(_identityServiceMock.Object, _mapperMock.Object);
        _fixture = new Fixture();
    }

    [Theory]
    [AutoData]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists(Guid userId, User user, UserDto userDto)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(x => x.Map<UserDto>(user)).Returns(userDto);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        result.Should().Be(userDto);
    }

    [Theory]
    [AutoData]
    public async Task GetUserByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist(Guid userId)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var act = async () => await _userService.GetUserByIdAsync(userId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [AutoData]
    public async Task AddToRoleAsync_ShouldSucceed_WhenOperationIsSuccessful(Guid userId, Roles role)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.AddToRoleAsync(userId, role.ToString()))
            .ReturnsAsync((true, string.Empty));

        // Act
        var act = async () => await _userService.AddToRoleAsync(userId, role);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Theory]
    [AutoData]
    public async Task AddToRoleAsync_ShouldThrowServiceException_WhenOperationFails(Guid userId, Roles role, string errorMessage)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.AddToRoleAsync(userId, role.ToString()))
            .ReturnsAsync((false, errorMessage));

        // Act
        var act = async () => await _userService.AddToRoleAsync(userId, role);

        // Assert
        await act.Should().ThrowAsync<ServiceException>();
    }

    [Theory]
    [AutoData]
    public async Task UpdateUserAsync_ShouldSucceed_WhenUserExistsAndUpdateIsSuccessful(UserUpdateRequest request, User user)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.GetUserByIdAsync(request.Id)).ReturnsAsync(user);
        _identityServiceMock.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(true);

        // Act
        var act = async () => await _userService.UpdateUserAsync(request);

        // Assert
        await act.Should().NotThrowAsync();
        _mapperMock.Verify(x => x.Map(request, user), Times.Once);
    }

    [Theory]
    [AutoData]
    public async Task UpdateUserAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist(UserUpdateRequest request)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.GetUserByIdAsync(request.Id)).ReturnsAsync((User?)null);

        // Act
        var act = async () => await _userService.UpdateUserAsync(request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [AutoData]
    public async Task UpdateUserAsync_ShouldThrowServiceException_WhenUpdateFails(UserUpdateRequest request, User user)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.GetUserByIdAsync(request.Id)).ReturnsAsync(user);
        _identityServiceMock.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(false);

        // Act
        var act = async () => await _userService.UpdateUserAsync(request);

        // Assert
        await act.Should().ThrowAsync<ServiceException>();
    }

    [Fact]
    public async Task GetPagedUsersWithRolesAsync_ShouldReturnPagedUsersWithRoles()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var users = new List<User> { _fixture.Create<User>() };
        var paginatedUsers = new PaginatedList<User>(users, users.Count, pageNumber, pageSize);
        var userWithRolesDto = _fixture.Create<UserWithRolesDto>();
        var roles = new[] { "User" };

        _identityServiceMock.Setup(x => x.GetUsersAsync(pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedUsers);
        _mapperMock.Setup(x => x.Map<UserWithRolesDto>(It.IsAny<User>())).Returns(userWithRolesDto);
        _identityServiceMock.Setup(x => x.GetUserRolesAsync(It.IsAny<Guid>())).ReturnsAsync(roles);

        // Act
        var result = await _userService.GetPagedUsersWithRolesAsync(pageNumber, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items[0].Roles.Should().BeEquivalentTo(roles);
    }

    [Theory]
    [AutoData]
    public async Task DeleteUserAsync_ShouldSucceed_WhenUserExists(Guid userId, User user)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _identityServiceMock.Setup(x => x.DeleteUserAsync(userId)).ReturnsAsync(true);

        // Act
        var act = async () => await _userService.DeleteUserAsync(userId);

        // Assert
        await act.Should().NotThrowAsync();
        _identityServiceMock.Verify(x => x.DeleteUserAsync(userId), Times.Once);
    }

    [Theory]
    [AutoData]
    public async Task DeleteUserAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist(Guid userId)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var act = async () => await _userService.DeleteUserAsync(userId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [AutoData]
    public async Task RestoreUserAsync_ShouldSucceed(Guid userId)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.RestoreUserAsync(userId)).ReturnsAsync((true, []));

        // Act
        var act = async () => await _userService.RestoreUserAsync(userId);

        // Assert
        await act.Should().NotThrowAsync();
        _identityServiceMock.Verify(x => x.RestoreUserAsync(userId), Times.Once);
    }
}
