using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports
{
    public interface IFinishPLPUReportService
    {
        Task<List<FinishRow>> GetFinishPLPUReportsAsync(DateTime from, DateTime to, CancellationToken ct = default);
    }
}
