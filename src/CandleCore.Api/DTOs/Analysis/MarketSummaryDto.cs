namespace CandleCore.Api.DTOs.Analysis;

public sealed class MarketSummaryDto
{
    public string Symbol { get; init; } = default!;
    public string Interval { get; init; } = default!;

    public int CandleCount { get; init; }

    public decimal LatestClose { get; init; }
    public decimal AverageClose { get; init; }
    public decimal HighestHigh { get; init; }
    public decimal LowestLow { get; init; }
}
