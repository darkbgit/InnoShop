using Refit;
using Shared.Core.Enums;
using Shared.Core.Requests;
using Shared.Core.Responses;

namespace ProductManagement.API.Clients;

public interface IAuthClient
{
    [Post("/auth/user-info")]
    Task<UserInfoResponse> GetUserInfo(TokenRequest request);

    [Post("/auth/validate-token")]
    Task<bool> ValidateToken(TokenRequest request);

    [Post("/users/{id}/get-roles")]
    Task<Roles> GetUserRole(Guid id);
}
