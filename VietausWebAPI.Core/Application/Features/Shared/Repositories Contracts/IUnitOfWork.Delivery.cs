using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IDeliveryOrderRepository DeliveryOrderRepository { get; }
        IDeliveryOrderDetailRepository DeliveryOrderDetailRepository { get; }
        IDelivererRepository DelivererRepository { get; }
        IDelivererInforRepository DelivererInforRepository { get; }
        IDeliveryOrderPORepository DeliveryOrderPORepository { get; }
    }
}
