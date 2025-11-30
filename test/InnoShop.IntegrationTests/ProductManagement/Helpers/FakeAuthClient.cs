using ProductManagement.API.Clients;
using Shared.Core.Requests;
using Shared.Core.Responses;
using Shared.Core.Enums;

namespace InnoShop.IntegrationTests.ProductManagement.Helpers;

public class FakeAuthClient : IAuthClient
{
    public Task<UserInfoResponse> GetUserInfo(TokenRequest request)
    {
        var user = new UserInfoResponse { Id = Guid.NewGuid() };
        return Task.FromResult(user);
    }

    public Task<bool> ValidateToken(TokenRequest request)
    {
        return Task.FromResult(true);
    }

    public Task<Roles> GetUserRole(Guid id)
    {
        return Task.FromResult(Roles.User);
    }
}
