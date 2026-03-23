using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IFinishPLPUReportRepository FinishPLPUReportRepository { get; }
    }
}
