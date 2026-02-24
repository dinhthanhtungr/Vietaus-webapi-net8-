using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1.Manufacturing
{
    [ApiController]
    [Route("api/mfgformula")]
    [AllowAnonymous]
    public class MfgFormulaController : Controller
    {
        private readonly IMfgFormulaService _mfgFormulaService;

        public MfgFormulaController(IMfgFormulaService mfgFormulaService)
        {
            _mfgFormulaService = mfgFormulaService;
        }
        [HttpGet]
        public async Task<IActionResult> GetMfgFormulaById([FromQuery] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID cannot be empty.");
            }
            try
            {
                var result = await _mfgFormulaService.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Manufacturing formula with ID {id} not found.");
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

        [HttpGet("View")]
        public async Task<IActionResult> GetMfgFormulaByIdForView([FromQuery] Guid mfgOrderId, [FromQuery] Guid mfgFormulaId, CancellationToken ct)
        {
            if (mfgOrderId == Guid.Empty || mfgFormulaId == Guid.Empty)
            {
                return BadRequest("MfgOrderId and MfgFormulaId cannot be empty.");
            }
            try
            {
                var result = await _mfgFormulaService.GetByIdForViewAsync(mfgOrderId, mfgFormulaId, ct);
                if (result == null)
                {
                    return NotFound(result);
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

        [HttpGet("viewmaterial")]
        public async Task<IActionResult> GetMaterialsByFormulaIdAsync([FromQuery] Guid mfgFormulaId, CancellationToken ct)
        {
            if (mfgFormulaId == Guid.Empty)
            {
                return BadRequest("MfgFormulaId cannot be empty.");
            }
            try
            {
                var result = await _mfgFormulaService.GetMaterialsByFormulaIdAsync(mfgFormulaId, ct);
                if (result == null)
                {
                    return NotFound(result);
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllMfgFormulas([FromQuery] MfgFormulaQuery query)
        {
            try
            {
                var result = await _mfgFormulaService.GetAllAsync(query);
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

        [HttpGet("all/compare-formulas")]
        public async Task<IActionResult> GetCompareFormulas([FromQuery] MfgFormulaQuery query)
        {
            try
            {
                var result = await _mfgFormulaService.GetCompareFormulaAsync(query);
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

        [HttpGet("all/version")]
        public async Task<IActionResult> GetAllMfgFormulasVersion([FromQuery] MfgProductionOrderQuery query)
        {
            try
            {
                var result = await _mfgFormulaService.GetFormulaVersionsPagedAsync(query);
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
        public async Task<IActionResult> CreateMfgFormula([FromBody] PostMfgFormula req, CancellationToken ct = default)
        {
            if (req == null)
            {
                return BadRequest("Request body cannot be null.");
            }
            try
            {
                var result = await _mfgFormulaService.CreateAsync(req, ct);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
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

        [HttpPatch]
        public async Task<IActionResult> UpsertMfgFormula([FromBody] PatchMfgFormula req, CancellationToken ct = default)
        {
            if (req == null)
            {
                return BadRequest("Request body cannot be null.");
            }
            try
            {
                var result = await _mfgFormulaService.UpsertFormulaAsync(req, ct);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
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
    }
}
