namespace CandleCore.Api.Infrastructure.Persistence;

public sealed class CandleSeedHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CandleSeedHostedService> _logger;

    public CandleSeedHostedService(
        IServiceScopeFactory scopeFactory,
        ILogger<CandleSeedHostedService> logger
    )
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var seeder = scope.ServiceProvider.GetRequiredService<CandleSeeder>();
            await seeder.SeedAsync(stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Candle seed cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Candle seed failed; API will keep running.");
        }
    }
}
