using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts
{
    public interface IDeliveryOrderDetailRepository
    {
        IQueryable<DeliveryOrderDetail> Query(bool track = true);
        Task AddAsync(DeliveryOrderDetail deliveryOrderDetail, CancellationToken ct = default);
    }
}
