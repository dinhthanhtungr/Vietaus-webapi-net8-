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
        /// <summary>
        /// Khởi tạo đối tượng InventoryReceiptsController
        /// </summary>
        /// <param name="inventoryReceiptsService"></param>
        public InventoryReceiptsController(IInventoryReceiptsService inventoryReceiptsService)
        {
            _inventoryReceiptsService = inventoryReceiptsService;
        }
        /// <summary>
        /// Thêm mới danh sách phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsDTO"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> AddInventoryReceipts([FromBody] InventoryReceiptsDTO inventoryReceiptsDTO)
        {
            await _inventoryReceiptsService.AddInventoryReceiptsServiceAsync(inventoryReceiptsDTO);
            return Ok( new { message = "Request complion" });
        }
        /// <summary>
        /// Lấy tất cả danh sách phiếu nhập kho
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllInventoryReceipts()
        {
            var resuilt = await _inventoryReceiptsService.GetAllInventoryReceiptsServiceAsync();
            return Ok(resuilt);
        }
        /// <summary>
        /// Tìm kiếm danh sách phiếu nhập kho theo các tiêu chí tìm kiếm và trả về kết quả phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IActionResult> SearchInventoryReceipts([FromQuery] InventoryReceiptsQuery? query)
        {
            var resuilt = await _inventoryReceiptsService.AddInventoryReceiptsServiceAsync(query);
            return Ok(resuilt);
        }
        /// <summary>
        /// Cập nhật giá cho danh sách phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsUpdatePriceDTO"></param>
        /// <returns></returns>
        [HttpPatch("UpdatePrice")]
        public async Task<IActionResult> UpdatePrice([FromBody] InventoryReceiptsUpdatePriceDTO inventoryReceiptsUpdatePriceDTO)
        {
            await _inventoryReceiptsService.UpdateInventoryReceiptsServiceAsync(inventoryReceiptsUpdatePriceDTO);
            return Ok(new { message = "Request complion" });
        }
    }
}

