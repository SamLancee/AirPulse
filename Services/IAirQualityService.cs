using AirPulse.Models;
using AirPulse.Models.DTOs;

namespace AirPulse.Services;

public interface IAirQualityService
{
    IReadOnlyList<CityLocation> GetConfiguredCities();
    Task<OpenMeteoDto?> GetCurrentAirQualityForCityAsync(string cityName);
}