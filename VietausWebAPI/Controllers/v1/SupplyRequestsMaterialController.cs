using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/SupplyRequestsMaterial")]
    [AllowAnonymous]
    public class SupplyRequestsMaterialController : Controller
    {
        private readonly ISupplyRequestsMaterialDatumService _supplyRequestsMaterialDatumService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="supplyRequestsMaterialDatumService"></param>
        public SupplyRequestsMaterialController (ISupplyRequestsMaterialDatumService supplyRequestsMaterialDatumService)
        {
            _supplyRequestsMaterialDatumService = supplyRequestsMaterialDatumService;
        }

        /// <summary>
        /// Thêm đề xuất mới
        /// </summary>
        /// <param name="supplyRequestsMaterialDatumDTO"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> AddSupplyRequestMaterial([FromBody] SupplyRequestsMaterialDatumDTO supplyRequestsMaterialDatumDTO)
        {
            await _supplyRequestsMaterialDatumService.AddSupplyRequestsMaterialDatumAsync(supplyRequestsMaterialDatumDTO);
            return Ok(new { Message = "Request complion" });
        }

        [HttpPost("approve-receipt")]
        public async Task<IActionResult> ApproveReceipt([FromBody] ApproveReceiptDTO supplyRequestsMaterialDatumDTO)
        {
            await _supplyRequestsMaterialDatumService.ApproveAndUpdateAsync(supplyRequestsMaterialDatumDTO);
            return Ok(new { Message = "Request complion" });
        }

        /// <summary>
        /// Lấy tất cả đề xuất
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllSupplyRequestsMaterial()
        {
            
            var result = await _supplyRequestsMaterialDatumService.GetAllSupplyRequestsMaterialDatumAsync();
            return Ok(result);
        }

        /// <summary>
        /// Cập nhật trạng thái đề xuấtt
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPatch("UpdateRequestStatus")]
        public async Task<IActionResult> UpdateRequestStatus([FromQuery] string requestId, [FromQuery] string status )
        {
            await _supplyRequestsMaterialDatumService.UpdateRequestStatusAsyncService(requestId, status);
            return Ok(new { Message = "Request status updated" });
        }

        [HttpPatch("SuccessRequestStatus")]
        public async Task<IActionResult> SuccessRequestStatus([FromBody] SuccessRequestStatusQuery query)
        {
            await _supplyRequestsMaterialDatumService.SuccessRequestStatusAsyncService(query.RequestId, query.Note, query.Status);
            return Ok(new { Message = "Request status updated" });
        }
    }
}
