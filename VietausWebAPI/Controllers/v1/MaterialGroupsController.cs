using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/MaterialGroups")]
    [AllowAnonymous]
    public class MaterialGroupsController : Controller
    {
        private readonly IMaterialGroupsService _materialGroupsService;

        public MaterialGroupsController(IMaterialGroupsService materialGroupsService)
        {
            _materialGroupsService = materialGroupsService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddMaterialGroup([FromBody] MaterialGroupsDTO materialGroupDTO)
        {
            await _materialGroupsService.AddMaterialGroupServiceAsync(materialGroupDTO);
            return Ok(new { message = "Request complion" });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMaterialGroups()
        {
            var result = await _materialGroupsService.GetAllMaterialGroupsServiceAsync();
            return Ok(result);
        }
    }
}
