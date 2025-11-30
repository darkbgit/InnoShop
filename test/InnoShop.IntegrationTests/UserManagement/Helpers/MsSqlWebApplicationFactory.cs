using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using UserManagement.Core.Interfaces;
using UserManagement.Infrastructure.Data;

namespace InnoShop.IntegrationTests.UserManagement.Helpers;

public class MsSqlWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Strong_Password_123!") 
        .Build();

    public Task InitializeAsync() => _dbContainer.StartAsync();

    public new Task DisposeAsync() => _dbContainer.StopAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<InnoShopUserContext>));

            if (descriptor != null)
               services.Remove(descriptor);

            services.AddDbContext<InnoShopUserContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });

            var emailServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IEmailService));

            if (emailServiceDescriptor != null)
               services.Remove(emailServiceDescriptor);

            services.AddSingleton<IEmailService, FakeEmailService>();

            var sp = services.BuildServiceProvider();
            
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<InnoShopUserContext>();

            db.Database.EnsureCreated();
        });
    }
}

