using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulaAdjustments;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulaAdjustments;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.WebAPI.Controllers.v1.Manufacturing
{
    [ApiController]
    [Route("api/mfgformulaadjustments")]
    [AllowAnonymous]
    public class MfgFormulaAdjustmentController : ControllerBase
    {
        private readonly IMfgAdjustmentService _service;

        public MfgFormulaAdjustmentController(IMfgAdjustmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] ManufacturingFormulaAdjustmentQuery query,
            CancellationToken ct = default)
        {
            var result = await _service.GetAllAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return BadRequest("Id không hợp lệ.");

            var result = await _service.GetByIdAsync(id, ct);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{id:guid}/batch-groups")]
        public async Task<IActionResult> GetBatchGroups(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return BadRequest("Id khong hop le.");

            var result = await _service.GetBatchGroupsAsync(id, ct);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("by-production-order/{mfgProductionOrderId:guid}/batch-groups")]
        public async Task<IActionResult> GetBatchGroupsByProductionOrderId(
            Guid mfgProductionOrderId,
            CancellationToken ct = default)
        {
            if (mfgProductionOrderId == Guid.Empty)
                return BadRequest("MfgProductionOrderId khong hop le.");

            var result = await _service.GetBatchGroupsByProductionOrderIdAsync(mfgProductionOrderId, ct);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] PostManufacturingFormulaAdjustment request,
            CancellationToken ct = default)
        {
            if (request == null)
                return BadRequest(OperationResult<GetManufacturingFormulaAdjustment>.Fail("Request không được để trống."));

            var result = await _service.CreateAsync(request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}")]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch(
            Guid id,
            [FromBody] PatchManufacturingFormulaAdjustment request,
            CancellationToken ct = default)
        {
            if (request == null)
                return BadRequest(OperationResult<GetManufacturingFormulaAdjustment>.Fail("Request không được để trống."));

            var result = await _service.PatchAsync(id, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id:guid}/batches")]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBatch(
            Guid id,
            [FromBody] PostManufacturingFormulaAdjustmentBatch request,
            CancellationToken ct = default)
        {
            if (request == null)
                return BadRequest(OperationResult<GetManufacturingFormulaAdjustment>.Fail("Request khong duoc de trong."));

            var result = await _service.AddBatchAsync(id, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id:guid}/batch-ranges")]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBatchRange(
            Guid id,
            [FromBody] PostManufacturingFormulaAdjustmentBatchRange request,
            CancellationToken ct = default)
        {
            if (request == null)
                return BadRequest(OperationResult<GetManufacturingFormulaAdjustment>.Fail("Request khong duoc de trong."));

            var result = await _service.AddBatchRangeAsync(id, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id:guid}/batches/{batchId:guid}")]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchBatch(
            Guid id,
            Guid batchId,
            [FromBody] PostManufacturingFormulaAdjustmentBatchRange request,
            CancellationToken ct = default)
        {
            if (request == null)
                return BadRequest(OperationResult<GetManufacturingFormulaAdjustment>.Fail("Request khong duoc de trong."));

            var result = await _service.PatchBatchAsync(id, batchId, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}/batches/{batchId:guid}")]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<GetManufacturingFormulaAdjustment>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBatch(
            Guid id,
            Guid batchId,
            CancellationToken ct = default)
        {
            var result = await _service.DeleteBatchAsync(id, batchId, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
        {
            var result = await _service.DeleteAsync(id, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
