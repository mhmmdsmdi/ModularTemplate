using Framework.Api.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModularControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });
        return services;
    }
}