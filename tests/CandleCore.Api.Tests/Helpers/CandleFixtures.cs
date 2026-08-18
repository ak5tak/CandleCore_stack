using CandleCore.Api.Constants;
using CandleCore.Api.Entities;
using CandleCore.Api.Infrastructure.Persistence;

namespace CandleCore.Api.Tests.Helpers;

public static class CandleFixtures
{
    public const string Symbol = "BTCUSDT";
    public const string Interval = "1h";

    public static readonly DateTime Jan1Hour10 = new(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
    public static readonly DateTime Jan1Hour11 = new(2026, 1, 1, 11, 0, 0, DateTimeKind.Utc);
    public static readonly DateTime Jan1Hour12 = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    public static readonly DateTime Jan1Hour13 = new(2026, 1, 1, 13, 0, 0, DateTimeKind.Utc);

    public static readonly DateTime RangeStart = new(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static readonly DateTime RangeEnd = RangeStart.AddHours(5);

    public static Candle Candle(
        DateTime openTime,
        decimal open,
        decimal high,
        decimal low,
        decimal close,
        decimal volume,
        string? symbol = null,
        string? interval = null
    )
    {
        return new Candle
        {
            Id = Guid.NewGuid(),
            Symbol = symbol ?? Symbol,
            Interval = interval ?? Interval,
            OpenTime = openTime,
            CloseTime = openTime.AddHours(1).AddSeconds(-1),
            Open = open,
            High = high,
            Low = low,
            Close = close,
            Volume = volume,
        };
    }

    public static async Task SeedAsync(AppDbContext dbContext, params Candle[] candles)
    {
        dbContext.Candles.AddRange(candles);
        await dbContext.SaveChangesAsync();
    }

    public static Task SeedTwoHourPairAsync(AppDbContext dbContext) =>
        SeedAsync(
            dbContext,
            Candle(Jan1Hour10, 100, 110, 90, 105, 10),
            Candle(Jan1Hour11, 105, 120, 100, 115, 12)
        );
}
