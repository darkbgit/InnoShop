using Shared.Core.Responses;
using UserManagement.Core.Requests;

namespace UserManagement.Core.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<string> LoginAsync(LoginRequest request);
    Task<UserInfoResponse?> GetUserInfoAsync(string token);
    Task<bool> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default);
    Task ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default);
}
