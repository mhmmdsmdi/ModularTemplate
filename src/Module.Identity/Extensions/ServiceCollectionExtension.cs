using Microsoft.Extensions.DependencyInjection;
using Module.Identity.Core;
using Module.Identity.Jwt;

namespace Module.Identity.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterIdentityServices(this IServiceCollection services,
        IdentitySettings identitySettings)
    {
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}