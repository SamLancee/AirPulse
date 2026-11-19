using AirPulse_API.Models;
using AirPulse.Shared;

namespace AirPulse_API.Services;

public interface IAirQualityService
{
    Task SyncCityDataAsync(CityLocation city, CancellationToken cancellationToken = default);
    Task<List<AirSample>> GetSamplesAsync(string city, DateTime? start, DateTime? end);
}