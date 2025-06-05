using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.DTOs.InventoryReceipts;
using VietausWebAPI.Core.Application.Usecases.InventoryReceipts.ServiceContracts;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/InventoryReceipts")]
    [AllowAnonymous]
    public class InventoryReceiptsController : Controller
    {
        private readonly IInventoryReceiptService _inventoryReceiptService;

        public InventoryReceiptsController(IInventoryReceiptService inventoryReceiptService)
        {
            _inventoryReceiptService = inventoryReceiptService;
        }

        [HttpGet("GetMaterialReceiptId")]
        public async Task<IActionResult> GetMaterialReceiptId([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }
            try
            {
                var result = await _inventoryReceiptService.GetMaterialReceiptIdService(id);
                if (result == null || !result.Any())
                {
                    return NotFound("No inventory receipts found for the provided ID.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateFieldChange")]
        public async Task<IActionResult> UpdateFieldChange(int id, [FromBody] FieldUpdateDTO field)
        {
            try
            {
                await _inventoryReceiptService.UpdateFieldChangeService(id, field);
                return Ok(new { message = "Field updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }
    }
}

