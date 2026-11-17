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

        // 2. Bepaal de gekozen stad (default naar Amsterdam of de eerste stad)
        var selectedCityName = string.IsNullOrWhiteSpace(city) 
            ? (configuredCities.FirstOrDefault()?.Name ?? "Amsterdam") 
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
    [HttpGet]
[HttpGet]
public async Task<IActionResult> Vergelijken(string? cityA, string? cityB)
{
    // configuredCities levert List<CityLocation> op
    var configuredCities = _airQualityService.GetConfiguredCities();

    // Haal de .Name (of pas aan naar .City als jouw property zo heet) op
    cityA ??= configuredCities.FirstOrDefault()?.Name ?? "Amsterdam";
    cityB ??= configuredCities.Skip(1).FirstOrDefault()?.Name ?? "Berlijn";

    // Haal beide steden parallel op
    var taskA = _airQualityService.GetCurrentAirQualityForCityAsync(cityA);
    var taskB = _airQualityService.GetCurrentAirQualityForCityAsync(cityB);

    await Task.WhenAll(taskA, taskB);

    var dataA = await taskA;
    var dataB = await taskB;

    var viewModel = new CompareAirQualityViewModel
    {
        CityA = cityA,
        CityB = cityB,
        DataCityA = dataA,
        DataCityB = dataB,
        AvailableCities = configuredCities.Select(c => new SelectListItem
        {
            Value = c.Name, // Gebruik hier ook .Name in plaats van het hele object
            Text = c.Name
        }).ToList(),
        ErrorMessage = (dataA?.Current == null || dataB?.Current == null)
            ? "Kon voor één of beide steden geen actuele meetgegevens ophalen."
            : null
    };

    return View(viewModel);
}
}