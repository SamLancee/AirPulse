using System.Globalization;
using System.Text.Json;
using AirPulse.Models.DTOs;

namespace AirPulse.Services;

public class OpenMeteoClient
{
    private readonly HttpClient _httpClient;

    public OpenMeteoClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://air-quality-api.open-meteo.com/v1/");
    }

    public async Task<OpenMeteoDto?> FetchAirQualityAsync(double latitude, double longitude)
{
    try
    {
        var lat = latitude.ToString(CultureInfo.InvariantCulture);
        var lon = longitude.ToString(CultureInfo.InvariantCulture);
        var url = $"air-quality?latitude={lat}&longitude={lon}&current=european_aqi,pm2_5,pm10,nitrogen_dioxide&timezone=auto";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<OpenMeteoDto>(json, options);
    }
    catch (Exception)
    {
        // Vangt DNS-fouten, time-outs, ongeldige URL's en verbroken verbindingen af
        return null;
    }
}
}