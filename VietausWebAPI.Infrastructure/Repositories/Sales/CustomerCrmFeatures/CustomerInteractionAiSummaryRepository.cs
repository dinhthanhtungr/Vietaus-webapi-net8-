using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Sales.CustomerCrmFeatures
{
    public class CustomerInteractionAiSummaryRepository : Repository<CustomerInteractionAiSummary>, ICustomerInteractionAiSummaryRepository
    {
        public CustomerInteractionAiSummaryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
