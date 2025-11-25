using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts
{
    public interface IDelivererRepository
    {
        IQueryable<Deliverer> Query(bool track = true);
        Task AddAsync(Deliverer deliverer, CancellationToken ct = default);
        Task RemoveAsync (Deliverer deliverer, CancellationToken ct = default);
    }
}
