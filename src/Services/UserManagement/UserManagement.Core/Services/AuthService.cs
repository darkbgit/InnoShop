using System.Security.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Core.Responses;
using UserManagement.Core.Exceptions;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Requests;

namespace UserManagement.Core.Services;

public class AuthService(IIdentityService identityService,
    IJwtTokenService jwtTokenService) : IAuthService
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

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

        var (Success, Error, _) = await _identityService.CreateUserAsync(request.Email, request.Password);

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
}
