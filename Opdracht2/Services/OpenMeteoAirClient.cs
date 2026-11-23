using System.Globalization;
using System.Net.Http.Json;
using AirPulse_API.Data;
using AirPulse_API.Models;
using AirPulse.Shared;
using Microsoft.EntityFrameworkCore;

namespace AirPulse_API.Services;

public class OpenMeteoAirClient : IAirQualityService
{
    private readonly HttpClient _httpClient;
    private readonly AirPulseDbContext _dbContext;
    private readonly ILogger<OpenMeteoAirClient> _logger;

    public OpenMeteoAirClient(HttpClient httpClient, AirPulseDbContext dbContext, ILogger<OpenMeteoAirClient> logger)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SyncCityDataAsync(CityLocation city, CancellationToken cancellationToken = default)
    {
        // Gebruik InvariantCulture zodat decimalen altijd met een punt (52.37) worden geformatteerd
        var lat = city.Latitude.ToString(CultureInfo.InvariantCulture);
        var lon = city.Longitude.ToString(CultureInfo.InvariantCulture);

        var url = $"https://air-quality-api.open-meteo.com/v1/air-quality?latitude={lat}&longitude={lon}&hourly=european_aqi,pm2_5,pm10,nitrogen_dioxide&past_days=7&forecast_days=4&timezone=Europe%2FAmsterdam";

        var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url, cancellationToken);

        if (response?.Hourly == null)
        {
            _logger.LogWarning("Geen data ontvangen van Open-Meteo voor {City}", city.Name);
            return;
        }

        var now = DateTime.UtcNow;
        var localNow = DateTime.Now;

        for (int i = 0; i < response.Hourly.Time.Count; i++)
{
    if (!DateTime.TryParse(response.Hourly.Time[i], out var timestamp))
        continue;

    bool isForecast = timestamp > localNow;

    // 1. Zoek specifiek naar het record van DIT type (Forecast OF Werkelijk)
    var existingSample = await _dbContext.AirSamples
        .FirstOrDefaultAsync(s => s.City == city.Name 
                               && s.Timestamp == timestamp 
                               && s.IsForecast == isForecast, cancellationToken);

    if (existingSample != null)
    {
        // Update alleen een bestaand record van hetzelfde type
        existingSample.EuropeanAqi = response.Hourly.EuropeanAqi.ElementAtOrDefault(i);
        existingSample.Pm25 = isForecast ? null : response.Hourly.Pm25.ElementAtOrDefault(i);
        existingSample.Pm10 = isForecast ? null : response.Hourly.Pm10.ElementAtOrDefault(i);
        existingSample.NitrogenDioxide = isForecast ? null : response.Hourly.NitrogenDioxide.ElementAtOrDefault(i);
        existingSample.LastUpdated = now;
    }
    else
    {
        // 2. Bestaat er nog geen record met deze IsForecast-waarde? Voeg een NIEUWE rij toe!
        _dbContext.AirSamples.Add(new AirSample
        {
            City = city.Name,
            Timestamp = timestamp,
            IsForecast = isForecast,
            EuropeanAqi = response.Hourly.EuropeanAqi.ElementAtOrDefault(i),
            Pm25 = isForecast ? null : response.Hourly.Pm25.ElementAtOrDefault(i),
            Pm10 = isForecast ? null : response.Hourly.Pm10.ElementAtOrDefault(i),
            NitrogenDioxide = isForecast ? null : response.Hourly.NitrogenDioxide.ElementAtOrDefault(i),
            LastUpdated = now
        });
    }
}

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Luchtkwaliteit voor {City} succesvol gesynchroniseerd.", city.Name);
    }

    public async Task<List<AirSample>> GetSamplesAsync(string city, DateTime? start, DateTime? end)
    {
        // Default start = vandaag (00:00), default end = start + 7 dagen
        var startDate = start?.Date ?? DateTime.Today;
        var endDate = end?.Date.AddDays(1).AddTicks(-1) ?? startDate.AddDays(7);

        return await _dbContext.AirSamples
            .Where(s => s.City.ToLower() == city.ToLower()
                     && s.Timestamp >= startDate
                     && s.Timestamp <= endDate)
            .OrderBy(s => s.Timestamp)
            .ToListAsync();
    }
}