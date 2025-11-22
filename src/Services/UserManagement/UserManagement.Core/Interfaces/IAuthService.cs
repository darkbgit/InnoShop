using Shared.Core.Responses;
using UserManagement.Core.Requests;

namespace UserManagement.Core.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<string> LoginAsync(LoginRequest request);
    Task<UserInfoResponse?> GetUserInfoAsync(string token);
}
