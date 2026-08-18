using CandleCore.Api.Constants;
using CandleCore.Api.Entities;

namespace CandleCore.Api.Services.MarketData;

public class CandleProvider
{
    private readonly CandleQueryService _candleQueryService;
    private readonly CandleAggregationService _aggregationService;

    public CandleProvider(
        CandleQueryService candleQueryService,
        CandleAggregationService aggregationService
    )
    {
        _candleQueryService = candleQueryService;
        _aggregationService = aggregationService;
    }

    public virtual async Task<List<Candle>> GetCandlesAsync(
        string symbol,
        string timeframe,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default
    )
    {
        var baseCandles = await _candleQueryService.GetCandlesAsync(
            symbol,
            SupportedIntervals.BaseInterval,
            startDate,
            endDate,
            cancellationToken
        );

        if (timeframe == SupportedIntervals.BaseInterval)
        {
            return baseCandles;
        }

        return _aggregationService.Aggregate(baseCandles, timeframe);
    }
}
