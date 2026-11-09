using AirPulse.Services;
using AirPulse.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Voeg de services toe aan de DI-container
builder.Services.AddHttpClient<OpenMeteoClient>();
builder.Services.AddScoped<IAirQualityService, AirQualityService>();

builder.Services.AddHostedService<AirQualityCollectorService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=airpulse.db"));
var app = builder.Build();

// Maakt automatisch de database en tabellen aan als ze nog niet bestaan
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
