using System.Security.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Core.Responses;
using UserManagement.Core.Exceptions;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Options;
using UserManagement.Core.Requests;

namespace UserManagement.Core.Services;

public class AuthService(IIdentityService identityService,
    IJwtTokenService jwtTokenService,
    IEmailService emailService,
    IOptions<FrontendOptions> options) : IAuthService
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IEmailService _emailService = emailService;
    private readonly FrontendOptions _frontendOptions = options.Value;

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var isValid = await _identityService.CheckPasswordAsync(request.UsernameOrEmail, request.Password);

        if (!isValid)
            throw new AuthenticationException("Could not authenticate user.");

        var user = await _identityService.GetUserByEmailAsync(request.UsernameOrEmail) ??
            throw new AuthenticationException("Could not authenticate user.");

        var token = await _jwtTokenService.GenerateTokenAsync(user.Id);

        return token;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        if (await _identityService.IsEmailExistsAsync(request.Email))
            throw new ServiceException("User with same email already exists.");

        var (Success, Error, _) = await _identityService.RegisterUserAsync(request.Email, request.Password);

        if (!Success)
            throw new ServiceException(Error);
    }

    public async Task<UserInfoResponse?> GetUserInfoAsync(string token)
    {
        var id = _jwtTokenService.GetClaim(token, JwtRegisteredClaimNames.NameId);

        if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guidId))
        {
            return null;
        }

        var user = await _identityService.GetUserByIdAsync(guidId);

        if (user == null)
        {
            return null;
        }

        var result = new UserInfoResponse
        {
            Id = user.Id,
            Roles = [.. await _identityService.GetUserRolesAsync(user.Id)],
        };
        return result;
    }

    public async Task<bool> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.ConfirmEmailAsync(email, token, cancellationToken);

        return result.Success;
    }

    public async Task ForgotPasswordAsync(string email, CancellationToken cancellationToken = default)
    {
        var token = await _identityService.GeneratePasswordResetTokenAsync(email);

        if (string.IsNullOrWhiteSpace(token))
            return;

        var resetLink = $"{_frontendOptions.ResetPasswordUrl}?{token}";
        var body = $"Click here: {resetLink}";
        await _emailService.SendEmailAsync(email, "Reset password", body, cancellationToken);
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.ResetPasswordAsync(email, token, newPassword, cancellationToken);

        return result.Success;
    }
}
