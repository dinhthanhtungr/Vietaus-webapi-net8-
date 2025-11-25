using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Planning.Queries.SchedualFeatures;
using VietausWebAPI.Core.Application.Features.Planning.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1.Planning
{
    [ApiController]
    [Route("api/Schedueal")]
    [AllowAnonymous]
    public class ScheduealController : Controller
    {
        private readonly IScheduealService _scheduealService;

        public ScheduealController(IScheduealService scheduealService   )
        {
            _scheduealService = scheduealService;
        }

        //[HttpGet("GetPaged")]
        //public async Task<IActionResult> GetScheduealPaged([FromQuery] SchedualQuery? query)
        //{
        //    var result = await _scheduealService.GetSchedualPageAsync(query);
        //    return Ok(result);
        //}

        //[HttpGet("GetById/{ExternalId}")]
        //public async Task<IActionResult> GetScheduealById(string ExternalId)
        //{
        //    var result = await _scheduealService.GetScheduealByIdAsync(ExternalId);
        //    return Ok(result);
        //}
    }
}
