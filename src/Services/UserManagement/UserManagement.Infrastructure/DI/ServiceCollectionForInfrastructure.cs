using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using UserManagement.Core.Interfaces;
using UserManagement.Infrastructure.Authentication;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Interfaces;
using UserManagement.Infrastructure.Mapping;
using UserManagement.Infrastructure.Options;
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
        services.AddScoped<IEmailService, EmailService>();

        var productOptions = configuration.GetSection(ProductOptions.SectionName).Get<ProductOptions>()
                ?? throw new ArgumentNullException("ProductOptions is not configured");

        services.AddOptions<ProductOptions>().Bind(configuration.GetSection(ProductOptions.SectionName));

        services.AddOptions<EmailOptions>()
            .Bind(configuration.GetSection(EmailOptions.SectionName));

        services.AddHttpClient<IProductIntegrationService, ProductIntegrationService>(client =>
        {
            client.BaseAddress = new Uri(productOptions.Url);
        }).AddTransientHttpErrorPolicy(policyBuilder =>
            policyBuilder.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
            
        services.AddScoped<IProductIntegrationService, ProductIntegrationService>();
    }
}
