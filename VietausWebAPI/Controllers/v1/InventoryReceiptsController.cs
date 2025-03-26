using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IInventoryReceiptsService _inventoryReceiptsService;

        public InventoryReceiptsController(IInventoryReceiptsService inventoryReceiptsService)
        {
            _inventoryReceiptsService = inventoryReceiptsService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddInventoryReceipts([FromBody] InventoryReceiptsDTO inventoryReceiptsDTO)
        {
            await _inventoryReceiptsService.AddInventoryReceiptsServiceAsync(inventoryReceiptsDTO);
            return Ok( new { message = "Request complion" });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllInventoryReceipts()
        {
            var resuilt = await _inventoryReceiptsService.GetAllInventoryReceiptsServiceAsync();
            return Ok(resuilt);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchInventoryReceipts([FromQuery] InventoryReceiptsQuery? query)
        {
            var resuilt = await _inventoryReceiptsService.AddInventoryReceiptsServiceAsync(query);
            return Ok(resuilt);
        }
    }
}

