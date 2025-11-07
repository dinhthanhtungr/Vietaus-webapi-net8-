using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.TimelineFeature.Queries;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1.TimelineFeature
{
    [ApiController]
    [Route("api/timeline")]
    [AllowAnonymous]
    public class TimelineController : Controller
    {
        private readonly ITimelineService _timelineService;

        public TimelineController(ITimelineService timelineService)
        {
            _timelineService = timelineService;
        }

        /// <summary>
        /// Lấy dữ liệu thông tin trạng thái đơn hàng theo timeline
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMerchadiseTimelineAsync([FromQuery] TimelineQuery query, CancellationToken ct = default)
        {
            var result = await _timelineService.GetMerchadiseTimelineAsync(query, ct);
            return Ok(result);
        }

        /// <summary>
        /// Lấy dữ liệu thông tin trạng thái lệnh sản xuất theo timeline
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //[HttpGet("manufacturing")]
        //public async Task<IActionResult> GetManufacturingTimelineDetailAsync([FromQuery] TimelineQuery query, CancellationToken ct = default)
        //{
        //    var result = await _timelineService.GetManufacturingTimelineDetailAsync(query, ct);
        //    return Ok(result);
        //}

        /// <summary>
        /// Lấy dữ liệu thông tin trạng thái lệnh sản xuất theo timeline
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("merchadiseDetail")]
        public async Task<IActionResult> GetMerchadiseTimelineDetailAsync([FromQuery] TimelineQuery query, CancellationToken ct = default)
        {
            var result = await _timelineService.GetMerchadiseTimelineDetailAsync(query, ct);
            return Ok(result);
        }
    }
}
