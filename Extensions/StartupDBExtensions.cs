using EBISX_POS.API.Data;
using EFCore.AutomaticMigrations;

namespace EBISX_POS.API.Extensions
{
    public static class StartupDBExtensions
    {
        public static async void CreteDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var dataContext = services.GetRequiredService<DataContext>();
            var journalContext = services.GetRequiredService<JournalContext>();

            dataContext.Database.EnsureCreated();
            journalContext.Database.EnsureCreated();

            //MigrateDatabaseToLatestVersion.Execute(dataContext);
            //MigrateDatabaseToLatestVersion.Execute(journalContext);

            dataContext.MigrateToLatestVersion();
            journalContext.MigrateToLatestVersion();
            await SeedData.InitializeAsync(services);
        }
    }
}
