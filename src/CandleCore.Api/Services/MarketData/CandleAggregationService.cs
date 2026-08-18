using CandleCore.Api.Constants;
using CandleCore.Api.Entities;

namespace CandleCore.Api.Services.MarketData;

public class CandleAggregationService
{
    public List<Candle> Aggregate(List<Candle> candles, string targetInterval)
    {
        if (candles.Count == 0)
        {
            return [];
        }

        var sortedCandles = candles.OrderBy(x => x.OpenTime).ToList();

        return targetInterval switch
        {
            SupportedIntervals.FourHour => AggregateByHours(sortedCandles, 4),
            SupportedIntervals.OneDay => AggregateByDays(sortedCandles, 1),
            SupportedIntervals.OneWeek => AggregateByWeeks(sortedCandles),
            SupportedIntervals.OneMonth => AggregateByMonths(sortedCandles),
            _ => throw new ArgumentException($"Unsupported interval: {targetInterval}"),
        };
    }

    private static List<Candle> AggregateByHours(List<Candle> candles, int hours)
    {
        return candles
            .GroupBy(x => new
            {
                x.OpenTime.Year,
                x.OpenTime.Month,
                x.OpenTime.Day,
                Bucket = x.OpenTime.Hour / hours,
            })
            .Select(g => BuildAggregatedCandle(g))
            .OrderBy(x => x.OpenTime)
            .ToList();
    }

    private static List<Candle> AggregateByDays(List<Candle> candles, int days)
    {
        return candles
            .GroupBy(x => x.OpenTime.Date)
            .Select(g => BuildAggregatedCandle(g))
            .OrderBy(x => x.OpenTime)
            .ToList();
    }

    private static List<Candle> AggregateByWeeks(List<Candle> candles)
    {
        return candles
            .GroupBy(x =>
            {
                var date = x.OpenTime.Date;
                var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
                return date.AddDays(-diff);
            })
            .Select(g => BuildAggregatedCandle(g))
            .OrderBy(x => x.OpenTime)
            .ToList();
    }

    private static List<Candle> AggregateByMonths(List<Candle> candles)
    {
        return candles
            .GroupBy(x => new DateTime(x.OpenTime.Year, x.OpenTime.Month, 1))
            .Select(g => BuildAggregatedCandle(g))
            .OrderBy(x => x.OpenTime)
            .ToList();
    }

    private static Candle BuildAggregatedCandle(IEnumerable<Candle> group)
    {
        var candles = group.OrderBy(x => x.OpenTime).ToList();

        var first = candles.First();
        var last = candles.Last();

        return new Candle
        {
            Id = Guid.NewGuid(),
            Symbol = first.Symbol,
            Interval = "aggregated",
            OpenTime = first.OpenTime,
            CloseTime = last.CloseTime,

            Open = first.Open,
            High = candles.Max(x => x.High),
            Low = candles.Min(x => x.Low),
            Close = last.Close,
            Volume = candles.Sum(x => x.Volume),
        };
    }
}
