using CandleCore.Api.Constants;
using CandleCore.Api.DTOs.MarketData;
using CandleCore.Api.Requests.MarketData;

namespace CandleCore.Api.Services.MarketData;

public sealed class MarketDataQueryService : IMarketDataQueryService
{
    private readonly CandleProvider _candleProvider;
    private readonly CandleQueryService _candleQueryService;

    public MarketDataQueryService(
        CandleProvider candleProvider,
        CandleQueryService candleQueryService
    )
    {
        _candleProvider = candleProvider;
        _candleQueryService = candleQueryService;
    }

    public async Task<IReadOnlyList<CandleDto>> GetCandlesAsync(
        CandleRangeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var candles = await _candleProvider.GetCandlesAsync(
            request.Symbol,
            request.Timeframe,
            request.StartDate,
            request.EndDate,
            cancellationToken
        );

        return candles.Select(CandleDto.FromEntity).ToList();
    }

    public async Task<DatasetBoundsDto?> GetBoundsAsync(
        BoundsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var bounds = await _candleQueryService.GetBoundsAsync(
            request.Symbol,
            SupportedIntervals.BaseInterval,
            cancellationToken
        );

        if (bounds is null)
        {
            return null;
        }

        var (first, last, count) = bounds.Value;

        return new DatasetBoundsDto
        {
            Symbol = request.Symbol,
            Interval = SupportedIntervals.BaseInterval,
            FirstOpenTime = first,
            LastOpenTime = last,
            CandleCount = count,
        };
    }
}
