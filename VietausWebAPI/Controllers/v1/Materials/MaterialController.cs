using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.WebAPI.Controllers.v1.Materials
{
    [ApiController]
    [Route("api/material")]
    [AllowAnonymous]
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService) 
        {
            _materialService = materialService;
        }

        [HttpPost("AddMaterial")]
        public async Task<IActionResult> AddNewMaterial([FromBody] PostMaterial material)
        {
            if (material == null)
            {
                return BadRequest("Group data is required.");
            }
            var result = await _materialService.AddNewMaterialAsync(material);
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
