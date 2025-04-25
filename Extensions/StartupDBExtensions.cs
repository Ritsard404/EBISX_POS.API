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
                    // Create a connection string without the database
                    var builder = new MySqlConnectionStringBuilder(connectionString);
                    builder.Database = null; // Remove database from connection string
                    
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
                // Create a connection string without the database
                var builder = new MySqlConnectionStringBuilder(connectionString);
                builder.Database = null; // Remove database from connection string

                using var connection = new MySqlConnection(builder.ToString());
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = $"CREATE DATABASE IF NOT EXISTS `{databaseName}`";
                await command.ExecuteNonQueryAsync();
            }

            private async Task<bool> TableExistsAsync(DbContext context, string tableName)
            {
                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();
                try
                {
                    using var command = connection.CreateCommand();
                    command.CommandText = $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = '{tableName}'";
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result) > 0;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }

            public async Task InitializeAsync()
            {
                try
                {
                    var posConnectionString = _configuration.GetConnectionString("POSConnection");
                    var journalConnectionString = _configuration.GetConnectionString("JournalConnection");

                    // Check MySQL server connection without database
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

                    // Check if database exists and can connect
                    if (!await dataContext.Database.CanConnectAsync())
                    {
                        _logger.LogInformation("Creating POS database...");
                        await dataContext.Database.EnsureCreatedAsync();
                        //_logger.LogInformation("Seeding initial data for POS database...");
                        //await SeedData.InitializeAsync(_services);
                    }
                    else
                    {
                        _logger.LogInformation("POS database exists, checking for migrations...");
                        var pendingMigrations = await dataContext.Database.GetPendingMigrationsAsync();
                        if (pendingMigrations.Any())
                        {
                            _logger.LogInformation("Applying pending migrations for POS database...");
                            await dataContext.Database.MigrateAsync();
                        }
                        else
                        {
                            _logger.LogInformation("No pending migrations for POS database.");
                        }

                        // Check if we need to seed data
                        //if (!await dataContext.User.AnyAsync())
                        //{
                        //    _logger.LogInformation("No users found, seeding initial data...");
                        //    await SeedData.InitializeAsync(_services);
                        //}
                    }

                    // Check if database exists and can connect
                    if (!await journalContext.Database.CanConnectAsync())
                    {
                        _logger.LogInformation("Creating Journal database...");
                        await journalContext.Database.EnsureCreatedAsync();
                    }
                    else
                    {
                        _logger.LogInformation("Journal database exists, checking for migrations...");
                        var pendingMigrations = await journalContext.Database.GetPendingMigrationsAsync();
                        if (pendingMigrations.Any())
                        {
                            _logger.LogInformation("Applying pending migrations for Journal database...");
                            await journalContext.Database.MigrateAsync();
                        }
                        else
                        {
                            _logger.LogInformation("No pending migrations for Journal database.");
                        }
                    }

                    // Check for existing tables in Journal database
                    var accountJournalExists = await TableExistsAsync(journalContext, "accountjournal");
                    if (accountJournalExists)
                    {
                        _logger.LogInformation("Table 'accountjournal' already exists in Journal database.");
                    }
                    else
                    {
                        _logger.LogInformation("Table 'accountjournal' does not exist, it will be created with migrations.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while initializing the database.");
                    // Don't throw the exception, just log it
                    // This allows the application to continue even if some tables already exist
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
