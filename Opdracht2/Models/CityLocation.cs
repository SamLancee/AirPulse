namespace AirPulse_API.Models;

public class AirPulseSettings
{
    public int FetchIntervalMinutes { get; set; } = 60;
    public List<CityLocation> Cities { get; set; } = new();
}

public class CityLocation
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}