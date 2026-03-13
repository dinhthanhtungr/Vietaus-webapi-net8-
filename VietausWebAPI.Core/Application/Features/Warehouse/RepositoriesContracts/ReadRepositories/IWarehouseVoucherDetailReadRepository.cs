using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;

namespace VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories
{
    public interface IWarehouseVoucherDetailReadRepository
    {
        Task<WarehouseVoucherDetail?> GetByIdAsync(long voucherDetailId, CancellationToken ct);
    }
}
