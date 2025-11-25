using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts
{
    public interface IDeliveryOrderPORepository
    {
        IQueryable<DeliveryOrderPO> Query(bool track = true);
        Task AddAsync(DeliveryOrderPO deliveryOrderPO, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<DeliveryOrderPO> deliveryOrderPOs, CancellationToken ct = default);
    }
}
