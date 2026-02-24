using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.Queries;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Catergories;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;

namespace VietausWebAPI.WebAPI.Controllers.v1.Materials
{
    [ApiController]
    [Route("api/category")]
    [AllowAnonymous]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _CategoryService;
        private readonly IExternalIdService _externalIdService;

        public CategoryController(ICategoryService categoryService, IExternalIdService externalIdService)
        {
            _CategoryService = categoryService;
            _externalIdService = externalIdService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CategoryQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _CategoryService.GetCategoriesAsync(query, ct);
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

        [HttpGet("unit")]
        public async Task<IActionResult> GetUnits([FromQuery] UnitQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _CategoryService.GetUnitsAsync(query, ct);
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

        [HttpGet("ExternalIDGenerate")]
        public async Task<IActionResult> ExternalIDGenerate([FromQuery] string prefix, CancellationToken ct = default)
        {
            try
            {
                var result = await _externalIdService.NextAsync(prefix, ct);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, $"{ex.Message}");
            }
        }
    }
}
