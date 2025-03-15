using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/SupplyRequestsMaterial")]
    [AllowAnonymous]
    public class SupplyRequestsMaterialController : Controller
    {
        private readonly ISupplyRequestsMaterialDatumService _supplyRequestsMaterialDatumService;

        public SupplyRequestsMaterialController (ISupplyRequestsMaterialDatumService supplyRequestsMaterialDatumService)
        {
            _supplyRequestsMaterialDatumService = supplyRequestsMaterialDatumService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddSupplyRequestMaterial([FromBody] SupplyRequestsMaterialDatumDTO supplyRequestsMaterialDatumDTO)
        {
            await _supplyRequestsMaterialDatumService.AddSupplyRequestsMaterialDatumAsync(supplyRequestsMaterialDatumDTO);
            return Ok(new { Message = "Request complion" });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllSupplyRequestsMaterial()
        {

            var result = await _supplyRequestsMaterialDatumService.GetAllSupplyRequestsMaterialDatumAsync();
            return Ok(result);
        }


    }
}
