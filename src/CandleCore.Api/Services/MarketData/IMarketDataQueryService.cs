using CandleCore.Api.DTOs.MarketData;
using CandleCore.Api.Requests.MarketData;

namespace CandleCore.Api.Services.MarketData;

public interface IMarketDataQueryService
{
    Task<IReadOnlyList<CandleDto>> GetCandlesAsync(
        CandleRangeRequest request,
        CancellationToken cancellationToken = default
    );

    Task<DatasetBoundsDto?> GetBoundsAsync(
        BoundsRequest request,
        CancellationToken cancellationToken = default
    );
}
