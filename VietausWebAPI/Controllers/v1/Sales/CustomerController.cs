using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
{
    [ApiController]
    [Route("api/customer")]
    [AllowAnonymous]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost("AddCustomer")]
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
        public async Task<IActionResult> GetAllCustomer([FromQuery] CustomerQuery? query)
        {
            var result = await _customerService.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("GetCustomerByIdAsync")]
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

        [HttpPatch("DeleteCustomerByIdAsync")]
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

        [HttpPatch("UpdateCustomerAsync")]
        public async Task<IActionResult> UpdateCustomerAsync([FromBody] PatchCustomer customer)
        {
            if (customer == null || customer.CustomerId == Guid.Empty)
            {
                return BadRequest("Invalid customer data.");
            }
            await _customerService.UpdateCustomerAsync(customer);

            return Ok();

        }

        [HttpGet("GetCustomerByEmployeeAssignment")]
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
