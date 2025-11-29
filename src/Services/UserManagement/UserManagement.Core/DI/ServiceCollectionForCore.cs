using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Mapping;
using UserManagement.Core.Options;
using UserManagement.Core.Services;

namespace UserManagement.Core.DI;

public static class ServiceCollectionForCore
{
    public static void RegisterDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

        services.AddOptions<FrontendOptions>()
            .Bind(configuration.GetSection(FrontendOptions.SectionName));
    }
}
