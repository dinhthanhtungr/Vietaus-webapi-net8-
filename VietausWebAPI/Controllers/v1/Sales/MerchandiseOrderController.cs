using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/merchandiseorder")]
    [AllowAnonymous]
    public class MerchandiseOrderController : Controller
    {
        private readonly IMerchandiseOrderService _merchandiseOrderService;

        public MerchandiseOrderController(IMerchandiseOrderService merchandiseOrderService)
        {
            _merchandiseOrderService = merchandiseOrderService;
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


    }
}
