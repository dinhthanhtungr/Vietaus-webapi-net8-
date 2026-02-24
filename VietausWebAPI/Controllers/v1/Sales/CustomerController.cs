using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/customer")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("AddCustomer")]
        [Authorize(Roles = RoleSets.Sales)]
        public async Task<IActionResult> AddCustomer([FromBody] PostCustomer customer)
        {
            if (customer == null)
            {
                return BadRequest("Group data is required.");
            }
            var result = await _customerService.AddNewCustomer(customer);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpGet("GetAllCustomer")]
        [Authorize(Roles = RoleSets.CanSeeAllCustomer)]
        public async Task<IActionResult> GetAllCustomer([FromQuery] CustomerQuery? query)
        {
            var result = await _customerService.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("GetCustomerByIdAsync")]
        //[Authorize(Roles = RoleSets.Sales)]
        public async Task<IActionResult> GetCustomerByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid customer ID.");
            }
            var result = await _customerService.GetCustomerByIdAsync(id);
            if (result == null)
            {
                return NotFound("Customer not found.");
            }
            return Ok(result);
        }

        [HttpGet("lead-owner/{customerId}")]
        public async Task<IActionResult> GetCustomerLeadOwner(Guid customerId, CancellationToken ct)
        {
            var result = await _customerService.GetCustomerLeadOwner(customerId, ct);

            if (!result.Success)
                return BadRequest(result); // hoặc NotFound, tùy bạn

            return Ok(result);
        }


        [HttpGet("GetCustomerByIdForSalesAsync")]
        [Authorize(Roles = RoleSets.Sales)]
        public async Task<IActionResult> GetCustomerByIdForSalesAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid customer ID.");
            }
            var result = await _customerService.GetCustomerByIdForSalesAsync(id, ct);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpPatch("DeleteCustomerByIdAsync")]
        [Authorize(Roles = RoleSets.Deleters)]
        public async Task<IActionResult> DeleteCustomerByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid customer ID.");
            }
            var result = await _customerService.DeleteCustomerByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateCustomerAsync([FromBody] PatchCustomer request)
        {
            if (request == null || request.CustomerId == Guid.Empty)
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                var result = await _customerService.UpdateCustomerAsync(request);
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

        [HttpGet("GetCustomerByEmployeeAssignment")]
        [Authorize(Roles = RoleSets.Sales)]
        public async Task<IActionResult> GetCustomerByEmployeeAssignment(
            [FromQuery] CustomerQuery query,
            CancellationToken ct = default)
        {
            var result = await _customerService.GetCustomerByEmployeeAssignment(query, ct);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("No delivery orders found.");
            }
        }
    }
}
