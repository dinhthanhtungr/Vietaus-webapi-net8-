using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts
{
    public interface IPurchaseOrderDetailRepository
    {
        IQueryable<PurchaseOrderDetail> Query(bool track = true);
        Task AddAsync(PurchaseOrderDetail PurchaseOrderDetail, CancellationToken ct = default);
    }
}
