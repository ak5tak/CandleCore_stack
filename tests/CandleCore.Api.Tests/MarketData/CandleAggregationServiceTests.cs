using CandleCore.Api.Entities;
using CandleCore.Api.Services.MarketData;
using CandleCore.Api.Tests.Helpers;
using FluentAssertions;

namespace CandleCore.Api.Tests.MarketData;

public class CandleAggregationServiceTests
{
    private readonly CandleAggregationService _sut = new();
    private static readonly DateTime Jan1 = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Aggregate_Should_Create_4h_Candle_From_1h_Candles()
    {
        var candles = new List<Candle>
        {
            CandleFixtures.Candle(Jan1.AddHours(0), 100, 110, 90, 105, 10),
            CandleFixtures.Candle(Jan1.AddHours(1), 105, 120, 95, 115, 20),
            CandleFixtures.Candle(Jan1.AddHours(2), 115, 125, 100, 120, 30),
            CandleFixtures.Candle(Jan1.AddHours(3), 120, 130, 80, 125, 40),
        };

        var result = _sut.Aggregate(candles, "4h");

        result.Should().ContainSingle();
        var aggregated = result.Single();
        aggregated.Open.Should().Be(100);
        aggregated.High.Should().Be(130);
        aggregated.Low.Should().Be(80);
        aggregated.Close.Should().Be(125);
        aggregated.Volume.Should().Be(100);
    }

    [Fact]
    public void Aggregate_Should_Create_1d_Candle_From_Representative_1h_Candles()
    {
        var candles = new List<Candle>
        {
            CandleFixtures.Candle(Jan1.AddHours(0), 100, 110, 90, 105, 10),
            CandleFixtures.Candle(Jan1.AddHours(12), 105, 150, 95, 140, 20),
            CandleFixtures.Candle(Jan1.AddHours(23), 140, 145, 80, 130, 30),
        };

        var result = _sut.Aggregate(candles, "1d");

        result.Should().ContainSingle();
        result.Single().High.Should().Be(150);
        result.Single().Low.Should().Be(80);
        result.Single().Close.Should().Be(130);
    }

    [Fact]
    public void Aggregate_Should_Create_1w_Candle_From_1h_Candles()
    {
        var monday = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc);
        var candles = new List<Candle>
        {
            CandleFixtures.Candle(monday, 100, 110, 90, 105, 10),
            CandleFixtures.Candle(monday.AddDays(2).AddHours(12), 105, 150, 95, 140, 20),
            CandleFixtures.Candle(monday.AddDays(6).AddHours(23), 140, 145, 80, 130, 30),
        };

        var result = _sut.Aggregate(candles, "1w");

        result.Should().ContainSingle();
        result.Single().Volume.Should().Be(60);
    }

    [Fact]
    public void Aggregate_Should_Create_1M_Candle_From_1h_Candles()
    {
        var candles = new List<Candle>
        {
            CandleFixtures.Candle(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), 100, 110, 90, 105, 10),
            CandleFixtures.Candle(new DateTime(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc), 105, 160, 95, 150, 20),
            CandleFixtures.Candle(new DateTime(2026, 1, 31, 23, 0, 0, DateTimeKind.Utc), 150, 155, 80, 130, 30),
        };

        var result = _sut.Aggregate(candles, "1M");

        result.Should().ContainSingle();
        result.Single().High.Should().Be(160);
        result.Single().Volume.Should().Be(60);
    }

    [Fact]
    public void Aggregate_Should_Return_Empty_When_No_Candles()
    {
        _sut.Aggregate(new List<Candle>(), "4h").Should().BeEmpty();
    }

    [Fact]
    public void Aggregate_Should_Throw_When_TargetInterval_Is_Unsupported()
    {
        var candles = new List<Candle> { CandleFixtures.Candle(Jan1, 100, 110, 90, 105, 10) };

        var act = () => _sut.Aggregate(candles, "2h");

        act.Should().Throw<ArgumentException>().WithMessage("Unsupported interval: 2h");
    }
}
