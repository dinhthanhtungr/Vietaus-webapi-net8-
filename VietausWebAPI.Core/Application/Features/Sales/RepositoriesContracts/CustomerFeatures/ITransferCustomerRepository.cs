using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures
{
    public interface ITransferCustomerRepository
    {
        //Task<PagedResult<CustomerTransferLog>> GetPageCustomerTransferLog(CustomerTransferQuery query);
        //Task<TransferCustomerDTO?> GetTransferApiDtoAsync(Guid logId, CancellationToken ct = default);
        IQueryable<CustomerTransferLog> Query(); // base query
        Task AddAsync(CustomerTransferLog log, CancellationToken ct = default);

    }
}
