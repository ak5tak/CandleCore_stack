namespace CandleCore.Api.DTOs.Analysis;

public sealed class RiskAnalysisDto
{
    public string Symbol { get; init; } = default!;
    public string Interval { get; init; } = default!;

    public int Lookback { get; init; }

    public decimal AverageClose { get; init; }
    public decimal Volatility { get; init; }
    public decimal MaximumDrawdownPercent { get; init; }
}
