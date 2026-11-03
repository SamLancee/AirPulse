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
        // Cultuur invariant zorgt dat 51.4416 met een punt wordt geformatteerd in plaats van een komma
        var lat = latitude.ToString(CultureInfo.InvariantCulture);
        var lon = longitude.ToString(CultureInfo.InvariantCulture);
        var url = $"air-quality?latitude={lat}&longitude={lon}&current=european_aqi,pm2_5,pm10,nitrogen_dioxide";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        return JsonSerializer.Deserialize<OpenMeteoDto>(json, options);
    }
}