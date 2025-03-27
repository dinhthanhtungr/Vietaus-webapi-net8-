using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/ApprovalHistoryMaterial")]
    [AllowAnonymous]
    public class ApprovalHistoryMaterialController : Controller
    {
        private readonly IApprovalHistoryMaterialService _approvalHistoryMaterialService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="approvalHistoryMaterialService"></param>
        public ApprovalHistoryMaterialController(IApprovalHistoryMaterialService approvalHistoryMaterialService)
        {
            _approvalHistoryMaterialService = approvalHistoryMaterialService;
        }
        /// <summary>
        /// Lấy lịch sử phê duyệt vật tư
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        public async Task<IActionResult> GetAllApprovalHistoryMaterial([FromQuery] string? query)
        {
            await _approvalHistoryMaterialService.GetApprovalHistoryMaterialServiceAsync(query);
            return Ok(new { Message = "Request complion" });
        }
        /// <summary>
        /// Thêm lịch sử phê duyệt vật tư
        /// </summary>
        /// <param name="approvalHistoryMaterialPostDTO"></param>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> AddApprovalHistoryMaterial([FromBody] ApprovalHistoryMaterialPostDTO approvalHistoryMaterialPostDTO)
        {
            await _approvalHistoryMaterialService.AddApprovalHistoryMaterialServiceAsync(approvalHistoryMaterialPostDTO);
            return Ok(new { Message = "Approval history material added successfully" });
        }
    }
}
