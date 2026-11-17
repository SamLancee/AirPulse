using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirPulse.Models;

public class AirQualityView
{
    public string SelectedCity { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<SelectListItem> AvailableCities { get; set; } = new();
    
    // De meest recente meting (voor je grote AQI badge & cards)
    public AirQualitySample? LatestSample { get; set; }
    
    // Alle metingen in de gekozen periode (voor je Chart.js grafiek & min/max/avg)
    public List<AirQualitySample> HistorySamples { get; set; } = new();
    
    public string? ErrorMessage { get; set; }
}