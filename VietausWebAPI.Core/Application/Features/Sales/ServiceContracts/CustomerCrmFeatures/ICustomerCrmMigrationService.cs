using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Migrations;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures
{
    public interface ICustomerCrmMigrationService
    {
        Task<byte[]> ExportLegacyWorkToCustomerCrmWorkbookAsync(
            CustomerCrmLegacyExportQuery query,
            CancellationToken ct = default);

        Task<byte[]> ExportCurrentCustomerCrmWorkbookAsync(
            CustomerCrmLegacyExportQuery query,
            CancellationToken ct = default);

        Task<OperationResult<CustomerCrmMigrationImportResultDto>> ImportCustomerCrmWorkbookAsync(
            Stream workbookStream,
            CustomerCrmMigrationImportOptions options,
            CancellationToken ct = default);
    }
}
