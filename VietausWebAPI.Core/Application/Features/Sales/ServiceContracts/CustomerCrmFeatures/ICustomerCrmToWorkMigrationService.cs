using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Migrations;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures
{
    public interface ICustomerCrmToWorkMigrationService
    {
        Task<OperationResult<CustomerCrmToWorkMigrationResultDto>> MigrateAsync(
            CustomerCrmToWorkMigrationOptions options,
            CancellationToken ct = default);
    }
}
