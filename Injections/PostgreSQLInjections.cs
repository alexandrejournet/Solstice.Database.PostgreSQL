using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Solstice.Database.PostgreSQL.Injections;

public static class PostgreSqlInjections
{
    public static void AddDatabaseContext<TDbContext>(this IServiceCollection services, string config, bool migrate = false)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options =>
        {
#if DEBUG
            options.EnableSensitiveDataLogging(true);
#endif
            options.UseNpgsql(config);
        }, ServiceLifetime.Transient);

        if (migrate)
        {
            services.BuildServiceProvider()!.GetService<TDbContext>()!.Database.Migrate();
        }
    }

    public static void AddDatabaseContext<TDbContext>(this IServiceCollection services, string config, ILoggerFactory? loggerFactory, bool migrate = false)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>(options =>
        {
#if DEBUG
            options.EnableSensitiveDataLogging(true);
            if (loggerFactory is not null)
            {
                options.UseLoggerFactory(loggerFactory);
            }
#endif
            options.UseNpgsql(config);
        }, ServiceLifetime.Transient);

        if (migrate)
        {
            services.BuildServiceProvider()!.GetService<TDbContext>()!.Database.Migrate();
        }
    }
}