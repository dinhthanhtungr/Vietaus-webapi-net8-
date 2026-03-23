using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IFinishPLPUReportRepository FinishPLPUReportRepository { get; }
    }
}
