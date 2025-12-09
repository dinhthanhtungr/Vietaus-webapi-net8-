using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;

namespace VietausWebAPI.Core.Application.Features.CompanyFeatures.RepositoriesContracts
{
    public interface ICompanyRepository : IRepository<Company>
    {

    }
}
