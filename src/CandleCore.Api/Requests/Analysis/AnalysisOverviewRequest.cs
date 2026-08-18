using CandleCore.Api.Constants;

namespace CandleCore.Api.Requests.Analysis;

public sealed class AnalysisOverviewRequest
{
    public string Symbol { get; init; } = MarketDefaults.Symbol;
    public string Timeframe { get; init; } = MarketDefaults.Timeframe;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int Lookback { get; init; } = MarketDefaults.Lookback;
}
