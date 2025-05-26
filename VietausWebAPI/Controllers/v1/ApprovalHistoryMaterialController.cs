using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.Application.Usecases.Approvals.ServiceContracts;
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
        private readonly IApprovalService _approvalService;
        private readonly IGetApprovalRequestAndInventoryService _getApprovalRequestAndInventoryService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="approvalHistoryMaterialService"></param>
        public ApprovalHistoryMaterialController(IApprovalHistoryMaterialService approvalHistoryMaterialService, IApprovalService approvalService, IGetApprovalRequestAndInventoryService getApprovalRequestAndInventoryService)
        {
            _approvalHistoryMaterialService = approvalHistoryMaterialService;
            _approvalService = approvalService;
            _getApprovalRequestAndInventoryService = getApprovalRequestAndInventoryService;
        }
        /// <summary>
        /// Lấy lịch sử phê duyệt vật tư
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        public async Task<IActionResult> GetAllApprovalHistoryMaterial([FromQuery] string? query)
        {
            var result = await _approvalHistoryMaterialService.GetApprovalHistoryMaterialServiceAsync(query);
            return Ok(result);
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

        //[HttpPatch("ChangeApprovalHistory")]
        //public async Task<IActionResult> ChangeApprovalHistory([FromQuery] string requestId, [FromQuery] string note, [FromQuery] string EmployeeId)
        //{
        //    await _approvalHistoryMaterialService.ChangeApprovalHistoryServiceAsync(requestId, note, EmployeeId);
        //    return Ok();
        //}

        [HttpPost("SaveApprovalRequestService")]
        public async Task<IActionResult> SaveApprovalHistory([FromBody] ApprovalRequestDTO request)
        {
            await _approvalService.SaveApprovalRequestService(request);
            return Ok(new { Message = "successfully" });
        }

        [HttpGet("GetApprovalRequest")]
        public async Task<IActionResult> GetApprovalRequest([FromQuery] SupplyRequestsQuery approvalQuery)
        {
            var result = await _approvalService.SendApprovalRequestService(approvalQuery);
            return Ok(result);
        }

        [HttpPost("approve-receipt")]
        public async Task<IActionResult> ApproveReceipt([FromBody] ApprovalHistoryAndInventoryRequestDTO supplyRequestsMaterialDatumDTO)
        {
            await _getApprovalRequestAndInventoryService.ExecuteAsync(supplyRequestsMaterialDatumDTO);
            return Ok(new { Message = "Request complion" });
        }
    }
}
