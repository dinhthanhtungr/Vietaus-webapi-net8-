using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.QuotationDTOs;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.QuotationFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/quotations")]
    [Authorize(Roles = RoleSets.Sales)]
    public sealed class QuotationController : ControllerBase
    {
        private readonly IQuotationService _quotationService;

        public QuotationController(IQuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperationResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<Guid>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateQuotationRequest request, CancellationToken ct)
        {
            var result = await _quotationService.CreateAsync(request, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(OperationResult<PagedResult<QuotationSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] QuotationQuery query, CancellationToken ct)
        {
            var result = await _quotationService.GetPagedAsync(query, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OperationResult<QuotationDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResult<QuotationDetailDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _quotationService.GetByIdAsync(id, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}/pdf")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> PrintPdf(Guid id, CancellationToken ct)
        {
            var result = await _quotationService.PrintPdfAsync(id, ct);
            if (!result.Success || result.Data == null)
                return BadRequest(result);

            return File(result.Data, "application/pdf", $"quotation-{id}.pdf");
        }
    }
}
