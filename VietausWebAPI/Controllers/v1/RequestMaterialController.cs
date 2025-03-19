using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.DTO.PostDTO;
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
 
        public RequestMaterialController (IRequestMaterialService requestMaterialService)
        {
            _requestMaterialService = requestMaterialService;
        }       

        [HttpPost("RequestData")]
        public async Task<IActionResult> CreateRequest([FromBody] RequestDTO requestDTO)
        {
            try
            {
                var requestId = await _requestMaterialService.CreateRequestMaterial(requestDTO);
                return Ok(new { requestId = requestId });
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            }
        }

        //[HttpGet("ResponseRequestID")]
        //public async 
}   
