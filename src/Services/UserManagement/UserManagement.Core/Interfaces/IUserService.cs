using Shared.Core.Enums;
using Shared.Core.Models;
using UserManagement.Core.DTOs;
using UserManagement.Core.Requests;

namespace UserManagement.Core.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<PaginatedList<UserWithRolesDto>> GetPagedUsersWithRolesAsync(int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task AddToRoleAsync(Guid userId, Roles role);
    Task UpdateUserAsync(UserUpdateRequest request);
    Task DeleteUserAsync(Guid userId);
}
