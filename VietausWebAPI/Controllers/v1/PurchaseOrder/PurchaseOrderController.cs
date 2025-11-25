using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Queries;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.WebAPI.Controllers.v1.PurchaseOrder
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class PurchaseOrderController : Controller
    {

        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IPurchaseOrderPdfService _purchaseOrderPdfService;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService, IPurchaseOrderPdfService purchaseOrderPdfService)
        {
            _purchaseOrderService = purchaseOrderService;
            _purchaseOrderPdfService = purchaseOrderPdfService;
        }

        //[HttpGet("{materialId}/stock")]
        //public async Task<IActionResult> GetMaterialStock(Guid materialId)
        //{
        //    var stock = await _purchaseOrderService.GetMaterialStockAsync(materialId);
        //    if (stock == null)
        //        return NotFound();

        //    return Ok(stock);
        //}

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders([FromQuery] PurchaseOrderQuery query)
        {
            var result = await _purchaseOrderService.GetAllAsync(query);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{purchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderById(Guid purchaseOrderId)
        {
            var result = await _purchaseOrderService.GetPurchaseOrderByIdAsync(purchaseOrderId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        //[HttpGet("GetPurchaseOrderDetailByIdAsync/{purchaseOrderId}")]
        //public async Task<IActionResult> GetPurchaseOrderDetailByIdAsync(Guid purchaseOrderId)
        //{
        //    var result = await _purchaseOrderService.GetPurchaseOrderDetailByIdAsync(purchaseOrderId, CancellationToken.None);
        //    if (result == null)
        //        return NotFound();
        //    return Ok(result);
        //}

        [HttpPost]
        public async Task<IActionResult> PostPurchaseOrderSnapshot([FromBody] PostPurchaseOrder purchaseOrder)
        {
            var result = await _purchaseOrderService.CreateAsync(purchaseOrder);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePurchaseOrderSnapshot([FromBody] PatchPurchaseOrder purchaseOrder)
        {
            var result = await _purchaseOrderService.UpdateAsync(purchaseOrder);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("GeneratePdf")]
        public async Task<IActionResult> GeneratePurchaseOrderPdf([FromQuery] Guid purchaseOrderId)
        {
            var pdfBytes = await _purchaseOrderPdfService.GenerateAsync(purchaseOrderId);
            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                return NotFound("Purchase order not found or PDF generation failed.");
            }
            return File(pdfBytes, "application/pdf", "PurchaseOrder.pdf");
        }
    }
}
