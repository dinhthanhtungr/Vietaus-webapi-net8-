using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1.DevandqaFeatures
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
        public async Task<IActionResult> GetProductStandards([FromQuery] ProductStandardQuery query)
        {
            var result = await _productStandardService.GetPagedListAsync(query, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetProductStandardById([FromQuery] ProductStandardQuery id)
        {
            var result = await _productStandardService.GetByIdAsync(id, CancellationToken.None);
            return Ok(result);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostProductStandard([FromBody] PostProductStandard productStandard)
        {
            if (productStandard == null)
            {
                return BadRequest("Product standard cannot be null.");
            }
            await _productStandardService.AddAsync(productStandard, CancellationToken.None);
            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProductStandard([FromBody] PatchProductStandard productStandard)
        {
            if (productStandard == null)
            {
                return BadRequest("Product standard cannot be null.");
            }
            await _productStandardService.UpdateAsync(productStandard, CancellationToken.None);
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProductStandard(Guid id)
        {
            await _productStandardService.SoftDeleteAsync(id, CancellationToken.None);
            return Ok();
        }
    }
}
