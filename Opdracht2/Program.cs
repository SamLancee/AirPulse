using AirPulse_API.Data;
using AirPulse_API.Models;
using AirPulse_API.Services;
using AirPulse.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuratie binden
builder.Services.Configure<AirPulseSettings>(
    builder.Configuration.GetSection("AirQualitySettings"));

// 2. Database (SQLite) registreren
builder.Services.AddDbContext<AirPulseDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. HttpClient & Services registreren via DI
builder.Services.AddHttpClient<IAirQualityService, OpenMeteoAirClient>();

// 4. Background Worker registreren
builder.Services.AddHostedService<AirQualitySyncWorker>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Zorg dat de SQLite database automatisch wordt aangemaakt bij opstarten
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AirPulseDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();