using EBISX_POS.API.Data;
using EBISX_POS.API.Services.Interfaces;
using EBISX_POS.API.Services.Repositories;
using EBISX_POS.API.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add configuration services to the container.
builder.Services.Configure<FilePaths>(
    builder.Configuration.GetSection("FilePaths"));

// Get connection strings from configuration
var posConnectionString = builder.Configuration.GetConnectionString("POSConnection");
var journalConnectionString = builder.Configuration.GetConnectionString("JournalConnection");

// Register the primary context for the POS database using Pomelo MySQL Provider
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(posConnectionString, ServerVersion.AutoDetect(posConnectionString)));

// Register a separate context for the Journal database
builder.Services.AddDbContext<JournalContext>(options =>
    options.UseMySql(journalConnectionString, ServerVersion.AutoDetect(journalConnectionString)));


// Add Scope of Interface and Repository
builder.Services.AddScoped<IAuth, AuthRepository>();
builder.Services.AddScoped<IMenu, MenuRepository>();
builder.Services.AddScoped<IOrder, OrderRepository>();
builder.Services.AddScoped<IPayment, PaymentRepository>();
builder.Services.AddScoped<IJournal, JournalRepository>();
builder.Services.AddScoped<IEbisxAPI, EbisxAPIRepository>();
builder.Services.AddLogging();

// Add CORS
builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Allow any URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddOptions();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.InitializeAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
