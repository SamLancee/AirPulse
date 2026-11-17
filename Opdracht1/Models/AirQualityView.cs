using AirPulse.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirPulse.Models;

public class AirQualityView
{
    public string SelectedCity { get; set; } = string.Empty;
    public List<SelectListItem> AvailableCities { get; set; } = new();
    public OpenMeteoDto? CurrentAirQuality { get; set; }
    public string? ErrorMessage { get; set; }
}