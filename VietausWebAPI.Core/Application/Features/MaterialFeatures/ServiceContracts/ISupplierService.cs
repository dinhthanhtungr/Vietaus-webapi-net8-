using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts
{
    public interface ISupplierService
    {
        Task<OperationResult> AddNewSuplier(PostSupplier supplier, CancellationToken ct = default);
        Task<OperationResult<PagedResult<GetSupplierSummary>>> GetAllAsync(SupplierQuery query, CancellationToken ct = default);

        Task<OperationResult<GetSupplier>> GetSupplierByIdAsync(Guid id, CancellationToken ct = default);
        Task<OperationResult> UpdateCustomerAsync(PatchSupplier supplier);

        Task<OperationResult> DeleteSupplierByIdAsync(Guid id);
    }
}
