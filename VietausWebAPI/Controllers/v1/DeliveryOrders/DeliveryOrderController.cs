using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers.Excels;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Queries;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.Querys;

namespace VietausWebAPI.WebAPI.Controllers.v1.DeliveryOrders
{
    [ApiController]
    [Route("api/deliveryorders")]
    [AllowAnonymous]
    public class DeliveryOrderController : Controller
    {
        private readonly IDeliveryOrderService _deliveryOrderService;
        private readonly IDeliveryOrderPdfService _deliveryOrderPdfService;
        private readonly IExportDeliveryPlan _exportDeliveryPlan;

        public DeliveryOrderController(IDeliveryOrderService deliveryOrderService, IDeliveryOrderPdfService deliveryOrderPdfService, IExportDeliveryPlan exportDeliveryPlan )
        {
            _deliveryOrderService = deliveryOrderService;
            _deliveryOrderPdfService = deliveryOrderPdfService;
            _exportDeliveryPlan = exportDeliveryPlan;
        }   

        [HttpGet]
        public async Task<IActionResult> GetDeliveryOrderById([FromQuery] Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID cannot be empty.");
            }
            try
            {
                var result = await _deliveryOrderService.GetAsync(id, ct);
                if (result == null)
                {
                    return NotFound($"Delivery order with ID {id} not found.");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateDeliveryOrder([FromBody] PatchDeliveryOrder deliveryOrderDto,  CancellationToken ct = default)
        {
            if (deliveryOrderDto == null || deliveryOrderDto.Id == Guid.Empty)
            {
                return BadRequest("Valid DeliveryOrderDto data is required.");
            }
            var result = await _deliveryOrderService.UpdateAsync(deliveryOrderDto, ct);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        
        [HttpPatch("SoftDelete")]
        public async Task<IActionResult> SoftDeleteDeliveryOrder([FromQuery] Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID cannot be empty.");
            }
            var result = await _deliveryOrderService.SoftDeleteAsync(id, ct);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateDeliveryOrder([FromBody] PostDeliveryOrder postDeliveryOrder, CancellationToken ct = default)
        {
            if (postDeliveryOrder == null)
            {
                return BadRequest("PostDeliveryOrder data is required.");
            }
            var result = await _deliveryOrderService.CreateAsync(postDeliveryOrder, ct);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpGet("GetSelectableLinesAsync")]
        public async Task<IActionResult> GetSelectableLinesAsync([FromQuery] DeliveryOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _deliveryOrderService.GetSelectableLinesAsync(query, ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("GetAllDeliverers")]
        public async Task<IActionResult> GetAllDeliverers([FromQuery] DelivererQuery query, CancellationToken ct = default)
        {
            var result = await _deliveryOrderService.GetAllDelivererAsync(query, ct);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("No deliverers found.");
            }
        }

        [HttpGet("GetAllDeliveryOrders")]
        public async Task<IActionResult> GetAllDeliveryOrders([FromQuery] DeliveryOrderQuery query, CancellationToken ct = default)
        {
            var result = await _deliveryOrderService.GetAllAsync(query, ct);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("No delivery orders found.");
            }
        }

        [HttpGet("GeneratePdf")]
        public async Task<IActionResult> GenerateDeliveryOrderPdf([FromQuery] Guid deliveryOrderId, CancellationToken ct = default)
        {
            if (deliveryOrderId == Guid.Empty)
            {
                return BadRequest("Delivery Order ID cannot be empty.");
            }
            try
            {
                var pdfBytes = await _deliveryOrderPdfService.GenerateAsync(deliveryOrderId, ct);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return NotFound("Delivery Order not found or PDF generation failed.");
                }
                return File(pdfBytes, "application/pdf", $"DeliveryOrder_{deliveryOrderId}.pdf");
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred during PDF generation.");
            }
        }


        [HttpGet("export-delivery-plan")]
        public async Task<IActionResult> ExportDeliveryPlan([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        {
            var rows = await _deliveryOrderService.BuildRowsAsync(from, to, ct);
            var bytes = _exportDeliveryPlan.ExportDeliveryPlanExcel(rows);

            var fileName = $"DS_GiaoHang_{from:yyyyMMdd}_{to:yyyyMMdd}.xlsx";
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        [HttpGet("export-delivery-finish")]
        public async Task<IActionResult> ExportDeliveryFinish(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            CancellationToken ct)
        {
            // guard nhẹ giống style bạn
            if (from == default || to == default)
                return BadRequest("from/to is required.");

            if (to.Date < from.Date)
                return BadRequest("to must be >= from.");

            var rows = await _deliveryOrderService.BuildDeliveryFinishRowsAsync(from, to, ct);

            var bytes = _exportDeliveryPlan.ExportDeliveryFinish(rows, from, to);

            var fileName = $"DS_GiaoHang_HoanTat_{from:yyyyMMdd}_{to:yyyyMMdd}.xlsx";
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
