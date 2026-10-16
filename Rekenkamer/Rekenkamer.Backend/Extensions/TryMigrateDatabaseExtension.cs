using Microsoft.EntityFrameworkCore;
using Rekenkamer.Infrastructure.Data.Entities;

namespace Rekenkamer.Backend.Extensions;

public static class TryMigrateDatabaseExtension
{
    public static void TryMigrateDatabase(
        this IServiceProvider servicesProvider)
    {
        using (var scope = servicesProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
                using var db = context.CreateDbContext();

                if (db.Database.IsRelational())
                {
                    db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Er is een fout opgetreden tijdens het uitvoeren van de database migraties.\r\n{ex.Message}");
            }
        }
    }
}
