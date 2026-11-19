using AirPulse.Models;
using AirPulse.Shared;

namespace AirPulse.Services;

public class AirQualityService : IAirQualityService
{
    private readonly AirPulseApiClient _apiClient;

    public AirQualityService(AirPulseApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<string>> GetAvailableCitiesAsync()
    {
        return await _apiClient.FetchCitiesAsync();
    }

    public async Task<List<AirSample>> GetAirQualityHistoryAsync(string cityName, DateTime? start = null, DateTime? end = null)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            return new List<AirSample>();
        }

        return await _apiClient.FetchAirQualityAsync(cityName, start, end);
    }
}