namespace AirPulse.Models;

public class AirQualityData
{
    public string CityName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public double? EuropeanAqi { get; set; }
    public double? Pm25 { get; set; }
    public double? Pm10 { get; set; }
    public double? NitrogenDioxide { get; set; }

    public string AqiCategory { get; set; } = "Onbekend";
    public string AqiBadgeClass { get; set; } = "secondary";
    public string HealthAdvice { get; set; } = string.Empty;
}