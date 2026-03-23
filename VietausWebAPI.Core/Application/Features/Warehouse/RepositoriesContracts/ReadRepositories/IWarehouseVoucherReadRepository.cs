using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;

namespace VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories
{
    public interface IWarehouseVoucherReadRepository : IRepository<WarehouseVoucher>
    {
        Task<WarehouseVoucher?> GetByIdAsync(long voucherId, CancellationToken ct);
    }
}
