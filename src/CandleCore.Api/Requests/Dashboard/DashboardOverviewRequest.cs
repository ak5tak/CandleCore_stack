using CandleCore.Api.Constants;

namespace CandleCore.Api.Requests.Dashboard;

public sealed class DashboardOverviewRequest
{
    public string Symbol { get; init; } = MarketDefaults.Symbol;
    public string Timeframe { get; init; } = MarketDefaults.Timeframe;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int Lookback { get; init; } = MarketDefaults.Lookback;
}
