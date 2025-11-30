using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using InnoShop.IntegrationTests.UserManagement.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Core.Interfaces;
using UserManagement.Infrastructure.Data;

namespace InnoShop.IntegrationTests.UserManagement;

public class IdentityServiceTests(MsSqlWebApplicationFactory factory) : IClassFixture<MsSqlWebApplicationFactory>
{
    private readonly IServiceScopeFactory _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

    // [Fact]
    // public async Task GeneratePasswordResetTokenAsync_ShouldGenerateToken_ForExistingUser()
    // {
    //     // Arrange
    //     var userEmail = $"testuser_{Guid.NewGuid()}@example.com";
    //     var userPassword = "Password123!";
    //     await CreateUserAsync(userEmail, userPassword);

    //     using var scope = _scopeFactory.CreateScope();
    //     var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();

    //     // Act
    //     var token = await identityService.GeneratePasswordResetTokenAsync(userEmail);

    //     // Assert
    //     token.Should().NotBeNullOrEmpty();
    // }
    
    // [Fact]
    // public async Task ResetPasswordAsync_ShouldSucceed_WithValidToken()
    // {
    //     // Arrange
    //     var userEmail = $"testuser_{Guid.NewGuid()}@example.com";
    //     var originalPassword = "Password123!";
    //     var newPassword = "NewPassword456!";
    //     var user = await CreateUserAsync(userEmail, originalPassword);

    //     using var scope = _scopeFactory.CreateScope();
    //     var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
    //     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    //     var token = await identityService.GeneratePasswordResetTokenAsync(userEmail);

    //     // Act
    //     var result = await identityService.ResetPasswordAsync(userEmail, token, newPassword);

    //     // Assert
    //     result.Success.Should().BeTrue();
    //     result.Errors.Should().BeEmpty();

    //     var updatedUser = await userManager.FindByEmailAsync(userEmail);
    //     var isPasswordCorrect = await userManager.CheckPasswordAsync(updatedUser!, newPassword);
    //     isPasswordCorrect.Should().BeTrue();
    // }

    // [Fact]
    // public async Task GeneratePasswordResetTokenAsync_ShouldThrowException_ForNonExistingUser()
    // {
    //     // Arrange
    //     using var scope = _scopeFactory.CreateScope();
    //     var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
    //     var nonExistentEmail = "nonexistent@example.com";

    //     // Act
    //     Func<Task> act = async () => await identityService.GeneratePasswordResetTokenAsync(nonExistentEmail);

    //     // Assert
    //     await act.Should().ThrowAsync<Exception>().WithMessage("User not found");
    // }

    // private async Task<ApplicationUser> CreateUserAsync(string email, string password)
    // {
    //     using var scope = _scopeFactory.CreateScope();
    //     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    //     var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
    //     var result = await userManager.CreateAsync(user, password);

    //     if (!result.Succeeded)
    //     {
    //         throw new Exception($"Failed to create user for testing: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    //     }

    //     return user;
    // }


    [Fact]
    public async Task RegisterUser_ShouldCreateUser_WhenDataIsValid()
    {
        // Arrange
        var email = $"test_{Guid.NewGuid()}@example.com";
        var password = "Password123!";

        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IIdentityService>();

        // Act
        var result = await service.RegisterUserAsync(email, password);

        // Assert
        result.Success.Should().BeTrue();
        result.UserId.Should().NotBeEmpty();

        // Verification: Check if we can fetch it
        var user = await service.GetUserByIdAsync(result.UserId);
        user.Should().NotBeNull();
        user.Email.Should().Be(email);
    }

    [Fact]
    public async Task CheckPassword_ShouldReturnTrue_ForCorrectCredentials()
    {
        // Arrange
        var email = $"auth_{Guid.NewGuid()}@example.com";
        var password = "Password123!";

        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IIdentityService>();
        
        await service.RegisterUserAsync(email, password);

        // Act & Assert
        (await service.CheckPasswordAsync(email, password)).Should().BeTrue();
        (await service.CheckPasswordAsync(email, "WrongPass!")).Should().BeFalse();
    }

