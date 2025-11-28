using Shared.Core.Models;
using UserManagement.Domain.Entities;

namespace UserManagement.Core.Interfaces;

public interface IIdentityService
{
    Task<(bool Success, string Error, Guid UserId)> RegisterUserAsync(string email, string password);
    Task<bool> CheckPasswordAsync(string email, string password);
    Task<bool> IsEmailExistsAsync(string email);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<PaginatedList<User>> GetUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<(bool Success, string Error)> AddToRoleAsync(Guid userId, string role);
    Task<IReadOnlyList<string>> GetUserRolesAsync(Guid userId);

    Task<(bool Success, string[] Errors)> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default);
    Task<string> GeneratePasswordResetTokenAsync(string email);
    Task<(bool Success, string[] Errors)> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default);

    Task<(bool Success, string[] Errors)> RestoreUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
