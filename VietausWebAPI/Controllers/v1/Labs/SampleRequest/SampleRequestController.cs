using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Queries.CreateSampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs.SampleRequest
{
    [ApiController]
    [Route("api/samplerequest")]
    [AllowAnonymous]
    public class SampleRequestController : Controller
    {
        private readonly ISampleRequestService _sampleRequestService;
        private readonly ISampleRequestImageService _sampleRequestImageService;

        public SampleRequestController(ISampleRequestService sampleRequestService, ISampleRequestImageService sampleRequestImageService)
        {
            _sampleRequestService = sampleRequestService;
            _sampleRequestImageService = sampleRequestImageService;
        }

        /// <summary>
        /// Tạo mới một yêu cầu mẫu với sản phẩm
        /// Nếu ProductId được cung cấp, sẽ sử dụng sản phẩm đã tồn tại.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        //[ProducesResponseType(typeof(SampleRequestDTO), StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSampleRequest([FromBody] CreateSampleWithProductRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _sampleRequestService.CreateAsync(request);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
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

        [HttpGet("GetAll")]
        //[Authorize(Roles = "Admin")] // chỉ Admin/SaleManager
        public async Task<IActionResult> GetAllSampleRequests([FromQuery] SampleRequestQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _sampleRequestService.GetAllAsync(query, ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetSampleRequestById(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            try
            {
                var result = await _sampleRequestService.GetByIdAsync(id, ct);
                if (result == null)
                {
                    return NotFound("Sample request not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("Delete/{id}")]
        public async Task<IActionResult> DeleteSampleRequest(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            try
            {
                var result = await _sampleRequestService.DeleteSampleRequestAsync(id);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateSampleRequest([FromBody] UpdateSampleRequest id, CancellationToken ct = default)
        {
            if (id.SampleRequestId == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            try
            {
                var result = await _sampleRequestService.UpdateSampleRequestAsync(id, default);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("UploadImage/{sampleRequestId}")]
        [RequestSizeLimit(50 * 1024 * 1024)] // Giới hạn kích thước upload tối đa 10MB

        public async Task<IActionResult> Upload(Guid sampleRequestId, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0) return BadRequest("File rỗng.");
            var res = await _sampleRequestImageService.UploadAsync(sampleRequestId, file.FileName, file.ContentType, file.Length, file.OpenReadStream(), ct);
            return Ok(res);
        }

        [HttpGet]
        public Task<List<SampleRequestImageDto>> List(Guid sampleRequestId, CancellationToken ct)
        => _sampleRequestImageService.ListAsync(sampleRequestId, ct);

    }
}
