using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Enums;
using Shared.Core.Requests;
using Shared.Core.Responses;
using UserManagement.API.Requests;
using UserManagement.Core.DTOs;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Requests;

namespace UserManagement.API.Controllers;

[Authorize]
[Route("api/user-management/[controller]")]
[ApiController]
public class UsersController(IUserService usersService)
    : ControllerBase
{
    private readonly IUserService _usersService = usersService;

    [Authorize(Roles = nameof(Roles.Admin))]
    [HttpGet("paginated-with-roles")]
    public async Task<IActionResult> GetUsersForAdminList([FromQuery] PaginationRequest request)
    {
        var users = await _usersService.GetPagedUsersWithRolesAsync(request.PageIndex, request.PageSize);

        return Ok(users);
    }

    [HttpGet("user")]
    public async Task<ActionResult<UserDto>> GetUser()
    {
        var id = HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;

        if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guidId))
        {
            return Unauthorized();
        }

        var user = await _usersService.GetUserByIdAsync(guidId);

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(user);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var user = await _usersService.GetUserByIdAsync(id);

        if (user == null)
        {
            return BadRequest();
        }

        return user;
    }

    [AllowAnonymous]
    [HttpPost("{id}/add-to-role")]
    public async Task<ActionResult> AddUserToRole([FromRoute] Guid id, [FromBody] AddToRoleRequest request)
    {
        if (!Enum.TryParse<Roles>(request.Role, out var role))
        {
            return BadRequest();
        }

        await _usersService.AddToRoleAsync(id, role);

        return Ok();
    }

    // /// <summary>
    // /// Create user.
    // /// </summary>
    // [AllowAnonymous]
    // [HttpPost("register")]
    // public async Task<ActionResult<UserDto>> AddUser([FromBody] RegisterRequest request)
    // {
    //     var id = await _usersService.CreateUserAsync(request);

    //     var user = await _usersService.GetUserByIdAsync(id);

    //     return CreatedAtAction(nameof(GetUserById), new { id }, user);
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        await _usersService.UpdateUserAsync(request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _usersService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        await _usersService.DeleteUserAsync(id);

        return NoContent();
    }
}
