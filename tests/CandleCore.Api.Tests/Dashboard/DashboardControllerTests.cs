using CandleCore.Api.Controllers;
using CandleCore.Api.DTOs.Dashboard;
using CandleCore.Api.Services.Dashboard;
using CandleCore.Api.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CandleCore.Api.Tests.Dashboard;

public class DashboardControllerTests
{
    [Fact]
    public async Task GetOverview_Should_Return_Composed_Result_For_Valid_Request()
    {
        await using var dbContext = TestDb.CreateContext();
        await CandleFixtures.SeedTwoHourPairAsync(dbContext);
        var controller = CreateController(dbContext);

        var result = await controller.GetOverview(
            CandleFixtures.Symbol,
            CandleFixtures.Interval,
            CandleFixtures.Jan1Hour10,
            CandleFixtures.Jan1Hour12,
            lookback: 2
        );

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var overview = okResult.Value.Should().BeOfType<DashboardOverviewDto>().Subject;
        overview.Symbol.Should().Be(CandleFixtures.Symbol);
        overview.CandleCount.Should().Be(2);
        overview.LatestClose.Should().Be(115);
        overview.ChangePercent.Should().NotBeNull();
    }

    [Fact]
    public async Task GetOverview_Should_Return_Ok_Null_When_No_Candles_Exist()
    {
        await using var dbContext = TestDb.CreateContext();
        var controller = CreateController(dbContext);

        var result = await controller.GetOverview(
            CandleFixtures.Symbol,
            CandleFixtures.Interval,
            CandleFixtures.Jan1Hour10,
            CandleFixtures.Jan1Hour12
        );

        var json = result.Should().BeOfType<JsonResult>().Subject;
        json.Value.Should().BeNull();
    }

    private static DashboardController CreateController(
        CandleCore.Api.Infrastructure.Persistence.AppDbContext dbContext
    )
    {
        var candleProvider = CountingCandleProvider.Create(dbContext);
        return new DashboardController(new DashboardQueryService(candleProvider));
    }
}
