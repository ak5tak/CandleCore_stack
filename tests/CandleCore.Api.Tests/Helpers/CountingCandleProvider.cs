using CandleCore.Api.Entities;
using CandleCore.Api.Infrastructure.Persistence;
using CandleCore.Api.Services.MarketData;

namespace CandleCore.Api.Tests.Helpers;

public sealed class CountingCandleProvider : CandleProvider
{
    public int LoadCount { get; private set; }

    public CountingCandleProvider(
        CandleQueryService candleQueryService,
        CandleAggregationService aggregationService
    )
        : base(candleQueryService, aggregationService) { }

    public static CountingCandleProvider Create(AppDbContext dbContext)
    {
        var candleQueryService = new CandleQueryService(dbContext);
        var aggregationService = new CandleAggregationService();
        return new CountingCandleProvider(candleQueryService, aggregationService);
    }

    public override Task<List<Candle>> GetCandlesAsync(
        string symbol,
        string timeframe,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default
    )
    {
        LoadCount++;
        return base.GetCandlesAsync(symbol, timeframe, startDate, endDate, cancellationToken);
    }
}
