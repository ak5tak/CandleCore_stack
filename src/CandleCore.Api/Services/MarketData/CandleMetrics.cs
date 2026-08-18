using CandleCore.Api.Entities;

namespace CandleCore.Api.Services.MarketData;

/// <summary>Pure candle metrics shared by Analysis and Dashboard. No DI.</summary>
public static class CandleMetrics
{
    public readonly record struct SummaryStats(
        int CandleCount,
        decimal LatestClose,
        decimal AverageClose,
        decimal HighestHigh,
        decimal LowestLow
    );

    public readonly record struct PriceChangeStats(
        int Lookback,
        decimal OldestClose,
        decimal LatestClose,
        decimal ChangePercent
    );

    public static SummaryStats? TrySummary(IReadOnlyList<Candle> candles)
    {
        if (candles.Count == 0)
        {
            return null;
        }

        var latestClose = candles[^1].Close;
        var highestHigh = candles.Max(x => x.High);

        return new SummaryStats(
            CandleCount: candles.Count,
            LatestClose: latestClose,
            AverageClose: candles.Average(x => x.Close),
            HighestHigh: highestHigh,
            LowestLow: candles.Min(x => x.Low)
        );
    }

    public static PriceChangeStats? TryPriceChange(IReadOnlyList<Candle> candles, int lookback)
    {
        var relevantCandles = candles.TakeLast(lookback).ToList();

        if (relevantCandles.Count < 2)
        {
            return null;
        }

        var oldestClose = relevantCandles[0].Close;
        var latestClose = relevantCandles[^1].Close;

        if (oldestClose == 0)
        {
            return null;
        }

        return new PriceChangeStats(
            Lookback: relevantCandles.Count,
            OldestClose: oldestClose,
            LatestClose: latestClose,
            ChangePercent: ((latestClose - oldestClose) / oldestClose) * 100
        );
    }
}
