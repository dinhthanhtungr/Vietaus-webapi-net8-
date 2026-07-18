using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts;
using VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/customer-crm")]
    [Authorize]
    public class CustomerCrmController : Controller
    {
        private readonly ICustomerCrmService _customerCrmService;

        public CustomerCrmController(ICustomerCrmService customerCrmService)
        {
            _customerCrmService = customerCrmService;
        }

        [HttpPost("interactions")]
        public async Task<IActionResult> CreateInteraction([FromBody] CreateCustomerInteractionRequest request, CancellationToken ct)
        {
            var result = await _customerCrmService.CreateInteractionAsync(request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("interactions/{id:guid}")]
        public async Task<IActionResult> UpdateInteraction(Guid id, [FromBody] UpdateCustomerInteractionRequest request, CancellationToken ct)
        {
            var result = await _customerCrmService.UpdateInteractionAsync(id, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("interactions/{id:guid}")]
        public async Task<IActionResult> GetInteractionById(Guid id, CancellationToken ct)
        {
            var result = await _customerCrmService.GetInteractionByIdAsync(id, ct);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("customers/{customerId:guid}/interactions")]
        public async Task<IActionResult> GetCustomerInteractions(Guid customerId, [FromQuery] CustomerInteractionQuery query, CancellationToken ct)
        {
            var result = await _customerCrmService.GetCustomerInteractionsAsync(customerId, query, ct);
            return Ok(result);
        }

        /// <summary>
        /// Generates or regenerates an AI interaction summary for a customer.
        /// Returns HTTP 429 with Retry-After when the local Gemini quota is exhausted.
        /// </summary>
        /// <param name="customerId">Customer id that owns the interaction history.</param>
        /// <param name="request">Summary generation scope and filter options.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The generated AI summary or a quota response.</returns>
        [HttpPost("customers/{customerId:guid}/ai-summary/generate")]
        public async Task<IActionResult> GenerateCustomerInteractionAiSummary(
            Guid customerId,
            [FromBody] GenerateCustomerInteractionAiSummaryRequest request,
            CancellationToken ct)
        {
            var result = await _customerCrmService.GenerateCustomerInteractionAiSummaryAsync(customerId, request, ct);
            if (!result.Success && result.Data?.RateLimit?.CanRequest == false)
            {
                Response.Headers.RetryAfter = result.Data.RateLimit.RetryAfterSeconds.ToString();
                return StatusCode(StatusCodes.Status429TooManyRequests, result);
            }

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Generates AI interaction summaries for multiple customers while storing each summary separately.
        /// Returns HTTP 429 with Retry-After when the batch is stopped by local Gemini quota exhaustion before any customer succeeds.
        /// </summary>
        /// <param name="request">Batch summary generation scope and selected customer ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Batch generation result with per-customer status.</returns>
        [HttpPost("customers/ai-summary/generate-batch")]
        public async Task<IActionResult> GenerateCustomerInteractionAiSummaryBatch(
            [FromBody] GenerateCustomerInteractionAiSummaryBatchRequest request,
            CancellationToken ct)
        {
            var result = await _customerCrmService.GenerateCustomerInteractionAiSummaryBatchAsync(request, ct);
            if (!result.Success && result.Data?.RateLimit?.CanRequest == false)
            {
                Response.Headers.RetryAfter = result.Data.RateLimit.RetryAfterSeconds.ToString();
                return StatusCode(StatusCodes.Status429TooManyRequests, result);
            }

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Gets the latest stored AI interaction summary for a customer without calling Gemini.
        /// </summary>
        /// <param name="customerId">Customer id that owns the AI summary.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The latest stored AI summary for the customer.</returns>
        [HttpGet("customers/{customerId:guid}/ai-summary/latest")]
        public async Task<IActionResult> GetLatestCustomerInteractionAiSummary(Guid customerId, CancellationToken ct)
        {
            var result = await _customerCrmService.GetLatestCustomerInteractionAiSummaryAsync(customerId, ct);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Gets the current local Gemini quota state for CRM AI summaries without consuming a request.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Current RPM/RPD usage, remaining requests, and retry timing.</returns>
        [HttpGet("ai-summary/quota")]
        public async Task<IActionResult> GetCustomerInteractionAiSummaryQuota(CancellationToken ct)
        {
            var result = await _customerCrmService.GetCustomerInteractionAiSummaryQuotaAsync(ct);
            return Ok(result);
        }

        [HttpGet("customers/activity-headers")]
        public async Task<IActionResult> GetCustomerActivityHeaders([FromQuery] CustomerCrmActivityHeaderQuery query, CancellationToken ct)
        {
            var result = await _customerCrmService.GetCustomerActivityHeadersAsync(query, ct);
            return Ok(result);
        }

        [HttpPost("follow-up-tasks")]
        public async Task<IActionResult> CreateFollowUpTask([FromBody] CreateCustomerFollowUpTaskRequest request, CancellationToken ct)
        {
            var result = await _customerCrmService.CreateFollowUpTaskAsync(request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("follow-up-tasks/{id:guid}")]
        public async Task<IActionResult> UpdateFollowUpTask(Guid id, [FromBody] UpdateCustomerFollowUpTaskRequest request, CancellationToken ct)
        {
            var result = await _customerCrmService.UpdateFollowUpTaskAsync(id, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("follow-up-tasks/{id:guid}/complete")]
        public async Task<IActionResult> CompleteFollowUpTask(Guid id, [FromBody] CompleteCustomerFollowUpTaskRequest request, CancellationToken ct)
        {
            var result = await _customerCrmService.CompleteFollowUpTaskAsync(id, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("follow-up-tasks/my")]
        public async Task<IActionResult> GetMyFollowUpTasks([FromQuery] CustomerFollowUpTaskQuery query, CancellationToken ct)
        {
            var result = await _customerCrmService.GetMyFollowUpTasksAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("customers/{customerId:guid}/follow-up-tasks")]
        public async Task<IActionResult> GetCustomerFollowUpTasks(Guid customerId, [FromQuery] CustomerFollowUpTaskQuery query, CancellationToken ct)
        {
            var result = await _customerCrmService.GetCustomerFollowUpTasksAsync(customerId, query, ct);
            return Ok(result);
        }

        [HttpPost("follow-up-tasks/{taskId:guid}/assignees")]
        public async Task<IActionResult> AddFollowUpTaskAssignee(Guid taskId, [FromBody] AddCustomerFollowUpTaskAssigneeRequest request, CancellationToken ct)
        {
            var result = await _customerCrmService.AddFollowUpTaskAssigneeAsync(taskId, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("follow-up-tasks/{taskId:guid}/assignees/group")]
        public async Task<IActionResult> AddFollowUpTaskGroupAssignees(Guid taskId, [FromBody] AddCustomerFollowUpTaskGroupAssigneesRequest request, CancellationToken ct)
        {
            var result = await _customerCrmService.AddFollowUpTaskGroupAssigneesAsync(taskId, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("follow-up-tasks/{taskId:guid}/assignees/{employeeId:guid}")]
        public async Task<IActionResult> RemoveFollowUpTaskAssignee(Guid taskId, Guid employeeId, CancellationToken ct)
        {
            var result = await _customerCrmService.RemoveFollowUpTaskAssigneeAsync(taskId, employeeId, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("follow-up-tasks/{taskId:guid}/assignees")]
        public async Task<IActionResult> GetFollowUpTaskAssignees(Guid taskId, CancellationToken ct)
        {
            var result = await _customerCrmService.GetFollowUpTaskAssigneesAsync(taskId, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("work-plans")]
        public async Task<IActionResult> CreateWorkPlan([FromBody] CreateCustomerWorkPlanRequest request, CancellationToken ct)
        {
            var result = await _customerCrmService.CreateWorkPlanAsync(request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("work-plans/{id:guid}")]
        public async Task<IActionResult> UpdateWorkPlan(Guid id, [FromBody] UpdateCustomerWorkPlanRequest request, CancellationToken ct)
        {
            var result = await _customerCrmService.UpdateWorkPlanAsync(id, request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("customers/{customerId:guid}/work-plans")]
        public async Task<IActionResult> GetCustomerWorkPlans(Guid customerId, [FromQuery] CustomerWorkPlanQuery query, CancellationToken ct)
        {
            var result = await _customerCrmService.GetCustomerWorkPlansAsync(customerId, query, ct);
            return Ok(result);
        }

        [HttpGet("calendar/events")]
        public async Task<IActionResult> GetCalendarEvents([FromQuery] CustomerCrmCalendarQuery query, CancellationToken ct)
        {
            var result = await _customerCrmService.GetCalendarEventsAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("calendar/activity-report")]
        public async Task<IActionResult> GetCustomerActivityCalendarReport([FromQuery] CustomerActivityCalendarReportQuery query, CancellationToken ct)
        {
            var result = await _customerCrmService.GetCustomerActivityCalendarReportAsync(query, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("calendar/activity-report/export-excel")]
        public async Task<IActionResult> ExportCustomerActivityCalendarReportExcel([FromQuery] CustomerActivityCalendarReportQuery query, CancellationToken ct)
        {
            var bytes = await _customerCrmService.ExportCustomerActivityCalendarReportExcelAsync(query, ct);
            var fileName = $"customer-activity-calendar-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("dashboard/sale")]
        public async Task<IActionResult> GetSaleDashboard(CancellationToken ct)
        {
            var result = await _customerCrmService.GetSaleDashboardAsync(ct);
            return Ok(result);
        }

        [HttpGet("dashboard/leader")]
        public async Task<IActionResult> GetLeaderDashboard(CancellationToken ct)
        {
            var result = await _customerCrmService.GetLeaderDashboardAsync(ct);
            return Ok(result);
        }

        [HttpGet("dashboard/director")]
        public async Task<IActionResult> GetDirectorDashboard(CancellationToken ct)
        {
            var result = await _customerCrmService.GetDirectorDashboardAsync(ct);
            return Ok(result);
        }
    }
}
