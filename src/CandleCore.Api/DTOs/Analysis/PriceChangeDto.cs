namespace CandleCore.Api.DTOs.Analysis;

public sealed class PriceChangeDto
{
    public string Symbol { get; init; } = default!;
    public string Interval { get; init; } = default!;

    public int Lookback { get; init; }

    public decimal OldestClose { get; init; }
    public decimal LatestClose { get; init; }

    public decimal ChangePercent { get; init; }
}
