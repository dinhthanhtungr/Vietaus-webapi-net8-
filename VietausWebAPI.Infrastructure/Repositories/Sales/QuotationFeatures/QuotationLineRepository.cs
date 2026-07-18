using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.QuotationFeatures;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Sales.QuotationFeatures
{
    public class QuotationLineRepository : Repository<QuotationLine>, IQuotationLineRepository
    {
        public QuotationLineRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
