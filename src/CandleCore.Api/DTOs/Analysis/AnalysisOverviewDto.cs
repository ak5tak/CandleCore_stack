namespace CandleCore.Api.DTOs.Analysis;

public sealed class AnalysisOverviewDto
{
    public MarketSummaryDto? Summary { get; init; }
    public PriceChangeDto? PriceChange { get; init; }
    public RiskAnalysisDto? RiskAnalysis { get; init; }
    public MarketBehaviourDto? MarketBehaviour { get; init; }
    public ProbabilityDto? Probability { get; init; }
}
