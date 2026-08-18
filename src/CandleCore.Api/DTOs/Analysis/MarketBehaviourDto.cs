namespace CandleCore.Api.DTOs.Analysis;

public sealed class MarketBehaviourDto
{
    public string Symbol { get; init; } = default!;
    public string Interval { get; init; } = default!;

    public int Lookback { get; init; }

    public decimal AverageCandleRange { get; init; }

    public int LongestBullishStreakPeriods { get; init; }
    public decimal LongestBullishStreakReturnPercent { get; init; }

    public int LongestBearishStreakPeriods { get; init; }
    public decimal LongestBearishStreakReturnPercent { get; init; }
}
