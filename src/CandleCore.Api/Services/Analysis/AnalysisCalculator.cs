using CandleCore.Api.DTOs.Analysis;
using CandleCore.Api.Entities;
using CandleCore.Api.Services.MarketData;

namespace CandleCore.Api.Services.Analysis;

public sealed class AnalysisCalculator
{
    public AnalysisOverviewDto BuildOverview(
        IReadOnlyList<Candle> candles,
        string symbol,
        string timeframe,
        int lookback
    )
    {
        return new AnalysisOverviewDto
        {
            Summary = BuildSummary(candles, symbol, timeframe),
            PriceChange = BuildPriceChange(candles, symbol, timeframe, lookback),
            RiskAnalysis = BuildRiskAnalysis(candles, symbol, timeframe, lookback),
            MarketBehaviour = BuildMarketBehaviour(candles, symbol, timeframe, lookback),
            Probability = BuildProbability(candles, symbol, timeframe, lookback),
        };
    }

    public MarketSummaryDto? BuildSummary(
        IReadOnlyList<Candle> candles,
        string symbol,
        string timeframe
    )
    {
        var stats = CandleMetrics.TrySummary(candles);
        if (stats is null)
        {
            return null;
        }

        return new MarketSummaryDto
        {
            Symbol = symbol,
            Interval = timeframe,
            CandleCount = stats.Value.CandleCount,
            LatestClose = stats.Value.LatestClose,
            AverageClose = stats.Value.AverageClose,
            HighestHigh = stats.Value.HighestHigh,
            LowestLow = stats.Value.LowestLow,
        };
    }

    public PriceChangeDto? BuildPriceChange(
        IReadOnlyList<Candle> candles,
        string symbol,
        string timeframe,
        int lookback
    )
    {
        var stats = CandleMetrics.TryPriceChange(candles, lookback);
        if (stats is null)
        {
            return null;
        }

        return new PriceChangeDto
        {
            Symbol = symbol,
            Interval = timeframe,
            Lookback = stats.Value.Lookback,
            OldestClose = stats.Value.OldestClose,
            LatestClose = stats.Value.LatestClose,
            ChangePercent = stats.Value.ChangePercent,
        };
    }

    public RiskAnalysisDto? BuildRiskAnalysis(
        IReadOnlyList<Candle> candles,
        string symbol,
        string timeframe,
        int lookback
    )
    {
        var relevantCandles = candles.TakeLast(lookback).ToList();

        if (relevantCandles.Count < 2)
        {
            return null;
        }

        var averageClose = relevantCandles.Average(x => x.Close);
        var variance = relevantCandles.Average(x => Math.Pow((double)(x.Close - averageClose), 2));

        return new RiskAnalysisDto
        {
            Symbol = symbol,
            Interval = timeframe,
            Lookback = relevantCandles.Count,
            AverageClose = averageClose,
            Volatility = (decimal)Math.Sqrt(variance),
            MaximumDrawdownPercent = CalculateMaximumDrawdownPercent(relevantCandles),
        };
    }

    public MarketBehaviourDto? BuildMarketBehaviour(
        IReadOnlyList<Candle> candles,
        string symbol,
        string timeframe,
        int lookback
    )
    {
        var relevantCandles = candles.TakeLast(lookback).ToList();

        if (relevantCandles.Count == 0)
        {
            return null;
        }

        var (bullishPeriods, bullishReturn) = FindLongestStreak(relevantCandles, bullish: true);
        var (bearishPeriods, bearishReturn) = FindLongestStreak(relevantCandles, bullish: false);

        return new MarketBehaviourDto
        {
            Symbol = symbol,
            Interval = timeframe,
            Lookback = relevantCandles.Count,
            AverageCandleRange = relevantCandles.Average(x => x.High - x.Low),
            LongestBullishStreakPeriods = bullishPeriods,
            LongestBullishStreakReturnPercent = bullishReturn,
            LongestBearishStreakPeriods = bearishPeriods,
            LongestBearishStreakReturnPercent = bearishReturn,
        };
    }

    public ProbabilityDto? BuildProbability(
        IReadOnlyList<Candle> candles,
        string symbol,
        string timeframe,
        int lookback
    )
    {
        var relevantCandles = candles.TakeLast(lookback).ToList();

        if (relevantCandles.Count == 0)
        {
            return null;
        }

        var upCandles = relevantCandles.Count(x => x.Close > x.Open);
        var downCandles = relevantCandles.Count(x => x.Close < x.Open);
        var neutralCandles = relevantCandles.Count(x => x.Close == x.Open);
        var total = relevantCandles.Count;

        return new ProbabilityDto
        {
            Symbol = symbol,
            Interval = timeframe,
            Lookback = total,
            UpCandles = upCandles,
            DownCandles = downCandles,
            NeutralCandles = neutralCandles,
            ProbabilityUp = ((decimal)upCandles / total) * 100,
            ProbabilityDown = ((decimal)downCandles / total) * 100,
            ProbabilityNeutral = ((decimal)neutralCandles / total) * 100,
        };
    }

    private static decimal CalculateMaximumDrawdownPercent(IReadOnlyList<Candle> candles)
    {
        decimal peak = 0;
        decimal maxDrawdown = 0;

        foreach (var candle in candles)
        {
            if (candle.Close > peak)
            {
                peak = candle.Close;
            }

            if (peak == 0)
            {
                continue;
            }

            var drawdown = ((peak - candle.Close) / peak) * 100;
            if (drawdown > maxDrawdown)
            {
                maxDrawdown = drawdown;
            }
        }

        return maxDrawdown;
    }

    private static (int Periods, decimal ReturnPercent) FindLongestStreak(
        IReadOnlyList<Candle> candles,
        bool bullish
    )
    {
        var bestPeriods = 0;
        var bestReturn = 0m;
        var currentStart = -1;
        var currentLength = 0;

        for (var i = 0; i < candles.Count; i++)
        {
            var matches = bullish
                ? candles[i].Close > candles[i].Open
                : candles[i].Close < candles[i].Open;

            if (matches)
            {
                if (currentLength == 0)
                {
                    currentStart = i;
                }

                currentLength++;
            }
            else if (currentLength > 0)
            {
                if (currentLength > bestPeriods)
                {
                    bestPeriods = currentLength;
                    bestReturn = CalculateStreakReturnPercent(candles, currentStart, currentLength);
                }

                currentLength = 0;
                currentStart = -1;
            }
        }

        if (currentLength > bestPeriods)
        {
            bestPeriods = currentLength;
            bestReturn = CalculateStreakReturnPercent(candles, currentStart, currentLength);
        }

        return (bestPeriods, bestReturn);
    }

    private static decimal CalculateStreakReturnPercent(
        IReadOnlyList<Candle> candles,
        int streakStartIndex,
        int streakLength
    )
    {
        var endClose = candles[streakStartIndex + streakLength - 1].Close;
        var baseline =
            streakStartIndex > 0
                ? candles[streakStartIndex - 1].Close
                : candles[streakStartIndex].Open;

        if (baseline == 0)
        {
            return 0;
        }

        return ((endClose - baseline) / baseline) * 100;
    }
}
