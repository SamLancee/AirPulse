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
    
    public bool IsCityABetter => LatestCityA?.EuropeanAqi != null && LatestCityB?.EuropeanAqi != null 
                             && LatestCityA.EuropeanAqi < LatestCityB.EuropeanAqi;

    public bool IsCityBBetter => LatestCityA?.EuropeanAqi != null && LatestCityB?.EuropeanAqi != null 
                                && LatestCityB.EuropeanAqi < LatestCityA.EuropeanAqi;

    public string ComparisonSummary
    {
        get
        {
            if (LatestCityA?.EuropeanAqi == null || LatestCityB?.EuropeanAqi == null)
                return "Onvoldoende actuele meetgegevens om beide steden te vergelijken.";

            double diff = Math.Abs(LatestCityA.EuropeanAqi.Value - LatestCityB.EuropeanAqi.Value);

            if (diff == 0)
                return $"{CityA} en {CityB} hebben momenteel exact dezelfde luchtkwaliteit (AQI: {LatestCityA.EuropeanAqi:0}).";

            string betterCity = IsCityABetter ? CityA : CityB;
            string worseCity = IsCityABetter ? CityB : CityA;

            return $"{betterCity} heeft momenteel een betere luchtkwaliteit dan {worseCity} ({diff:0} punten lagere AQI).";
        }
    }
}