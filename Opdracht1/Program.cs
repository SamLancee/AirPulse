using AirPulse.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Koppel de HttpClient aan jouw eigen backend API (Opdracht 2)
builder.Services.AddHttpClient<AirPulseApiClient>(client =>
{
    var baseUrl = builder.Configuration["AirPulseApi:BaseUrl"] ?? "http://localhost:5087/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<IAirQualityService, AirQualityService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
//Checking
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();