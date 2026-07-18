using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Migrations;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/customer-crm-to-work-migration")]
    [Authorize(Roles = RoleSets.Admins)]
    public sealed class CustomerCrmToWorkMigrationController : ControllerBase
    {
        private readonly ICustomerCrmToWorkMigrationService _migrationService;

        public CustomerCrmToWorkMigrationController(ICustomerCrmToWorkMigrationService migrationService)
        {
            _migrationService = migrationService;
        }

        [HttpPost("run")]
        public async Task<IActionResult> Run(
            [FromBody] CustomerCrmToWorkMigrationOptions? options,
            CancellationToken ct)
        {
            var result = await _migrationService.MigrateAsync(options ?? new CustomerCrmToWorkMigrationOptions(), ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
