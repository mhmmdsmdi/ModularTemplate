using Common.Extensions;
using Framework.Api.Controllers;
using Framework.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiFrameworkServices(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            //url segment => {version}
            options.AssumeDefaultVersionWhenUnspecified = true; //default => false;
            options.DefaultApiVersion = new ApiVersion(1, 0); //v1.0 == v1
            options.ReportApiVersions = true;

            //ApiVersion.TryParse("1.0", out var version10);
            //ApiVersion.TryParse("1", out var version1);
            //var a = version10 == version1;

            //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            // api/posts?api-version=1

            //options.ApiVersionReader = new UrlSegmentApiVersionReader();
            // api/v1/posts

            //options.ApiVersionReader = new HeaderApiVersionReader(new[] { "Api-Version" });
            // header => Api-Version : 1

            //options.ApiVersionReader = new MediaTypeApiVersionReader()

            //options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"), new UrlSegmentApiVersionReader())
            // combine of [querystring] & [urlsegment]
        });

        services.AddModularControllers();

        services.RegisterValidatorsAsServices();

        return services;
    }

    private static IServiceCollection AddModularControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add(typeof(OkResultAttribute));
                options.Filters.Add(typeof(NotFoundResultAttribute));
                options.Filters.Add(typeof(ContentResultFilterAttribute));
                options.Filters.Add(typeof(ModelStateValidationAttribute));
                options.Filters.Add(typeof(BadRequestResultFilterAttribute));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            })
            .ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });
        return services;
    }
}