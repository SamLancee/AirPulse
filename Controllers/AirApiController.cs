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

    [HttpGet("{city}")]
    public async Task<IActionResult> GetAirQualityByCity(
    string city, 
    [FromQuery] DateTime? start, 
    [FromQuery] DateTime? end)
    {
    var startDate = start ?? DateTime.UtcNow.AddDays(-7);
    var endDate = end ?? startDate.AddDays(7);

    // 1. Haal alle ruwe samples binnen het tijdvak op
    var rawSamples = await _context.AirQualitySamples
        .Where(s => s.City.ToLower() == city.ToLower() 
                 && s.Timestamp >= startDate 
                 && s.Timestamp <= endDate)
        .ToListAsync();

    // 2. Groepeer op dag + uur en pak per uur het LAATSTE (meest recente) meetpunt
    var hourlySamples = rawSamples
        .GroupBy(s => new { s.Timestamp.Date, s.Timestamp.Hour })
        .Select(group => group.OrderByDescending(s => s.Timestamp).First())
        .OrderBy(s => s.Timestamp)
        .Select(s => new
        {
            // Toon netjes het hele uur in lokale tijd (bijv. "12-11 09:00")
            time = s.Timestamp.ToLocalTime().ToString("dd-MM HH:00"),
            aqi = s.EuropeanAqi,
            pm25 = s.Pm25,
            pm10 = s.Pm10,
            no2 = s.NitrogenDioxide
        })
        .ToList();

    return Ok(new
    {
        city = city,
        samples = hourlySamples
    });
}
}