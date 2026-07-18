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
    public class CustomerWorkPlanRepository : Repository<CustomerWorkPlan>, ICustomerWorkPlanRepository
    {
        public CustomerWorkPlanRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
