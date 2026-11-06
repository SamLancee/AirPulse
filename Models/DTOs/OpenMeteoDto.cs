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

    public string FormattedTime
    {
        get
        {
            if (string.IsNullOrEmpty(Time) || !DateTime.TryParse(Time, out var parsedDate))
            {
                return Time ?? "-";
            }

            var today = DateTime.Today;

            if (parsedDate.Date == today)
            {
                return $"vandaag om {parsedDate:HH:mm}";
            }
            if (parsedDate.Date == today.AddDays(-1))
            {
                return $"gisteren om {parsedDate:HH:mm}";
            }

            // Fallback voor oudere data: bijv. "05-11-2026 om 10:00"
            return $"{parsedDate:dd-MM-yyyy} om {parsedDate:HH:mm}";
        }
    }

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
        <= 20 => "primary",   // Groen
        <= 40 => "info",      // Lichtblauw / Cyaan
        <= 60 => "warning",   // Geel
        <= 80 => "danger",    // Rood / Oranje
        > 80 => "dark",       // Donkerrood / Zwart
        _ => "secondary"
    };

    public int GetPercentage(double? value, double threshold)
    {
        if (!value.HasValue || value <= 0) return 0;
        // Rekent uit hoeveel % van de drempelwaarde is bereikt, met een max van 100%
        return (int)Math.Min((value.Value / threshold) * 100, 100);
    }

    public string GetPollutantColor(double? value, double threshold)
    {
        if (!value.HasValue) return "secondary";

        // Groen onder 50% van de grenswaarde, geel tussen 50-80%, rood erboven
        var ratio = value.Value / threshold;
        if (ratio < 0.5) return "info";   // Blauw
        if (ratio < 0.8) return "warning";   // Geel
        return "danger";                     // Rood / attentie
    }
}