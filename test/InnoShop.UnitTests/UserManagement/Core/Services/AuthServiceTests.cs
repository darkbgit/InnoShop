using System.Security.Authentication;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using InnoShop.UnitTests.Helpers;
using Microsoft.Extensions.Options;
using Moq;
using UserManagement.Core.Exceptions;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Options;
using UserManagement.Core.Requests;
using UserManagement.Core.Services;
using UserManagement.Domain.Entities;

namespace InnoShop.UnitTests.UserManagement.Core.Services;

public class AuthServiceTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly IOptions<FrontendOptions> _frontendOptions;
    private readonly AuthService _authService;
    private readonly IFixture _fixture;

    public AuthServiceTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _emailServiceMock = new Mock<IEmailService>();
        _frontendOptions = Options.Create(new FrontendOptions { ResetPasswordUrl = "http://localhost" });
        _authService = new AuthService(
            _identityServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _emailServiceMock.Object,
            _frontendOptions);
        _fixture = FixtureFactory.GetFixture();
    }

    [Theory]
    [AutoData]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid(LoginRequest request, User user, string token)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.CheckPasswordAsync(request.UsernameOrEmail, request.Password))
            .ReturnsAsync(true);
        _identityServiceMock.Setup(x => x.GetUserByEmailAsync(request.UsernameOrEmail))
            .ReturnsAsync(user);
        _jwtTokenServiceMock.Setup(x => x.GenerateTokenAsync(user.Id))
            .ReturnsAsync(token);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.Should().Be(token);
    }

    [Theory]
    [AutoData]
    public async Task LoginAsync_ShouldThrowAuthenticationException_WhenCredentialsAreInvalid(LoginRequest request)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.CheckPasswordAsync(request.UsernameOrEmail, request.Password))
            .ReturnsAsync(false);

        // Act
        var act = async () => await _authService.LoginAsync(request);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>();
    }

    [Theory]
    [AutoData]
    public async Task LoginAsync_ShouldThrowAuthenticationException_WhenUserNotFound(LoginRequest request)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.CheckPasswordAsync(request.UsernameOrEmail, request.Password))
            .ReturnsAsync(true);
        _identityServiceMock.Setup(x => x.GetUserByEmailAsync(request.UsernameOrEmail))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _authService.LoginAsync(request);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>();
    }

    [Theory]
    [AutoData]
    public async Task RegisterAsync_ShouldSucceed_WhenUserDoesNotExist(RegisterRequest request)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.IsEmailExistsAsync(request.Email))
            .ReturnsAsync(false);
        _identityServiceMock.Setup(x => x.RegisterUserAsync(request.Email, request.Password))
            .ReturnsAsync((true, string.Empty, Guid.NewGuid()));

        // Act
        var act = async () => await _authService.RegisterAsync(request);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Theory]
    [AutoData]
    public async Task RegisterAsync_ShouldThrowServiceException_WhenUserAlreadyExists(RegisterRequest request)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.IsEmailExistsAsync(request.Email))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _authService.RegisterAsync(request);

        // Assert
        await act.Should().ThrowAsync<ServiceException>();
    }

    [Theory]
    [AutoData]
    public async Task RegisterAsync_ShouldThrowServiceException_WhenRegistrationFails(RegisterRequest request, string errorMessage)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.IsEmailExistsAsync(request.Email))
            .ReturnsAsync(false);
        _identityServiceMock.Setup(x => x.RegisterUserAsync(request.Email, request.Password))
            .ReturnsAsync((false, errorMessage, Guid.Empty));

        // Act
        var act = async () => await _authService.RegisterAsync(request);

        // Assert
        await act.Should().ThrowAsync<ServiceException>().WithMessage(errorMessage);
    }

    [Theory]
    [AutoData]
    public async Task GetUserInfoAsync_ShouldReturnUserInfo_WhenTokenIsValid(string token, Guid userId, string[] roles)
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.Id, userId)
            .Create();

        _jwtTokenServiceMock.Setup(x => x.GetClaim(token, "nameid")).Returns(userId.ToString());
        _identityServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _identityServiceMock.Setup(x => x.GetUserRolesAsync(userId)).ReturnsAsync(roles);

        // Act
        var result = await _authService.GetUserInfoAsync(token);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Roles.Should().BeEquivalentTo(roles);
    }

    [Theory]
    [AutoData]
    public async Task GetUserInfoAsync_ShouldReturnNull_WhenTokenIsInvalid(string token)
    {
        // Arrange
        _jwtTokenServiceMock.Setup(x => x.GetClaim(token, "nameid")).Returns(string.Empty);

        // Act
        var result = await _authService.GetUserInfoAsync(token);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [AutoData]
    public async Task GetUserInfoAsync_ShouldReturnNull_WhenUserNotFound(string token, Guid userId)
    {
        // Arrange
        _jwtTokenServiceMock.Setup(x => x.GetClaim(token, "nameid")).Returns(userId.ToString());
        _identityServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _authService.GetUserInfoAsync(token);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [AutoData]
    public async Task ConfirmEmailAsync_ShouldReturnTrue_WhenConfirmationSucceeds(string email, string token)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.ConfirmEmailAsync(email, token, It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, []));

        // Act
        var result = await _authService.ConfirmEmailAsync(email, token);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public async Task ConfirmEmailAsync_ShouldReturnFalse_WhenConfirmationFails(string email, string token)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.ConfirmEmailAsync(email, token, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, ["Error"]));

        // Act
        var result = await _authService.ConfirmEmailAsync(email, token);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [AutoData]
    public async Task ForgotPasswordAsync_ShouldSendEmail_WhenUserExists(string email, string token)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.GeneratePasswordResetTokenAsync(email))
            .ReturnsAsync(token);

        // Act
        await _authService.ForgotPasswordAsync(email);

        // Assert
        _emailServiceMock.Verify(x => x.SendEmailAsync(email, "Reset password", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoData]
    public async Task ForgotPasswordAsync_ShouldNotSendEmail_WhenTokenGenerationFails(string email)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.GeneratePasswordResetTokenAsync(email))
            .ReturnsAsync(string.Empty);

        // Act
        await _authService.ForgotPasswordAsync(email);

        // Assert
        _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [AutoData]
    public async Task ResetPasswordAsync_ShouldReturnTrue_WhenResetSucceeds(string email, string token, string newPassword)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.ResetPasswordAsync(email, token, newPassword, It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, []));

        // Act
        var result = await _authService.ResetPasswordAsync(email, token, newPassword);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public async Task ResetPasswordAsync_ShouldReturnFalse_WhenResetFails(string email, string token, string newPassword)
    {
        // Arrange
        _identityServiceMock.Setup(x => x.ResetPasswordAsync(email, token, newPassword, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, ["Error"]));

        // Act
        var result = await _authService.ResetPasswordAsync(email, token, newPassword);

        // Assert
        result.Should().BeFalse();
    }
}
