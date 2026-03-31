using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PostDtos;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs.SampleRequest.ColorChipRecords
{
    [ApiController]
    [Route("api/color-chip-records")]
    [AllowAnonymous]
    public class ColorChipRecordPostController : Controller
    {
        private readonly IColorChipRecordWriteServices _colorChipRecordWriteServices;

        public ColorChipRecordPostController(IColorChipRecordWriteServices colorChipRecordWriteServices)
        {
            _colorChipRecordWriteServices = colorChipRecordWriteServices;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateColorChipRecord([FromBody] CreateColorChipRecordRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _colorChipRecordWriteServices.CreateAsync(request);
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
    }
}
