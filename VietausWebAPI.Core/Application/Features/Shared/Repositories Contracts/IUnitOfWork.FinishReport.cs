using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        // PLPU Report
        IFinishPLPUReportRepository FinishPLPUReportRepository { get; }

        // Sale Report
        IMerchandiseOrderReportRepositorys MerchandiseOrderReportRepositorys { get; }
    }
}
