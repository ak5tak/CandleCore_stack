namespace CandleCore.Api.DTOs.MarketData;

public sealed class DatasetBoundsDto
{
    public string Symbol { get; init; } = default!;
    public string Interval { get; init; } = default!;
    public DateTime FirstOpenTime { get; init; }
    public DateTime LastOpenTime { get; init; }
    public int CandleCount { get; init; }
}
