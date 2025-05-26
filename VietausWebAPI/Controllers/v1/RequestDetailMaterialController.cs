using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.ServiceContracts;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/RequestDetailMaterial")]
    [AllowAnonymous]
    public class RequestDetailMaterialController : Controller
    {
        private readonly IRequestDetailMaterialService _requestDetailMaterialService;
        private readonly IMaterialRequestDetailService _materialRequestDetailService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestDetailMaterialService"></param>
        public RequestDetailMaterialController (IRequestDetailMaterialService requestDetailMaterialService, IMaterialRequestDetailService materialRequestDetailService)
        {
            _requestDetailMaterialService = requestDetailMaterialService;
            _materialRequestDetailService = materialRequestDetailService;
        }
        /// <summary>
        /// Thêm mới danh sách các vật tư đề xuất đi chung với lại request
        /// </summary>
        /// <param name="requestDetailMaterialDatumPostDTO"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> AddRequestDetailMaterial([FromBody] RequestDetailMaterialDatumPostDTO requestDetailMaterialDatumPostDTO)
        {
            await _requestDetailMaterialService.AddRequestDetailServiceAsync(requestDetailMaterialDatumPostDTO);
            return Ok(new { Message = "Request complion" });
        }
        /// <summary>
        /// Lấy tất cả danh sách vật tư đề xuất
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllRequestDetailMaterial()
        {
            var result = await _requestDetailMaterialService.GetAllRequestDetailServiceAsync();
            return Ok(result);
        }

        //[HttpGet("GetSearch")]
        //public async Task<IActionResult> GetSearchRequestDetailMaterial(string requestId)
        //{
        //    var result = await _requestDetailMaterialService.GetSearchRequestDetailServiceAsync(requestId);
        //    return Ok(result);
        //}        
        
        [HttpGet("GetSearch")]
        public async Task<IActionResult> GetSearchRequestDetailMaterial(string requestId)
        {
            var result = await _materialRequestDetailService.GetRequestDetailServieAsync(requestId);
            return Ok(result);
        }
    }
}
