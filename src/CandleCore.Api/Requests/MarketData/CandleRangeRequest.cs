using CandleCore.Api.Constants;

namespace CandleCore.Api.Requests.MarketData;

public sealed class CandleRangeRequest
{
    public string Symbol { get; init; } = MarketDefaults.Symbol;
    public string Timeframe { get; init; } = MarketDefaults.Timeframe;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
