using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/transfers")]
    [AllowAnonymous]
    public class TransfersController : Controller
    {
        private readonly ITransferCustomerService _svc;
        public TransfersController(ITransferCustomerService svc) { _svc = svc; }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TransferCustomerDTO>>> List(
            [FromQuery] CustomerTransferQuery query,
            CancellationToken ct = default)
        {
            var result = await _svc.GetTransfersAsync(query, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TransferCustomerDTO>> Create(
            [FromBody] TransferCustomersRequest req,
            CancellationToken ct)
        {
            var dto = await _svc.CreateTransferAsync(req, ct);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpGet("{id:guid}", Name = "GetTransferById")]
        [ProducesResponseType(typeof(TransferCustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransferCustomerDTO>> Get(Guid id, CancellationToken ct)
        {
            var dto = await _svc.GetTransferByIdAsync(id, ct);
            return dto is null ? NotFound() 
                : Ok(dto);
        }
    }

}
