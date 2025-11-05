using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Core.Interfaces;
using ProductManagement.DataAccess.Data;
using ProductManagement.DataAccess.Repositories;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.DataAccess.DI;

public static class ServiceCollectionForDataAccess
{
    public static void RegisterDependencies(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<InnoShopContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IRepository<Product>, ProductRepository>();
        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IReadRepository<Category>, CategoryReadRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
