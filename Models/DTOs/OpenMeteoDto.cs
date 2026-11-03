using System.Text.Json.Serialization;

namespace AirPulse.Models.DTOs;

public class OpenMeteoDto
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("current")]
    public CurrentAirQualityDto? Current { get; set; }
}

public class CurrentAirQualityDto
{
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("interval")]
    public int Interval { get; set; }

    [JsonPropertyName("european_aqi")]
    public double? EuropeanAqi { get; set; }

    [JsonPropertyName("pm2_5")]
    public double? Pm25 { get; set; }

    [JsonPropertyName("pm10")]
    public double? Pm10 { get; set; }

    [JsonPropertyName("nitrogen_dioxide")]
    public double? NitrogenDioxide { get; set; }
}