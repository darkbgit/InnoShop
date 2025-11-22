using Shared.Core.Enums;
using Shared.Core.Models;
using UserManagement.Core.DTOs;
using UserManagement.Core.Requests;

namespace UserManagement.Core.Interfaces;

public interface IUserService
{
    //Task<UserInfoResponse?> GetUserInfoAsync(string token);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<PaginatedList<UserWithRolesDto>> GetPagedUsersWithRolesAsync(int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
    //Task<Guid> CreateUserAsync(RegisterRequest request);
    Task AddToRoleAsync(Guid userId, Roles role);
    Task UpdateUserAsync(UserUpdateRequest request);
    Task DeleteUserAsync(Guid userId);
}
