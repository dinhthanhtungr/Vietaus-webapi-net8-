using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports
{
    public interface IFinishPLPUReportRepository
    {
        Task<List<FinishRow>> GetFinishReportAsync(FinishReportQuery query, CancellationToken ct);
    }
}
