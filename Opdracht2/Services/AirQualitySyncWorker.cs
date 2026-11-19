using AirPulse_API.Models;
using Microsoft.Extensions.Options;
using AirPulse.Shared;

namespace AirPulse_API.Services;

public class AirQualitySyncWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly AirPulseSettings _settings;
    private readonly ILogger<AirQualitySyncWorker> _logger;

    public AirQualitySyncWorker(
        IServiceScopeFactory scopeFactory,
        IOptions<AirPulseSettings> settings,
        ILogger<AirQualitySyncWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AirQualitySyncWorker gestart. Interval: {Minutes} minuten.", _settings.FetchIntervalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            await SyncAllCitiesAsync(stoppingToken);

            var delay = TimeSpan.FromMinutes(Math.Max(1, _settings.FetchIntervalMinutes));
            await Task.Delay(delay, stoppingToken);
        }
    }

    private async Task SyncAllCitiesAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var airService = scope.ServiceProvider.GetRequiredService<IAirQualityService>();

        foreach (var city in _settings.Cities)
        {
            try
            {
                await airService.SyncCityDataAsync(city, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij synchroniseren van stad {City}", city.Name);
            }
        }
    }
}