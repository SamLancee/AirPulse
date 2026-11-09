using Microsoft.EntityFrameworkCore;
using AirPulse.Models;

namespace AirPulse.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<AirQualitySample> AirQualitySamples => Set<AirQualitySample>();
}