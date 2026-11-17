namespace AirPulse_API.Models;

public class AirSample
{
    public int Id { get; set; }
    public string City { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public int? EuropeanAqi { get; set; }
    public double? Pm25 { get; set; }
    public double? Pm10 { get; set; }
    public double? NitrogenDioxide { get; set; }
    public DateTime LastUpdated { get; set; }
}