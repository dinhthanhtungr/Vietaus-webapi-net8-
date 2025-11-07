using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Queries;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.ManufacturingTimeline;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.MerchadiseTimeline;
using VietausWebAPI.Core.Application.Features.TimelineFeature.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts
{
    public interface ITimelineService
    {
        Task AddEventLogAsync(EventLogModels query, CancellationToken ct = default);
        Task AddEventLogRangeAsync(IEnumerable<EventLogModels> queries, CancellationToken ct = default);

        /// <summary>
        /// Lấy dữ liệu thông tin trạng thái đơn hàng theo timeline
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetMerchadiseTimeline>> GetMerchadiseTimelineAsync(TimelineQuery query, CancellationToken ct = default);

        /// <summary>
        /// Lấy dữ liệu thông tin trạng thái của từng chi tiết trong đơn hàng theo timeline
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetMerchadiseTimelineInforDetail>> GetMerchadiseTimelineDetailAsync(TimelineQuery query, CancellationToken ct = default);

        /// <summary>
        /// Lấy dữ liệu thông tin trạng thái đơn hàng theo timeline
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //Task<PagedResult<GetManufacturingTimeline>> GetManufacturingTimelineDetailAsync(TimelineQuery query, CancellationToken ct = default);
    }
}
