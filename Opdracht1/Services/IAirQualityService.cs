using AirPulse.Models;

namespace AirPulse.Services;

public interface IAirQualityService
{
    Task<List<string>> GetAvailableCitiesAsync();
    Task<List<AirQualitySample>> GetAirQualityHistoryAsync(string cityName, DateTime? start = null, DateTime? end = null);
}