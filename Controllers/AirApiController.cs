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

        var samples = await _context.AirQualitySamples
            .Where(s => s.City.ToLower() == city.ToLower() 
                     && s.Timestamp >= startDate 
                     && s.Timestamp <= endDate)
            .OrderBy(s => s.Timestamp)
            .Select(s => new
            {
                time = s.Timestamp.ToString("dd-MM HH:mm"),
                aqi = s.EuropeanAqi,
                pm25 = s.Pm25,
                pm10 = s.Pm10,
                no2 = s.NitrogenDioxide
            })
            .ToListAsync();

        return Ok(new
        {
            city = city,
            samples = samples
        });
    }
}