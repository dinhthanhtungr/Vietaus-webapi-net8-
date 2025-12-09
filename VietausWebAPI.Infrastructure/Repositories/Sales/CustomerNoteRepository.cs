using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class CustomerNoteRepository : Repository<CustomerNote>, ICustomerNoteRepository
    {
        public CustomerNoteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
