using CandleCore.Api.Constants;
using CandleCore.Api.Controllers;
using CandleCore.Api.DTOs.MarketData;
using CandleCore.Api.Services.MarketData;
using CandleCore.Api.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CandleCore.Api.Tests.MarketData;

public class MarketDataControllerTests
{
    [Fact]
    public async Task GetCandles_Should_Return_CandleDto_List()
    {
        await using var dbContext = TestDb.CreateContext();
        var candle = CandleFixtures.Candle(
            CandleFixtures.RangeStart,
            100,
            110,
            90,
            105,
            10,
            interval: SupportedIntervals.BaseInterval
        );
        await CandleFixtures.SeedAsync(dbContext, candle);
        var controller = CreateController(dbContext);

        var result = await controller.GetCandles(
            CandleFixtures.Symbol,
            CandleFixtures.Interval,
            CandleFixtures.RangeStart,
            CandleFixtures.RangeStart.AddHours(2)
        );

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var candles = okResult.Value.Should().BeAssignableTo<List<CandleDto>>().Subject;
        candles.Should().ContainSingle();
        candles[0].Close.Should().Be(105);
    }

    [Fact]
    public async Task GetBounds_Should_Return_DatasetBoundsDto_When_Candles_Exist()
    {
        await using var dbContext = TestDb.CreateContext();
        await CandleFixtures.SeedAsync(
            dbContext,
            CandleFixtures.Candle(
                CandleFixtures.RangeStart,
                100,
                110,
                90,
                105,
                10,
                interval: SupportedIntervals.BaseInterval
            )
        );
        var controller = CreateController(dbContext);

        var result = await controller.GetBounds(CandleFixtures.Symbol, CandleFixtures.Interval);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var bounds = okResult.Value.Should().BeOfType<DatasetBoundsDto>().Subject;
        bounds.CandleCount.Should().Be(1);
        bounds.FirstOpenTime.Should().Be(CandleFixtures.RangeStart);
        bounds.LastOpenTime.Should().Be(CandleFixtures.RangeStart);
    }

    [Fact]
    public async Task GetBounds_Should_Return_Ok_Null_When_No_Candles()
    {
        await using var dbContext = TestDb.CreateContext();
        var controller = CreateController(dbContext);

        var result = await controller.GetBounds(CandleFixtures.Symbol, CandleFixtures.Interval);

        var json = result.Should().BeOfType<JsonResult>().Subject;
        json.Value.Should().BeNull();
    }

    private static MarketDataController CreateController(
        CandleCore.Api.Infrastructure.Persistence.AppDbContext dbContext
    )
    {
        var candleQueryService = new CandleQueryService(dbContext);
        var candleProvider = new CandleProvider(
            candleQueryService,
            new CandleAggregationService()
        );
        var marketDataQueryService = new MarketDataQueryService(candleProvider, candleQueryService);
        return new MarketDataController(marketDataQueryService);
    }
}
