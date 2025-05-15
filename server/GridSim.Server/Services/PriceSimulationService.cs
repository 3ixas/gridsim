using System;
using System.Threading;
using System.Threading.Tasks;

namespace GridSim.Server.Services;

public class PriceSimulationService : BackgroundService
{
    private readonly TimeProvider _timeProvider;
    private readonly Random _random = new();
    public decimal CurrentPrice { get; private set; } = 40m;

    public PriceSimulationService()
    {
        _timeProvider = TimeProvider.System;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = _timeProvider.GetUtcNow().LocalDateTime;
            var basePrice = GetTimeBasedPrice(now);
            var randomNoise = (decimal)(_random.NextDouble() * 2 - 1); // ±1
            CurrentPrice = Math.Round(basePrice + randomNoise, 2);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private decimal GetTimeBasedPrice(DateTime time)
    {
        var hour = time.Hour;

        return hour switch
        {
            >= 6 and <= 9 => 45m,  // Morning peak
            >= 17 and <= 21 => 50m, // Evening peak
            >= 0 and <= 5 => 35m,   // Night dip
            >= 10 and <= 16 => 38m, // Day low
            _ => 40m                // Default
        };
    }
}