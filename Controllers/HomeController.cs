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
    public async Task<IActionResult> Index(string? city)
    {
        // 1. Haal de 8 steden op uit de configuratie
        var configuredCities = _airQualityService.GetConfiguredCities();

        // 2. Bepaal de gekozen stad (default naar Eindhoven of de eerste stad)
        var selectedCityName = string.IsNullOrWhiteSpace(city) 
            ? (configuredCities.FirstOrDefault()?.Name ?? "Eindhoven") 
            : city;

        // 3. Haal live data op voor de geselecteerde stad
        var airData = await _airQualityService.GetCurrentAirQualityForCityAsync(selectedCityName);

        // 4. Bouw het ViewModel met de dropdown opties
        var viewModel = new AirQualityView
        {
            SelectedCity = selectedCityName,
            AvailableCities = configuredCities.Select(c => new SelectListItem
            {
                Value = c.Name,
                Text = c.Name,
                Selected = c.Name.Equals(selectedCityName, StringComparison.OrdinalIgnoreCase)
            }).ToList(),
            CurrentAirQuality = airData,
            ErrorMessage = airData?.Current == null 
                ? $"Kon geen actuele gegevens ophalen voor {selectedCityName}." 
                : null
        };

        return View(viewModel);
    }
}