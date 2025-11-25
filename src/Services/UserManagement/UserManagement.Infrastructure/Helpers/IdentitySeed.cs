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
        const string user1Email = "user1@user.com";
        const string user2Email = "user2@user.com";
        const string user3Email = "user3@user.com";



        string adminPassword = "AdminAdmin1!";
        string user1Password = "UserUser1!";
        string user2Password = "UserUser2!";
        string user3Password = "UserUser3!";

        var adminId = Guid.Parse("00000000-0000-0000-0000-000000000010");

        var user1Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var user2Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var user3Id = Guid.Parse("00000000-0000-0000-0000-000000000003");



        await SeedRolesAsync(roleManager);

        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            var user = new ApplicationUser()
            {
                Id = adminId,
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

        if (await userManager.FindByNameAsync(user1Email) == null)
        {
            var user = new ApplicationUser()
            {
                Id = user1Id,
                Email = user1Email,
                UserName = user1Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, user1Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.User.ToString());
            }
        }
        if (await userManager.FindByNameAsync(user2Email) == null)
        {
            var user = new ApplicationUser()
            {
                Id = user2Id,
                Email = user2Email,
                UserName = user2Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, user2Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.User.ToString());
            }
        }
        if (await userManager.FindByNameAsync(user3Email) == null)
        {
            var user = new ApplicationUser()
            {
                Id = user3Id,
                Email = user3Email,
                UserName = user3Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, user3Password);
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
