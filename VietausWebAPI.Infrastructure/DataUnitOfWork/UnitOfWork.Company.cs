using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public ICompanyRepository CompanyRepository { get; }
        public IEmployeesRepository EmployeesCommonRepository { get; }
    }
}
