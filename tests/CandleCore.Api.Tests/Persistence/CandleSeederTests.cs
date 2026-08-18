using CandleCore.Api.Constants;
using CandleCore.Api.Infrastructure.Binance;
using CandleCore.Api.Infrastructure.Persistence;
using CandleCore.Api.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CandleCore.Api.Tests.Persistence;

public class CandleSeederTests
{
    [Fact]
    public async Task SeedAsync_Should_Insert_Candles_Ordered_By_OpenTime()
    {
        await using var dbContext = TestDb.CreateContext();
        var fakeClient = new HistoricalFakeBinanceClient(historyCount: 5);
        var seeder = CreateSeeder(dbContext, fakeClient, seedCandleCount: 10);

        await seeder.SeedAsync();

        var candles = await dbContext.Candles.OrderBy(x => x.OpenTime).ToListAsync();
        candles.Should().HaveCount(5);
        candles.Select(x => x.Symbol).Should().OnlyContain(s => s == MarketDefaults.Symbol);
        candles.Select(x => x.Interval).Should().OnlyContain(i => i == MarketDefaults.Timeframe);
        candles.Should().BeInAscendingOrder(x => x.OpenTime);
        candles.Min(x => x.OpenTime).Should().Be(CandleFixtures.RangeStart);
        fakeClient.CallCount.Should().Be(1);
        fakeClient.RequestedEndTimes.Should().Equal((long?)null);
    }

    [Fact]
    public async Task SeedAsync_Should_Paginate_Backward_With_EndTime()
    {
        await using var dbContext = TestDb.CreateContext();
        var fakeClient = new HistoricalFakeBinanceClient(historyCount: 2500);
        var seeder = CreateSeeder(dbContext, fakeClient, seedCandleCount: 2500);

        await seeder.SeedAsync();

        var candles = await dbContext.Candles.OrderBy(x => x.OpenTime).ToListAsync();
        candles.Should().HaveCount(2500);
        candles.Should().BeInAscendingOrder(x => x.OpenTime);
        fakeClient.CallCount.Should().Be(3);
        fakeClient.RequestedEndTimes[0].Should().BeNull();

        var firstPageOldestMs = new DateTimeOffset(candles[1500].OpenTime).ToUnixTimeMilliseconds();
        fakeClient.RequestedEndTimes[1].Should().Be(firstPageOldestMs - 1);

        var secondPageOldestMs = new DateTimeOffset(candles[500].OpenTime).ToUnixTimeMilliseconds();
        fakeClient.RequestedEndTimes[2].Should().Be(secondPageOldestMs - 1);
    }

    [Fact]
    public async Task SeedAsync_Should_Cap_Inserts_At_SeedCandleCount()
    {
        await using var dbContext = TestDb.CreateContext();
        var fakeClient = new HistoricalFakeBinanceClient(historyCount: 12);
        var seeder = CreateSeeder(dbContext, fakeClient, seedCandleCount: 5);

        await seeder.SeedAsync();

        var candles = await dbContext.Candles.OrderBy(x => x.OpenTime).ToListAsync();
        candles.Should().HaveCount(5);
        candles.Should().BeInAscendingOrder(x => x.OpenTime);
        fakeClient.CallCount.Should().Be(1);
    }

    [Fact]
    public async Task SeedAsync_Should_Resume_When_Partial_Seed_Exists()
    {
        await using var dbContext = TestDb.CreateContext();
        var fakeClient = new HistoricalFakeBinanceClient(historyCount: 5);
        await CandleFixtures.SeedAsync(
            dbContext,
            CandleFixtures.Candle(CandleFixtures.RangeStart, 100, 110, 90, 105, 10),
            CandleFixtures.Candle(CandleFixtures.RangeStart.AddHours(1), 100, 110, 90, 105, 10)
        );
        var seeder = CreateSeeder(dbContext, fakeClient, seedCandleCount: 5);

        await seeder.SeedAsync();

        var candles = await dbContext.Candles.OrderBy(x => x.OpenTime).ToListAsync();
        candles.Should().HaveCount(5);
        candles.Select(x => x.OpenTime).Should().OnlyHaveUniqueItems();
        candles.Min(x => x.OpenTime).Should().Be(CandleFixtures.RangeStart);
        fakeClient.CallCount.Should().Be(1);
    }

