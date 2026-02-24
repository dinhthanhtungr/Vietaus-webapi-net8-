using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts.SupplierFeatures;

//using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;

namespace VietausWebAPI.WebAPI.Controllers.v1.Materials
{
    [ApiController]
    [Route("api/supplier")]
    [AllowAnonymous]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSupplier([FromBody] PostSupplier postSupplier)
        {
            if (postSupplier == null)
            {
                return BadRequest("Request cannot be null.");
            }
            try
            {
                var result = await _supplierService.AddNewSuplier(postSupplier);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMaterial([FromQuery] SupplierQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _supplierService.GetAllAsync(query, ct);
                return Ok(result);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}", Name = "GetSupplierById")]
        [ProducesResponseType(typeof(GetSupplier), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetSupplier>> Get(Guid id, CancellationToken ct)
        {
            var dto = await _supplierService.GetSupplierByIdAsync(id, ct);
            return dto is null ? NotFound()
                : Ok(dto);
        }

        [HttpPatch("UpdateSupplierAsync")]
        public async Task<IActionResult> UpdateSupplierAsync([FromBody] PatchSupplier supplier)
        {
            if (supplier == null || supplier.SupplierId == Guid.Empty)
            {
                return BadRequest("Invalid customer data.");
            }
            await _supplierService.UpdateSupplierAsync(supplier);

            return Ok();

        }

        [HttpPatch("DeleteSupplierByIdAsync")]
        public async Task<IActionResult> DeleteCustomerByIdAsync([FromQuery] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid customer ID.");
            }
            var result = await _supplierService.DeleteSupplierByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

    }
}
