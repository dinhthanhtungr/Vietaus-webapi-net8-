using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Usecases.Suppliers.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/Supplier")]
    [AllowAnonymous]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllSupplierName()
        {
            var result = await _supplierService.GetAllSupplierName();
            return Ok(result);
        }

        [HttpGet("GetSupplierAddress/{supplierId}")]
        public async Task<IActionResult> GetSupplierAddress(Guid supplierId)
        {
            if (supplierId == Guid.Empty)
            {
                return BadRequest("Invalid supplier ID.");
            }
            var result = await _supplierService.GetSupplierAddress(supplierId);
            if (result == null || !result.Any())
            {
                return NotFound("No addresses found for the specified supplier.");
            }
            return Ok(result);
        }
    }
}
