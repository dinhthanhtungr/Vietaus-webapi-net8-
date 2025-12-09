using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/transfers")]
    //[AllowAnonymous]
    public class TransfersController : Controller
    {
        private readonly ITransferCustomerService _svc;
        public TransfersController(ITransferCustomerService svc) { _svc = svc; }


        [HttpGet]
        [Authorize(Roles = RoleSets.Sales)]
        public async Task<ActionResult<PagedResult<TransferCustomerDTO>>> List(
            [FromQuery] CustomerTransferQuery query,
            CancellationToken ct = default)
        {
            var result = await _svc.GetTransfersAsync(query, ct);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleSets.Sales)]
        public async Task<IActionResult> Create([FromBody] TransferCustomersRequest req, CancellationToken ct)
        {
            if (req is null)
                return BadRequest(OperationResult<TransferCustomerDTO>.Fail("Request data is required."));

            var op = await _svc.CreateTransferAsync(req, ct);

            // Trả đúng khuôn OperationResult<TransferCustomerDTO>
            if (op.Success)
            {
                return Ok(op); // 200 + body là OperationResult<TransferCustomerDTO>
            }

            return BadRequest(OperationResult<TransferCustomerDTO>.Fail(op.Message ?? "Thao tác thất bại"));
        }

        [HttpGet("{id:guid}", Name = "GetTransferById")]
        [ProducesResponseType(typeof(TransferCustomerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = RoleSets.Sales)]
        public async Task<ActionResult<TransferCustomerDTO>> Get(Guid id, CancellationToken ct)
        {
            var dto = await _svc.GetTransferByIdAsync(id, ct);
            return dto is null ? NotFound() 
                : Ok(dto);
        }

        /// <summary>
        /// Leader giao 1 khách hàng tiềm năng (Lead) cho 1 Sale trong nhóm
        /// </summary>
        [HttpPost("assign-lead")]
        public async Task<IActionResult> AssignLead([FromBody] AssignLeadRequest req, CancellationToken ct)
        {
            if (req == null)
            {
                return BadRequest(OperationResult.Fail("Request không hợp lệ."));
            }

            var result = await _svc.CreateAssignLeadRequestAsync(req, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }

}
