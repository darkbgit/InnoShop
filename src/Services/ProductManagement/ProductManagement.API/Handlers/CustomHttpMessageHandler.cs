using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ProductManagement.API.Handlers;

public class CustomHttpMessageHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var isAuthenticated = _httpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true };

        if (!isAuthenticated)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        string? token = null;

        token = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Authentication)
            ?.Value;

        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
