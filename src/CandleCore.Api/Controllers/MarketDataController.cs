using CandleCore.Api.Constants;
using CandleCore.Api.Requests.MarketData;
using CandleCore.Api.Services.MarketData;
using CandleCore.Api.Validation;
using Microsoft.AspNetCore.Mvc;

namespace CandleCore.Api.Controllers;

[ApiController]
[Route("api/market-data")]
public class MarketDataController : ControllerBase
{
    private readonly IMarketDataQueryService _marketDataQueryService;

    public MarketDataController(IMarketDataQueryService marketDataQueryService)
    {
        _marketDataQueryService = marketDataQueryService;
    }

    [HttpGet("candles")]
    public async Task<IActionResult> GetCandles(
        string symbol = MarketDefaults.Symbol,
        string timeframe = MarketDefaults.Timeframe,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default
    )
    {
        if (
            RequestValidation.ValidateSymbolTimeframeAndDateRange(
                symbol,
                timeframe,
                startDate,
                endDate,
                out var from,
                out var to
            ) is
            { } error
        )
        {
            return BadRequest(new { message = error });
        }

        var request = new CandleRangeRequest
        {
            Symbol = symbol,
            Timeframe = timeframe,
            StartDate = from,
            EndDate = to,
        };

        var candles = await _marketDataQueryService.GetCandlesAsync(request, cancellationToken);
        return Ok(candles);
    }

    [HttpGet("bounds")]
    public async Task<IActionResult> GetBounds(
        string symbol = MarketDefaults.Symbol,
        string timeframe = MarketDefaults.Timeframe,
        CancellationToken cancellationToken = default
    )
    {
        if (RequestValidation.ValidateSymbolAndTimeframe(symbol, timeframe) is { } error)
        {
            return BadRequest(new { message = error });
        }

        var request = new BoundsRequest { Symbol = symbol, Timeframe = timeframe };
        var bounds = await _marketDataQueryService.GetBoundsAsync(request, cancellationToken);

        // No candles yet is a normal empty read: 200 with JSON null.
        // JsonResult(null) keeps 200 + body null; Ok(null) is often executed as 204 No Content.
        return bounds is null ? new JsonResult(null) : Ok(bounds);
    }
}
