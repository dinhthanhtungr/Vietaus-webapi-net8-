using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.GetDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts.SupplierFeatures   
{
    public interface ISupplierReadRepository
    {
        // ======================================================================== Get ======================================================================== 
        Task<PagedResult<GetSupplierSummary>> GetPagedSummaryAsync(SupplierQuery query, CancellationToken ct);
        Task<GetSupplier?> GetDetailAsync(Guid id, CancellationToken ct);
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix, CancellationToken ct);
    }
}
