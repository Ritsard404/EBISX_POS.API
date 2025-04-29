using EBISX_POS.API.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace EBISX_POS.API.Extensions
{
    public static class StartupDBExtensions
    {
        private class DatabaseInitializer
        {
            private readonly ILogger<DatabaseInitializer> _logger;
            private readonly IServiceProvider _services;
            private readonly IConfiguration _configuration;

            public DatabaseInitializer(ILogger<DatabaseInitializer> logger, IServiceProvider services, IConfiguration configuration)
            {
                _logger = logger;
                _services = services;
                _configuration = configuration;
            }

            private async Task<bool> CheckMySqlServerConnectionAsync(string connectionString)
            {
                try
                {
                    var builder = new MySqlConnectionStringBuilder(connectionString);
                    builder.Database = null;
                    
                    using var connection = new MySqlConnection(builder.ToString());
                    await connection.OpenAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to connect to MySQL server");
                    return false;
                }
            }

            private async Task CreateDatabaseIfNotExistsAsync(string connectionString, string databaseName)
            {
                var builder = new MySqlConnectionStringBuilder(connectionString);
                builder.Database = null;

                using var connection = new MySqlConnection(builder.ToString());
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = $"CREATE DATABASE IF NOT EXISTS `{databaseName}`";
                await command.ExecuteNonQueryAsync();
            }

            private async Task InitializeDatabaseAsync<TContext>(TContext context, string databaseName) where TContext : DbContext
            {
                try
                {
                    _logger.LogInformation($"Initializing {databaseName} database...");
                    
                    // Get pending migrations
                    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
                    
                    if (!pendingMigrations.Any() && !appliedMigrations.Any())
                    {
                        // First time setup - no migrations exist
                        _logger.LogInformation($"No migrations found for {databaseName}. Creating initial schema...");
                        await context.Database.EnsureCreatedAsync();
                        
                        // Create the migrations history table manually
                        await context.Database.ExecuteSqlRawAsync(@"
                            CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
                                `MigrationId` varchar(150) NOT NULL,
                                `ProductVersion` varchar(32) NOT NULL,
                                PRIMARY KEY (`MigrationId`)
                            ) CHARACTER SET=utf8mb4;");
                            
                        // Insert the initial migration record
                        await context.Database.ExecuteSqlRawAsync(@"
                            INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
                            VALUES ('InitialCreate', '8.0.2');");
                    }
                    else
                    {
                        // Apply any pending migrations
                        if (pendingMigrations.Any())
                        {
                            _logger.LogInformation($"Applying {pendingMigrations.Count()} pending migrations for {databaseName}...");
                            await context.Database.MigrateAsync();
                        }
                        else
                        {
                            _logger.LogInformation($"No pending migrations for {databaseName}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error initializing {databaseName} database");
                    throw;
                }
            }

            public async Task InitializeAsync()
            {
                try
                {
                    var posConnectionString = _configuration.GetConnectionString("POSConnection");
                    var journalConnectionString = _configuration.GetConnectionString("JournalConnection");

                    // Check MySQL server connection
                    if (!await CheckMySqlServerConnectionAsync(posConnectionString))
                    {
                        _logger.LogError("Cannot connect to MySQL server. Please ensure MySQL server is running.");
                        return;
                    }

                    // Create databases if they don't exist
                    await CreateDatabaseIfNotExistsAsync(posConnectionString, "ebisx_pos");
                    await CreateDatabaseIfNotExistsAsync(journalConnectionString, "ebisx_journal");

                    var dataContext = _services.GetRequiredService<DataContext>();
                    var journalContext = _services.GetRequiredService<JournalContext>();

                    // Initialize both databases
                    await InitializeDatabaseAsync(dataContext, "POS");
                    await InitializeDatabaseAsync(journalContext, "Journal");

                    //// Check if we need to seed initial data
                    //if (!await dataContext.User.AnyAsync())
                    //{
                    //    _logger.LogInformation("Seeding initial data for POS database...");
                    //    await SeedData.InitializeAsync(_services);
                    //    _logger.LogInformation("Initial data seeded successfully.");
                    //}
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while initializing the database.");
                    throw; // Re-throw to prevent application startup if database initialization fails
                }
            }
        }

        public static async Task InitializeDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var initializer = new DatabaseInitializer(
                services.GetRequiredService<ILogger<DatabaseInitializer>>(),
                services,
                services.GetRequiredService<IConfiguration>()
            );
            
            await initializer.InitializeAsync();
        }
    }
}
