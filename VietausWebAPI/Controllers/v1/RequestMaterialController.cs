using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.Infrastructure.Repositories;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class RequestMaterialController : Controller
    {
        private readonly IRequestMaterialService _requestMaterialService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestMaterialService"></param>
        public RequestMaterialController (IRequestMaterialService requestMaterialService)
        {
            _requestMaterialService = requestMaterialService;
        }
        /// <summary>
        /// Tạo một đề xuất mua vật tư với đầy đủ các thông số
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        //[HttpPost("RequestData")]
        //public async Task<IActionResult> CreateRequest([FromBody] RequestMaterialDTO requestDTO)
        //{
        //    try
        //    {
        //        var requestId = await _requestMaterialService.CreateRequestMaterial(requestDTO);
        //        return Ok(new { Message = "success" });
        //    }

        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        /// <summary>
        /// Lay ra ma de xuat cuoi cung
        /// </summary>
        /// <returns></returns>
        [HttpGet("ResponseRequestID")]
        public async Task<IActionResult> GetGetLastRequestId()
        {
            var result = await _requestMaterialService.GetLastRequestIdService();

            return Ok(new { RequestId = result });
        }
        /// <summary>
        /// Tim kiếm theo các điều kiện 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("ResponseMaterials")]
        public async Task<IActionResult> GetMaterial([FromQuery] RequestMaterialQuery? query)
        {
            var result = await _requestMaterialService.GetMaterialAsyncService(query);
            return Ok(result);
        }

        /// <summary>
        /// Lấy ra danh sách đã được trải phẳng vật tư theo các điều kiện tìm kiếm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("FlatRequestMaterials")]
        public async Task<IActionResult> GetFlatRequestMaterial([FromQuery] RequestMaterialQuery? query)
        {
            var result = await _requestMaterialService.FlatRequestMaterialService(query);
            return Ok(result);
        }

    }

}   
