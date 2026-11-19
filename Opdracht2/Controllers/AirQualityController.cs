using AirPulse_API.Models;
using AirPulse_API.Services;
using AirPulse.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AirPulse_API.Controllers;

[ApiController]
[Route("api/air")]
public class AirQualityController : ControllerBase
{
    private readonly IAirQualityService _airQualityService;
    private readonly AirPulseSettings _settings;

    public AirQualityController(IAirQualityService airQualityService, IOptions<AirPulseSettings> settings)
    {
        _airQualityService = airQualityService;
        _settings = settings.Value;
    }

    [HttpGet("cities")]
    public ActionResult<IEnumerable<string>> GetCities()
    {
        return Ok(_settings.Cities.Select(c => c.Name));
    }

    [HttpGet("{city}")]
    public async Task<ActionResult<List<AirSample>>> GetAirQuality(
        string city,
        [FromQuery] DateTime? start,
        [FromQuery] DateTime? end)
    {
        var samples = await _airQualityService.GetSamplesAsync(city, start, end);

        if (samples.Count == 0)
        {
            return NotFound(new { message = $"Geen metingen gevonden voor '{city}' in de opgegeven periode." });
        }

        return Ok(samples);
    }
}