using CandleCore.Api.DTOs.Dashboard;
using CandleCore.Api.Requests.Dashboard;

namespace CandleCore.Api.Services.Dashboard;

public interface IDashboardQueryService
{
    Task<DashboardOverviewDto?> GetOverviewAsync(
        DashboardOverviewRequest request,
        CancellationToken cancellationToken = default
    );
}
