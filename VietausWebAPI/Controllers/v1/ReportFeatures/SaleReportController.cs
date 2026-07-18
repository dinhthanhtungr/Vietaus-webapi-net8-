using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Services;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Services.SaleReports;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.WebAPI.Controllers.v1.ReportFeatures
{
    [ApiController]
    [Route("api/salereport")]
    [AllowAnonymous]
    public class SaleReportController : ControllerBase
    {
        private readonly IMerchandiseOrderReportServices _merchandiseOrderReportService;

        public SaleReportController(
            IMerchandiseOrderReportServices merchandiseOrderReportService)
        {
            _merchandiseOrderReportService = merchandiseOrderReportService;
        }

        /// <summary>
        /// Lấy danh sách báo cáo đơn hàng bán có phân trang
        /// </summary>
        [HttpGet("merchandise-orders")]
        public async Task<IActionResult> GetMerchandiseOrderReport(
            [FromQuery] MerchandiseOrderReportQuery query,
            CancellationToken ct)
        {
            var result = await _merchandiseOrderReportService
                .GetSummaryMOReportAsync(query, ct);

            return Ok(result);
        }


        [HttpGet("merchandise-orders/inactive-customers")]
        public async Task<ActionResult<PagedResult<InactiveCustomerReportDto>>> GetInactiveCustomersReport(
            [FromQuery] MerchandiseOrderReportQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _merchandiseOrderReportService.GetInactiveCustomersReportAsync(
                query,
                cancellationToken);

            return Ok(result);
        }


        /// <summary>
        /// Lấy danh sách dòng đơn hàng bán còn thiếu khối lượng giao.
        /// Dữ liệu và công thức giống Summary: RemainingQuantity = RequestedQuantity - DeliveredQuantity.
        /// </summary>
        [HttpGet("merchandise-orders/delivery-shortages")]
        public async Task<IActionResult> GetDeliveryShortageReport(
            [FromQuery] MerchandiseOrderReportQuery query,
            CancellationToken ct)
        {
            var result = await _merchandiseOrderReportService
                .GetDeliveryShortageReportAsync(query, ct);

            return Ok(result);
        }

        /// <summary>
        /// Lấy số liệu tổng quan để vẽ biểu đồ báo cáo đơn hàng bán.
        /// </summary>
        [HttpGet("merchandise-orders/headers")]
        public async Task<IActionResult> GetMerchandiseOrderHeaderReport(
            [FromQuery] MerchandiseOrderReportQuery query,
            CancellationToken ct)
        {
            var result = await _merchandiseOrderReportService
                .GetMerchandiseOrderHeaderReportAsync(query, ct);

            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách đơn hàng bán có phân trang riêng cho infinity scroll.
        /// Page size tối đa là 15.
        /// </summary>
        [HttpGet("merchandise-orders/rows")]
        public async Task<IActionResult> GetMerchandiseOrderRows(
            [FromQuery] MerchandiseOrderReportQuery query,
            CancellationToken ct)
        {
            var result = await _merchandiseOrderReportService
                .GetMerchandiseOrderRowsAsync(query, ct);

            return Ok(result);
        }

        /// <summary>
        /// Lấy báo cáo chi tiết theo sale, gồm doanh thu đã giao, khối lượng, giá vốn và lợi nhuận thực.
        /// </summary>
        [HttpGet("merchandise-orders/sale-details")]
        public async Task<IActionResult> GetSaleDetailReport(
            [FromQuery] MerchandiseOrderReportQuery query,
            CancellationToken ct)
        {
            var result = await _merchandiseOrderReportService
                .GetSaleDetailReportAsync(query, ct);

            return Ok(result);
        }

        /// <summary>
        /// Lấy báo cáo doanh thu, khối lượng, giá vốn và lợi nhuận theo loại sản phẩm.
        /// Các sản phẩm có mã màu kết thúc bằng C được gom vào nhóm Compound.
        /// </summary>
        [HttpGet("merchandise-orders/product-categories")]
        public async Task<IActionResult> GetProductCategoryReport(
            [FromQuery] MerchandiseOrderReportQuery query,
            CancellationToken ct)
        {
            var result = await _merchandiseOrderReportService
                .GetProductCategoryReportAsync(query, ct);

            return Ok(result);
        }

        /// <summary>
        /// Lấy chi tiết báo cáo của một đơn hàng bán.
        /// </summary>
        [HttpGet("merchandise-orders/{merchandiseOrderId:guid}/details")]
        public async Task<IActionResult> GetMerchandiseOrderDetailReport(
            [FromRoute] Guid merchandiseOrderId,
            [FromQuery] MerchandiseOrderReportQuery query,
            CancellationToken ct)
        {
            var result = await _merchandiseOrderReportService
                .GetMerchandiseOrderDetailReportAsync(merchandiseOrderId, query, ct);

            return Ok(result);
        }

        /// <summary>
        /// Xuất Excel báo cáo đơn hàng bán
        /// </summary>
        [HttpGet("merchandise-orders/export-excel")]
        public async Task<IActionResult> ExportMerchandiseOrderReportExcel(
            [FromQuery] MerchandiseOrderReportQuery query,
            CancellationToken ct)
        {
            var bytes = await _merchandiseOrderReportService
                .ExportSummaryMOReportExcelAsync(query, ct);

            var fileName = $"Bao-cao-don-hang-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";

            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
