using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IPurchaseOrderRepository PurchaseOrderRepository { get; }
        public IPurchaseOrderDetailRepository PurchaseOrderDetailRepository { get; }
        public IPurchaseOrderSnapshotRepository PurchaseOrderSnapshotRepository { get; }
        public IPurchaseOrderLinkRepository PurchaseOrderLinkRepository { get; }
    }
}
