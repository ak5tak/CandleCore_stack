using CandleCore.Api.Infrastructure.Binance;

namespace CandleCore.Api.Tests.Helpers;

public sealed class HistoricalFakeBinanceClient : IBinanceClient
{
    public int CallCount { get; private set; }

    public List<long?> RequestedEndTimes { get; } = [];

    private readonly int _maxPerBatch;
    private readonly List<BinanceKlineDto> _history;

    public HistoricalFakeBinanceClient(
        int historyCount = 5,
        int maxPerBatch = 1000,
        DateTime? newestOpenTime = null
    )
    {
        _maxPerBatch = maxPerBatch;
        var newest = newestOpenTime ?? CandleFixtures.RangeStart.AddHours(historyCount - 1);
        _history = new List<BinanceKlineDto>(historyCount);

        for (var i = 0; i < historyCount; i++)
        {
            var openTime = newest.AddHours(-(historyCount - 1 - i));
            var openMs = new DateTimeOffset(
                DateTime.SpecifyKind(openTime, DateTimeKind.Utc)
            ).ToUnixTimeMilliseconds();

            _history.Add(
                new BinanceKlineDto
                {
                    OpenTime = openMs,
                    Open = "100",
                    High = "110",
                    Low = "95",
                    Close = "105",
                    Volume = "10",
                    CloseTime = openMs + 3_600_000 - 1,
                }
            );
        }
    }

    public Task<List<BinanceKlineDto>> GetKlinesAsync(
        string symbol,
        string interval,
        int limit = 1000,
        long? endTime = null,
        CancellationToken cancellationToken = default
    )
    {
        CallCount++;
        RequestedEndTimes.Add(endTime);

        var available = endTime is long endTimeMs
            ? _history.Where(k => k.OpenTime <= endTimeMs)
            : _history;

        var batchLimit = Math.Min(limit, _maxPerBatch);
        var page = available
            .OrderByDescending(k => k.OpenTime)
            .Take(batchLimit)
            .OrderBy(k => k.OpenTime)
            .ToList();

        return Task.FromResult(page);
    }
}

public sealed class EmptyHistoricalFakeBinanceClient : IBinanceClient
{
    public int CallCount { get; private set; }

    public Task<List<BinanceKlineDto>> GetKlinesAsync(
        string symbol,
        string interval,
        int limit = 1000,
        long? endTime = null,
        CancellationToken cancellationToken = default
    )
    {
        CallCount++;
        return Task.FromResult(new List<BinanceKlineDto>());
    }
}

public sealed class ThrowingFakeBinanceClient : IBinanceClient
{
    public Task<List<BinanceKlineDto>> GetKlinesAsync(
        string symbol,
        string interval,
        int limit = 1000,
        long? endTime = null,
        CancellationToken cancellationToken = default
    )
    {
        throw new HttpRequestException("Binance unavailable");
    }
}
