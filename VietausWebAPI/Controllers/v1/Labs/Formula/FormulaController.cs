using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Composition;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.FormulaFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs.Formula
{
    [ApiController]
    [Route("api/formula")]
    [AllowAnonymous]
    public class FormulaController : Controller
    {
        private readonly IFormulaService _formulaService;

        public FormulaController(IFormulaService formulaService)
        {
            _formulaService = formulaService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFormula([FromBody] PostFormula request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _formulaService.CreateAsync(request);
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
        public async Task<IActionResult> GetAllFormulas([FromQuery] FormulaQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _formulaService.GetAllAsync(query, ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpsertFormula([FromBody] PatchFormula request, CancellationToken ct = default)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _formulaService.UpsertFormulaAsync(request, ct);
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

        [HttpPatch("UpdateInformation")]
        public async Task<IActionResult> UpdateInformation([FromBody] PatchFormulaInformation request, CancellationToken ct = default)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _formulaService.UpdateInformationAsync(request, ct);
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
                var pdfBytes = await _formulaService.ExportToPdfAsync(deliveryOrderId, ct);
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
        [HttpGet("ExportXml")]
        public async Task<IActionResult> ExportXml([FromQuery] Guid productId, CancellationToken ct)
        {
            var bytes = await _formulaService.ExportToXmlAsync(productId, ct);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"formula_product_{productId}.xlsx");
        }
    }
}