    [Fact]
    public async Task SoftDelete_ShouldHideUser_And_Restore_ShouldBringBack()
    {
        // Arrange
        var email = $"restore_{Guid.NewGuid()}@example.com";
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IIdentityService>();

        var (Success, Error, UserId) = await service.RegisterUserAsync(email, "Pass123!");
        Success.Should().BeTrue();
        Error.Should().BeEmpty();
        var userId = UserId;

        // Act & Assert
        var deleteResult = await service.DeleteUserAsync(userId);
        deleteResult.Should().BeTrue();

        var deletedUser = await service.GetUserByIdAsync(userId);
        deletedUser.Should().BeNull();

        var restoreResult = await service.RestoreUserAsync(userId);
        restoreResult.Success.Should().BeTrue();

        var restoredUser = await service.GetUserByIdAsync(userId);
        restoredUser.Should().NotBeNull();
        restoredUser.Id.Should().Be(userId);
    }

    [Fact]
    public async Task PasswordReset_Flow_ShouldUpdatePassword()
    {
        // Arrange
        var email = $"reset_{Guid.NewGuid()}@example.com";
        var oldPass = "OldPass1!";
        var newPass = "NewPass2@";

        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IIdentityService>();
        await service.RegisterUserAsync(email, oldPass);

        // Act 1
        var token = await service.GeneratePasswordResetTokenAsync(email);
        token.Should().NotBeNullOrEmpty();

        var resetResult = await service.ResetPasswordAsync(email, token, newPass);
        resetResult.Success.Should().BeTrue();

        // Assert
        (await service.CheckPasswordAsync(email, oldPass)).Should().BeFalse();
        (await service.CheckPasswordAsync(email, newPass)).Should().BeTrue();
    }

    [Fact]
    public async Task AddToRole_ShouldAssignRole_WhenRoleExists()
    {
        // Arrange
        var email = $"role_{Guid.NewGuid()}@example.com";
        var roleName = "Admin";

        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IIdentityService>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new ApplicationRole(roleName));
        }

        var regResult = await service.RegisterUserAsync(email, "Pass123!");

        // Act
        var roleResult = await service.AddToRoleAsync(regResult.UserId, roleName);

        // Assert
        roleResult.Success.Should().BeTrue();

        var roles = await service.GetUserRolesAsync(regResult.UserId);
        roles.Should().Contain(roleName);
    }

    [Fact]
    public async Task ConfirmEmail_ShouldVerifyUser()
    {
        // Arrange
        var email = $"confirm_{Guid.NewGuid()}@example.com";
        
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IIdentityService>();
        
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await service.RegisterUserAsync(email, "Pass123!");
        var user = await userManager.FindByEmailAsync(email);
        user.Should().NotBeNull();

        // Act
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var encodedToken = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(token));

        var result = await service.ConfirmEmailAsync(email, encodedToken);

        // Assert
        result.Success.Should().BeTrue();
        
        var updatedUser = await userManager.FindByEmailAsync(email);
        updatedUser.Should().NotBeNull();
        updatedUser.EmailConfirmed.Should().BeTrue();
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnPaginatedResults()
    {
        // Arrange
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IIdentityService>();

        for (int i = 0; i < 15; i++)
        {
            await service.RegisterUserAsync($"page_{i}_{Guid.NewGuid()}@test.com", "Pass123!");
        }

        // Act
        var page1 = await service.GetUsersAsync(1, 10);
        var page2 = await service.GetUsersAsync(2, 10);

        // Assert
        page1.Items.Count.Should().Be(10);
        page2.Items.Count.Should().BeGreaterThanOrEqualTo(5);
        
        page1.Items.Select(x => x.Id).Should().NotIntersectWith(page2.Items.Select(x => x.Id));
    }
}

