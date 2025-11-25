using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IPurchaseOrderRepository PurchaseOrderRepository { get; }
        IPurchaseOrderDetailRepository PurchaseOrderDetailRepository { get; }
        IPurchaseOrderSnapshotRepository PurchaseOrderSnapshotRepository { get; }
        IPurchaseOrderLinkRepository PurchaseOrderLinkRepository { get; }
    }
}
