using System.Net.Http.Json;
using AirPulse.Models;

namespace AirPulse.Services;

public class AirPulseApiClient
{
    private readonly HttpClient _httpClient;

    public AirPulseApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<string>> FetchCitiesAsync()
    {
        try
        {
            var cities = await _httpClient.GetFromJsonAsync<List<string>>("api/air/cities");
            return cities ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    public async Task<List<AirQualitySample>> FetchAirQualityAsync(string city, DateTime? start = null, DateTime? end = null)
    {
        try
        {
            var url = $"api/air/{Uri.EscapeDataString(city)}";
            var queryParams = new List<string>();

            if (start.HasValue)
                queryParams.Add($"start={start.Value:yyyy-MM-dd}");
            if (end.HasValue)
                queryParams.Add($"end={end.Value:yyyy-MM-dd}");

            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            var samples = await _httpClient.GetFromJsonAsync<List<AirQualitySample>>(url);
            return samples ?? new List<AirQualitySample>();
        }
        catch
        {
            return new List<AirQualitySample>();
        }
    }
}