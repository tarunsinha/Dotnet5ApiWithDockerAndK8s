using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace dotnet5.Migrations
{
    public static class DBMigrations
    {
        public static IHost MigrateDatabase<T>(this IHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    var migrateDbPolicy = Policy
                        .Handle<Exception>()
                        .WaitAndRetry(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) / 2));

                    migrateDbPolicy.Execute(() =>
                    {
                        db.Database.Migrate();
                    });
                    // db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return webHost;
        }
    }
}
