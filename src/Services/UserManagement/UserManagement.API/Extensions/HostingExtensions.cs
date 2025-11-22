using Microsoft.AspNetCore.Identity;
using UserManagement.API.Middlewares;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Helpers;

namespace UserManagement.API.Extensions;

public static class HostingExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        try
        {
            await IdentitySeed.SeedAsync(userManager, roleManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}
