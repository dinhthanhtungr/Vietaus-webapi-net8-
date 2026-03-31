using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.Querys.ExtruderOperationHistoryQuery;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.ServiceContracts.ExtruderOperationHistoryFeatures.GetServices;

namespace VietausWebAPI.WebAPI.Controllers.v1.ShiftReportFeatures.ExtruderOperationHistoryFeatures
{
    [ApiController]
    [Route("api/extruder-operation-history")]
    [AllowAnonymous]
    public class ExtruderOperationHistoryController : Controller
    {
        private readonly IExtruderOperationHistoryService _extruderOperationHistoryService;

        public ExtruderOperationHistoryController(IExtruderOperationHistoryService extruderOperationHistoryService)
        {
            _extruderOperationHistoryService = extruderOperationHistoryService;
        }

        [HttpGet("external/{id}")]
        public async Task<IActionResult> GetExtruderOperationHistoryByExternalId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be empty.");
            }
            try
            {
                var result = await _extruderOperationHistoryService.GetByExternalIdAsync(id);
                if (result == null)
                {
                    return NotFound($"Extruder operation history with ID {id} not found.");
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

        [HttpGet("is-valid")]
        public async Task<IActionResult> IsValidExtruderOperationHistory([FromQuery] ExtruderOperationHistoryQuery query)
        {
            if (query == null || (string.IsNullOrWhiteSpace(query.ProductCode) && string.IsNullOrWhiteSpace(query.ExternalId)))
            {
                return BadRequest("External ID cannot be empty.");
            }
            try
            {
                var isValid = await _extruderOperationHistoryService.GetIsValidAsync(query);
                return Ok(new { IsValid = isValid });
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

        [HttpGet("page")]
        public async Task<IActionResult> GetPagedOperationHistory([FromQuery] ExtruderOperationHistoryQuery query)
        {
            if (query == null)
            {
                return BadRequest("Query cannot be null.");
            }
            try
            {
                var result = await _extruderOperationHistoryService.GetPagedAsync(query);
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
    }
}