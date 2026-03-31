using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Helpers.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports;

namespace VietausWebAPI.WebAPI.Controllers.v1.ReportFeatures
{
    [ApiController]
    [Route("api/plpureport")]
    [AllowAnonymous]
    public class PLPUReportController : Controller
    {
        private readonly IFinishPLPUReportService _finishPLPUReportService;
        private readonly IExportFinishReportExcel _exportFinishReportExcel;

        public PLPUReportController(IFinishPLPUReportService finishPLPUReportService, IExportFinishReportExcel exportFinishReportExcel)
        {
            _finishPLPUReportService = finishPLPUReportService;
            _exportFinishReportExcel = exportFinishReportExcel;
        }

        [HttpGet("export-finish-plpu-report")]
        public async Task<IActionResult> ExportFinishPLPUReport([FromQuery] DateTime from,
                                                                [FromQuery] DateTime to, 
                                                                CancellationToken ct = default)
        {
            var result = await _finishPLPUReportService.GetFinishPLPUReportsAsync(from, to, ct);
            var fileBytes = _exportFinishReportExcel.ExportFinishReportExcel(result);

            var fileName = $"BaoCaoGiaoHangHoanTat_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