    [Fact]
    public async Task SeedAsync_Should_Fetch_Only_Remaining_Pages_On_Resume()
    {
        await using var dbContext = TestDb.CreateContext();
        var existing = Enumerable
            .Range(0, 2000)
            .Select(i =>
                CandleFixtures.Candle(CandleFixtures.RangeStart.AddHours(i), 100, 110, 90, 105, 10)
            )
            .ToArray();
        await CandleFixtures.SeedAsync(dbContext, existing);
        var fakeClient = new HistoricalFakeBinanceClient(historyCount: 2500);
        var seeder = CreateSeeder(dbContext, fakeClient, seedCandleCount: 2500);

        await seeder.SeedAsync();

        fakeClient.CallCount.Should().Be(1);
        var candles = await dbContext.Candles.ToListAsync();
        candles.Should().HaveCount(2500);
        candles.Select(x => x.OpenTime).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task SeedAsync_Should_Fetch_When_Count_Is_Below_SeedCandleCount()
    {
        await using var dbContext = TestDb.CreateContext();
        await CandleFixtures.SeedTwoHourPairAsync(dbContext);
        var fakeClient = new HistoricalFakeBinanceClient(historyCount: 5);
        var seeder = CreateSeeder(dbContext, fakeClient, seedCandleCount: 5);

        await seeder.SeedAsync();

        fakeClient.CallCount.Should().Be(1);
        var candles = await dbContext.Candles.ToListAsync();
        candles.Should().HaveCount(5);
        candles.Select(x => x.OpenTime).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task SeedAsync_Should_Not_Fetch_When_SeedCandleCount_Reached()
    {
        await using var dbContext = TestDb.CreateContext();
        await CandleFixtures.SeedAsync(
            dbContext,
            CandleFixtures.Candle(CandleFixtures.Jan1Hour10, 100, 110, 90, 105, 10),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour11, 105, 120, 100, 115, 12),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour12, 115, 125, 110, 120, 11),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour13, 120, 130, 115, 125, 9),
            CandleFixtures.Candle(CandleFixtures.Jan1Hour13.AddHours(1), 125, 135, 120, 130, 8)
        );
        var fakeClient = new HistoricalFakeBinanceClient();
        var seeder = CreateSeeder(dbContext, fakeClient, seedCandleCount: 5);

        await seeder.SeedAsync();

        fakeClient.CallCount.Should().Be(0);
        (await dbContext.Candles.CountAsync()).Should().Be(5);
    }

    [Fact]
    public async Task SeedAsync_Should_Insert_Nothing_When_Binance_Returns_Empty()
    {
        await using var dbContext = TestDb.CreateContext();
        var fakeClient = new EmptyHistoricalFakeBinanceClient();
        var seeder = CreateSeeder(dbContext, fakeClient);

        await seeder.SeedAsync();

        fakeClient.CallCount.Should().Be(1);
        (await dbContext.Candles.ToListAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task SeedAsync_Should_Not_Throw_When_Binance_Fails()
    {
        await using var dbContext = TestDb.CreateContext();
        var seeder = CreateSeeder(dbContext, new ThrowingFakeBinanceClient());

        var act = async () => await seeder.SeedAsync();

        await act.Should().NotThrowAsync();
        (await dbContext.Candles.ToListAsync()).Should().BeEmpty();
    }

    private static CandleSeeder CreateSeeder(
        AppDbContext dbContext,
        IBinanceClient binanceClient,
        int seedCandleCount = 10
    )
    {
        return new CandleSeeder(
            dbContext,
            binanceClient,
            NullLogger<CandleSeeder>.Instance,
            seedCandleCount
        );
    }
}
