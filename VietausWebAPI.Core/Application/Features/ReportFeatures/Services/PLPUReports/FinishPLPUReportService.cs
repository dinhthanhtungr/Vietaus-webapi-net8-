using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.Services.PLPUReports
{
    public class FinishPLPUReportService : IFinishPLPUReportService
    {
        private readonly IFinishPLPUReportRepository _FinishPLPUReportRepository;

        public FinishPLPUReportService(IFinishPLPUReportRepository FinishPLPUReportRepository)
        {
            _FinishPLPUReportRepository = FinishPLPUReportRepository;
        }
    }
}
