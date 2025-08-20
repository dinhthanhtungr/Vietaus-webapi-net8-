using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures
{
    public interface ITransferCustomerService
    {
        Task<PagedResult<TransferCustomerDTO>> GetTransfersAsync(
            CustomerTransferQuery query,
            CancellationToken ct = default);

        Task<TransferCustomerDTO?> GetTransferByIdAsync(Guid id, CancellationToken ct = default);

        Task<TransferCustomerDTO> CreateTransferAsync(TransferCustomersRequest req, CancellationToken ct);
    }
}
