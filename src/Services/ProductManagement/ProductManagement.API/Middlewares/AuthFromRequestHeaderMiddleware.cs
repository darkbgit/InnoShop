using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using ProductManagement.API.Clients;
using Refit;
using Shared.Core.Requests;
using Shared.Core.Responses;

namespace ProductManagement.API.Middlewares;

internal class AuthFromRequestHeaderMiddleware(RequestDelegate next,
    IAuthClient authClient)
{
    private readonly RequestDelegate _next = next;
    private readonly IAuthClient _authClient = authClient;

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers[HeaderNames.Authorization].FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader))
        {
            await _next(context);
            return;
        }

        var token = authHeader.Split(" ").LastOrDefault();

        if (string.IsNullOrEmpty(token))
        {
            await _next(context);
            return;
        }

        var request = new TokenRequest
        {
            Token = token,
        };

        var isValid = false;

        try
        {
            isValid = await _authClient.ValidateToken(request);
        }
        catch (ApiException)
        {
            await _next(context);
            return;
        }

        if (!isValid)
        {
            await _next(context);
            return;
        }

        UserInfoResponse? userInfoResponse = null;

        try
        {
            userInfoResponse = await _authClient.GetUserInfo(request);
        }
        catch (ApiException)
        {
            await _next(context);
            return;
        }

        if (userInfoResponse == null)
        {
            await _next(context);
            return;
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Authentication, authHeader),
            new Claim(ClaimTypes.NameIdentifier, userInfoResponse.Id.ToString()),
        };

        var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

        foreach (var userInfoRole in userInfoResponse.Roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, userInfoRole));
        }

        context.User = new ClaimsPrincipal(identity);

        await _next(context);
    }
}
