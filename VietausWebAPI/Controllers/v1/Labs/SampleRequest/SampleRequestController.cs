using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Queries.CreateSampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.WebAPI.Helpers;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs.SampleRequest
{
    [ApiController]
    [Route("api/samplerequest")]
    [AllowAnonymous]
    public class SampleRequestController : Controller
    {
        private readonly ISampleRequestService _sampleRequestService;
        private readonly ICurrentUser _CurrentUser;

        public SampleRequestController(ISampleRequestService sampleRequestService, ICurrentUser currentUser)
        {
            _sampleRequestService = sampleRequestService;
            _CurrentUser = currentUser;
        }

        /// <summary>
        /// Tạo mới một yêu cầu mẫu với sản phẩm
        /// Nếu ProductId được cung cấp, sẽ sử dụng sản phẩm đã tồn tại.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Create")]
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
                    return BadRequest(result);        // body vẫn là OperationResult

                return StatusCode(StatusCodes.Status201Created, result);
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


        [HttpGet("GetForVU")]
        //[Authorize(Roles = "Admin")] // chỉ Admin/SaleManager
        public async Task<IActionResult> GetSampleRequestForVUManufacturing([FromQuery] ProductQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _sampleRequestService.GetSampleRequestForVUManufacturing(query, ct);
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
        public async Task<IActionResult> UpdateSampleRequest([FromBody] UpdateSampleRequest request, CancellationToken ct = default)
        {
            if (request.SampleRequestId == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            try
            {
                if(request.Status == "New")
                {
                    request.CreatedBy = _CurrentUser.EmployeeId;
                }
                request.UpdatedBy = _CurrentUser.EmployeeId;
                var result = await _sampleRequestService.UpdateSampleRequestAsync(request, default);
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

        [HttpPatch("update-colour-code-name/{id}")]
        public async Task<IActionResult> UpdateColourCodeName(Guid id, [FromQuery] string newColourCode, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            if (string.IsNullOrWhiteSpace(newColourCode))
            {
                return BadRequest("New colour code cannot be empty.");
            }
            try
            {
                var result = await _sampleRequestService.UpdateColourCodeName(id, newColourCode, ct);
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

        //[HttpPost("UploadImage/{sampleRequestId}")]
        //[RequestSizeLimit(50 * 1024 * 1024)] // Giới hạn kích thước upload tối đa 10MB

        //public async Task<IActionResult> Upload(Guid sampleRequestId, IFormFile file, CancellationToken ct)
        //{
        //    if (file is null || file.Length == 0) return BadRequest("File rỗng.");
        //    var res = await _sampleRequestImageService.UploadAsync(sampleRequestId, file.FileName, file.ContentType, file.Length, file.OpenReadStream(), ct);
        //    return Ok(res);
        //}

        //[HttpGet]
        //public Task<List<SampleRequestImageDto>> List(Guid sampleRequestId, CancellationToken ct)
        //=> _sampleRequestImageService.ListAsync(sampleRequestId, ct);

    }
}
