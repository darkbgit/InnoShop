using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Core.Interfaces;
using UserManagement.Infrastructure.Authentication;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Mapping;
using UserManagement.Infrastructure.Services;

namespace UserManagement.Infrastructure.DI;

public class ServiceCollectionForInfrastructure
{
    public static void RegisterDependencies(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<InnoShopUserContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                    options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<InnoShopUserContext>();
        //.AddClaimsPrincipalFactory<>();

        services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
    }
}
