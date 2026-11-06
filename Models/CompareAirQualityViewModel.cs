using Microsoft.AspNetCore.Mvc.Rendering;
using AirPulse.Models.DTOs;

namespace AirPulse.Models;

public class CompareAirQualityViewModel
{
    public string CityA { get; set; } = string.Empty;
    public string CityB { get; set; } = string.Empty;

    public OpenMeteoDto? DataCityA { get; set; }
    public OpenMeteoDto? DataCityB { get; set; }

    public List<SelectListItem> AvailableCities { get; set; } = new();
    public string? ErrorMessage { get; set; }
}