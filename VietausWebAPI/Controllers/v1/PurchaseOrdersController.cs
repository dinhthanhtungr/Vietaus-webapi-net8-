using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders.Query;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class PurchaseOrdersController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IPurchaseOrderDetailsService _purchaseOrderDetailsService;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService, IPurchaseOrderDetailsService purchaseOrderDetailsService)
        {
            _purchaseOrderService = purchaseOrderService;
            _purchaseOrderDetailsService = purchaseOrderDetailsService;
        }

        [HttpGet("GeneratePONumber")]
        public async Task<IActionResult> GeneratePONumber()
        {
            try
            {
                var poNumber = await _purchaseOrderService.GeneratePONumberAsync();
                return Ok(new { PONumber = poNumber });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpPost("CreatePurchaseOrder")]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderDTO purchaseOrder)
        {
            if (purchaseOrder == null)
            {
                return BadRequest(new { Message = "Invalid purchase order data." });
            }
            try
            {
                await _purchaseOrderService.CreatePurchaseOrderAsync(purchaseOrder);
                return Ok(new { Message = "success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("GetPurchaseOrders")]
        public async Task<IActionResult> GetPurchaseOrders([FromQuery] GetPOQuery query)
        {
            try
            {
                var result = await _purchaseOrderService.GetPurchaseOrdersServiceAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("GetPurchaseOrderDetails")]
        public async Task<IActionResult> GetPurchaseOrderDetails([FromQuery] Guid POID)
        {
            if (POID == Guid.Empty)
            {
                return BadRequest(new { Message = "Invalid Purchase Order ID." });
            }
            try
            {
                var result = await _purchaseOrderDetailsService.ShowPurchaseOrderDetailServiceAsync(POID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("delete-punchaseOrder")]
        public async Task<IActionResult> DeletePurchaseOrder([FromQuery] Guid poid)
        {
            if (poid == Guid.Empty)
            {
                return BadRequest(new { Message = "Invalid Purchase Order ID." });
            }
            try
            {
                await _purchaseOrderService.DeletePurchaseOrderAsync(poid);
                return Ok(new { Message = "Purchase order deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("SuccessPunchaseOrder")]
        public async Task<IActionResult> SuccessPunchaseOrder([FromQuery] Guid guid)
        {
            if (guid == Guid.Empty)
            {
                return BadRequest(new { Message = "Invalid Purchase Order ID." });
            }
            try
            {
                await _purchaseOrderService.SuccessPunchaseOrderAsync(guid);
                return Ok(new { Message = "Purchase order processed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
