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
}