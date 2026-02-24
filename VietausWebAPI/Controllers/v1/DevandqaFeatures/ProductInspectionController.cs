using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VietausWebAPI.WebAPI.Controllers.v1.DevandqaFeatures
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

        [HttpGet("GetPaged")]
        public async Task<IActionResult> GetProductInspections([FromQuery] ProductInspectionQuery query)
        {

            try
            {
                var result = await _productInspectionService.GetProductInspectionPagedAsync(query, CancellationToken.None);
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

        [HttpGet("GetProductCOA")]
        public async Task<IActionResult> GetProductCOA([FromQuery] ProductInspectionQuery id)
        {

            try
            {
                var result = await _productInspectionService.GetProductCOAService(id, CancellationToken.None);
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

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {

            try
            {
                var result = await _productInspectionService.GetProductInspectionByIdAsync(id, CancellationToken.None);
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

        [HttpPost("Post")]
        public async Task<IActionResult> PostProductInspection([FromBody] PostProductInspectionRequest productInspection)
        {
            if (productInspection == null)
            {
                return BadRequest("Product inspection cannot be null.");
            }
            await _productInspectionService.PostProductInspectionServiceAsync(productInspection, CancellationToken.None);
            return Ok();
        }

        [HttpGet("GeneralPdf/{id:guid}")]
        public async Task<IActionResult> GeneralPdf([FromRoute] Guid id, CancellationToken ct)
        {
            var rs = await _productInspectionService.GeneralPdfService(id, ct);

            if (!rs.Success || rs.Data is null || rs.Data.Length == 0)
                return BadRequest(rs.Message ?? "Không tạo được PDF.");

            // ✅ Trả bytes PDF (FE ReadAsByteArrayAsync vẫn hoạt động)
            return File(rs.Data, "application/pdf");
        }
    }
}
