using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Helpers.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports;

namespace VietausWebAPI.WebAPI.Controllers.v1.ReportFeatures
{
    [ApiController]
    [Route("api/plpureport")]
    [AllowAnonymous]
    public class PLPUReportController : Controller
    {
        private readonly IFinishPLPUReportService _finishPLPUReportService;
        private readonly IPLPUPurchaseOverviewReportService _plpuPurchaseOverviewReportService;
        private readonly IExportFinishReportExcel _exportFinishReportExcel;

        public PLPUReportController(
            IFinishPLPUReportService finishPLPUReportService,
            IPLPUPurchaseOverviewReportService plpuPurchaseOverviewReportService,
            IExportFinishReportExcel exportFinishReportExcel)
        {
            _finishPLPUReportService = finishPLPUReportService;
            _plpuPurchaseOverviewReportService = plpuPurchaseOverviewReportService;
            _exportFinishReportExcel = exportFinishReportExcel;
        }

        [HttpGet("export-finish-plpu-report")]
        public async Task<IActionResult> ExportFinishPLPUReport([FromQuery] DateTime from,
                                                                [FromQuery] DateTime to, 
                                                                CancellationToken ct = default)
        {
            var result = await _finishPLPUReportService.GetFinishPLPUReportsAsync(from, to, ct);
            var fileBytes = _exportFinishReportExcel.ExportFinishReportExcel(result);

            var fileName = $"BaoCaoGiaoHangHoanTat_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

        [HttpGet("purchase-overview")]
        public async Task<IActionResult> GetPurchaseOverview([FromQuery] PLPUPurchaseOverviewQuery query,
                                                             CancellationToken ct = default)
        {
            var result = await _plpuPurchaseOverviewReportService.GetOverviewAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("purchase-orders/headers")]
        public async Task<IActionResult> GetPurchaseOrderHeaderReport([FromQuery] PLPUPurchaseOverviewQuery query,
                                                                      CancellationToken ct = default)
        {
            var result = await _plpuPurchaseOverviewReportService.GetHeaderReportAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("purchase-orders/rows")]
        public async Task<IActionResult> GetPurchaseOrderRows([FromQuery] PLPUPurchaseOverviewQuery query,
                                                              CancellationToken ct = default)
        {
            var result = await _plpuPurchaseOverviewReportService.GetRowsAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("purchase-orders/{purchaseOrderId:guid}/details")]
        public async Task<IActionResult> GetPurchaseOrderDetailReport([FromRoute] Guid purchaseOrderId,
                                                                      CancellationToken ct = default)
        {
            var result = await _plpuPurchaseOverviewReportService.GetOrderDetailAsync(purchaseOrderId, ct);
            return Ok(result);
        }

        [HttpGet("purchase-details")]
        public async Task<IActionResult> GetPurchaseDetails([FromQuery] PLPUPurchaseOverviewQuery query,
                                                            CancellationToken ct = default)
        {
            var result = await _plpuPurchaseOverviewReportService.GetDetailLinesAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("purchase-receipts")]
        public async Task<IActionResult> GetPurchaseReceipts([FromQuery] PLPUPurchaseOverviewQuery query,
                                                             CancellationToken ct = default)
        {
            var result = await _plpuPurchaseOverviewReportService.GetReceiptsAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("purchase-qc")]
        public async Task<IActionResult> GetPurchaseQc([FromQuery] PLPUPurchaseOverviewQuery query,
                                                       CancellationToken ct = default)
        {
            var result = await _plpuPurchaseOverviewReportService.GetQcAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("purchase-suppliers")]
        public async Task<IActionResult> GetPurchaseSuppliers([FromQuery] PLPUPurchaseOverviewQuery query,
                                                              CancellationToken ct = default)
        {
            var result = await _plpuPurchaseOverviewReportService.GetSupplierPerformanceAsync(query, ct);
            return Ok(result);
        }
    }
}
