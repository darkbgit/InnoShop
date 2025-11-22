using Microsoft.AspNetCore.Builder;
using Shared.Core.Middlewares;

namespace Shared.Core.Extensions;

public static class HostingExtensions
{
    public static void UseCustomExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
