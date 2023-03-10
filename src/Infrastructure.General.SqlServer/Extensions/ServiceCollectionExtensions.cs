using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.General.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection AddSqlServerDatabaseContext<T>(this IServiceCollection services, string connectionString) where T : DbContext
    {
        services.AddDbContext<T>(option =>
        {
            option.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName));
        });

        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        dbContext.Database.Migrate();
        return services;
    }
}