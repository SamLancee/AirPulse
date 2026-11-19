using Microsoft.AspNetCore.Mvc.Rendering;
using AirPulse.Shared;

namespace AirPulse.Models;

public class CompareAirQualityViewModel
{
    public string CityA { get; set; } = string.Empty;
    public string CityB { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<SelectListItem> AvailableCities { get; set; } = new();

    // Meest recente meting per stad (voor side-by-side cards)
    public AirSample? LatestCityA { get; set; }
    public AirSample? LatestCityB { get; set; }

    // Volledige reeks per stad (voor 2 lijnen in de Chart.js grafiek!)
    public List<AirSample> HistoryCityA { get; set; } = new();
    public List<AirSample> HistoryCityB { get; set; } = new();

    public string? ErrorMessage { get; set; }
}