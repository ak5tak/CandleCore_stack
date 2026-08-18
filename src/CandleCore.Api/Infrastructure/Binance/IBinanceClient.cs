namespace CandleCore.Api.Infrastructure.Binance;

public interface IBinanceClient
{
    Task<List<BinanceKlineDto>> GetKlinesAsync(
        string symbol,
        string interval,
        int limit = 1000,
        long? endTime = null,
        CancellationToken cancellationToken = default
    );
}
