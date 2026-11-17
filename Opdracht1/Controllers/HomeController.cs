using AirPulse.Models;
using AirPulse.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirPulse.Controllers;

public class HomeController : Controller
{
    private readonly IAirQualityService _airQualityService;

    public HomeController(IAirQualityService airQualityService)
    {
        _airQualityService = airQualityService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? city, DateTime? start, DateTime? end)
    {
        // 1. Haal de beschikbare steden op uit de backend API (/api/air/cities)
        var availableCities = await _airQualityService.GetAvailableCitiesAsync();

        // 2. Bepaal de gekozen stad (default naar Amsterdam of de eerste stad uit de API)
        var selectedCityName = string.IsNullOrWhiteSpace(city)
            ? (availableCities.FirstOrDefault() ?? "Amsterdam")
            : city;

        // Default startdatum op 2 dagen geleden zetten als er niets is gekozen, 
        // zodat je direct de opgebouwde historie in de grafiek ziet
        var queryStart = start ?? DateTime.Today.AddDays(-2);
        var queryEnd = end ?? queryStart.AddDays(7);

        // 3. Haal de historie + actuele metingen op uit jouw eigen API
        var samples = await _airQualityService.GetAirQualityHistoryAsync(selectedCityName, queryStart, queryEnd);
        var latestSample = samples.LastOrDefault();

        // 4. Bouw het ViewModel
        var viewModel = new AirQualityView
        {
            SelectedCity = selectedCityName,
            StartDate = queryStart,
            EndDate = queryEnd,
            AvailableCities = availableCities.Select(c => new SelectListItem
            {
                Value = c,
                Text = c,
                Selected = c.Equals(selectedCityName, StringComparison.OrdinalIgnoreCase)
            }).ToList(),
            LatestSample = latestSample,
            HistorySamples = samples,
            ErrorMessage = latestSample == null
                ? $"Kon geen gegevens ophalen voor {selectedCityName}. Controleer of de backend API draait."
                : null
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Vergelijken(string? cityA, string? cityB, DateTime? start, DateTime? end)
    {
        var availableCities = await _airQualityService.GetAvailableCitiesAsync();

        cityA ??= availableCities.FirstOrDefault() ?? "Amsterdam";
        cityB ??= availableCities.Skip(1).FirstOrDefault() ?? "Parijs";

        var queryStart = start ?? DateTime.Today.AddDays(-2);
        var queryEnd = end ?? queryStart.AddDays(7);

        // Haal beide steden parallel op uit jouw eigen backend API
        var taskA = _airQualityService.GetAirQualityHistoryAsync(cityA, queryStart, queryEnd);
        var taskB = _airQualityService.GetAirQualityHistoryAsync(cityB, queryStart, queryEnd);

        await Task.WhenAll(taskA, taskB);

        var samplesA = await taskA;
        var samplesB = await taskB;

        var latestA = samplesA.LastOrDefault();
        var latestB = samplesB.LastOrDefault();

        var viewModel = new CompareAirQualityViewModel
        {
            CityA = cityA,
            CityB = cityB,
            StartDate = queryStart,
            EndDate = queryEnd,
            AvailableCities = availableCities.Select(c => new SelectListItem
            {
                Value = c,
                Text = c
            }).ToList(),
            LatestCityA = latestA,
            LatestCityB = latestB,
            HistoryCityA = samplesA,
            HistoryCityB = samplesB,
            ErrorMessage = (latestA == null || latestB == null)
                ? "Kon voor één of beide steden geen meetgegevens ophalen uit de backend API."
                : null
        };

        return View(viewModel);
    }
}