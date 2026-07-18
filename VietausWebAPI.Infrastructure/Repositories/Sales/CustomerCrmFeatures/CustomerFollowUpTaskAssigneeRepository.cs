using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Sales.CustomerCrmFeatures
{
    public class CustomerFollowUpTaskAssigneeRepository : Repository<CustomerFollowUpTaskAssignee>, ICustomerFollowUpTaskAssigneeRepository
    {
        public CustomerFollowUpTaskAssigneeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
