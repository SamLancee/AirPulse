using AirPulse.Models;
using AirPulse.Models.DTOs;

namespace AirPulse.Services;

public class AirQualityService : IAirQualityService
{
    private readonly OpenMeteoClient _client;
    private readonly List<CityLocation> _cities;

    public AirQualityService(OpenMeteoClient client, IConfiguration configuration)
    {
        _client = client;
        // Leest de array direct uit appsettings.json onder "AirQualitySettings:Cities"
        _cities = configuration.GetSection("AirQualitySettings:Cities").Get<List<CityLocation>>() 
                  ?? new List<CityLocation>();
    }

    public IReadOnlyList<CityLocation> GetConfiguredCities() => _cities;

    public async Task<OpenMeteoDto?> GetCurrentAirQualityForCityAsync(string cityName)
    {
        var city = _cities.FirstOrDefault(c => c.Name.Equals(cityName, StringComparison.OrdinalIgnoreCase));
        if (city == null)
        {
            return null;
        }

        return await _client.FetchAirQualityAsync(city.Latitude, city.Longitude);
    }
}