using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Purchases
{
    public class PurchaseOrderLinkRepository : Repository<PurchaseOrderLink>, IPurchaseOrderLinkRepository
    {
        public PurchaseOrderLinkRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
