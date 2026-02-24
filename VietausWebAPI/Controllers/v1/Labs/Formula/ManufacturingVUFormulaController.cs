using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.FormulaFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.FormulaFeatures;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs.Formula
{
    [ApiController]
    [Route("api/manufacturing-vu-formula")]
    [AllowAnonymous]
    public class ManufacturingVUFormulaController : Controller
    {
        private readonly IManufacturingVUFormulaService _manufacturingVUFormulaService;

        public ManufacturingVUFormulaController(IManufacturingVUFormulaService manufacturingVUFormulaService)
        {
            _manufacturingVUFormulaService = manufacturingVUFormulaService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateManufacturingVUFormula([FromBody] PostManufacturingVUFormula request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _manufacturingVUFormulaService.CreateAsync(request);
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

        [HttpGet("GetById/{id:guid}")]
        public async Task<IActionResult> GetManufacturingVUFormulaById([FromRoute] Guid id, CancellationToken ct = default)
        {
            try
            {
                var result = await _manufacturingVUFormulaService.GetAsync(id, ct);
                if (!result.Success)
                {
                    return NotFound(result.Message);
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
        public async Task<IActionResult> GetAllManufacturingVUFormulas([FromQuery] ManufacturingVUFormulaQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _manufacturingVUFormulaService.GetAllAsync(query, ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateManufacturingVUFormula([FromBody] PatchManufacturingVUFormula request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _manufacturingVUFormulaService.UpdateAsync(request);
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



        [HttpGet("GeneratePdf")]
        public async Task<IActionResult> GenerateDeliveryOrderPdf([FromQuery] Guid deliveryOrderId, CancellationToken ct = default)
        {
            if (deliveryOrderId == Guid.Empty)
            {
                return BadRequest("Delivery Order ID cannot be empty.");
            }
            try
            {
                var pdfBytes = await _manufacturingVUFormulaService.ExportToPdfAsync(deliveryOrderId, ct);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return NotFound("Delivery Order not found or PDF generation failed.");
                }
                return File(pdfBytes, "application/pdf", $"DeliveryOrder_{deliveryOrderId}.pdf");
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred during PDF generation.");
            }
        }
    }
}
