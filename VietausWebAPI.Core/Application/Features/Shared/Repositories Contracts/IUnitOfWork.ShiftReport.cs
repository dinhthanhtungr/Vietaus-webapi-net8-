using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.EndOfShiftReportFeatures;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IEndOfShiftReportReadRepositories EndOfShiftReportReadRepositories { get; }
    }
}
