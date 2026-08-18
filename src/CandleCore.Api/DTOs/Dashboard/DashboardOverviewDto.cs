namespace CandleCore.Api.DTOs.Dashboard;

public sealed class DashboardOverviewDto
{
    public string Symbol { get; init; } = default!;
    public string Interval { get; init; } = default!;
    public int CandleCount { get; init; }
    public decimal LatestClose { get; init; }
    public decimal HighestHigh { get; init; }
    public decimal LowestLow { get; init; }
    public decimal AthDistancePercent { get; init; }
    public decimal? ChangePercent { get; init; }
    public decimal TotalVolume { get; init; }
    public DateTime? FirstCandleTime { get; init; }
    public DateTime? LastCandleTime { get; init; }
}
