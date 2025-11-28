using AutoMapper;
using Shared.Core.Enums;
using Shared.Core.Exceptions;
using Shared.Core.Models;
using UserManagement.Core.DTOs;
using UserManagement.Core.Exceptions;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Requests;

namespace UserManagement.Core.Services;

public class UserService(IIdentityService identityService,
    IMapper mapper) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly IIdentityService _identityService = identityService;

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _identityService.GetUserByIdAsync(id)
            ?? throw new NotFoundException("User not found.");

        var result = _mapper.Map<UserDto>(user);

        return result;
    }

    public async Task AddToRoleAsync(Guid userId, Roles role)
    {
        var (Success, Error) = await _identityService.AddToRoleAsync(userId, role.ToString());

        if (!Success)
        {
            throw new ServiceException($"Couldn't add user to role. {Error}");
        }
    }

    public async Task UpdateUserAsync(UserUpdateRequest request)
    {
        var user = await _identityService.GetUserByIdAsync(request.Id)
            ?? throw new NotFoundException("User not found.");

        _mapper.Map(request, user);

        var result = await _identityService.UpdateUserAsync(user);

        if (!result)
            throw new ServiceException("Could not update user. Try again");
    }

    public async Task<PaginatedList<UserWithRolesDto>> GetPagedUsersWithRolesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var users = await _identityService.GetUsersAsync(pageNumber, pageSize, cancellationToken);

        var usersWithRoles = new List<UserWithRolesDto>();
        foreach (var user in users.Items)
        {
            var dto = _mapper.Map<UserWithRolesDto>(user);
            dto.Roles = await _identityService.GetUserRolesAsync(user.Id);
            usersWithRoles.Add(dto);
        }

        var result = new PaginatedList<UserWithRolesDto>(usersWithRoles,
            users.TotalCount,
            pageNumber,
            pageSize);

        return result;
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        _ = await _identityService.GetUserByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");

        await _identityService.DeleteUserAsync(userId);
    }
}
