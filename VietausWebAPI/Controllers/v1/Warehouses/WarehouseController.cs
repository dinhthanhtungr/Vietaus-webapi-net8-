using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Helpers;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1.Warehouses
{
    [ApiController]
    [Route("api/warehouse")]
    [AllowAnonymous]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseReadService _warehouseReadService;
        private readonly IWarehouseSnapshotService _warehouseSnapshotService;
        private readonly IWarehouseReservationService _warehouseReservationService; 
        private readonly IStockAvailableExcel _stockAvailableExcel;

        private readonly IWarehouseVoucherReadService _service;

        public WarehouseController(IWarehouseReadService warehouseReadService
            , IWarehouseSnapshotService warehouseSnapshotService
            , IWarehouseReservationService warehouseReservationService
            , IStockAvailableExcel stockAvailableExcel
            , IWarehouseVoucherReadService readService)
        {
            _warehouseReadService = warehouseReadService;
            _warehouseSnapshotService = warehouseSnapshotService;
            _warehouseReservationService = warehouseReservationService;
            _stockAvailableExcel = stockAvailableExcel;
            _service = readService;
        }

        // Theo ID công thức
        [HttpGet("{formulaId:guid}/materials/availability")]
        public async Task<ActionResult<List<VaAvailabilityVm>>> GetMaterialsAvailabilityById(
            Guid formulaId, CancellationToken ct)
            => Ok(await _warehouseReadService.GetVaAvailabilityAsync(formulaId, ct));

        [HttpGet]
        public async Task<ActionResult<List<GetStockAvaiable>>> GetWarehouseSnapshotsByMfgId(
            [FromQuery] WarehouseReadServiceQuery query,
            CancellationToken ct)
        {
            var result = await _warehouseReadService.GetStockAvailableAsync(query);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("snapshot-reserve")]
        public async Task<ActionResult> CreateVaSnapshotAndReservations(
            [FromBody] Core.Application.Features.Warehouse.DTOs.WarehouseWriteServices.CreateVaSnapshotAndReservations req,
            CancellationToken ct)
        {
            //return BadRequest("Not implemented yet");
            var result = await _warehouseReservationService.ReserveAvailabilityAsync(req, ct);
            if (!result.Success) return Ok(result);
            return Ok(result);
        }


        [HttpGet("export-stock-available")]
        public async Task<IActionResult> ExportStockAvailable([FromQuery] WarehouseReadServiceQuery query)
        {
            var dataResult = await _warehouseReadService.GetStockAvailableExportAsync(query);
            if (!dataResult.Success || dataResult.Data == null)
                return BadRequest(dataResult.Message);

            var bytes = _stockAvailableExcel.Render(dataResult.Data);

            var fileName = $"TonKho_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }


        [HttpGet("voucher-list")]
        public async Task<IActionResult> GetList([FromQuery] WarehouseVoucherReadQuery query)
        {
            var result = await _service.GetWarehouseVouchersAsync(query);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{voucherId:long}")]
        public async Task<IActionResult> GetById(long voucherId)
        {
            var result = await _service.GetWarehouseVoucherByIdAsync(voucherId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
