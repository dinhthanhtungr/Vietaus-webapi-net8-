using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.PatchDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.PostDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords;

namespace VietausWebAPI.WebAPI.Controllers.v1.Manufacturing.ColorChipManufacturingRecords
{
    [ApiController]
    [Route("api/manufacturing/color-chip-manufacturing-records")]
    public class ColorChipManufacturingRecordController : Controller
    {
        private readonly IGetColorChipManufacturingRecordService _getService;
        private readonly IPostColorChipManufacturingRecordService _postService;
        private readonly IUpsertColorChipManufacturingRecordService _upsertService;

        private readonly IColorChipManufacturingRecordPrintPdfService _printPdfService;

        public ColorChipManufacturingRecordController(
            IGetColorChipManufacturingRecordService getService,
            IPostColorChipManufacturingRecordService postService,
            IUpsertColorChipManufacturingRecordService upsertService,
            IColorChipManufacturingRecordPrintPdfService printPdfService)
        {
            _getService = getService;
            _postService = postService;
            _upsertService = upsertService;
            _printPdfService = printPdfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] ColorChipManufacturingRecordQuery query, CancellationToken ct)
        {
            var result = await _getService.GetPagedAsync(query, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{colorChipMfgRecordId:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid colorChipMfgRecordId, CancellationToken ct = default)
        {
            var result = await _getService.GetByIdAsync(colorChipMfgRecordId, ct);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] PostColorChipManufacturingRecordRequest dto, CancellationToken ct = default)
        {
            var result = await _postService.CreateAsync(dto, ct);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{colorChipMfgRecordId:guid}/print-pdf")]
        public async Task<IActionResult> PrintPdfAsync(
            Guid colorChipMfgRecordId,
            CancellationToken ct = default)
        {
            var result = await _printPdfService.PrintByIdAsync(colorChipMfgRecordId, ct);

            if (!result.Success)
                return BadRequest(result.Message);

            if (result.Data == null || result.Data.Length == 0)
                return BadRequest("PDF file is empty.");

            var fileName = $"ColorChipManufacturingRecord_{colorChipMfgRecordId:N}.pdf";

            return File(result.Data, "application/pdf", fileName);
        }

        [HttpPatch("{colorChipMfgRecordId:guid}")]
        public async Task<IActionResult> UpdateAsync(
            Guid colorChipMfgRecordId,
            [FromBody] UpsertColorChipManufacturingRecordRequest dto,
            CancellationToken ct = default)
        {
            if (dto == null)
                return BadRequest("Request không được để trống.");

            dto.ColorChipMfgRecordId = colorChipMfgRecordId;

            var result = await _upsertService.UpsertAsync(dto, ct);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

    }
}
