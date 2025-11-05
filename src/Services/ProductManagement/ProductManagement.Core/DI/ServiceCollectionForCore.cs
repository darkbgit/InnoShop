using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Core.Mapping;

namespace ProductManagement.Core.DI;

public class ServiceCollectionForCore()
{
    public static void RegisterDependencies(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionForCore).Assembly));

        services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
    }
}
