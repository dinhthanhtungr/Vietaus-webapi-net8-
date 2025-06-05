using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.DTOs.Materials;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.ServiceContracts;
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
        private readonly IMaterialsService _materialsService;
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="materialSuppliersService"></param>
        public MaterialSuppliersController(IMaterialSupplierService materialSuppliersService, IMaterialsService materialsService)
        {
            _materialSuppliersService = materialSuppliersService;
            _materialsService = materialsService;
        }
        /// <summary>
        /// Thêm mới nhà cung cấp vật liệu
        /// </summary>
        /// <param name="materialSuppliersDTO"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> AddMaterialSupplier([FromBody] MaterialSuppliersDTO materialSuppliersDTO)
        {
            await _materialSuppliersService.AddMaterialSuppliersServiceAsync(materialSuppliersDTO);
            return Ok(new { message = "Request complion" });
        }
        /// <summary>
        /// Lấy tất cả nhà cung cấp vật liệu
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMaterialSuppliers()
        {
            var resuilt = await _materialSuppliersService.GetAllMaterialSuppliersServiceAsync();
            return Ok(resuilt);
        }


        [HttpPost("AddNewMaterials")]
        public async Task<IActionResult> GetAllMaterials(List<MaterialsDTO> materialsDTO)
        {
            await _materialsService.CreateMaterialAsync(materialsDTO);
            return Ok();
        }
    }
}
