using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts
{
    public interface IDeliveryOrderRepository
    {
        IQueryable<DeliveryOrder> Query(bool track = true);
        Task AddAsync(DeliveryOrder deliveryOrder, CancellationToken ct = default);
    }
}
