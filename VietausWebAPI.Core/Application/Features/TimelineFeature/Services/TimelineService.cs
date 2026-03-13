using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.ManufacturingTimeline;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.MerchadiseTimeline;
using VietausWebAPI.Core.Application.Features.TimelineFeature.Queries;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.AuditSchema;
using VietausWebAPI.Core.Domain.Enums.Logs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static QuestPDF.Helpers.Colors;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.Services
{
    public class TimelineService : ITimelineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IVisibilityHelper _visibilityHelper;
        public TimelineService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IVisibilityHelper visibilityHelper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _visibilityHelper = visibilityHelper;
        }

        // ======================================================================== Get ======================================================================== 

        /// <summary>
        /// Lấy dữ liệu cho timeline đơn hàng merchandise
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<PagedResult<GetMerchadiseTimeline>>> GetMerchadiseTimelineAsync(TimelineQuery query, CancellationToken ct = default)
        {
            // 0) Chuẩn hoá phân trang
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 15;

            // Chuẩn hoá range ngày
            DateTime? fromC = query.FromCreated?.Date;
            DateTime? toCExcl = query.ToCreated?.Date.AddDays(1);
            bool hasCreatedRange = fromC.HasValue || toCExcl.HasValue;

            // Keyword
            string? kw = string.IsNullOrWhiteSpace(query.Keyword) ? null : query.Keyword.Trim();
            string? likeKw = kw is null ? null : $"%{kw}%";

            // Status hiện tại của order
            string? orderStatus = string.IsNullOrWhiteSpace(query.Status) ? null : query.Status.Trim();

            var viewer = await _visibilityHelper.BuildViewerScopeAsync(ct);

            // 1) Base query: MerchandiseOrder
            var baseQ = _unitOfWork.MerchandiseOrderRepository.Query()
                .AsNoTracking()
                .Where(mo => mo.IsActive);

            baseQ = _visibilityHelper.ApplyMerchandiseOrder(baseQ, viewer);

            // Nếu có id cụ thể
            if (query.id.HasValue && query.id.Value != Guid.Empty)
                baseQ = baseQ.Where(mo => mo.MerchandiseOrderId == query.id.Value);

            // 1.1) Keyword trên order / customer / detail product snapshot
            if (likeKw != null)
            {
                baseQ = baseQ.Where(mo =>
                    (mo.ExternalId != null && EF.Functions.ILike(mo.ExternalId, likeKw)) ||
                    EF.Functions.ILike(mo.CustomerNameSnapshot, likeKw) ||
                    EF.Functions.ILike(mo.CustomerExternalIdSnapshot, likeKw) ||
                    _unitOfWork.MerchandiseOrderRepository.QueryDetail(false)
                        .Any(md =>
                            md.MerchandiseOrderId == mo.MerchandiseOrderId &&
                            md.IsActive &&
                            md.ProductExternalIdSnapshot != null &&
                            EF.Functions.ILike(md.ProductExternalIdSnapshot, likeKw)
                        )
                );
            }

            // 1.2) Lọc theo status hiện tại của order
            if (orderStatus != null)
            {
                baseQ = baseQ.Where(mo =>
                    mo.Status != null &&
                    mo.Status == orderStatus);
            }

            // 1.3) Áp range From/ToCreated theo CreatedScope
            if (hasCreatedRange && query.CreatedScope == TimelineScope.Merchandise.ToString())
            {
                if (fromC.HasValue) baseQ = baseQ.Where(mo => mo.CreateDate >= fromC.Value);
                if (toCExcl.HasValue) baseQ = baseQ.Where(mo => mo.CreateDate < toCExcl.Value);
            }

            if (hasCreatedRange && query.CreatedScope == TimelineScope.Manufacturing.ToString())
            {
                baseQ = baseQ.Where(mo =>
                    _unitOfWork.MfgOrderPORepository.Query(false)
                        .Any(link =>
                            link.IsActive &&
                            link.Detail.MerchandiseOrderId == mo.MerchandiseOrderId &&
                            link.ProductionOrder.IsActive &&
                            (!fromC.HasValue || link.ProductionOrder.CreatedDate >= fromC.Value) &&
                            (!toCExcl.HasValue || link.ProductionOrder.CreatedDate < toCExcl.Value)
                        )
                );
            }

            if (hasCreatedRange && query.CreatedScope == TimelineScope.Delivery.ToString())
            {
                baseQ = baseQ.Where(mo =>
                    _unitOfWork.DeliveryOrderPORepository.Query(false)
                        .Any(dop =>
                            dop.IsActive &&
                            dop.MerchandiseOrderId == mo.MerchandiseOrderId &&
                            (!fromC.HasValue || dop.DeliveryOrder.CreatedDate >= fromC.Value) &&
                            (!toCExcl.HasValue || dop.DeliveryOrder.CreatedDate < toCExcl.Value)
                        )
                );
            }

            if (hasCreatedRange && query.CreatedScope == TimelineScope.Requisition.ToString())
            {
                baseQ = baseQ.Where(mo =>
                    _unitOfWork.MerchandiseOrderRepository.QueryDetail(false)
                        .Any(md =>
                            md.MerchandiseOrderId == mo.MerchandiseOrderId &&
                            md.IsActive &&
                            (!fromC.HasValue || md.DeliveryRequestDate >= fromC.Value) &&
                            (!toCExcl.HasValue || md.DeliveryRequestDate < toCExcl.Value)
                        )
                );
            }

            // 1.4) Lọc theo log chỉ cho những filter thật sự thuộc log
            bool hasLogFilter =
                query.CreatedBy.HasValue ||
                query.CompanyId.HasValue ||
                query.EventType != default(EventType);

            if (hasLogFilter)
            {
                baseQ = baseQ.Where(mo =>
                    _unitOfWork.EventLogRepository.Query(false)
                        .Any(e =>
                            e.IsActive &&
                            e.SourceId == mo.MerchandiseOrderId &&
                            (!query.CreatedBy.HasValue || e.EmployeeID == query.CreatedBy.Value) &&
                            (!query.CompanyId.HasValue || e.CompanyId == query.CompanyId.Value) &&
                            (query.EventType == default(EventType) || e.EventType == query.EventType)
                        )
                );
            }

            // 2) Tổng số
            var totalCount = await baseQ.CountAsync(ct);

            // 3) Lấy header từ MerchandiseOrder
            var pageOrders = await baseQ
                .OrderByDescending(mo => mo.CreateDate)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(mo => new GetMerchadiseTimeline
                {
                    MerchandiseOrderId = mo.MerchandiseOrderId,
                    ExternalId = mo.ExternalId ?? string.Empty,
                    CreatedName = mo.CreatedByNavigation != null
                        ? (mo.CreatedByNavigation.FullName ?? string.Empty)
                        : string.Empty,
                    Status = mo.Status ?? string.Empty,
                    TotalPrice = mo.TotalPrice ?? 0m,
                    Vat = mo.Vat ?? 0m,
                    CreatedDate = mo.CreateDate,
                    CustomerName = mo.CustomerNameSnapshot,
                    CustomerExternalId = mo.CustomerExternalIdSnapshot,
                    Details = new List<GetMerchadiseTimelineDetail>()
                })
                .ToListAsync(ct);

            if (pageOrders.Count == 0)
            {
                return OperationResult<PagedResult<GetMerchadiseTimeline>>.Ok(
                    new PagedResult<GetMerchadiseTimeline>(
                        pageOrders,
                        totalCount,
                        query.PageNumber,
                        query.PageSize
                    )
                );
            }

            var orderIds = pageOrders.Select(o => o.MerchandiseOrderId).ToList();

            // 4) Lấy timeline details từ EventLog
            var logsQ = _unitOfWork.EventLogRepository.Query(false)
                .Where(e => e.IsActive && orderIds.Contains(e.SourceId));

            if (query.CreatedBy.HasValue)
                logsQ = logsQ.Where(e => e.EmployeeID == query.CreatedBy.Value);

            if (query.CompanyId.HasValue)
                logsQ = logsQ.Where(e => e.CompanyId == query.CompanyId.Value);

            if (query.EventType != default(EventType))
                logsQ = logsQ.Where(e => e.EventType == query.EventType);

            // KHÔNG filter lại theo query.Status ở đây
            // vì Status header là trạng thái hiện tại của order, không phải log status

            var logs = await logsQ
                .OrderBy(e => e.SourceId)
                .ThenBy(e => e.CreatedDate)
                .Select(e => new
                {
                    OrderId = e.SourceId,
                    Status = e.Status,
                    CreatedDate = e.CreatedDate,
                    CreatedByName = e.CreatedByNavigation != null
                        ? (e.CreatedByNavigation.FullName ?? string.Empty)
                        : string.Empty,
                    CompanyName = e.Company != null
                        ? (e.Company.Name ?? string.Empty)
                        : string.Empty,
                    Note = e.Note
                })
                .ToListAsync(ct);

            // 5) Group logs -> details
            var grouped = logs
                .GroupBy(l => l.OrderId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(l => new GetMerchadiseTimelineDetail
                    {
                        Status = l.Status ?? string.Empty,
                        CreatedByName = l.CreatedByName,
                        CreatedDate = l.CreatedDate,
                        CompanyName = l.CompanyName,
                        Note = l.Note
                    }).ToList()
                );

            // 6) Gán details cho từng order
            foreach (var o in pageOrders)
            {
                if (grouped.TryGetValue(o.MerchandiseOrderId, out var details))
                    o.Details = details;
            }

            return OperationResult<PagedResult<GetMerchadiseTimeline>>.Ok(
                new PagedResult<GetMerchadiseTimeline>(
                    pageOrders,
                    totalCount,
                    query.PageNumber,
                    query.PageSize
                )
            );
        }

        /// <summary>
        /// Lấy dữ liệu thông tin trạng thái của từng chi tiết trong đơn hàng theo timeline của lệnh sản xuất Manufacturing
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<PagedResult<GetMerchadiseTimelineInforDetail>>> GetMerchadiseTimelineDetailAsync(TimelineQuery query, CancellationToken ct = default)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 15;

            if (query.id == Guid.Empty)
                throw new Exception("Id đơn hàng không hợp lệ.");

            var page = Math.Max(1, query.PageNumber);
            var size = Math.Max(1, query.PageSize);

            // ----------------------------------------------------
            // 1) Base query MFG link theo đơn hàng
            // ----------------------------------------------------
            var mfgBaseQ = _unitOfWork.MfgOrderPORepository.Query(false)
                .AsNoTracking()
                .Where(link =>
                    link.IsActive &&
                    link.Detail.MerchandiseOrderId == query.id &&
                    link.ProductionOrder.IsActive);

            // Distinct theo MfgProductionOrderId ngay từ DB
            var mfgPageQ = mfgBaseQ
                .GroupBy(link => new
                {
                    link.MfgProductionOrderId,
                    link.ProductionOrder.ExternalId,
                    link.ProductionOrder.ProductExternalIdSnapshot,
                    link.ProductionOrder.ProductNameSnapshot,
                    link.ProductionOrder.ProductId,
                    link.ProductionOrder.ExpectedDate
                })
                .Select(g => new
                {
                    MfgProductionOrderId = g.Key.MfgProductionOrderId,
                    ExternalId = g.Key.ExternalId,
                    ColourCode = g.Key.ProductExternalIdSnapshot,
                    ProductName = g.Key.ProductNameSnapshot,
                    ProductId = g.Key.ProductId,
                    ExpectedCompletionDate = g.Key.ExpectedDate
                });

            var totalMfg = await mfgPageQ.CountAsync(ct);

            if (totalMfg == 0)
            {
                return OperationResult<PagedResult<GetMerchadiseTimelineInforDetail>>.Ok(
                    new PagedResult<GetMerchadiseTimelineInforDetail>(
                        new List<GetMerchadiseTimelineInforDetail>(),
                        0,
                        page,
                        size
                    )
                );
            }

            var pageMfg = await mfgPageQ
                .OrderBy(x => x.ExternalId)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(ct);

            var pageMfgIds = pageMfg.Select(x => x.MfgProductionOrderId).ToList();
            var productIdsInPage = pageMfg.Select(x => x.ProductId).Distinct().ToList();

            // ----------------------------------------------------
            // 2) Delivery list theo ProductId của đơn hàng này
            // Tối ưu: không cần query doIds trước rồi Contains
            // ----------------------------------------------------
            var doByProduct = await _unitOfWork.DeliveryOrderPORepository.Query()
                .AsNoTracking()
                .Where(dop => dop.IsActive && dop.MerchandiseOrderId == query.id)
                .SelectMany(dop => dop.DeliveryOrder.Details
                    .Where(dod => dod.IsActive)
                    .Select(dod => new
                    {
                        dod.ProductId,
                        DOExternalId = dop.DeliveryOrder.ExternalId,
                        dod.LotNoList
                    }))
                .Where(x => !string.IsNullOrWhiteSpace(x.DOExternalId))
                .GroupBy(x => x.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    DOList = g
                        .GroupBy(x => new { x.DOExternalId, x.LotNoList })
                        .Select(x => new DeliveryInfoDto
                        {
                            DOExternalId = x.Key.DOExternalId!,
                            LotNoList = x.Key.LotNoList
                        })
                        .ToList()
                })
                .ToListAsync(ct);

            var deliveryDict = doByProduct.ToDictionary(
                x => x.ProductId,
                x => x.DOList
            );

            // ----------------------------------------------------
            // 3) MerchandiseOrderDetail aggregate theo ProductId
            // Tránh ToDictionary lỗi duplicate key
            // ----------------------------------------------------
            var merchDetails = await _unitOfWork.MerchandiseOrderRepository.QueryDetail(false)
                .AsNoTracking()
                .Where(d =>
                    d.MerchandiseOrderId == query.id &&
                    d.IsActive &&
                    productIdsInPage.Contains(d.ProductId))
                .GroupBy(d => d.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    RequestQuantity = g.Sum(x => (decimal?)x.ExpectedQuantity) ?? 0m,
                    RealQuantity = g.SelectMany(x => x.DeliveryOrderDetails.Where(dd => dd.IsActive))
                                    .Sum(dd => (decimal?)dd.Quantity) ?? 0m,
                    RequestDate = g.Min(x => x.DeliveryRequestDate),
                    UnitPrice = g.Average(x => (decimal)x.UnitPriceAgreed)
                })
                .ToDictionaryAsync(x => x.ProductId, x => x, ct);

            // ----------------------------------------------------
            // 4) Logs của các MFG trong trang hiện tại
            // Tối ưu: lấy thẳng CreatedByName + CompanyName trong query
            // ----------------------------------------------------
            var logsQ = _unitOfWork.EventLogRepository.Query()
                .AsNoTracking()
                .Where(e => e.IsActive && pageMfgIds.Contains(e.SourceId));

            if (!string.IsNullOrWhiteSpace(query.Status))
                logsQ = logsQ.Where(e => e.Status == query.Status);

            var logRows = await logsQ
                .OrderBy(e => e.SourceId)
                .ThenBy(e => e.CreatedDate)
                .Select(e => new
                {
                    e.SourceId,
                    e.Status,
                    e.CreatedDate,
                    CreatedByName = e.CreatedByNavigation != null
                        ? (e.CreatedByNavigation.FullName ?? string.Empty)
                        : string.Empty,
                    CompanyName = e.Company != null
                        ? (e.Company.Name ?? string.Empty)
                        : string.Empty,
                    e.Note
                })
                .ToListAsync(ct);

            var detailsByMfg = logRows
                .GroupBy(l => l.SourceId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(l => new GetMerchadiseTimelineDetail
                    {
                        Status = l.Status ?? string.Empty,
                        CreatedDate = l.CreatedDate,
                        CreatedByName = l.CreatedByName,
                        CompanyName = l.CompanyName,
                        Note = l.Note
                    }).ToList()
                );

            // ----------------------------------------------------
            // 5) Build items
            // ----------------------------------------------------
            var items = pageMfg.Select(m =>
            {
                var deliveries = deliveryDict.TryGetValue(m.ProductId, out var list)
                    ? list
                    : new List<DeliveryInfoDto>();

                var hasDetail = merchDetails.TryGetValue(m.ProductId, out var md);

                return new GetMerchadiseTimelineInforDetail
                {
                    ExternalId = m.ExternalId,
                    ColourCode = m.ColourCode,
                    ProductName = m.ProductName,

                    Deliveries = deliveries,

                    RequestQuantity = hasDetail ? md!.RequestQuantity : null,
                    RealQuantity = hasDetail ? md!.RealQuantity : null,
                    RequestDate = hasDetail ? md!.RequestDate : default,
                    ExpectedDate = m.ExpectedCompletionDate,
                    UnitPrice = hasDetail ? md!.UnitPrice : 0m,

                    Details = detailsByMfg.TryGetValue(m.MfgProductionOrderId, out var ds)
                        ? ds
                        : new List<GetMerchadiseTimelineDetail>()
                };
            }).ToList();

            // ----------------------------------------------------
            // 6) Return
            // ----------------------------------------------------
            return OperationResult<PagedResult<GetMerchadiseTimelineInforDetail>>.Ok(
                new PagedResult<GetMerchadiseTimelineInforDetail>(items, totalMfg, page, size)
            );
        }

        // ======================================================================== Post ======================================================================== 

        /// <summary>
        /// Thêm một EventLog
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task AddEventLogAsync(EventLogModels query, CancellationToken ct = default)
        {
            try
            {
                var EmployeeExisting = _unitOfWork.EmployeesRepository.Query()
                    .FirstOrDefault(e => e.EmployeeId == query.employeeId); 

                var newLog = new EventLog
                {
                    CompanyId = EmployeeExisting.CompanyId.GetValueOrDefault(),
                    DepartmentId = EmployeeExisting.PartId ?? throw new Exception("Employee's PartId is null"),
                    EmployeeID = query.employeeId,
                    SourceId = query.sourceId,
                    SourceCode = query.sourceCode ?? string.Empty,
                    EventType = query.eventType,
                    Status = query.status ?? string.Empty, // Fix: ensure Status is never null
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Note = query.note
                };
                return _unitOfWork.EventLogRepository.AddAsync(newLog, ct);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm nhật ký sự kiện: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm nhiều EventLog
        /// </summary>
        /// <param name="queries"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task AddEventLogRangeAsync(IEnumerable<EventLogModels> queries, CancellationToken ct = default)
        {
            try
            {
                var newLogs = new List<EventLog>();
                foreach (var query in queries)
                {
                    var EmployeeExisting = _unitOfWork.EmployeesRepository.Query()
                        .FirstOrDefault(e => e.EmployeeId == query.employeeId);
                    var newLog = new EventLog
                    {
                        CompanyId = EmployeeExisting.CompanyId.GetValueOrDefault(),
                        DepartmentId = EmployeeExisting.PartId ?? throw new Exception("Employee's PartId is null"),
                        EmployeeID = query.employeeId,
                        SourceId = query.sourceId,
                        SourceCode = query.sourceCode ?? string.Empty,
                        EventType = query.eventType,
                        Status = query.status ?? string.Empty, // Fix: ensure Status is never null
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        Note = query.note
                    };
                    newLogs.Add(newLog);
                }
                return _unitOfWork.EventLogRepository.AddRangeAsync(newLogs,  ct);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm nhật ký sự kiện: " + ex.Message);
            }
        }

    }

    // ======================================================================== Helper ======================================================================== 
    file static class ListExt
    {
        public static List<T> Also<T>(this List<T> list, Action<List<T>> act)
        {
            act(list);
            return list;
        }
    }
}
