namespace CandleCore.Api.Infrastructure.Binance;

public sealed class BinanceKlineDto
{
    public long OpenTime { get; set; }
    public string Open { get; set; } = default!;
    public string High { get; set; } = default!;
    public string Low { get; set; } = default!;
    public string Close { get; set; } = default!;
    public string Volume { get; set; } = default!;
    public long CloseTime { get; set; }
}
