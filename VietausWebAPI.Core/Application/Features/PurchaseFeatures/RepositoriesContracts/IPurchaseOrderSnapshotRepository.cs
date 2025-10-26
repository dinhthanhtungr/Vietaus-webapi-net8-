using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts
{
    public interface IPurchaseOrderSnapshotRepository
    {
        IQueryable<PurchaseOrderSnapshot> Query(bool track = true);
        Task AddAsync(PurchaseOrderSnapshot PurchaseOrderSnapshot, CancellationToken ct = default);
    }
}
