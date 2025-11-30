using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Middlewares;
using ProductManagement.DataAccess.Data;
using ProductManagement.DataAccess.Helpers;

namespace ProductManagement.API.Extensions;

internal static class HostingExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<InnoShopContext>();
        try
        {
            await SeedDatabase.SeedAsync(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }

    public static void UseAuthFromRequestHeaderMiddleware(this WebApplication app)
    {
        app.UseMiddleware<AuthFromRequestHeaderMiddleware>();
    }

    public static async Task ApplyMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<InnoShopContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}
