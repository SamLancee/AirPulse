using System;

namespace AirPulse.Models;

public class AirQualitySample
{
    public int Id { get; set; }
    public string City { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public double? EuropeanAqi { get; set; }
    public double? Pm25 { get; set; }
    public double? Pm10 { get; set; }
    public double? NitrogenDioxide { get; set; }
    public DateTime LastUpdated { get; set; }

    public string FormattedTime
    {
        get
        {
            var today = DateTime.Today;

            if (Timestamp.Date == today)
            {
                return $"vandaag om {Timestamp:HH:mm}";
            }
            if (Timestamp.Date == today.AddDays(-1))
            {
                return $"gisteren om {Timestamp:HH:mm}";
            }

            return $"{Timestamp:dd-MM-yyyy} om {Timestamp:HH:mm}";
        }
    }

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
        <= 20 => "primary",   // Groen/Blauw
        <= 40 => "info",      // Lichtblauw / Cyaan
        <= 60 => "warning",   // Geel
        <= 80 => "danger",    // Rood / Oranje
        > 80 => "dark",       // Donkerrood / Zwart
        _ => "secondary"
    };

    public int GetPercentage(double? value, double threshold)
    {
        if (!value.HasValue || value <= 0) return 0;
        return (int)Math.Min((value.Value / threshold) * 100, 100);
    }

    public string GetPollutantColor(double? value, double threshold)
    {
        if (!value.HasValue) return "secondary";

        var ratio = value.Value / threshold;
        if (ratio < 0.5) return "info";      // Blauw
        if (ratio < 0.8) return "warning";   // Geel
        return "danger";                     // Rood / attentie
    }
}