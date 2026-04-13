using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PostDtos;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature.ColorChipRecordFeatures;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs.SampleRequest.ColorChipRecords
{
    [ApiController]
    [Route("api/color-chip-records")]
    [AllowAnonymous]
    public class ColorChipRecordController : Controller
    {
        private readonly IColourChipRecordWriteServices _colorChipRecordWriteServices;
        private readonly IColourChipRecordReadServices _colorChipRecordReadServices;
        private readonly IColourChipRecordPrintPDFService _colourChipRecordPrintPDFService;

        public ColorChipRecordController(IColourChipRecordWriteServices colorChipRecordWriteServices
            , IColourChipRecordReadServices colorChipRecordReadServices
            , IColourChipRecordPrintPDFService colourChipRecordPrintPDFService)         
        {
            _colorChipRecordWriteServices = colorChipRecordWriteServices;
            _colorChipRecordReadServices = colorChipRecordReadServices;
            _colourChipRecordPrintPDFService = colourChipRecordPrintPDFService;
        }

        [HttpGet("product/{id:guid}")]
        public async Task<IActionResult> GetColorChipRecordByProductId(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _colorChipRecordReadServices.GetByProductIdAsync(id, cancellationToken);

                if (!result.Success)
                    return NotFound(result);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
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

        [HttpGet("product/{id:guid}/print-pdf")]
        public async Task<IActionResult> PrintColorChipRecordPdfByProductId(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _colourChipRecordPrintPDFService.PrintByProductIdAsync(id, cancellationToken);

                if (!result.Success)
                    return NotFound(result);

                if (result.Data == null || result.Data.Length == 0)
                    return BadRequest("PDF file is empty.");

                var fileName = $"ColorChipRecord_{id:N}.pdf";

                return File(result.Data, "application/pdf", fileName);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
