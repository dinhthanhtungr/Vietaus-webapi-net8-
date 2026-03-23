using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Helpers.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Services.QCInputByQCFeatures;

namespace VietausWebAPI.WebAPI.Controllers.v1.DevandqaFeatures
{
    [ApiController]
    [Route("api/iqcinput")]
    [AllowAnonymous]
    public class IQCInputController : Controller
    {
        private readonly IQCInputByQCService _qCInputByQCService;
        private readonly IExportQCInputByQCExcel _exportQCInputByQCExcel;
        public IQCInputController(IQCInputByQCService qCInputByQCService, IExportQCInputByQCExcel exportQCInputByQCExcel)
        {
            _qCInputByQCService = qCInputByQCService;
            _exportQCInputByQCExcel = exportQCInputByQCExcel;
        }

        [HttpGet("GetPaged")]
        public async Task<IActionResult> GetPagedSummary([FromQuery] Core.Application.Features.DevandqaFeatures.Queries.QCInputByQCFeatures.QCInputQuery query)
        {
            try
            {
                var result = await _qCInputByQCService.GetPagedSummaryAsync(query, CancellationToken.None);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("GetByVoucherDetailId")]
        public async Task<IActionResult> GetByVoucherDetailId([FromQuery] long voucherDetailId)
        {
            try
            {
                var result = await _qCInputByQCService.GetByVoucherDetailIdAsync(voucherDetailId, CancellationToken.None);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateIQCInput([FromBody] PostQCInputByQC input)
        {
            try
            {
                var result = await _qCInputByQCService.CreateAsync(input, CancellationToken.None);


                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("PatchByVoucherDetailId/{voucherDetailId}")]
        public async Task<IActionResult> PatchByVoucherDetailId([FromBody] PatchQCInputByQC input, [FromRoute] long voucherDetailId)
        {
            try
            {
                var result = await _qCInputByQCService.PatchByVoucherDetailIdAsync(input, voucherDetailId, CancellationToken.None);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportExcel([FromQuery] QCInputQuery query, CancellationToken ct)
        {
            var exportData = await _qCInputByQCService.BuildExcelExportDataAsync(query, ct);
            var bytes = _exportQCInputByQCExcel.ExportExcel(exportData);

            var fileName = $"QCInputByQC_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
