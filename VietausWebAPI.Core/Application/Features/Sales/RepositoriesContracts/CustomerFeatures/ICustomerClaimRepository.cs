using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;

namespace VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures
{
    public interface ICustomerClaimRepository : IRepository<CustomerClaim>
    {
        void UpdateRange(IEnumerable<CustomerClaim> customerClaims);
    }
}
