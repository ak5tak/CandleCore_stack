using CandleCore.Api.DTOs.Analysis;
using CandleCore.Api.Requests.Analysis;
using CandleCore.Api.Services.MarketData;

namespace CandleCore.Api.Services.Analysis;

public sealed class AnalysisQueryService : IAnalysisQueryService
{
    private readonly CandleProvider _candleProvider;
    private readonly AnalysisCalculator _calculator;

    public AnalysisQueryService(CandleProvider candleProvider, AnalysisCalculator calculator)
    {
        _candleProvider = candleProvider;
        _calculator = calculator;
    }

    public async Task<AnalysisOverviewDto?> GetOverviewAsync(
        AnalysisOverviewRequest request,
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

        return _calculator.BuildOverview(
            candles,
            request.Symbol,
            request.Timeframe,
            request.Lookback
        );
    }
}
