using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirPulse.Data;

namespace AirPulse.Controllers;

[ApiController]
[Route("api/air")]
public class AirApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public AirApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /api/air/{city}?start=2026-11-01&end=2026-11-08
    [HttpGet("{city}")]
    public async Task<IActionResult> GetAirQualityByCity(
        string city, 
        [FromQuery] DateTime? start, 
        [FromQuery] DateTime? end)
    {
        // 1. Bepaal startdatum (default naar 7 dagen geleden als start leeg is)
        var startDate = start ?? DateTime.UtcNow.AddDays(-7);

        // 2. Default eis: als 'end' niet is meegegeven, is end = start + 7 dagen
        var endDate = end ?? startDate.AddDays(7);

        // 3. Query uitvoeren op de opgeslagen samples
        var samples = await _context.AirQualitySamples
            .Where(s => s.City.ToLower() == city.ToLower() 
                     && s.Timestamp >= startDate 
                     && s.Timestamp <= endDate)
            .OrderBy(s => s.Timestamp)
            .Select(s => new
            {
                s.City,
                s.Timestamp,
                s.EuropeanAqi,
                s.Pm25,
                s.Pm10,
                s.NitrogenDioxide
            })
            .ToListAsync();

        if (!samples.Any())
        {
            return NotFound(new { message = $"Geen meetwaarden gevonden voor {city} binnen de opgegeven periode." });
        }

        return Ok(new
        {
            City = city,
            StartDate = startDate,
            EndDate = endDate,
            Count = samples.Count,
            Samples = samples
        });
    }
}