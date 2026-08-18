using CandleCore.Api.Entities;

namespace CandleCore.Api.DTOs.MarketData;

public sealed class CandleDto
{
    public Guid Id { get; init; }

    public string Symbol { get; init; } = default!;
    public string Interval { get; init; } = default!;

    public DateTime OpenTime { get; init; }
    public DateTime CloseTime { get; init; }

    public decimal Open { get; init; }
    public decimal High { get; init; }
    public decimal Low { get; init; }
    public decimal Close { get; init; }
    public decimal Volume { get; init; }

    public static CandleDto FromEntity(Candle candle) =>
        new()
        {
            Id = candle.Id,
            Symbol = candle.Symbol,
            Interval = candle.Interval,
            OpenTime = candle.OpenTime,
            CloseTime = candle.CloseTime,
            Open = candle.Open,
            High = candle.High,
            Low = candle.Low,
            Close = candle.Close,
            Volume = candle.Volume,
        };
}
