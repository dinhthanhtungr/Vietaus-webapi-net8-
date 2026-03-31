using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        // PLPU Report
        public IFinishPLPUReportRepository FinishPLPUReportRepository { get; }

        // Sale Report
        public IMerchandiseOrderReportRepositorys MerchandiseOrderReportRepositorys { get; }
    }
}
