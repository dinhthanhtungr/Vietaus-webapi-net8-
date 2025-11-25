using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;

namespace VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts
{
    public interface IWarehouseRequestDetailRepository
    {
        IQueryable<WarehouseRequestDetail> Query(bool track = true);
        Task AddAsync(WarehouseRequestDetail warehouseRequestDetail, CancellationToken ct = default);
    }
}
