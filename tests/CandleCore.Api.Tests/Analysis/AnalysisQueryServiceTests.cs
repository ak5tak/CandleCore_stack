using CandleCore.Api.Requests.Analysis;
using CandleCore.Api.Services.Analysis;
using CandleCore.Api.Tests.Helpers;
using FluentAssertions;

namespace CandleCore.Api.Tests.Analysis;

public class AnalysisQueryServiceTests
{
    [Fact]
    public async Task GetOverviewAsync_Should_Return_Composed_Result_And_Load_Candles_Once()
    {
        await using var dbContext = TestDb.CreateContext();
        await CandleFixtures.SeedAsync(
            dbContext,
            CandleFixtures.Candle(CandleFixtures.Jan1Hour10, 100, 110, 90, 100, 10),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour11, 100, 120, 95, 200, 10),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour12, 100, 130, 95, 300, 10)
        );

        var candleProvider = CountingCandleProvider.Create(dbContext);
        var service = new AnalysisQueryService(candleProvider, new AnalysisCalculator());

        var result = await service.GetOverviewAsync(
            new AnalysisOverviewRequest
            {
                Symbol = CandleFixtures.Symbol,
                Timeframe = CandleFixtures.Interval,
                StartDate = CandleFixtures.Jan1Hour10,
                EndDate = CandleFixtures.Jan1Hour13,
                Lookback = 3,
            }
        );

        candleProvider.LoadCount.Should().Be(1);
        result.Should().NotBeNull();
        result!.Summary!.CandleCount.Should().Be(3);
        result.Summary.LatestClose.Should().Be(300);
        result.PriceChange!.ChangePercent.Should().Be(200);
        result.RiskAnalysis.Should().NotBeNull();
        result.MarketBehaviour.Should().NotBeNull();
        result.Probability.Should().NotBeNull();
    }

    [Fact]
    public async Task GetOverviewAsync_Should_Compute_Drawdown_Behaviour_And_Streaks()
    {
        await using var dbContext = TestDb.CreateContext();
        await CandleFixtures.SeedAsync(
            dbContext,
            CandleFixtures.Candle(CandleFixtures.Jan1Hour10, 100, 120, 95, 110, 10),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour11, 110, 130, 105, 125, 10),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour12, 125, 128, 100, 105, 10),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour13, 105, 108, 90, 95, 10),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour13.AddHours(1), 95, 110, 94, 108, 10)
        );

        var candleProvider = CountingCandleProvider.Create(dbContext);
        var service = new AnalysisQueryService(candleProvider, new AnalysisCalculator());

        var result = await service.GetOverviewAsync(
            new AnalysisOverviewRequest
            {
                Symbol = CandleFixtures.Symbol,
                Timeframe = CandleFixtures.Interval,
                StartDate = CandleFixtures.Jan1Hour10,
                EndDate = CandleFixtures.Jan1Hour13.AddHours(2),
                Lookback = 100,
            }
        );

        candleProvider.LoadCount.Should().Be(1);
        result!.RiskAnalysis!.MaximumDrawdownPercent.Should().BeApproximately(24m, 0.0001m);
        result.MarketBehaviour!.LongestBullishStreakPeriods.Should().Be(2);
        result.MarketBehaviour.LongestBearishStreakPeriods.Should().Be(2);
    }

    [Fact]
    public async Task GetOverviewAsync_Should_Return_Null_When_No_Candles_Exist()
    {
        await using var dbContext = TestDb.CreateContext();
        var candleProvider = CountingCandleProvider.Create(dbContext);
        var service = new AnalysisQueryService(candleProvider, new AnalysisCalculator());

        var result = await service.GetOverviewAsync(
            new AnalysisOverviewRequest
            {
                Symbol = CandleFixtures.Symbol,
                Timeframe = CandleFixtures.Interval,
                StartDate = CandleFixtures.Jan1Hour10,
                EndDate = CandleFixtures.Jan1Hour13,
            }
        );

        candleProvider.LoadCount.Should().Be(1);
        result.Should().BeNull();
    }
}
