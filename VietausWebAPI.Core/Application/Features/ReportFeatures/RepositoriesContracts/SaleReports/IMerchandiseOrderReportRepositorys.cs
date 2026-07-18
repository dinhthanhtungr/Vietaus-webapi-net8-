using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports
{
    public interface IMerchandiseOrderReportRepositorys
    {
        Task<(IReadOnlyList<SummaryMOReportDto> Items, int TotalCount)> GetDeliveryPlanReportAsync(
            MerchandiseOrderReportQuery query,
            ViewerScope viewerScope,
            CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<InactiveCustomerReportDto> Items, int TotalCount)> GetInactiveCustomersReportAsync(
            MerchandiseOrderReportQuery query,
            ViewerScope viewerScope,
            CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<MerchandiseOrderReportRowDto> Items, int TotalCount)> GetDeliveryShortageReportAsync(
            MerchandiseOrderReportQuery query,
            ViewerScope viewerScope,
            CancellationToken cancellationToken = default);

        Task<List<SummaryMOReportDto>> GetDeliveryPlanReportForExportAsync(
            MerchandiseOrderReportQuery query,
            ViewerScope viewerScope,
            CancellationToken cancellationToken = default);

        Task<MerchandiseOrderReportHeaderDto> GetMerchandiseOrderHeaderReportAsync(
            MerchandiseOrderReportQuery query,
            ViewerScope viewerScope,
            CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<MerchandiseOrderReportRowDto> Items, int TotalCount)> GetMerchandiseOrderRowsAsync(
            MerchandiseOrderReportQuery query,
            ViewerScope viewerScope,
            CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<MerchandiseOrderSaleDetailReportDto> Items, int TotalCount)> GetSaleDetailReportAsync(
            MerchandiseOrderReportQuery query,
            ViewerScope viewerScope,
            CancellationToken cancellationToken = default);

        Task<MerchandiseOrderProductCategoryAnalyticsDto> GetProductCategoryReportAsync(
            MerchandiseOrderReportQuery query,
            ViewerScope viewerScope,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MerchandiseOrderReportDetailDto>> GetMerchandiseOrderDetailReportAsync(
            Guid merchandiseOrderId,
            MerchandiseOrderReportQuery query,
            ViewerScope viewerScope,
            CancellationToken cancellationToken = default);
    }
}
