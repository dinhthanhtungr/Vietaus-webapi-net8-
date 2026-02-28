using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.WebAPI.Helpers;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/merchandiseorder")]
    [AllowAnonymous]
    public class MerchandiseOrderController : Controller
    {
        private readonly IMerchandiseOrderService _merchandiseOrderService;
        private readonly ICurrentUser _CurrentUser;

        public MerchandiseOrderController(IMerchandiseOrderService merchandiseOrderService, ICurrentUser currentUser)
        {
            _merchandiseOrderService = merchandiseOrderService;
            _CurrentUser = currentUser;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMerchandiseOrder([FromBody] PostMerchandiseOrder request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _merchandiseOrderService.CreateAsync(request);

                if (!result.Success)
                    return BadRequest(result);        // body vẫn là OperationResult

                // Nếu service có OrderId/CollectionId, bạn có thể set header ở đây:
                // Trả 201 nhưng body vẫn chỉ là OperationResult theo ý bạn
                return StatusCode(StatusCodes.Status201Created, result);
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMerchandiseOrders([FromQuery] MerchandiseOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _merchandiseOrderService.GetAllAsync(query, ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{merchandiseOrderId:guid}")]
        public async Task<IActionResult> GetMerchandiseOrderById([FromRoute] Guid merchandiseOrderId, CancellationToken ct = default)
        {
            if (merchandiseOrderId == Guid.Empty)
            {
                return BadRequest("Invalid merchandise order ID.");
            }
            try
            {
                var result = await _merchandiseOrderService.GetByIdAsync(merchandiseOrderId, ct);
                if (result == null)
                {
                    return NotFound("Merchandise order not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("GetByCustomerId")]
        public async Task<IActionResult> GetLastMerchandiseOrderByCustomerId([FromQuery] Guid customerId, [FromQuery] Guid productId, CancellationToken ct = default)
        {
            if (customerId == Guid.Empty)
            {
                return BadRequest("Invalid customer ID.");
            }
            try
            {
                var result = await _merchandiseOrderService.GetLastMerchandiseOrderByCustomerIdAsync(customerId, productId, ct);
                if (result == null)
                {
                    return NotFound("Merchandise order not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateMerchandiseOrderInformation([FromBody] PatchMerchandiseOrderInformation request, CancellationToken ct = default)
        {
            if (request == null || request.MerchandiseOrderId == Guid.Empty)
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                var result = await _merchandiseOrderService.UpdateInformationAsync(request, ct);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        [HttpPatch("Approve")]
        public async Task<IActionResult> UpdateAppoveStatus([FromBody] PatchMerchandiseOrderInformation request, CancellationToken ct = default)
        {
            if (request == null || request.MerchandiseOrderId == Guid.Empty)
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                var result = await _merchandiseOrderService.UpdateApproveStatus(request, ct);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("soft-delete")]
        public async Task<IActionResult> SoftDeleteMerchandiseOrder([FromBody] PatchMerchandiseOrderInformation request, CancellationToken ct = default)
        {
            if (request == null || request.MerchandiseOrderId == Guid.Empty)
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                var result = await _merchandiseOrderService.CancelMerchadiseOrder(request, ct);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
