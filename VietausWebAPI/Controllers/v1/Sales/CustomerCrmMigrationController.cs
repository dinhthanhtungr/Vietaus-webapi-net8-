using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Migrations;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/customer-crm-migration")]
    [Authorize]
    public sealed class CustomerCrmMigrationController : ControllerBase
    {
        private readonly ICustomerCrmMigrationService _migrationService;

        public CustomerCrmMigrationController(ICustomerCrmMigrationService migrationService)
        {
            _migrationService = migrationService;
        }

        [HttpGet("export")]
        [Authorize(Roles = RoleSets.CanSeeAllCustomer)]
        public async Task<IActionResult> ExportLegacyWorkToCustomerCrmWorkbook(
            [FromQuery] CustomerCrmLegacyExportQuery query,
            CancellationToken ct)
        {
            var bytes = await _migrationService.ExportCurrentCustomerCrmWorkbookAsync(query, ct);
            var fileName = $"customer-crm-migration-{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
