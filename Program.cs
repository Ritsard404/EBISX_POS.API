using EBISX_POS.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();
builder.WebHost.UseUrls("http://localhost:5166");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

// Initialize database
await app.InitializeDatabaseAsync();

app.Run();
