using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports
{
    public interface IPLPUPurchaseOverviewReportRepository
    {
        Task<PLPUPurchaseReportRawData> GetRawDataAsync(PLPUPurchaseOverviewQuery query, CancellationToken ct);
    }
}
