using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductStandardFeature;
using VietausWebAPI.Core.Application.Features.Labs.Helpers;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Labs.Services;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs
{
    [ApiController]
    [Route("api/ProductInspection")]
    [AllowAnonymous]
    public class ProductInspectionController : Controller
    {
        private readonly IProductInspectionService _productInspectionService;

        public ProductInspectionController(IProductInspectionService productInspectionService)
        {
            _productInspectionService = productInspectionService;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostProductInspection([FromBody] PostProductInspectionRequest ProductInspection)
        {
            if (ProductInspection == null)
            {
                return BadRequest("Product standard cannot be null.");
            }
            var result = await _productInspectionService.PostProductInspectionServiceAsync(ProductInspection);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpGet("GetPaged")]
        public async Task<IActionResult> GetProductInspectionPaged([FromQuery] ProductInspectionQuery? query)
        {
            var result = await _productInspectionService.GetProductInspectionPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetProductInspectionById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            var productInspection = await _productInspectionService.GetProductInspectionByIdAsync(id);
            if (productInspection == null)
            {
                return NotFound($"Product inspection with ID {id} not found.");
            }
            return Ok(productInspection);
        }

        [HttpDelete("DeleteCOA/{id}")]
        public async Task<IActionResult> DeleteCOA(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            try
            {
                await _productInspectionService.DeleteCOAService(id);
                return Ok($"COA with ID {id} deleted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GeneralPdf/{id}")]
        public async Task<IActionResult> GeneralPdf(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            try
            {
                var pdfBytes = await _productInspectionService.GeneralPdfService(id);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return NotFound($"PDF for Product Inspection with ID {id} not found.");
                }
                return File(pdfBytes, "application/pdf", $"{id}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportPDF([FromQuery] StatisticalReportQuery query)
        {
            var pdfBytes = await _productInspectionService.GeneralQCPdfService(query);
            return File(pdfBytes, "application/pdf", "phieu-kiem-tra-chat-luong.pdf");
        }
    }
}
