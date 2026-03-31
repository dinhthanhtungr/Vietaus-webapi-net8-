using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.Helpers.PLPUReports
{
    public interface IExportFinishReportExcel
    {
        byte[] ExportFinishReportExcel(List<FinishRow> rows);
    }
}
