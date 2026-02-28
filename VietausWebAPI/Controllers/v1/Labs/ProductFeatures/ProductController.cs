using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.ProductFeatures;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs.ProductFeatures
{
    [ApiController]
    [Route("api/product")]
    [AllowAnonymous]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _productService.GetAllAsync(query, ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetActionResultAsync([FromQuery] Guid query, CancellationToken ct = default)
        {
            try
            {
                var result = await _productService.GetInformationByIdAsync(query, ct);
                return Ok(result);
            }

            catch(Exception ex)
            {
                return StatusCode(500, "Error Db API");
            }
        }


        [HttpPatch("ChangeCustomerByProduct")]
        public async Task<IActionResult> ChangeCustomerByProduct([FromBody] ChangeCustomerForProductRequest request, CancellationToken ct = default)
        {
            if (request == null) return BadRequest("Request cannot be null.");
            if (request.ProductId == Guid.Empty) return BadRequest("Invalid ProductId.");
            if (request.NewCustomerId == Guid.Empty) return BadRequest("Invalid CustomerId.");

            try
            {
                var result = await _productService.ChangeCustomerByProductAsync(
                    request, ct);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result); // result.Data = số SR đã update
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
