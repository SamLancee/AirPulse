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

    // Bepaal de tekstuele status op basis van European AQI banden
    public string AqiCategory => EuropeanAqi switch
    {
        <= 20 => "Zeer goed",
        <= 40 => "Goed",
        <= 60 => "Matig",
        <= 80 => "Slecht",
        <= 100 => "Zeer slecht",
        > 100 => "Extreem slecht",
        _ => "Onbekend"
    };

    // Bepaal de Bootstrap kleurklasse
    public string AqiBadgeClass => EuropeanAqi switch
    {
        <= 20 => "success",   // Groen
        <= 40 => "info",      // Lichtblauw / Cyaan
        <= 60 => "warning",   // Geel
        <= 80 => "danger",    // Rood / Oranje
        > 80 => "dark",       // Donkerrood / Zwart
        _ => "secondary"
    };
}