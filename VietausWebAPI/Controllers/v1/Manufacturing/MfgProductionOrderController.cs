using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material;

namespace VietausWebAPI.WebAPI.Controllers.v1.Manufacturing
{
    [ApiController]
    [Route("api/mfgproductionorder")]
    [AllowAnonymous]

    public class MfgProductionOrderController : Controller
    {
        private readonly IMfgProductionOrderService _mfgProductionOrderService;

        public MfgProductionOrderController(IMfgProductionOrderService mfgProductionOrderService)
        {
            _mfgProductionOrderService = mfgProductionOrderService;
        }

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

        //[HttpPost]
        //public async Task<IActionResult> CreateMfgProductionOrder([FromBody] PostMfgProductionOrder request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest("Request cannot be null.");
        //    }

        //    try
        //    {
        //        var result = await _mfgProductionOrderService.CreateAsync(request);
        //        if (!result.Success)
        //        {
        //            return BadRequest(result.Message);
        //        }
        //        return Ok(result);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (not shown here for brevity)
        //        return StatusCode(500, "An unexpected error occurred.");
        //    }
        //}

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
    }
}
