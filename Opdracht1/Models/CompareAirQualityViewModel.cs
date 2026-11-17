using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirPulse.Models;

public class CompareAirQualityViewModel
{
    public string CityA { get; set; } = string.Empty;
    public string CityB { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<SelectListItem> AvailableCities { get; set; } = new();

    // Meest recente meting per stad (voor side-by-side cards)
    public AirQualitySample? LatestCityA { get; set; }
    public AirQualitySample? LatestCityB { get; set; }

    // Volledige reeks per stad (voor 2 lijnen in de Chart.js grafiek!)
    public List<AirQualitySample> HistoryCityA { get; set; } = new();
    public List<AirQualitySample> HistoryCityB { get; set; } = new();

    public string? ErrorMessage { get; set; }
}