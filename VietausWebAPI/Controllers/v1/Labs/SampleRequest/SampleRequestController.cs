using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs.SampleRequest
{
    [ApiController]
    [Route("api/samplerequest")]
    [AllowAnonymous]
    public class SampleRequestController : Controller
    {
        private readonly ISampleRequestService _sampleRequestService;

        public SampleRequestController(ISampleRequestService sampleRequestService)
        {
            _sampleRequestService = sampleRequestService;
        }

        /// <summary>
        /// Tạo mới một yêu cầu mẫu với sản phẩm
        /// Nếu ProductId được cung cấp, sẽ sử dụng sản phẩm đã tồn tại.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(SampleRequestDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSampleRequest([FromBody] CreateSampleWithProductRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _sampleRequestService.CreateAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
