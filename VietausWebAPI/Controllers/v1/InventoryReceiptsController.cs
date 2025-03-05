using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.DTO;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/InventoryReceipts/Add")]
    [AllowAnonymous]
    public class InventoryReceiptsController : Controller
    {
        private readonly IInventoryReceiptsService _inventoryReceiptsService;

        public InventoryReceiptsController(IInventoryReceiptsService inventoryReceiptsService)
        {
            _inventoryReceiptsService = inventoryReceiptsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddInventoryReceipts([FromBody] InventoryReceiptsDTO inventoryReceiptsDTO)
        {
            await _inventoryReceiptsService.AddInventoryReceiptsAsync(inventoryReceiptsDTO);
            return Ok( new { message = "Request complion" });
        }
    }
}

