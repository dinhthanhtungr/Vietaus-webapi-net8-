using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IDeliveryOrderRepository DeliveryOrderRepository { get; }
        public IDeliveryOrderDetailRepository DeliveryOrderDetailRepository { get; }
        public IDelivererRepository DelivererRepository { get; }
        public IDelivererInforRepository DelivererInforRepository { get; }
        public IDeliveryOrderPORepository DeliveryOrderPORepository { get; }
    }
}
