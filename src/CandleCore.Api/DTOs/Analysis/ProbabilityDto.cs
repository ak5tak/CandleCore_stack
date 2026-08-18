namespace CandleCore.Api.DTOs.Analysis;

public sealed class ProbabilityDto
{
    public string Symbol { get; init; } = default!;
    public string Interval { get; init; } = default!;

    public int Lookback { get; init; }

    public int UpCandles { get; init; }
    public int DownCandles { get; init; }
    public int NeutralCandles { get; init; }

    public decimal ProbabilityUp { get; init; }
    public decimal ProbabilityDown { get; init; }
    public decimal ProbabilityNeutral { get; init; }
}
