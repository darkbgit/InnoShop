using Microsoft.AspNetCore.Mvc;
using Shared.Core.Requests;
using Shared.Core.Responses;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Requests;

namespace UserManagement.API.Controllers;

[Route("api/user-management/auth")]
[ApiController]
public class AuthenticationController(IAuthService authService, IJwtTokenService jwtTokenService)
    : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        return Ok(result);
    }

    [HttpPost("validate-token")]
    public ActionResult<bool> ValidateToken([FromBody] TokenRequest request)
    {
        var result = _jwtTokenService.ValidateToken(request.Token);
        return result;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _authService.RegisterAsync(request);
        return Ok();
    }

    [HttpPost("user-info")]
    public async Task<ActionResult<UserInfoResponse>> GetUserInfo([FromBody] TokenRequest request)
    {
        var userInfo = await _authService.GetUserInfoAsync(request.Token);

        if (userInfo == null)
        {
            return BadRequest();
        }

        return userInfo;
    }
}
