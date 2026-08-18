using CandleCore.Api.Requests.Dashboard;
using CandleCore.Api.Services.Dashboard;
using CandleCore.Api.Tests.Helpers;
using FluentAssertions;

namespace CandleCore.Api.Tests.Dashboard;

public class DashboardQueryServiceTests
{
    [Fact]
    public async Task GetOverviewAsync_Should_Return_Composed_Result_And_Load_Candles_Once()
    {
        await using var dbContext = TestDb.CreateContext();
        await CandleFixtures.SeedAsync(
            dbContext,
            CandleFixtures.Candle(CandleFixtures.Jan1Hour10, 100, 110, 90, 100, 10),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour11, 100, 120, 95, 200, 12),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour12, 100, 130, 95, 300, 8)
        );

        var candleProvider = CountingCandleProvider.Create(dbContext);
        var service = new DashboardQueryService(candleProvider);

        var result = await service.GetOverviewAsync(
            new DashboardOverviewRequest
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
        result!.CandleCount.Should().Be(3);
        result.LatestClose.Should().Be(300);
        result.ChangePercent.Should().Be(200);
        result.TotalVolume.Should().Be(30);
        result.AthDistancePercent.Should().BeApproximately(((300m - 130m) / 130m) * 100, 0.0001m);
    }

    [Fact]
    public async Task GetOverviewAsync_Should_Return_Null_When_No_Candles_Exist()
    {
        await using var dbContext = TestDb.CreateContext();
        var candleProvider = CountingCandleProvider.Create(dbContext);
        var service = new DashboardQueryService(candleProvider);

        var result = await service.GetOverviewAsync(
            new DashboardOverviewRequest
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

    [Fact]
    public async Task GetOverviewAsync_Should_Return_Null_ChangePercent_When_Insufficient_Candles()
    {
        await using var dbContext = TestDb.CreateContext();
        await CandleFixtures.SeedAsync(
            dbContext,
            CandleFixtures.Candle(CandleFixtures.Jan1Hour10, 100, 110, 90, 105, 10)
        );

        var candleProvider = CountingCandleProvider.Create(dbContext);
        var service = new DashboardQueryService(candleProvider);

        var result = await service.GetOverviewAsync(
            new DashboardOverviewRequest
            {
                Symbol = CandleFixtures.Symbol,
                Timeframe = CandleFixtures.Interval,
                StartDate = CandleFixtures.Jan1Hour10,
                EndDate = CandleFixtures.Jan1Hour11,
                Lookback = 100,
            }
        );

        result.Should().NotBeNull();
        result!.ChangePercent.Should().BeNull();
        result.CandleCount.Should().Be(1);
    }
}
