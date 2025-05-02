using EBISX_POS.API.Data;
using EBISX_POS.API.Services;
using EBISX_POS.API.Services.Interfaces;
using EBISX_POS.API.Services.Repositories;
using EBISX_POS.API.Settings;
using Microsoft.EntityFrameworkCore;


namespace EBISX_POS.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add configuration services
            services.Configure<FilePaths>(configuration.GetSection("FilePaths"));

            // Register database contexts
            services.AddDatabaseContexts(configuration);

            // Register repositories
            services.AddRepositories();

            // Add CORS with any origin (without credentials)
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddLogging();
            services.AddOptions();

            return services;
        }

        private static IServiceCollection AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var posConnectionString = configuration.GetConnectionString("POSConnection");
            var journalConnectionString = configuration.GetConnectionString("JournalConnection");

            services.AddDbContext<DataContext>(options =>
                options.UseMySql(posConnectionString, ServerVersion.AutoDetect(posConnectionString)));

            services.AddDbContext<JournalContext>(options =>
                options.UseMySql(journalConnectionString, ServerVersion.AutoDetect(journalConnectionString)));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuth, AuthRepository>();
            services.AddScoped<IMenu, MenuRepository>();
            services.AddScoped<IOrder, OrderRepository>();
            services.AddScoped<IPayment, PaymentRepository>();
            services.AddScoped<IJournal, JournalRepository>();
            services.AddScoped<IEbisxAPI, EbisxAPIRepository>();
            services.AddScoped<IReport, ReportRepository>();
            services.AddScoped<IInvoiceNumberService, InvoiceNumberService>();

            return services;
        }
    }
}