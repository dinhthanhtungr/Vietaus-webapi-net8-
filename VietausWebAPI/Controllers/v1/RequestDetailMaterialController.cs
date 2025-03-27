using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestDetailMaterialService"></param>
        public RequestDetailMaterialController (IRequestDetailMaterialService requestDetailMaterialService)
        {
            _requestDetailMaterialService = requestDetailMaterialService;
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
    }
}
