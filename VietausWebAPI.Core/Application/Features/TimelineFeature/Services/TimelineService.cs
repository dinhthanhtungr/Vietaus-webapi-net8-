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
        /// Lấy dữ liệu thông tin trạng thái đơn hàng theo timeline
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        //public async Task<PagedResult<GetManufacturingTimeline>> GetManufacturingTimelineDetailAsync(TimelineQuery query, CancellationToken ct = default)
        //{
        //    if (query.PageNumber <= 0) query.PageNumber = 1;
        //    if (query.PageSize <= 0) query.PageSize = 15;
        //    if (query.id == Guid.Empty)
        //        throw new Exception("Id đơn hàng không hợp lệ.");

        //    // 1) Lấy các MfgProductionOrderId thuộc đơn
        //    var mfgIds = await _unitOfWork.MfgProductionOrderRepository.Query(false)
        //        .Where(mpo => mpo.MerchandiseOrderId == query.id && (mpo.IsActive == null || mpo.IsActive == true))
        //        .Select(x => x.MfgProductionOrderId)
        //        .ToListAsync(ct);

        //    //var merchadiseExisting = await _unitOfWork.MerchandiseOrderRepository.Query()


        //    // 2) Nguồn log = { id đơn } ∪ { tất cả MfgProductionOrderId }
        //    var sourceIds = new List<Guid>(1 + mfgIds.Count);
        //    sourceIds.AddRange(mfgIds);

        //    // 3) Base logs + filter tùy query
        //    var baseLogs = _unitOfWork.EventLogRepository.Query()
        //        .Where(e => e.IsActive && sourceIds.Contains(e.SourceId));

        //    if (!string.IsNullOrWhiteSpace(query.Status))
        //        baseLogs = baseLogs.Where(e => e.Status == query.Status);

        //    // 4) Nhóm theo SourceId để phân trang theo NHÓM timeline
        //    var groupedKeys = await baseLogs
        //        .GroupBy(e => e.SourceId)
        //        .Select(g => new
        //        {
        //            SourceId = g.Key,
        //            LatestCreated = g.Max(x => x.CreatedDate)
        //        })
        //        .OrderByDescending(x => x.LatestCreated)      // nhóm có hoạt động mới nhất lên trước
        //        .ToListAsync(ct);

        //    var totalGroups = groupedKeys.Count;
        //    var page = Math.Max(1, query.PageNumber);
        //    var size = Math.Max(1, query.PageSize);

        //    var pageGroups = groupedKeys
        //        .Skip((page - 1) * size)
        //        .Take(size)
        //        .ToList();

        //    var pageSourceIds = pageGroups.Select(x => x.SourceId).ToList();

        //    // 5) Lấy logs cho các nhóm ở trang hiện tại
        //    var pageLogs = await baseLogs
        //        .Where(e => pageSourceIds.Contains(e.SourceId))
        //        .OrderBy(e => e.SourceId)
        //        .ThenByDescending(e => e.CreatedDate)
        //        .Select(e => new
        //        {
        //            e.SourceId,
        //            e.Status,
        //            e.CreatedDate,
        //            e.EmployeeID,
        //            e.CompanyId,
        //            e.Note
        //        })
        //        .ToListAsync(ct);

        //    // Resolve Employee/Company cho trang hiện tại
        //    var creatorIds = pageLogs.Select(x => x.EmployeeID).Distinct().ToList();

        //    var empMap = creatorIds.Count == 0
        //        ? new Dictionary<Guid, (string FullName, string CompanyName)>()
        //        : await _unitOfWork.EmployeesRepository.Query()
        //            .AsNoTracking()
        //            .Where(emp => creatorIds.Contains(emp.EmployeeId))
        //            .Select(emp => new
        //            {
        //                emp.EmployeeId,
        //                FullName = emp.FullName ?? string.Empty,
        //                CompanyName = emp.Company != null ? emp.Company.Name : string.Empty
        //            })
        //            .ToDictionaryAsync(x => x.EmployeeId,
        //                               x => (x.FullName, x.CompanyName),
        //                               ct);

        //    // 6) Group logs -> details theo từng SourceId
        //    var detailBySource = pageLogs
        //        .GroupBy(l => l.SourceId)
        //        .ToDictionary(
        //            g => g.Key,
        //            g => g.Select(l => new GetManufacturingTimelineDetail
        //            {
        //                Status = l.Status ?? string.Empty,
        //                CreatedDate = l.CreatedDate,
        //                CreatedByName = empMap.TryGetValue(l.EmployeeID, out var emp) ? emp.FullName : string.Empty,
        //                CompanyName = empMap.TryGetValue(l.EmployeeID, out emp) ? emp.CompanyName : string.Empty,
        //                Note = l.Note
        //            }).ToList()
        //        );

        //    // 7) Resolve ExternalId + ColourCode cho từng timeline trong trang (toàn là MFG)
        //    var pageMfgIds = pageSourceIds; // vì sourceIds đã là danh sách MfgProductionOrderId

        //    // map MfgProductionOrderId -> (ExternalId, ColourCodeSnapshot)
        //    var mfgBrief = pageMfgIds.Count == 0
        //        ? new Dictionary<Guid, (string? ExternalId, string? ColourCodeSnapshot)>()
        //        : await _unitOfWork.MfgProductionOrderRepository.Query(false)
        //            .AsNoTracking()
        //            .Where(m => pageMfgIds.Contains(m.MfgProductionOrderId))
        //            .Select(m => new
        //            {
        //                m.MfgProductionOrderId,
        //                m.ExternalId,
        //                // Đổi đúng tên field snapshot của bạn tại đây:
        //                ColourCodeSnapshot = m.ProductExternalIdSnapshot // ví dụ: m.ColourCodeSnapshot hoặc m.ColorCodeSnapshot
        //            })
        //            .ToDictionaryAsync(
        //                x => x.MfgProductionOrderId,
        //                x => (x.ExternalId, x.ColourCodeSnapshot),
        //                ct
        //            );

        //    // 8) Lắp danh sách GetManufacturingTimeline theo thứ tự group của trang
        //    var items = new List<GetManufacturingTimeline>(pageGroups.Count);

        //    foreach (var grp in pageGroups)
        //    {
        //        var srcId = grp.SourceId;

        //        string? externalId = null;
        //        string? colourCode = null;

        //        if (mfgBrief.TryGetValue(srcId, out var info))
        //        {
        //            externalId = info.ExternalId;
        //            colourCode = info.ColourCodeSnapshot; // lấy thẳng từ snapshot của MFG
        //        }

        //        items.Add(new GetManufacturingTimeline
        //        {
        //            ExternalId = externalId,
        //            ColourCode = colourCode, // dùng đúng property "ColourCode" theo DTO của bạn
        //            Details = detailBySource.TryGetValue(srcId, out var ds)
        //                ? ds.OrderByDescending(d => d.CreatedDate).ToList()
        //                : new List<GetManufacturingTimelineDetail>()
        //        });
        //    }
        //    // 9) Trả về PagedResult (phân trang theo NHÓM timeline)
        //    return new PagedResult<GetManufacturingTimeline>(
        //        items,
        //        totalGroups,
        //        page,
        //        size
        //    );
        //}

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

            // Chuẩn hoá range ngày (bao trọn ngày cuối bằng < To+1d)
            DateTime? fromC = query.FromCreated?.Date;
            DateTime? toCExcl = query.ToCreated?.Date.AddDays(1);
            bool hasCreatedRange = fromC.HasValue || toCExcl.HasValue;

            // Keyword (ILIKE)
            string? kw = string.IsNullOrWhiteSpace(query.Keyword) ? null : query.Keyword!.Trim();
            string? likeKw = kw is null ? null : $"%{kw}%";

            var viewer = await _visibilityHelper.BuildViewerScopeAsync(ct);

            // 1) Base query: MerchandiseOrder
            var baseQ = _unitOfWork.MerchandiseOrderRepository.Query()
                .AsNoTracking()
                .Where(mo => mo.IsActive == true);


            baseQ = _visibilityHelper.ApplyMerchandiseOrder(baseQ, viewer);
            // Nếu có id cụ thể (trường hợp muốn tìm đúng 1 đơn)
            if (query.id.HasValue && query.id.Value != Guid.Empty)
                baseQ = baseQ.Where(mo => mo.MerchandiseOrderId == query.id.Value);

            // 1.1) Keyword trên MerchandiseOrder / Customer
            if (likeKw != null)
            {
                baseQ = baseQ.Where(mo =>
                    // Tìm trên ExternalId của đơn
                    (mo.ExternalId != null && EF.Functions.ILike(mo.ExternalId, likeKw)) ||

                    // *** CHANGED: dùng snapshot của khách hàng thay vì join Customer (nhẹ hơn)
                    (EF.Functions.ILike(mo.CustomerNameSnapshot, likeKw) ||
                     EF.Functions.ILike(mo.CustomerExternalIdSnapshot, likeKw))
                );
            }

            // 1.2) Áp range From/ToCreated theo CreatedScope

            // Merchandise: áp trên chính MerchandiseOrder.CreateDate (không đổi)
            if (hasCreatedRange && query.CreatedScope == TimelineScope.Merchandise.ToString())
            {
                if (fromC.HasValue) baseQ = baseQ.Where(mo => mo.CreateDate >= fromC.Value);
                if (toCExcl.HasValue) baseQ = baseQ.Where(mo => mo.CreateDate < toCExcl.Value);
            }

            // *** CHANGED: Manufacturing: bây giờ nối qua MfgOrderPO + MerchandiseOrderDetail + MfgProductionOrder
            if (hasCreatedRange && query.CreatedScope == TimelineScope.Manufacturing.ToString())
            {
                baseQ = baseQ.Where(mo =>
                    _unitOfWork.MfgOrderPORepository.Query(false)
                        .Any(link =>
                            link.IsActive &&

                            // Detail nằm trong đơn hàng này
                            link.Detail.MerchandiseOrderId == mo.MerchandiseOrderId &&

                            // ProductionOrder đang active
                            link.ProductionOrder.IsActive &&

                            // Filter theo CreatedDate của MfgProductionOrder
                            (!fromC.HasValue || link.ProductionOrder.CreatedDate >= fromC.Value) &&
                            (!toCExcl.HasValue || link.ProductionOrder.CreatedDate < toCExcl.Value)
                        )
                );
            }

            // Delivery: áp trên DeliveryOrder.CreateDate thông qua bảng nối DeliveryOrderPO
            // (giữ nguyên nếu schema DeliveryOrderPO vẫn giống cũ)
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

            // *** CHANGED: Requisition: MerchandiseOrderDetail.IsActive giờ là bool, DeliveryRequestDate không nullable
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

            // 1.4) Lọc theo LOGS (CreatedBy, CompanyId, EventType, Status)
            bool hasLogFilter =
                query.CreatedBy.HasValue ||
                query.CompanyId.HasValue ||
                query.EventType != default(EventType) ||
                !string.IsNullOrWhiteSpace(query.Status);

            if (hasLogFilter)
            {
                baseQ = baseQ.Where(mo =>
                    _unitOfWork.EventLogRepository.Query(false)
                        .Any(e =>
                            e.IsActive &&
                            e.SourceId == mo.MerchandiseOrderId &&
                            (!query.CreatedBy.HasValue || e.EmployeeID == query.CreatedBy.Value) &&
                            (!query.CompanyId.HasValue || e.CompanyId == query.CompanyId.Value) &&
                            (query.EventType == default(EventType) || e.EventType == query.EventType) &&
                            (string.IsNullOrWhiteSpace(query.Status) || e.Status == query.Status)
                        )
                );
            }

            // 2) Tổng số (sau khi áp hết filter) + phân trang
            var totalCount = await baseQ.CountAsync(ct);

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

                    CreatedDate = mo.CreateDate,

                    // *** CHANGED: dùng snapshot để đồng bộ với mô hình mới
                    CustomerName = mo.CustomerNameSnapshot,
                    CustomerExternalId = mo.CustomerExternalIdSnapshot,

                    Details = new List<GetMerchadiseTimelineDetail>()
                })
                .ToListAsync(ct);

            if (pageOrders.Count == 0)
                return OperationResult<PagedResult<GetMerchadiseTimeline>>.Ok(
                    new PagedResult<GetMerchadiseTimeline>(
                    pageOrders, totalCount, query.PageNumber, query.PageSize)
                );

            var orderIds = pageOrders.Select(o => o.MerchandiseOrderId).ToList();

            // 3) Lấy logs của các order trong trang
            var logsQ = _unitOfWork.EventLogRepository.Query()
                .Where(e => e.IsActive && orderIds.Contains(e.SourceId));

            if (query.CreatedBy.HasValue) logsQ = logsQ.Where(e => e.EmployeeID == query.CreatedBy.Value);
            if (query.CompanyId.HasValue) logsQ = logsQ.Where(e => e.CompanyId == query.CompanyId.Value);
            if (query.EventType != default(EventType)) logsQ = logsQ.Where(e => e.EventType == query.EventType);
            if (!string.IsNullOrWhiteSpace(query.Status)) logsQ = logsQ.Where(e => e.Status == query.Status);

            var logs = await logsQ
                .OrderBy(e => e.SourceId)
                .ThenBy(e => e.CreatedDate)
                .Select(e => new
                {
                    OrderId = e.SourceId,
                    e.Status,
                    e.CreatedDate,
                    e.EmployeeID,
                    e.CompanyId,
                    e.Note
                })
                .ToListAsync(ct);

            // 4) Resolve CreatedByName + CompanyName
            var creatorIds = logs.Select(l => l.EmployeeID).Distinct().ToList();
            var empMap = new Dictionary<Guid, (string FullName, string CompanyName)>();
            if (creatorIds.Count > 0)
            {
                empMap = await _unitOfWork.EmployeesRepository.Query()
                    .Where(e => creatorIds.Contains(e.EmployeeId))
                    .Select(e => new
                    {
                        e.EmployeeId,
                        FullName = e.FullName ?? string.Empty,
                        CompanyName = e.Company != null ? e.Company.Name : string.Empty
                    })
                    .ToDictionaryAsync(
                        x => x.EmployeeId,
                        x => (x.FullName, x.CompanyName),
                        ct);
            }

            // 5) Group logs -> details
            var grouped = logs
                .GroupBy(l => l.OrderId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(l => new GetMerchadiseTimelineDetail
                    {
                        Status = l.Status ?? string.Empty,
                        CreatedDate = l.CreatedDate,
                        CreatedByName = empMap.TryGetValue(l.EmployeeID, out var emp1) ? emp1.FullName : string.Empty,
                        CompanyName = empMap.TryGetValue(l.EmployeeID, out var emp2) ? emp2.CompanyName : string.Empty,
                        Note = l.Note
                    }).ToList()
                );

            // 6) Gán details về từng order
            foreach (var o in pageOrders)
                if (grouped.TryGetValue(o.MerchandiseOrderId, out var details))
                    o.Details = details;

            return OperationResult<PagedResult<GetMerchadiseTimeline>>.Ok(
                new PagedResult<GetMerchadiseTimeline>(
                pageOrders, totalCount, query.PageNumber, query.PageSize)
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

            string? kw = string.IsNullOrWhiteSpace(query.Keyword) ? null : query.Keyword!.Trim();

            // ----------------------------------------------------
            // 1) Lấy các lệnh sản xuất (MfgProductionOrder) gắn với đơn hàng này
            //    => BÂY GIỜ nối qua bảng MfgOrderPO + MerchandiseOrderDetail
            // ----------------------------------------------------
            var mfgQ = _unitOfWork.MfgOrderPORepository.Query(false)
                .Where(link =>
                    link.IsActive &&
                    link.Detail.MerchandiseOrderId == query.id &&          // đơn hàng này
                    link.ProductionOrder.IsActive                           // MFG active
                )
                .Select(link => new
                {
                    link.MfgProductionOrderId,
                    link.ProductionOrder.ExternalId,

                    ColourCode = link.ProductionOrder.ProductExternalIdSnapshot,
                    ProductName = link.ProductionOrder.ProductNameSnapshot,

                    ProductId = link.ProductionOrder.ProductId
                });

            // Nếu vì lý do nào đó 1 MFG gắn với nhiều detail, mình distinct theo MfgProductionOrderId
            var mfgExisting = await mfgQ
                .AsNoTracking()
                .ToListAsync(ct);

            var mfgDistinct = mfgExisting
                .GroupBy(x => x.MfgProductionOrderId)
                .Select(g => g.First())
                .ToList();

            if (mfgDistinct.Count == 0)
            {
                // FIX: Use OperationResult.Ok(data) instead of 4 arguments
                return OperationResult<PagedResult<GetMerchadiseTimelineInforDetail>>.Ok(
                    new PagedResult<GetMerchadiseTimelineInforDetail>(
                        new List<GetMerchadiseTimelineInforDetail>(),
                        0,
                        query.PageNumber,
                        query.PageSize
                    )
                );
            }

            var totalMfg = mfgDistinct.Count;

            // ----------------------------------------------------
            // 2) Lấy các DeliveryOrder liên quan tới đơn hàng này
            // ----------------------------------------------------
            var doIds = await _unitOfWork.DeliveryOrderPORepository.Query()
                .Where(dop => dop.IsActive == true && dop.MerchandiseOrderId == query.id)
                .Select(dop => dop.DeliveryOrderId)
                .Distinct()
                .ToListAsync(ct);

            // Gom DO.ExternalId theo ProductId từ chi tiết DO
            var doByProduct = await _unitOfWork.DeliveryOrderDetailRepository.Query()
                .Where(dod => doIds.Contains(dod.DeliveryOrderId) && dod.IsActive == true)
                .Select(dod => new
                {
                    dod.ProductId,
                    DOExternalId = dod.DeliveryOrder.ExternalId
                })
                .Where(x => x.DOExternalId != null && x.DOExternalId != "")
                .GroupBy(x => new { x.ProductId })
                .Select(g => new
                {
                    g.Key.ProductId,
                    DOList = g.Select(x => x.DOExternalId!).Distinct().ToList()
                })
                .ToListAsync(ct);

            var deliveryListStr = doByProduct.ToDictionary(
                k => k.ProductId,
                v => v.DOList
            );

            // ----------------------------------------------------
            // 3) Lấy MerchandiseOrderDetail của đơn để map Request/Real
            //    Model mới: IsActive là bool, DeliveryRequestDate KHÔNG nullable
            // ----------------------------------------------------
            var merchDetails = await _unitOfWork.MerchandiseOrderRepository.QueryDetail(false)
                .Where(d => d.MerchandiseOrderId == query.id && d.IsActive == true)
                .Select(d => new
                {
                    d.ProductId,
                    RequestQuantity = d.ExpectedQuantity,
                    RealQuantity = d.DeliveryOrderDetails
                                    .Where(dd => dd.IsActive == true)
                                    .Sum(dd => (decimal?)dd.Quantity) ?? 0m,
                    d.DeliveryRequestDate
                })
                .ToDictionaryAsync(x => x.ProductId, x => x, ct);

            // ----------------------------------------------------
            // 4) Phân trang theo MFG
            // ----------------------------------------------------
            var page = Math.Max(1, query.PageNumber);
            var size = Math.Max(1, query.PageSize);

            var pageMfg = mfgDistinct
                .OrderBy(x => x.ExternalId) // hoặc theo CreatedDate log nếu sau này muốn nâng cấp
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            var pageMfgIds = pageMfg.Select(x => x.MfgProductionOrderId).ToList();

            // ----------------------------------------------------
            // 5) Lấy log của các MFG trong trang hiện tại
            // ----------------------------------------------------
            var logsQ = _unitOfWork.EventLogRepository.Query()
                .Where(e => e.IsActive && pageMfgIds.Contains(e.SourceId));

            if (!string.IsNullOrWhiteSpace(query.Status))
                logsQ = logsQ.Where(e => e.Status == query.Status);

            var logRows = await logsQ
                .OrderBy(e => e.SourceId)
                .ThenByDescending(e => e.CreatedDate)
                .Select(e => new
                {
                    e.SourceId,
                    e.Status,
                    e.CreatedDate,
                    e.EmployeeID,
                    e.CompanyId,
                    e.Note
                })
                .ToListAsync(ct);

            // ----------------------------------------------------
            // 6) Resolve người tạo + công ty
            // ----------------------------------------------------
            var creatorIds = logRows.Select(l => l.EmployeeID).Distinct().ToList();
            var empMap = creatorIds.Count == 0
                ? new Dictionary<Guid, (string FullName, string CompanyName)>()
                : await _unitOfWork.EmployeesRepository.Query()
                    .Where(emp => creatorIds.Contains(emp.EmployeeId))
                    .Select(emp => new
                    {
                        emp.EmployeeId,
                        FullName = emp.FullName ?? string.Empty,
                        CompanyName = emp.Company != null ? emp.Company.Name : string.Empty
                    })
                    .ToDictionaryAsync(x => x.EmployeeId, x => (x.FullName, x.CompanyName), ct);

            var detailsByMfg = logRows
                .GroupBy(l => l.SourceId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(l => new GetMerchadiseTimelineDetail
                    {
                        Status = l.Status ?? string.Empty,
                        CreatedDate = l.CreatedDate,
                        CreatedByName = empMap.TryGetValue(l.EmployeeID, out var emp)
                            ? emp.FullName
                            : string.Empty,
                        CompanyName = empMap.TryGetValue(l.EmployeeID, out var emp2)
                            ? emp2.CompanyName
                            : string.Empty,
                        Note = l.Note
                    }).ToList()
                );

            // ----------------------------------------------------
            // 7) Build items cho trang hiện tại
            // ----------------------------------------------------
            var items = pageMfg.Select(m =>
            {
                var doList = deliveryListStr.TryGetValue(m.ProductId, out var list) ? list : new List<string>();
                var hasDetail = merchDetails.TryGetValue(m.ProductId, out var md);

                return new GetMerchadiseTimelineInforDetail
                {
                    ExternalId = m.ExternalId,
                    ColourCode = m.ColourCode,
                    ProductName = m.ProductName,
                    DeliveryList = string.Join(", ", doList),

                    RequestQuantity = hasDetail ? (decimal?)md.RequestQuantity : null,
                    RealQuantity = hasDetail ? (decimal?)md.RealQuantity : null,
                    RequestDate = hasDetail ? md.DeliveryRequestDate : default, // model mới: non-nullable

                    Details = detailsByMfg.TryGetValue(m.MfgProductionOrderId, out var ds)
                        ? ds.OrderBy(d => d.CreatedDate).ToList()
                        : new List<GetMerchadiseTimelineDetail>()
                };
            }).ToList();

            // ----------------------------------------------------
            // 8) Trả về PageResult
            // ----------------------------------------------------
            // FIX: Use OperationResult.Ok(data) instead of 4 arguments
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
