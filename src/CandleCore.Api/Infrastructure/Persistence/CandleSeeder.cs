using System.Globalization;
using CandleCore.Api.Constants;
using CandleCore.Api.Entities;
using CandleCore.Api.Infrastructure.Binance;
using Microsoft.EntityFrameworkCore;

namespace CandleCore.Api.Infrastructure.Persistence;

public class CandleSeeder
{
    private const int PageSize = 1000;
    private const int BatchSize = 1000;

    private readonly AppDbContext _dbContext;
    private readonly IBinanceClient _binanceClient;
    private readonly ILogger<CandleSeeder> _logger;
    private readonly int _seedCandleCount;

    public CandleSeeder(
        AppDbContext dbContext,
        IBinanceClient binanceClient,
        ILogger<CandleSeeder> logger
    )
        : this(dbContext, binanceClient, logger, MarketDefaults.SeedCandleCount) { }

    public CandleSeeder(
        AppDbContext dbContext,
        IBinanceClient binanceClient,
        ILogger<CandleSeeder> logger,
        int seedCandleCount
    )
    {
        _dbContext = dbContext;
        _binanceClient = binanceClient;
        _logger = logger;
        _seedCandleCount = seedCandleCount;
    }

    // Skip only when BTCUSDT 1h count >= SeedCandleCount (fully seeded).
    // A smaller count resumes: fetch remaining slots from Binance and insert OpenTimes not already in the DB.
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var existingCount = await CountSeededCandlesAsync(cancellationToken);

            if (existingCount >= _seedCandleCount)
            {
                _logger.LogInformation(
                    "Candle seed skipped; {Count} BTCUSDT 1h candles already present (target {Target}).",
                    existingCount,
                    _seedCandleCount
                );
                return;
            }

            _logger.LogInformation(
                "Candle seed starting; {Existing} BTCUSDT 1h candles present, target {Target}.",
                existingCount,
                _seedCandleCount
            );

            var remainingSlots = _seedCandleCount - existingCount;
            var klines = await FetchKlinesAsync(remainingSlots, cancellationToken);
            if (klines.Count == 0)
            {
                _logger.LogInformation("Binance returned no klines; seed insert skipped.");
                return;
            }

            var existingOpenTimes = (
                await _dbContext
                    .Candles.Where(x =>
                        x.Symbol == MarketDefaults.Symbol
                        && x.Interval == MarketDefaults.Timeframe
                    )
                    .Select(x => x.OpenTime)
                    .ToListAsync(cancellationToken)
            ).ToHashSet();
            var candles = klines
                .OrderBy(k => k.OpenTime)
                .Select(TryMapCandle)
                .OfType<Candle>()
                .Where(c => !existingOpenTimes.Contains(c.OpenTime))
                .Take(remainingSlots)
                .ToList();

            var inserted = 0;
            for (var i = 0; i < candles.Count; i += BatchSize)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var batch = candles.Skip(i).Take(BatchSize).ToList();
                inserted += await SaveBatchAsync(batch, cancellationToken);
            }

            var total = existingCount + inserted;
            _logger.LogInformation(
                "Candle seed finished; inserted {Inserted}, total {Total}.",
                inserted,
                total
            );
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Candle seed failed; aborting this seed run.");
        }
    }

    private Task<int> CountSeededCandlesAsync(CancellationToken cancellationToken) =>
        _dbContext.Candles.CountAsync(
            x => x.Symbol == MarketDefaults.Symbol && x.Interval == MarketDefaults.Timeframe,
            cancellationToken
        );

    private async Task<List<BinanceKlineDto>> FetchKlinesAsync(
        int remainingSlots,
        CancellationToken cancellationToken
    )
    {
        var collected = new List<BinanceKlineDto>(remainingSlots);
        long? endTime = null;

        while (collected.Count < remainingSlots)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var remaining = remainingSlots - collected.Count;
            var limit = Math.Min(PageSize, remaining);

            List<BinanceKlineDto> page;
            try
            {
                page = await _binanceClient.GetKlinesAsync(
                    MarketDefaults.Symbol,
                    MarketDefaults.Timeframe,
                    limit,
                    endTime,
                    cancellationToken
                );
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Binance kline page failed (limit {Limit}, endTime {EndTime}); aborting this seed run.",
                    limit,
                    endTime
                );
                return collected;
            }

            collected.AddRange(page);

            _logger.LogInformation(
                "Binance kline page: limit={Limit}, endTime={EndTime}, pageCount={PageCount}, collected={Collected}.",
                limit,
                endTime,
                page.Count,
                collected.Count
            );

            if (page.Count == 0)
            {
                break;
            }

            var oldestOpenTime = page.Min(k => k.OpenTime);
            endTime = oldestOpenTime - 1;

            if (page.Count < limit)
            {
                break;
            }
        }

        return collected;
    }

    private async Task<int> SaveBatchAsync(List<Candle> batch, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Saving candle batch of {BatchCount}.", batch.Count);

        try
        {
            await _dbContext.Candles.AddRangeAsync(batch, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return batch.Count;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(
                ex,
                "Candle batch save hit a uniqueness conflict; skipping this batch."
            );
            return 0;
        }
        finally
        {
            _dbContext.ChangeTracker.Clear();
        }
    }

    private Candle? TryMapCandle(BinanceKlineDto kline)
    {
        try
        {
            return MapCandle(kline);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Skipping malformed kline at OpenTime {OpenTime}.",
                kline.OpenTime
            );
            return null;
        }
    }

    private static Candle MapCandle(BinanceKlineDto kline)
    {
        return new Candle
        {
            Id = Guid.NewGuid(),
            Symbol = MarketDefaults.Symbol,
            Interval = MarketDefaults.Timeframe,
            OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(kline.OpenTime).UtcDateTime,
            CloseTime = DateTimeOffset.FromUnixTimeMilliseconds(kline.CloseTime).UtcDateTime,
            Open = decimal.Parse(kline.Open, CultureInfo.InvariantCulture),
            High = decimal.Parse(kline.High, CultureInfo.InvariantCulture),
            Low = decimal.Parse(kline.Low, CultureInfo.InvariantCulture),
            Close = decimal.Parse(kline.Close, CultureInfo.InvariantCulture),
            Volume = decimal.Parse(kline.Volume, CultureInfo.InvariantCulture),
        };
    }
}
