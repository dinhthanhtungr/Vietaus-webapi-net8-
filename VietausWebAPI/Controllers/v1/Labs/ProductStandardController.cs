using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductStandardFeature;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductStandardFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs
{
    [ApiController]
    [Route("api/ProductStandard")]
    [AllowAnonymous]
    public class ProductStandardController : Controller
    {
        private readonly IProductStandardService _productStandardService;

        public ProductStandardController(IProductStandardService productStandardService)
        {
            _productStandardService = productStandardService;
        }
        [HttpGet("GetSummary")]
        public async Task<IActionResult> GetProductStandards([FromQuery] ProductStandardQuery? query)
        {
            var result = await _productStandardService.GetProductStandardsPagedAsync(query);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetProductStandardById(Guid id)
        {
            var result = await _productStandardService.GetProductStandardIdAsync(id);
            return Ok(result);
        }
        [HttpGet("GetByProductId/{id}")]
        public async Task<IActionResult> GetProductStandardByProductId(Guid id)
        {
            try
            {
                var result = await _productStandardService.GetProductStandardProductIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Product standard with Product ID {id} not found.");
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Product standard not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log exception ở đây nếu cần
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostProductStandard([FromBody] ProductStandardInformation productStandard)
        {
            if (productStandard == null)
            {
                return BadRequest("Product standard cannot be null.");
            }
            await _productStandardService.PostProductStandardService(productStandard);
            return Ok();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateProductStandard(Guid id, [FromBody] ProductStandardInformation productStandard)
        {
            if (productStandard == null)
            {
                return BadRequest("Product standard cannot be null.");
            }
            await _productStandardService.UpdateProductStandardService(id, productStandard);
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProductStandard(Guid id)
        {
            await _productStandardService.DeleteProductStandardService(id);
            return Ok();
        }
    }
}
