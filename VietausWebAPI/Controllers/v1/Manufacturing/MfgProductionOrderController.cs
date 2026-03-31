using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.MfgGetInformationInforDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.MFGProductionOrderFeatures;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.WebAPI.Controllers.v1.Manufacturing
{
    [ApiController]
    [Route("api/mfgproductionorder")]
    [AllowAnonymous]

    public class MfgProductionOrderController : Controller
    {
        private readonly IMfgProductionOrderService _mfgProductionOrderService;
        private readonly IMfgProductionOrderRWService _mfgProductionOrderRWService;
        private readonly IMfgGetInformationService _mfgGetInformationService;    
        private readonly IMfgPostInformationService _mfgPostInformationService;
        private readonly IMfgUpsertInformationService _mfgUpsertInformationService;

        public MfgProductionOrderController(IMfgProductionOrderService mfgProductionOrderService
            , IMfgProductionOrderRWService mfgProductionOrderRWService
            , IMfgGetInformationService mfgGetInformationService
            , IMfgPostInformationService mfgPostInformationService
            , IMfgUpsertInformationService mfgUpsertInformationService)
        {
            _mfgProductionOrderService = mfgProductionOrderService;
            _mfgProductionOrderRWService = mfgProductionOrderRWService;
            _mfgGetInformationService = mfgGetInformationService;
            _mfgPostInformationService = mfgPostInformationService;
            _mfgUpsertInformationService = mfgUpsertInformationService;
        }

        //====================================================================================== Use old code from service MfgProductionOrder =========================================================================================
        [HttpPatch]
        public async Task<IActionResult> UpdateMfgProductionOrder([FromBody] PatchMfgProductionOrderInformation request)
        {
            if (request == null || request.MfgProductionOrderId == Guid.Empty)
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                var result = await _mfgProductionOrderService.UpdateInformationAsync(request);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
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

        [HttpPatch("finish")]
        public async Task<IActionResult> FinishMfgProductionOrderAsync([FromBody] Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            try
            {
                var result = await _mfgProductionOrderService.FinishMfgProductionOrderAsync(id, cancellationToken);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateMfgProductionOrder([FromBody] CreateMfgProductionOrderInternal request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _mfgProductionOrderService.CreateInternalAsync(request);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
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

        [HttpGet]
        public async Task<IActionResult> GetMfgProductionOrders([FromQuery] MfgProductionOrderQuery query)
        {
            try
            {
                var pagedResult = await _mfgProductionOrderService.GetAllAsync(query);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMfgProductionOrderById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            try
            {
                var mfgProductionOrder = await _mfgProductionOrderService.GetByIdAsync(id);
                if (mfgProductionOrder == null)
                {
                    return NotFound();
                }
                return Ok(mfgProductionOrder);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpGet("formulaforproductionorder")]
        public async Task<IActionResult> GetFormulaAndMfgFormula([FromQuery] FormulaAndMfgFormulaQuery query)
        {
            try
            {
                var pagedResult = await _mfgProductionOrderService.GetFormulaAndMfgFormulaAsync(query);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("mfgformulas")]
        public async Task<IActionResult> GetMfgFormulas([FromQuery] MfgProductionOrderQuery query)
        {
            try
            {
                var pagedResult = await _mfgProductionOrderService.GetAllMfgFormulaAsync(query);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        //====================================================================================== RW =========================================================================================

        [HttpGet("rw/{id:guid}")]
        public async Task<IActionResult> GetMfgProductionOrderrwById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID.");
            }
            try
            {
                var mfgProductionOrder = await _mfgProductionOrderRWService.GetByIdAsync(id);
                if (mfgProductionOrder == null)
                {
                    return NotFound();
                }
                return Ok(mfgProductionOrder);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpGet("{id:guid}/{fid:guid}")]
        public async Task<IActionResult> GetMfgProductionOrderById(Guid id, Guid fid, [FromQuery] FormulaType? formulaType, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid mfgProductionOrderId.");

            if (fid == Guid.Empty)
                return BadRequest("Invalid formulaId.");

            try
            {
                var mfgProductionOrder = await _mfgGetInformationService.GetAsync(id, fid, formulaType, ct);

                if (mfgProductionOrder == null)
                    return NotFound();

                return Ok(mfgProductionOrder);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("rw/{id:guid}")]
        public async Task<IActionResult> PatchMfgProductionOrderRw(Guid id, [FromBody] PatchStockMfgProductionOrderRequest req, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid ID.");

            try
            {
                var result = await _mfgUpsertInformationService.ExportFromStockAsync(req, ct);
                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(result.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
       
        [HttpPost("save-and-create-formula")]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveAndCreateFormula([FromBody] PostMfgFormulaAndUpdateMpoRequest req, CancellationToken ct)
        {
            if (req == null)
                return BadRequest(OperationResult.Fail("Dữ liệu gửi lên không hợp lệ."));

            var result = await _mfgUpsertInformationService.SaveAndCreateFormulaAsync(req, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("inform/{id:guid}")]
        [ProducesResponseType(typeof(GetMfgProductionOrderInform), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMfgProductionOrderInformById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid ID.");

            try
            {
                var result = await _mfgGetInformationService.GetByIdAsync(id);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("inform/{id:guid}")]
        public async Task<IActionResult> PatchMfgProductionOrderInform(
            [FromBody] PatchMfgProductionOrderInformRequest req,
            CancellationToken ct = default)
        {
            if (req.MfgProductionOrderId == Guid.Empty)
                return BadRequest(OperationResult.Fail("Invalid ID."));

            try
            {
                var result = await _mfgUpsertInformationService.PatchInformAsync(req, ct);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, OperationResult.Fail($"An unexpected error occurred: {ex.Message}"));
            }
        }

        [HttpPost("inform/create")]
        [ProducesResponseType(typeof(OperationResult<CreateMfgProductionOrderInformResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<CreateMfgProductionOrderInformResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OperationResult<CreateMfgProductionOrderInformResult>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMfgProductionOrderInform(
            [FromBody] CreateMfgProductionOrderInformRequest req,
            CancellationToken ct = default)
        {
            if (req == null)
                return BadRequest(OperationResult<CreateMfgProductionOrderInformResult>.Fail("Dữ liệu gửi lên không hợp lệ."));

            if (req.MerchandiseOrderId == Guid.Empty)
                return BadRequest(OperationResult<CreateMfgProductionOrderInformResult>.Fail("MerchandiseOrderId không hợp lệ."));

            if (req.MerchandiseOrderDetailId == Guid.Empty)
                return BadRequest(OperationResult<CreateMfgProductionOrderInformResult>.Fail("MerchandiseOrderDetailId không hợp lệ."));

            if (!req.ProductId.HasValue || req.ProductId.Value == Guid.Empty)
                return BadRequest(OperationResult<CreateMfgProductionOrderInformResult>.Fail("ProductId không hợp lệ."));

            try
            {
                var result = await _mfgPostInformationService.CreateInformAsync(req, ct);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    OperationResult<CreateMfgProductionOrderInformResult>.Fail(
                        $"An unexpected error occurred: {ex.Message}"));
            }
        }


        //====================================================================================== RW =========================================================================================
    }
}