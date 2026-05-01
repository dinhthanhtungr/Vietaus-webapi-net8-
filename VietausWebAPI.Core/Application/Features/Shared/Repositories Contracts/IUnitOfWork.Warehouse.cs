using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IWarehouseShelfStockRepository WarehouseShelfStockRepository { get; }
        IWarehouseTempStockRepository WarehouseTempStockRepository { get; }
        IWarehouseRequestDetailRepository WarehouseRequestDetailRepository { get; }
        IWarehouseRequestRepository WarehouseRequestRepository { get; }

        IWarehouseShelfLedgerReadRepository WarehouseShelfLedgerReadRepository { get; }
        IWarehouseVoucherReadRepository WarehouseVoucherReadRepository { get; }
        IWarehouseVoucherDetailReadRepository WarehouseVoucherDetailReadRepository { get; }
    }
}
