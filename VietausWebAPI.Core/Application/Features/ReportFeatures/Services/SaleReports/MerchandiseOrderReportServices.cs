using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Helpers.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.Services.SaleReports
{
    public class MerchandiseOrderReportService : IMerchandiseOrderReportServices
    {
        private readonly IMerchandiseOrderReportRepositorys _repository;
        private readonly IVisibilityHelper _visibilityHelper;

        public MerchandiseOrderReportService(
            IMerchandiseOrderReportRepositorys repository,
            IVisibilityHelper visibilityHelper)
        {
            _repository = repository;
            _visibilityHelper = visibilityHelper;
        }

        public async Task<PagedResult<SummaryMOReportDto>> GetSummaryMOReportAsync(
            MerchandiseOrderReportQuery query,
            CancellationToken ct = default)
        {
            query ??= new MerchandiseOrderReportQuery();

            var viewerScope = await _visibilityHelper.BuildViewerScopeAsync(ct);

            var (items, totalCount) = await _repository.GetDeliveryPlanReportAsync(query, viewerScope, ct);

            var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
            var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

            return new PagedResult<SummaryMOReportDto>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<byte[]> ExportSummaryMOReportExcelAsync(
            MerchandiseOrderReportQuery query,
            CancellationToken ct = default)
        {
            query ??= new MerchandiseOrderReportQuery();

            var viewerScope = await _visibilityHelper.BuildViewerScopeAsync(ct);

            var rows = await _repository.GetDeliveryPlanReportForExportAsync(query, viewerScope, ct);

            var helper = new ExportMerchandiseOrderReportExcelHelper();
            return helper.Export(rows);
        }

        public async Task<MerchandiseOrderReportHeaderDto> GetMerchandiseOrderHeaderReportAsync(
            MerchandiseOrderReportQuery query,
            CancellationToken cancellationToken = default)
        {
            query ??= new MerchandiseOrderReportQuery();

            var viewerScope = await _visibilityHelper.BuildViewerScopeAsync(cancellationToken);
            return await _repository.GetMerchandiseOrderHeaderReportAsync(query, viewerScope, cancellationToken);
        }

        public async Task<PagedResult<MerchandiseOrderReportRowDto>> GetMerchandiseOrderRowsAsync(
            MerchandiseOrderReportQuery query,
            CancellationToken cancellationToken = default)
        {
            query ??= new MerchandiseOrderReportQuery();

            var viewerScope = await _visibilityHelper.BuildViewerScopeAsync(cancellationToken);
            var (items, totalCount) = await _repository.GetMerchandiseOrderRowsAsync(query, viewerScope, cancellationToken);
            var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
            var pageSize = Math.Min(query.PageSize <= 0 ? 15 : query.PageSize, 15);

            return new PagedResult<MerchandiseOrderReportRowDto>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<MerchandiseOrderReportDetailDto>> GetMerchandiseOrderDetailReportAsync(
            Guid merchandiseOrderId,
            CancellationToken cancellationToken = default)
        {
            var viewerScope = await _visibilityHelper.BuildViewerScopeAsync(cancellationToken);
            return await _repository.GetMerchandiseOrderDetailReportAsync(merchandiseOrderId, viewerScope, cancellationToken);
        }
    }
}
