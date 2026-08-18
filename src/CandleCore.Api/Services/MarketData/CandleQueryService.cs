using CandleCore.Api.Entities;
using CandleCore.Api.Extensions;
using CandleCore.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CandleCore.Api.Services.MarketData;

public class CandleQueryService
{
    private readonly AppDbContext _dbContext;

    public CandleQueryService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Candle>> GetCandlesAsync(
        string symbol,
        string interval,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default
    )
    {
        startDate = startDate.AsUtc();
        endDate = endDate.AsUtc();

        return await _dbContext
            .Candles.AsNoTracking()
            .Where(x =>
                x.Symbol == symbol
                && x.Interval == interval
                && x.OpenTime >= startDate
                && x.OpenTime <= endDate
            )
            .OrderBy(x => x.OpenTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<(DateTime First, DateTime Last, int Count)?> GetBoundsAsync(
        string symbol,
        string interval,
        CancellationToken cancellationToken = default
    )
    {
        var query = _dbContext
            .Candles.AsNoTracking()
            .Where(x => x.Symbol == symbol && x.Interval == interval);

        var count = await query.CountAsync(cancellationToken);
        if (count == 0)
        {
            return null;
        }

        var first = await query.MinAsync(x => x.OpenTime, cancellationToken);
        var last = await query.MaxAsync(x => x.OpenTime, cancellationToken);

        return (first, last, count);
    }
}
