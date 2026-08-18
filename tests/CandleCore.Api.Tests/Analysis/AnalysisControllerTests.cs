using CandleCore.Api.Controllers;
using CandleCore.Api.DTOs.Analysis;
using CandleCore.Api.Services.Analysis;
using CandleCore.Api.Services.MarketData;
using CandleCore.Api.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CandleCore.Api.Tests.Analysis;

public class AnalysisControllerTests
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
        var overview = okResult.Value.Should().BeOfType<AnalysisOverviewDto>().Subject;
        overview.Summary.Should().NotBeNull();
        overview.PriceChange.Should().NotBeNull();
        overview.MarketBehaviour.Should().NotBeNull();
        overview.RiskAnalysis.Should().NotBeNull();
        overview.Probability.Should().NotBeNull();
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

    [Fact]
    public async Task GetOverview_Should_Return_BadRequest_For_Invalid_Lookback()
    {
        await using var dbContext = TestDb.CreateContext();
        var controller = CreateController(dbContext);

        var result = await controller.GetOverview(
            CandleFixtures.Symbol,
            CandleFixtures.Interval,
            CandleFixtures.Jan1Hour10,
            CandleFixtures.Jan1Hour12,
            lookback: 0
        );

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    private static AnalysisController CreateController(
        CandleCore.Api.Infrastructure.Persistence.AppDbContext dbContext
    )
    {
        var candleProvider = CountingCandleProvider.Create(dbContext);
        var analysisQueryService = new AnalysisQueryService(
            candleProvider,
            new AnalysisCalculator()
        );
        return new AnalysisController(analysisQueryService);
    }
}
