using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports
{
    public interface IPLPUPurchaseOverviewReportService
    {
        Task<PLPUPurchaseOverviewReportDto> GetOverviewAsync(PLPUPurchaseOverviewQuery query, CancellationToken ct = default);
        Task<PLPUPurchaseOrderHeaderDashboardDto> GetHeaderReportAsync(PLPUPurchaseOverviewQuery query, CancellationToken ct = default);
        Task<PagedResult<PLPUPurchaseOrderRowDto>> GetRowsAsync(PLPUPurchaseOverviewQuery query, CancellationToken ct = default);
        Task<PLPUPurchaseOrderDetailDashboardDto> GetOrderDetailAsync(Guid purchaseOrderId, CancellationToken ct = default);
        Task<PagedResult<PLPUPurchaseOrderDetailReportDto>> GetDetailLinesAsync(PLPUPurchaseOverviewQuery query, CancellationToken ct = default);
        Task<PagedResult<PLPUPurchaseReceiptReportDto>> GetReceiptsAsync(PLPUPurchaseOverviewQuery query, CancellationToken ct = default);
        Task<PagedResult<PLPUPurchaseQcReportDto>> GetQcAsync(PLPUPurchaseOverviewQuery query, CancellationToken ct = default);
        Task<List<PLPUSupplierPurchasePerformanceDto>> GetSupplierPerformanceAsync(PLPUPurchaseOverviewQuery query, CancellationToken ct = default);
    }
}
