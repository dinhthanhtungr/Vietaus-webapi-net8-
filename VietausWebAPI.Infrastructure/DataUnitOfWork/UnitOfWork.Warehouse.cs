using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IWarehouseShelfStockRepository WarehouseShelfStockRepository { get; }
        public IWarehouseTempStockRepository WarehouseTempStockRepository { get; }
        public IWarehouseRequestDetailRepository WarehouseRequestDetailRepository { get; }
        public IWarehouseRequestRepository WarehouseRequestRepository { get; }

        public IWarehouseShelfLedgerReadRepository WarehouseShelfLedgerReadRepository { get; }
        public IWarehouseVoucherReadRepository WarehouseVoucherReadRepository { get; }
        public IWarehouseVoucherDetailReadRepository WarehouseVoucherDetailReadRepository { get; }
    }
}
