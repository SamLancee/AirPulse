namespace AirPulse.Models.DTOs;

public class OpenMeteoAirQualityResponseDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public CurrentUnitsDto? CurrentUnits { get; set; }
    public CurrentAirQualityDto? Current { get; set; }
}

public class CurrentAirQualityDto
{
    public string? Time { get; set; }
    public int Interval { get; set; }
    public double? EuropeanAqi { get; set; }
    public double? Pm25 { get; set; }
    public double? Pm10 { get; set; }
    public double? NitrogenDioxide { get; set; }
}

public class CurrentUnitsDto
{
    public string? EuropeanAqi { get; set; }
    public string? Pm25 { get; set; }
    public string? Pm10 { get; set; }
    public string? NitrogenDioxide { get; set; }
}