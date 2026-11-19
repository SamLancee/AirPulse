using AirPulse.Models;
using AirPulse.Shared;

namespace AirPulse.Services;

public interface IAirQualityService
{
    Task<List<string>> GetAvailableCitiesAsync();
    Task<List<AirSample>> GetAirQualityHistoryAsync(string cityName, DateTime? start = null, DateTime? end = null);
}