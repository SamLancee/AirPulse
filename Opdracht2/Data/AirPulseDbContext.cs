using AirPulse_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AirPulse_API.Data;

public class AirPulseDbContext : DbContext
{
    public AirPulseDbContext(DbContextOptions<AirPulseDbContext> options) : base(options) { }

    public DbSet<AirSample> AirSamples => Set<AirSample>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AirSample>()
            .HasIndex(s => new { s.City, s.Timestamp })
            .IsUnique();
    }
}