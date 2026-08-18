using CandleCore.Api.Constants;

namespace CandleCore.Api.Requests.MarketData;

public sealed class BoundsRequest
{
    public string Symbol { get; init; } = MarketDefaults.Symbol;
    public string Timeframe { get; init; } = MarketDefaults.Timeframe;
}
