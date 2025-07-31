using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrdersPlanRepository;

namespace VietausWebAPI.WebAPI.Controllers.v1.Labs
{
    [ApiController]
    [Route("api/QCOutput")]
    [AllowAnonymous]
    public class QCOutputController : Controller
    {
        private readonly IQCOutputService _qCOutputService;

        public QCOutputController (IQCOutputService qCOutputService)
        {
            _qCOutputService = qCOutputService;
        }

        [HttpGet("GetPaged")]
        public async Task<IActionResult> GetMfgProductOrder([FromQuery] MfgPOLQuery query)
        {
            var result = await _qCOutputService.GetPageAsync(query);
            return Ok(result);
        }

        [HttpPut("{productId}/product-name")]
        public async Task<IActionResult> UpdateProductName(Guid productId, [FromBody] string newName)
        {
            await _qCOutputService.UpdateProductNameInPlansAsync(productId, newName);
            return Ok("Cập nhật tên sản phẩm thành công!");
        }
    }
}
