using CandleCore.Api.Constants;
using CandleCore.Api.Requests.Analysis;
using CandleCore.Api.Services.Analysis;
using CandleCore.Api.Validation;
using Microsoft.AspNetCore.Mvc;

namespace CandleCore.Api.Controllers;

[ApiController]
[Route("api/analysis")]
public class AnalysisController : ControllerBase
{
    private readonly IAnalysisQueryService _analysisQueryService;

    public AnalysisController(IAnalysisQueryService analysisQueryService)
    {
        _analysisQueryService = analysisQueryService;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview(
        string symbol = MarketDefaults.Symbol,
        string timeframe = MarketDefaults.Timeframe,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int lookback = MarketDefaults.Lookback,
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

        if (RequestValidation.ValidateLookback(lookback) is { } lookbackError)
        {
            return BadRequest(new { message = lookbackError });
        }

        var overview = await _analysisQueryService.GetOverviewAsync(
            new AnalysisOverviewRequest
            {
                Symbol = symbol,
                Timeframe = timeframe,
                StartDate = from,
                EndDate = to,
                Lookback = lookback,
            },
            cancellationToken
        );

        // No candles yet is a normal empty read: 200 with JSON null.
        // JsonResult(null) keeps 200 + body null; Ok(null) is often executed as 204 No Content.
        return overview is null ? new JsonResult(null) : Ok(overview);
    }
}
