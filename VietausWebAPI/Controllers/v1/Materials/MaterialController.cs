using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.GetDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.PatchDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.PostDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Material;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
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
      
        [HttpGet]
        public async Task<ActionResult<PagedResult<GetMaterialSummary>>> List(
            [FromQuery] MaterialQuery query,
            CancellationToken ct = default)
        {
            var result = await _materialService.GetAllAsync(query, ct);
            return Ok(result);

        }

        [HttpGet("m-p")]
        public async Task<ActionResult<PagedResult<GetMaterialSummary>>> ListMP(
            [FromQuery] MaterialQuery query,
            CancellationToken ct = default)
        {
            var result = await _materialService.GetAllMPAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<GetMaterial>> GetMaterialById(Guid Id,
            CancellationToken ct = default)
        {
            var result = await _materialService.GetMaterialByIdAsync(Id, ct);
            return Ok(result);

        }

        [HttpGet("PriceHistory")]
        public async Task<ActionResult<PagedResult<GetPriceHistory>>> GetMaterialPriceHistoryById(
            [FromQuery] MaterialQuery query,
            CancellationToken ct = default)
        {
            var result = await _materialService.GetMaterialPriceHistoryByIdAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("materialsupplier")]
        public async Task<ActionResult<PagedResult<GetMaterialSupplier>>> GetMaterialSupplier(
            [FromQuery] MaterialQuery query,
            CancellationToken ct = default)
        {
            var result = await _materialService.GetMaterialSupplierAsync(query, ct);
            return Ok(result);
        }

        [HttpPatch("upsert")]
        public async Task<IActionResult> UpsertMaterial([FromBody] PatchMaterial req)
        {
            if (req == null)
            {
                return BadRequest("Group data is required.");
            }
            var result = await _materialService.UpsertMaterialAsync(req);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPatch("delete/{Id:guid}")]
        public async Task<IActionResult> DeleteMaterial(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("Id is required.");
            }
            var result = await _materialService.DeleteMaterialAsync(Id);
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
