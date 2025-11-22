using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Core.Exceptions;
using Shared.Core.Models;
using Shared.Core.Responses;
using UserManagement.Core.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Services;

public class IdentityService(UserManager<ApplicationUser> userManager,
    IJwtTokenService jwtTokenService,
    IMapper mapper,
    ILogger<IdentityService> logger) : IIdentityService
{
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly ILogger<IdentityService> _logger = logger;

    public async Task<UserInfoResponse?> GetUserInfoAsync(string token)
    {
        var id = _jwtTokenService.GetClaim(token, JwtRegisteredClaimNames.NameId);

        if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guidId))
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(guidId.ToString());

        if (user == null)
        {
            return null;
        }

        var result = new UserInfoResponse
        {
            Id = user.Id,
            Roles = [.. await _userManager.GetRolesAsync(user)],
        };
        return result;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());

        var result = _mapper.Map<User>(appUser);

        return result;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var appUser = await _userManager.FindByEmailAsync(email);

        var result = _mapper.Map<User>(appUser);

        return result;
    }

    public async Task<(bool Success, string Error, Guid UserId)> CreateUserAsync(string email, string password)
    {
        var isExist = await _userManager.FindByEmailAsync(email);

        if (isExist != null)
        {
            return (false, "User with same email is exists.", Guid.Empty);
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var error = "Couldn't create user. try again.";

            if (result.Errors.Any())
            {
                error = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description).ToList());
            }

            return (false, error, Guid.Empty);
        }

        return (true, string.Empty, user.Id);
    }

    public async Task<bool> CheckPasswordAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return false;

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user != null;
    }

    public async Task<(bool Success, string Error)> AddToRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("User not found.");

        var result = await _userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to add user {UserId} to role {Role}. Errors: {Errors}",
                userId, role, string.Join(", ", result.Errors.Select(e => e.Description)));

            return (false, string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }

        return (true, string.Empty);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());

        if (appUser == null)
            return false;


        _mapper.Map(user, appUser);

        var result = await _userManager.UpdateAsync(appUser);

        if (!result.Succeeded)
        {
            var errors = string.Join(Environment.NewLine, result.Errors);
            _logger.LogError("Failed to update user {UserId}. Errors: {Errors}", user.Id, errors);

            return false;
        }

        return true;
    }

    public async Task<PaginatedList<User>> GetUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var users = await _userManager.Users
            .OrderBy(user => user.Email)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var usersWithRoles = users
            .Select(_mapper.Map<User>)
            .ToList();

        var count = await _userManager.Users.CountAsync(cancellationToken);

        var result = new PaginatedList<User>(usersWithRoles,
            count,
            pageNumber,
            pageSize);

        return result;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("User not found.");

        var result = await _userManager.DeleteAsync(appUser);

        if (!result.Succeeded)
        {
            var errors = string.Join(Environment.NewLine, result.Errors);
            _logger.LogError("Failed to delete user {UserId}. Errors: {Errors}", userId, errors);

            return false;
        }

        return true;
    }

    public async Task<IReadOnlyList<string>> GetUserRolesAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("User not found.");

        var roles = await _userManager.GetRolesAsync(user);

        return [.. roles];
    }
}
