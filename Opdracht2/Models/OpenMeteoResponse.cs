using System.Text.Json.Serialization;
using AirPulse.Shared;

namespace AirPulse_API.Models;

public class OpenMeteoResponse
{
    [JsonPropertyName("hourly")]
    public HourlyData? Hourly { get; set; }
}

public class HourlyData
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; } = new();

    [JsonPropertyName("european_aqi")]
    public List<int?> EuropeanAqi { get; set; } = new();

    [JsonPropertyName("pm2_5")]
    public List<double?> Pm25 { get; set; } = new();

    [JsonPropertyName("pm10")]
    public List<double?> Pm10 { get; set; } = new();

    [JsonPropertyName("nitrogen_dioxide")]
    public List<double?> NitrogenDioxide { get; set; } = new();
}