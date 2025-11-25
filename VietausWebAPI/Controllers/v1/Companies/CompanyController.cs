using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.Queries;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1.Companies
{
    [ApiController]
    [Route("api/company")]
    [AllowAnonymous]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CompaniesQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _companyService.GetCompaniesAsync(query, ct);
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
    }
}
