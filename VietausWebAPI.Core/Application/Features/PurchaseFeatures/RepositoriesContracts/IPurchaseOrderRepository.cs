using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts
{
    public interface IPurchaseOrderRepository
    {
        IQueryable<PurchaseOrder> Query(bool track = true);
        Task AddAsync(PurchaseOrder PurchaseOrder, CancellationToken ct = default);
    }
}
