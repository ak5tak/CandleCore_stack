using CandleCore.Api.DTOs.Dashboard;
using CandleCore.Api.Requests.Dashboard;
using CandleCore.Api.Services.MarketData;

namespace CandleCore.Api.Services.Dashboard;

public sealed class DashboardQueryService : IDashboardQueryService
{
    private readonly CandleProvider _candleProvider;

    public DashboardQueryService(CandleProvider candleProvider)
    {
        _candleProvider = candleProvider;
    }

    public async Task<DashboardOverviewDto?> GetOverviewAsync(
        DashboardOverviewRequest request,
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

        if (candles.Count == 0)
        {
            return null;
        }

        var summary = CandleMetrics.TrySummary(candles)!.Value;
        var priceChange = CandleMetrics.TryPriceChange(candles, request.Lookback);

        // First/Last use OpenTime on the provider-ordered ascending list.
        return new DashboardOverviewDto
        {
            Symbol = request.Symbol,
            Interval = request.Timeframe,
            CandleCount = summary.CandleCount,
            LatestClose = summary.LatestClose,
            HighestHigh = summary.HighestHigh,
            LowestLow = summary.LowestLow,
            AthDistancePercent =
                summary.HighestHigh == 0
                    ? 0
                    : ((summary.LatestClose - summary.HighestHigh) / summary.HighestHigh) * 100,
            ChangePercent = priceChange?.ChangePercent,
            TotalVolume = candles.Sum(c => c.Volume),
            FirstCandleTime = candles[0].OpenTime,
            LastCandleTime = candles[^1].OpenTime,
        };
    }
}
