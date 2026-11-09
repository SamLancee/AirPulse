using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AirPulse.Data;
using AirPulse.Models;

namespace AirPulse.Services;

public class AirQualityCollectorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AirQualityCollectorService> _logger;
    private readonly TimeSpan _period = TimeSpan.FromHours(1); // Interval voor periodiek pollen

    public AirQualityCollectorService(IServiceProvider serviceProvider, ILogger<AirQualityCollectorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_period);

        // Voer direct één keer uit bij opstarten
        await CollectSamplesAsync();

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await CollectSamplesAsync();
        }
    }

    private async Task CollectSamplesAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var airService = scope.ServiceProvider.GetRequiredService<IAirQualityService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var cities = airService.GetConfiguredCities();

            foreach (var city in cities)
            {
                var cityName = city.Name;
                var data = await airService.GetCurrentAirQualityForCityAsync(cityName);

                if (data?.Current != null)
                {
                    var sample = new AirQualitySample
                    {
                        City = cityName,
                        Timestamp = DateTime.UtcNow,
                        EuropeanAqi = data.Current.EuropeanAqi,
                        Pm25 = data.Current.Pm25,
                        Pm10 = data.Current.Pm10,
                        NitrogenDioxide = data.Current.NitrogenDioxide
                    };

                    dbContext.AirQualitySamples.Add(sample);
                }
            }

            await dbContext.SaveChangesAsync();
            _logger.LogInformation("Luchtkwaliteit samples succesvol opgeslagen.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout opgetreden bij het periodiek opslaan van luchtkwaliteitsdata.");
        }
    }
}