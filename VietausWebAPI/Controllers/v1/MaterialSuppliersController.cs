using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/MaterialSuppliers")]
    [AllowAnonymous]
    public class MaterialSuppliersController : Controller
    {
        private readonly IMaterialSupplierService _materialSuppliersService;

        public MaterialSuppliersController(IMaterialSupplierService materialSuppliersService)
        {
            _materialSuppliersService = materialSuppliersService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddMaterialSupplier([FromBody] MaterialSuppliersDTO materialSuppliersDTO)
        {
            await _materialSuppliersService.AddMaterialSuppliersServiceAsync(materialSuppliersDTO);
            return Ok(new { message = "Request complion" });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMaterialSuppliers()
        {
            var resuilt = await _materialSuppliersService.GetAllMaterialSuppliersServiceAsync();
            return Ok(resuilt);
        }
    }
}
