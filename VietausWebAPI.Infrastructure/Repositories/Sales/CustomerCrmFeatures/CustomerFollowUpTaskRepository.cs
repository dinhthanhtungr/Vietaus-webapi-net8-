using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Sales.CustomerCrmFeatures
{
    public class CustomerFollowUpTaskRepository : Repository<CustomerFollowUpTask>, ICustomerFollowUpTaskRepository
    {
        public CustomerFollowUpTaskRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
