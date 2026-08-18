using CandleCore.Api.DTOs.Analysis;
using CandleCore.Api.Requests.Analysis;

namespace CandleCore.Api.Services.Analysis;

public interface IAnalysisQueryService
{
    Task<AnalysisOverviewDto?> GetOverviewAsync(
        AnalysisOverviewRequest request,
        CancellationToken cancellationToken = default
    );
}
