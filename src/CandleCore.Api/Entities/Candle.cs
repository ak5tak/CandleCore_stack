namespace CandleCore.Api.Entities;

public class Candle
{
    public Guid Id { get; set; }

    public string Symbol { get; set; } = default!;
    public string Interval { get; set; } = default!;

    public DateTime OpenTime { get; set; }
    public DateTime CloseTime { get; set; }

    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }
}
