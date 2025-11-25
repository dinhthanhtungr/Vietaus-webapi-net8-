using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts
{
    public interface IPurchaseOrderLinkRepository : IRepository<PurchaseOrderLink> 
    {
    }
}
