using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures
{
    public interface ICustomerTransferLogRepository
    {
        // Base query: NoTracking để chỉ đọc (Service sẽ .Where/.Select/.ToListAsync)
        IQueryable<CustomerTransferLog> Query();

        // Thêm 1 log (header)
        Task AddAsync(CustomerTransferLog log, CancellationToken ct = default);
    }

}
