using System;

namespace AirPulse.Shared;

public class AirSample
{
    public int Id { get; set; }
    public string City { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public double? EuropeanAqi { get; set; }
    public double? Pm25 { get; set; }
    public double? Pm10 { get; set; }
    public double? NitrogenDioxide { get; set; }
    public bool IsForecast { get; set; }
    public DateTime LastUpdated { get; set; }

    // Berekende UI-properties
    public string FormattedTime
    {
        get
        {
            var today = DateTime.Today;

            if (Timestamp.Date == today)
                return $"vandaag om {Timestamp:HH:mm}";
            if (Timestamp.Date == today.AddDays(-1))
                return $"gisteren om {Timestamp:HH:mm}";

            return $"{Timestamp:dd-MM-yyyy} om {Timestamp:HH:mm}";
        }
    }

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

    public string AqiBadgeClass => EuropeanAqi switch
    {
        <= 20 => "primary",   // Groen/Blauw
        <= 40 => "info",      // Lichtblauw / Cyaan
        <= 60 => "warning",   // Geel
        <= 80 => "danger",    // Rood / Oranje
        > 80 => "dark",       // Donkerrood / Zwart
        _ => "secondary"
    };

    public string HealthAdvice => EuropeanAqi switch
    {
        <= 20 => "Uitstekende luchtkwaliteit. Ideaal voor alle buitenactiviteiten.",
        <= 40 => "Goede luchtkwaliteit. Geen beperkingen voor buitenactiviteiten.",
        <= 60 => "Matige luchtkwaliteit. Gevoelige groepen kunnen lichte klachten ervaren bij langdurige inspanning.",
        <= 80 => "Slechte luchtkwaliteit. Beperk zware inspanning buitenshuis bij luchtwegklachten.",
        <= 100 => "Zeer slechte luchtkwaliteit. Vermijd intensieve buitenactiviteiten.",
        > 100 => "Extreem slechte luchtkwaliteit. Blijf zoveel mogelijk binnen.",
        _ => "Geen gezondheidsadvies beschikbaar."
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