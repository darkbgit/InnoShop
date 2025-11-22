using Microsoft.AspNetCore.Identity;
using Shared.Core.Enums;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Helpers;

public static class IdentitySeed
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        const string adminEmail = "admin@admin.com";
        const string userEmail = "user@user.com";

        string adminPassword = "AdminAdmin1!";
        string userPassword = "UserUser1!";

        await SeedRolesAsync(roleManager);

        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            var user = new ApplicationUser()
            {
                Email = adminEmail,
                UserName = adminEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
        }

        if (await userManager.FindByNameAsync(userEmail) == null)
        {
            var user = new ApplicationUser()
            {
                Email = userEmail,
                UserName = userEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, userPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.User.ToString());
            }
        }
    }

    private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        if (await roleManager.FindByNameAsync(Roles.Admin.ToString()) == null)
        {
            await roleManager.CreateAsync(new ApplicationRole(Roles.Admin.ToString()));
        }

        if (await roleManager.FindByNameAsync(Roles.User.ToString()) == null)
        {
            await roleManager.CreateAsync(new ApplicationRole(Roles.User.ToString()));
        }
    }
}
