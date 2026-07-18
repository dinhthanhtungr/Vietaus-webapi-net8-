using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Application.Shared.Models.SaleAndMfgs;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.Notifications;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.Category;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;
using VietausWebAPI.Core.Domain.Enums.Merchadises;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;
using static QuestPDF.Helpers.Colors;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.MerchandiseOrderFeatures
{
    public class MerchandiseOrderService : IMerchandiseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMfgProductionOrderService _IMfgProductionOrderService;
        private readonly IExternalIdService _externalId;
        private readonly ITimelineService _TimelineService;
        private readonly ICurrentUser _CurrentUser;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public MerchandiseOrderService(IUnitOfWork unitOfWork
                                     , IExternalIdService idService
                                     , ITimelineService timelineService
                                     , IMapper mapper
                                     , IMfgProductionOrderService mfgProductionOrderService
                                     , ICurrentUser currentUser
            , INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _externalId = idService;
            _TimelineService = timelineService;
            _mapper = mapper;
            _IMfgProductionOrderService = mfgProductionOrderService;
            _CurrentUser = currentUser;
            _notificationService = notificationService;
        }


        // ======================================================================== Get ========================================================================
        /// <summary>
        /// Lấy danh sách đơn hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<GetMerchadiseOrder>> GetAllAsync(MerchandiseOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                var now = DateTime.Now;

                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var result = _unitOfWork.MerchandiseOrderRepository.Query();

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();

                    result = result.Where(x =>
                        (x.CustomerNameSnapshot ?? "").Contains(keyword) ||
                        (x.CustomerExternalIdSnapshot ?? "").Contains(keyword) ||
                        (x.ExternalId ?? "").Contains(keyword) ||
                        x.MerchandiseOrderDetails.Any(d =>
                            // Snapshot: đúng lịch sử
                            (d.ProductExternalIdSnapshot ?? "").Contains(keyword) ||
                            (d.ProductNameSnapshot ?? "").Contains(keyword) ||
                            // Canonical: đúng hiện hành
                            (d.Product != null && (
                                (d.Product.ColourCode ?? "").Contains(keyword) ||
                                (d.Product.Name ?? "").Contains(keyword)
                            ))
                        )
                    );
                }

                if (query.CompanyId.HasValue && query.CompanyId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.CompanyId == query.CompanyId.Value);
                }

                if (query.MerchandiseOrderId.HasValue && query.MerchandiseOrderId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.MerchandiseOrderId == query.MerchandiseOrderId.Value);
                }

                result = result.Where(f => f.IsActive == true);

                int totalCount = await result.CountAsync(ct);

                var items = await result
                    .OrderByDescending(c => c.CreateDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ProjectTo<GetMerchadiseOrder>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);
                foreach (var item in items)
                {
                    var shouldPause =
                        item.Status != MerchadiseStatus.Cancelled.ToString() &&
                        item.IsDeliveryPaused &&
                        (
                            item.DeliveryPausedFrom == null ||
                            item.DeliveryPausedFrom.Value.Date <= now.Date
                        ) &&
                        (
                            item.DeliveryPausedTo == null ||
                            item.DeliveryPausedTo.Value.Date >= now.Date
                        );

                    if (shouldPause)
                    {
                        item.Status = MerchadiseStatus.Paused.ToString();
                    }
                }

                return new PagedResult<GetMerchadiseOrder>(
                    items,
                    totalCount,
                    query.PageNumber,
                    query.PageSize
                );

            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thông tin của cụ thể một đơn hàng
        /// </summary>
        /// <param name="merchandiseOrderId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<GetMerchadiseOrderWithId?> GetByIdAsync(Guid merchandiseOrderId, CancellationToken ct = default)
        {
            try
            {
                return await _unitOfWork.MerchandiseOrderRepository.Query()
                    .Where(m => m.MerchandiseOrderId == merchandiseOrderId && m.IsActive == true)
                    .ProjectTo<GetMerchadiseOrderWithId>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(ct);
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin đơn hàng: {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Lấy thông tin sản phẩm liên quan trong đơn hàng cũ nhất của khách hàng với sản phẩm cụ thể
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<GetOldProductInformation?> GetLastMerchandiseOrderByCustomerIdAsync(Guid customerId, Guid productId, CancellationToken ct = default)
        {
            return await _unitOfWork.MerchandiseOrderRepository.Query()
                .Where(o => o.CustomerId == customerId && (o.IsActive) == true)
                .SelectMany(o => o.MerchandiseOrderDetails, (o, d) => new { o, d })
                .Where(x => x.d.ProductId == productId && x.d.IsActive == true && (x.d.Status == null || x.d.Status != MerchadiseStatus.Cancelled.ToString())) // <-- FIXED: closing parenthesis here
                .OrderByDescending(x => x.o.CreateDate)
                .Select(x => new GetOldProductInformation
                {
                    BagType = x.d.BagType,
                    PackageWeight = x.d.PackageWeight,
                    ExpectedQuantity = (int)x.d.ExpectedQuantity,
                    FormulaExternalIdSnapshot = x.d.FormulaExternalIdSnapshot,
                    Comment = x.d.Comment,
                    UnitPriceAgreed = x.d.UnitPriceAgreed,
                    CreateDate = x.o.CreateDate
                })
                .FirstOrDefaultAsync(ct);
        }

        // ======================================================================== Post ========================================================================

        /// <summary>
        /// Tạo đơn hàng mới kèm theo chi tiết và tự động tạo lệnh sản xuất nếu đơn hàng được duyệt 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<OperationResult<Guid>> CreateAsync(PostMerchandiseOrder req, CancellationToken ct = default)
        {

            int affected = 0;
            await using var tx = await _unitOfWork.BeginTransactionAsync();

            var userId = _CurrentUser.EmployeeId;
            var companyId = _CurrentUser.CompanyId;

            try
            {
                var now = DateTime.Now;
                var merchandiseOrder = _mapper.Map<MerchandiseOrder>(req);


                // 2) Build danh sách MPO + link
                var addOrders = new List<MfgProductionOrder>();
                var addLinks = new List<MfgOrderPO>();
                var outboxBuildBatch = new List<OutboxMessage>();

                merchandiseOrder.CreatedBy = userId;
                merchandiseOrder.CreateDate = now;
                merchandiseOrder.UpdatedBy = userId;
                merchandiseOrder.UpdatedDate = now;
                merchandiseOrder.CompanyId = companyId;

                merchandiseOrder.MerchandiseOrderDetails = (merchandiseOrder.MerchandiseOrderDetails ?? new List<MerchandiseOrderDetail>())
                    .Where(d => d != null
                            && d.FormulaId != Guid.Empty
                            && d.ProductId != Guid.Empty
                            && d.ExpectedQuantity > 0) // Lọc bỏ các chi tiết có Quantity <= 0
                    .ToList();


                if (merchandiseOrder.MerchandiseOrderId == Guid.Empty)
                    merchandiseOrder.MerchandiseOrderId = Guid.CreateVersion7();

                foreach (var detail in merchandiseOrder.MerchandiseOrderDetails)
                {
                    if (detail.MerchandiseOrderDetailId == Guid.Empty)
                        detail.MerchandiseOrderDetailId = Guid.CreateVersion7();         // nếu PK là Guid tự sinh

                    detail.MerchandiseOrderId = merchandiseOrder.MerchandiseOrderId;     // ✅ gán FK rõ ràng
                    detail.MerchandiseOrder = merchandiseOrder;                        // ✅ (khuyến nghị) gán navigation
                    detail.TotalPriceAgreed = Math.Round(detail.UnitPriceAgreed * detail.ExpectedQuantity, 2, MidpointRounding.AwayFromZero);
                }


                merchandiseOrder.TotalPrice = Math.Round(merchandiseOrder.MerchandiseOrderDetails.Sum(d => d.TotalPriceAgreed), 2, MidpointRounding.AwayFromZero);

                    
                // 2) ExternalId DHG (ddMMyy-#####)
                merchandiseOrder.ExternalId = await _externalId.NextAsync(DocumentPrefix.DHG.ToString(), ct: ct);

                // === Tạo bucket đính kèm ngay lúc tạo đơn hàng ===
                if (merchandiseOrder.AttachmentCollectionId == Guid.Empty)
                {
                    var bucket = new AttachmentCollection
                    {
                        AttachmentCollectionId = Guid.CreateVersion7()
                    };

                    // Cách 1: add bucket riêng (repo pattern)
                    await _unitOfWork.AttachmentCollectionRepository.AddAsync(bucket, ct);
                    merchandiseOrder.AttachmentCollectionId = bucket.AttachmentCollectionId;

                    // (Hoặc Cách 2: set navigation nếu có theo dõi)
                    // order.AttachmentCollection = bucket;
                }

                var customer = await _unitOfWork.CustomerRepository.Query(true)
                    .Where(c => c.CustomerId == merchandiseOrder.CustomerId
                                && c.CompanyId == merchandiseOrder.CompanyId)
                    .FirstOrDefaultAsync(ct);

                if (customer != null && customer.IsLead)
                {
                    var groupId = await _unitOfWork.MemberInGroupRepository.Query()
                        .Where(m => m.Profile == merchandiseOrder.CreatedBy && m.IsActive == true)
                        .Select(m => (Guid?)m.GroupId)
                        .FirstOrDefaultAsync(ct) ?? Guid.Empty;


                    if (groupId == Guid.Empty)
                        return OperationResult<Guid>.Fail("Không tìm thấy nhóm của nhân viên tạo đơn, không thể gán khách hàng.");

                    // 1) Set khách không còn là Lead
                    customer.IsLead =false;
                    customer.LeadStatus = LeadStatus.Open;

                    if (customer.CustomerId == Guid.Parse("019bd983-28a1-7231-810a-14c03e090b75"))
                    {
                        customer.IsLead = true;
                    }

                    var hasActiveAssignment = await _unitOfWork.CustomerAssignmentRepository.Query()
                        .AnyAsync(a =>
                            a.CustomerId == merchandiseOrder.CustomerId &&
                            a.CompanyId == merchandiseOrder.CompanyId &&
                            a.IsActive == true,
                            ct);

                    if (!hasActiveAssignment && groupId != Guid.Empty)
                    {
                        await _unitOfWork.CustomerAssignmentRepository.PostCustomerAssignment(
                            new CustomerAssignment
                            {
                                Id = Guid.CreateVersion7(),
                                CustomerId = merchandiseOrder.CustomerId,
                                EmployeeId = merchandiseOrder.CreatedBy,
                                GroupId = groupId,
                                CompanyId = merchandiseOrder.CompanyId,
                                IsActive = true,
                                CreatedDate = now,
                                CreatedBy = merchandiseOrder.CreatedBy,
                                UpdatedDate = now,
                                UpdatedBy = merchandiseOrder.CreatedBy
                            });
                    }


                    // 3) Lấy toàn bộ claim hiện tại
                    var allClaims = await _unitOfWork.CustomerClaimRepository.Query(true)
                        .Where(cl => cl.CustomerId == merchandiseOrder.CustomerId
                                  && cl.CompanyId == merchandiseOrder.CompanyId
                                  && cl.Type == ClaimType.Work
                                  && cl.IsActive)
                        .ToListAsync(ct);


                    // 5) Tắt (soft delete) toàn bộ claim của sale khác
                    foreach (var cl in allClaims.Where(cl => cl.EmployeeId != merchandiseOrder.CreatedBy))
                    {
                        cl.IsActive = false;
                        cl.ExpiresAt = now;
                    }

                    _unitOfWork.CustomerClaimRepository.UpdateRange(allClaims);

                    if (customer.CompanyId == Guid.Parse("019bd983-28a1-7231-810a-14c03e090b75"))
                    {

                    }


                    else
                    {
                        // 6) Tạo log chuyển Lead → Customer
                        var logId = Guid.CreateVersion7();
                        var log = new CustomerTransferLog
                        {
                            Id = logId,
                            FromEmployeeId = merchandiseOrder.CreatedBy,
                            ToEmployeeId = merchandiseOrder.CreatedBy,
                            FromGroupId = groupId,
                            ToGroupId = groupId,
                            TransferType = TransferType.Saled,
                            Note = $"Khách hàng chuyển từ Lead sang Customer khi tạo đơn {merchandiseOrder.ExternalId}",
                            CreatedDate = now,
                            CreatedBy = merchandiseOrder.CreatedBy,
                            CompanyId = merchandiseOrder.CompanyId,
                            DetailCustomerTransfers = new List<DetailCustomerTransfer>
                        {
                            new DetailCustomerTransfer { CustomerId = merchandiseOrder.CustomerId }
                        }
                        };

                        await _unitOfWork.CustomerTransferLogRepository.AddAsync(log, ct);
                    }


                }

                await _unitOfWork.MerchandiseOrderRepository.AddAsync(merchandiseOrder, ct);

                if (merchandiseOrder.Status == MerchadiseStatus.Approved.ToString())
                {
                    var orderSlim = new OrderSlim
                    {
                        MerchandiseOrderId = merchandiseOrder.MerchandiseOrderId,
                        ExternalId = merchandiseOrder.ExternalId ?? string.Empty,
                        CompanyId = merchandiseOrder.CompanyId,
                        CustomerId = merchandiseOrder.CustomerId,
                        CustomerExternalIdSnapshot = merchandiseOrder.CustomerExternalIdSnapshot,
                        CustomerNameSnapshot = merchandiseOrder.CustomerNameSnapshot,
                        Details = (merchandiseOrder.MerchandiseOrderDetails ?? new List<MerchandiseOrderDetail>())
                        .Select(d => new OrderDetailSlim
                        {
                            MerchandiseOrderDetailId = d.MerchandiseOrderDetailId,
                            ProductId = d.ProductId,
                            FormulaId = d.FormulaId,
                            FormulaExternalIdSnapshot = d.FormulaExternalIdSnapshot,
                            ExpectedQuantity = d.ExpectedQuantity,
                            UnitPriceAgreed = d.UnitPriceAgreed,
                            DeliveryRequestDate = d.DeliveryRequestDate,
                            Comment = d.Comment,
                            BagType = d.BagType
                        })
                        .ToList()
                    };

                    var ctx = await _IMfgProductionOrderService.BuildMfgContextAsync(orderSlim, ct);

                    foreach (var detail in orderSlim.Details)
                    {
                        // Tạo đầy đủ MPO/Link từ service (tuần tự)
                        var (order, link) = await _IMfgProductionOrderService
                            .CreateOneMfgBundleAsync(orderSlim, detail, ctx, userId, now, ct);

                        addOrders.Add(order);
                        addLinks.Add(link);

                        // Log sự kiện: chỉ Add vào DbSet (nếu log vào DB) — đừng SaveChanges
                        await _TimelineService.AddEventLogAsync(new EventLogModels
                        {
                            employeeId = userId,
                            eventType = EventType.ManufacturingProductOrder,
                            sourceCode = order.ExternalId,
                            sourceId = order.MfgProductionOrderId,
                            status = order.Status,
                            note = $"Created Manufacturing Order {order.ExternalId}"
                        }, ct);

                    }


                    // 4) AddRange đúng thứ tự (FK không phụ thuộc vì đã gán Id ngay trong code)
                    await _unitOfWork.MfgProductionOrderRepository.AddRangeAsync(addOrders, ct);
                    await _unitOfWork.MfgOrderPORepository.AddRangeAsync(addLinks, ct);
                }

                await _TimelineService.AddEventLogAsync(new EventLogModels
                {
                    employeeId = userId,
                    eventType = EventType.MerchadiseStatus,
                    sourceCode = merchandiseOrder.ExternalId ?? string.Empty,
                    sourceId = merchandiseOrder.MerchandiseOrderId,
                    status = merchandiseOrder.Status,
                    note = $"Created Purchase Order {merchandiseOrder.ExternalId}"
                }, ct);


                if (outboxBuildBatch.Count > 0)
                {
                    await _unitOfWork.OutboxMessages.AddRangeAsync(outboxBuildBatch, ct);
                }

                affected = await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);


                return affected > 0
                    ? OperationResult<Guid>.Ok(merchandiseOrder.AttachmentCollectionId, "Tạo đơn hàng thành công")
                    : OperationResult<Guid>.Fail("Thất bại.");
            }

            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult<Guid>.Fail($"Lỗi khi tạo đơn hàng: {ex.Message}");

            }
        }

        // ======================================================================== Patch ========================================================================

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> CancelMerchadiseOrder(PatchMerchandiseOrderInformation query, CancellationToken ct = default)
        {
            var now = DateTime.Now;

            // 1) Chặn từ sớm: đã có phiếu giao hoàn tất thì không cho xoá
            var hasLockedDelivery = await _unitOfWork.DeliveryOrderPORepository.Query(false)
                .AnyAsync(dop =>
                    dop.MerchandiseOrderId == query.MerchandiseOrderId
                    && dop.IsActive
                    && dop.DeliveryOrder.Status == "Completed", ct);

            if (hasLockedDelivery)
                return OperationResult.Fail("Đơn đã có phiếu giao hoàn tất, không thể xoá mềm.");

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            // 2) Lấy đơn cần xoá (tracking)
            var mo = await _unitOfWork.MerchandiseOrderRepository.Query(track: true)
                .FirstOrDefaultAsync(o => o.MerchandiseOrderId == query.MerchandiseOrderId, ct);

            if (mo == null) return OperationResult.Fail("Không tìm thấy đơn hàng.");

            if (!mo.IsActive)
            {
                await tx.CommitAsync(ct);
                return OperationResult.Ok("Đơn đã bị vô hiệu hóa trước đó.");
            }

            // 3) Lấy danh sách MfgProductionOrder liên quan (để log + update)
            //    Cascade đúng theo model: MfgOrderPO.Detail.MerchandiseOrderId
            var mfgMini = await _unitOfWork.MfgOrderPORepository.Query(false)
                .Where(x => x.IsActive
                            && x.Detail.IsActive
                            && x.Detail.MerchandiseOrderId == query.MerchandiseOrderId)
                .Select(x => new
                {
                    x.MfgProductionOrderId,
                    x.ProductionOrder.ExternalId
                })
                .ToListAsync(ct);

            var mfgIds = mfgMini.Select(x => x.MfgProductionOrderId).Distinct().ToList();

            // Chặn khi lệnh sản xuất đã chạy/đã hoàn tất:
            // var hasLockedMfg = await _unitOfWork.MfgProductionOrderRepository.Query(false)
            //   .AnyAsync(m => mfgIds.Contains(m.MfgProductionOrderId)
            //                 && m.IsActive
            //                 && m.Status != ManufacturingProductOrder.New.ToString()
            //                 && m.Status != ManufacturingProductOrder.Cancelled.ToString(), ct);
            // if (hasLockedMfg) return OperationResult.Fail("Đã có lệnh sản xuất đang chạy/hoàn tất, không thể xoá đơn.");

            // 4) Bulk updates: deactivate Details
            await _unitOfWork.MerchandiseOrderRepository.QueryDetail(true)
                .Where(d => d.MerchandiseOrderId == query.MerchandiseOrderId && d.IsActive)
                .ExecuteUpdateAsync(s => s
                    //.SetProperty(x => x.IsActive, _ => false)
                    .SetProperty(x => x.Status, _ => "Cancelled")
                , ct);


            if(mo.Status == MerchadiseStatus.New.ToString()|| mo.Status == MerchadiseStatus.Approved.ToString())
            {
                // 5) Bulk updates: deactivate link table MfgOrderPO (theo Detail.MerchandiseOrderId)

                await _unitOfWork.MfgOrderPORepository.Query(true)
                    .Where(x => x.IsActive && x.Detail.MerchandiseOrderId == query.MerchandiseOrderId)
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsActive, _ => false), ct);

                // 6) Bulk updates: deactivate MfgProductionOrder theo ids (nếu có)
                if (mfgIds.Count > 0)
                {
                    await _unitOfWork.MfgProductionOrderRepository.Query(true)
                        .Where(m => m.IsActive && mfgIds.Contains(m.MfgProductionOrderId))
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(x => x.IsActive, _ => false)
                            .SetProperty(x => x.Status, _ => ManufacturingProductOrder.Canceled.ToString())
                            .SetProperty(x => x.UpdatedBy, _ => query.UpdatedBy)
                            .SetProperty(x => x.UpdatedDate, _ => now)
                        , ct);
                }

            }

            // 7) Update MerchandiseOrder (tracked entity)
            //mo.IsActive = false;
            mo.Status = MerchadiseStatus.Cancelled.ToString(); // bạn đang dùng enum này
            mo.UpdatedBy = query.UpdatedBy;
            mo.UpdatedDate = now;
            mo.Note = string.IsNullOrWhiteSpace(query.DeletedReason)
                ? mo.Note
                : $"{mo.Note}\n[SoftDelete] {query.DeletedReason}".Trim();

            // 8) Logs: batch 1 lần
            var logs = mfgMini.Select(m => new EventLogModels
            {
                employeeId = query.UpdatedBy,
                eventType = EventType.ManufacturingProductOrder,
                sourceId = m.MfgProductionOrderId,
                sourceCode = m.ExternalId ?? string.Empty,
                status = ManufacturingProductOrder.Canceled.ToString(),
                note = $"Cascade soft delete from Merchandise {mo.ExternalId}"
            }).ToList();

            logs.Add(new EventLogModels
            {
                employeeId = query.UpdatedBy,
                eventType = EventType.MerchadiseStatus,
                sourceId = mo.MerchandiseOrderId,
                sourceCode = mo.ExternalId ?? string.Empty,
                status = MerchadiseStatus.Cancelled.ToString(),
                note = $"Soft delete Merchandise {mo.ExternalId}, reason: {query.DeletedReason}"
            });

            await _TimelineService.AddEventLogRangeAsync(logs, ct);

            await _unitOfWork.SaveChangesAsync();
            await tx.CommitAsync(ct);

            return OperationResult.Ok($"Đã hủy đơn {mo.ExternalId} và {mfgIds.Count} lệnh sản xuất liên quan.");
        }
        /// <summary>
        /// Cập nhật trang thái duyệt đơn hàng
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateApproveStatus(PatchMerchandiseOrderInformation query, CancellationToken ct = default)
        {
            await using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;
                var userId = _CurrentUser.EmployeeId;

                var mo = await _unitOfWork.MerchandiseOrderRepository.Query(track: true)
                    .Include(o => o.MerchandiseOrderDetails)
                    .FirstOrDefaultAsync(o => o.MerchandiseOrderId == query.MerchandiseOrderId && o.IsActive, ct);

                if (mo == null)
                    return OperationResult.Fail("Không tìm thấy đơn hàng.");

                var activeDetails = mo.MerchandiseOrderDetails
                    .Where(d => d.IsActive)
                    .ToList();

                if (activeDetails.Count == 0)
                    return OperationResult.Fail("Đơn hàng không có chi tiết hợp lệ để duyệt.");

                var detailIds = activeDetails
                    .Select(d => d.MerchandiseOrderDetailId)
                    .ToList();

                // Nếu nghiệp vụ yêu cầu: đã có MFG thì không cho approve lại
                var existingLinkedDetailIds = await _unitOfWork.MfgOrderPORepository.Query(track: false)
                    .Where(x => x.IsActive && detailIds.Contains(x.MerchandiseOrderDetailId))
                    .Select(x => x.MerchandiseOrderDetailId)
                    .ToListAsync(ct);

                if (existingLinkedDetailIds.Count > 0)
                {
                    return OperationResult.Fail("Một hoặc nhiều chi tiết đã có lệnh sản xuất, không thể duyệt lại.");
                }

                var orderSlim = new OrderSlim
                {
                    MerchandiseOrderId = mo.MerchandiseOrderId,
                    ExternalId = mo.ExternalId ?? string.Empty,
                    CompanyId = mo.CompanyId,
                    CustomerId = mo.CustomerId,
                    CustomerExternalIdSnapshot = mo.CustomerExternalIdSnapshot,
                    CustomerNameSnapshot = mo.CustomerNameSnapshot,
                    Details = activeDetails
                        .Select(d => new OrderDetailSlim
                        {
                            MerchandiseOrderDetailId = d.MerchandiseOrderDetailId,
                            ProductId = d.ProductId,
                            FormulaId = d.FormulaId,
                            FormulaExternalIdSnapshot = d.FormulaExternalIdSnapshot,
                            ExpectedQuantity = d.ExpectedQuantity,
                            UnitPriceAgreed = d.UnitPriceAgreed,
                            DeliveryRequestDate = d.DeliveryRequestDate,
                            Comment = d.Comment,
                            BagType = d.BagType
                        })
                        .ToList()
                };

                var ctx = await _IMfgProductionOrderService.BuildMfgContextAsync(orderSlim, ct);

                var createdOrders = new List<MfgProductionOrder>();
                var createdLinks = new List<MfgOrderPO>();

                foreach (var detail in orderSlim.Details)
                {
                    var (order, link) = await _IMfgProductionOrderService
                        .CreateOneMfgBundleAsync(orderSlim, detail, ctx, userId, now, ct);

                    if (order == null || link == null)
                        return OperationResult.Fail($"Không thể tạo MFG cho detail {detail.MerchandiseOrderDetailId}");

                    createdOrders.Add(order);
                    createdLinks.Add(link);

                    await _TimelineService.AddEventLogAsync(new EventLogModels
                    {
                        employeeId = userId,
                        eventType = EventType.ManufacturingProductOrder,
                        sourceCode = order.ExternalId,
                        sourceId = order.MfgProductionOrderId,
                        status = order.Status,
                        note = $"Created Manufacturing Order {order.ExternalId}"
                    }, ct);
                }

                // Validate cứng trước khi approve
                if (createdOrders.Count != orderSlim.Details.Count)
                    return OperationResult.Fail($"Tạo MFG không đầy đủ. Cần {orderSlim.Details.Count}, thực tế {createdOrders.Count}.");

                if (createdLinks.Count != orderSlim.Details.Count)
                    return OperationResult.Fail($"Tạo liên kết MFG không đầy đủ. Cần {orderSlim.Details.Count}, thực tế {createdLinks.Count}.");

                await _unitOfWork.MfgProductionOrderRepository.AddRangeAsync(createdOrders, ct);
                await _unitOfWork.MfgOrderPORepository.AddRangeAsync(createdLinks, ct);

                await _TimelineService.AddEventLogAsync(new EventLogModels
                {
                    employeeId = query.UpdatedBy,
                    eventType = EventType.MerchadiseStatus,
                    sourceCode = mo.ExternalId,
                    sourceId = mo.MerchandiseOrderId,
                    status = MerchadiseStatus.Approved.ToString(),
                    note = $"Approved Merchandise Order {mo.ExternalId}"
                }, ct);

                // Chỉ đổi status sau khi mọi thứ phía trên đã OK
                mo.Status = MerchadiseStatus.Approved.ToString();
                mo.UpdatedBy = query.UpdatedBy;
                mo.UpdatedDate = now;

                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);

                return OperationResult.Ok("Đã duyệt & tạo lệnh sản xuất.");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult.Fail($"Lỗi khi duyệt đơn hàng: {ex.Message}");
            }
        }
        /// <summary>
        /// Cập nhật thông tin đơn hàng mới
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateInformationAsync(PatchMerchandiseOrderInformation req, CancellationToken ct = default)
        {
            try
            {
                var current = await _unitOfWork.MerchandiseOrderRepository.Query(track: false)
                    .FirstOrDefaultAsync(m => m.MerchandiseOrderId == req.MerchandiseOrderId && m.IsActive, ct);

                if (current == null)
                    return OperationResult.Fail("Không tìm thấy đơn hàng.");


                if (req.Status == MerchadiseStatus.Approved.ToString() && current.Status != MerchadiseStatus.Approved.ToString())
                {
                    return await UpdateApproveStatus(req, ct);
                }

                await _unitOfWork.BeginTransactionAsync();

                var now = DateTime.Now;

                var merchandiseOrder = await _unitOfWork.MerchandiseOrderRepository.Query(track: true)
                    .FirstOrDefaultAsync(m => m.MerchandiseOrderId == req.MerchandiseOrderId && m.IsActive, ct);

                if (merchandiseOrder == null)
                    return OperationResult.Fail("Không tìm thấy đơn hàng.");

                PatchHelper.SetIfRef(req.Status, () => merchandiseOrder.Status, v => merchandiseOrder.Status = v);
                PatchHelper.SetIfRef(req.CustomerNameSnapshot, () => merchandiseOrder.CustomerNameSnapshot, v => merchandiseOrder.CustomerNameSnapshot = v);
                PatchHelper.SetIfRef(req.CustomerExternalIdSnapshot, () => merchandiseOrder.CustomerExternalIdSnapshot, v => merchandiseOrder.CustomerExternalIdSnapshot = v);
                PatchHelper.SetIfRef(req.PhoneSnapshot, () => merchandiseOrder.PhoneSnapshot, v => merchandiseOrder.PhoneSnapshot = v);
                PatchHelper.SetIfRef(req.Receiver, () => merchandiseOrder.Receiver, v => merchandiseOrder.Receiver = v);
                PatchHelper.SetIfRef(req.DeliveryAddress, () => merchandiseOrder.DeliveryAddress, v => merchandiseOrder.DeliveryAddress = v);
                PatchHelper.SetIf(req.Vat, () => merchandiseOrder.Vat.GetValueOrDefault(), v => merchandiseOrder.Vat = v);
                PatchHelper.SetIfRef(req.PaymentType, () => merchandiseOrder.PaymentType, v => merchandiseOrder.PaymentType = v);
                PatchHelper.SetIf(req.PaymentDate, () => merchandiseOrder.PaymentDate.GetValueOrDefault(), v => merchandiseOrder.PaymentDate = v);
                PatchHelper.SetIfRef(req.Note, () => merchandiseOrder.Note, v => merchandiseOrder.Note = v);
                PatchHelper.SetIfRef(req.ShippingMethod, () => merchandiseOrder.ShippingMethod, v => merchandiseOrder.ShippingMethod = v);
                PatchHelper.SetIfRef(req.PONo, () => merchandiseOrder.PONo, v => merchandiseOrder.PONo = v);

                merchandiseOrder.UpdatedDate = req.UpdatedDate ?? now;
                merchandiseOrder.UpdatedBy = req.UpdatedBy;

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return OperationResult.Ok("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<PatchPauseDeliveryOrder>> UpdatePauseDeliveryStatus(PatchPauseDeliveryOrder query, CancellationToken ct = default)
        {
            try
            {
                var userId = _CurrentUser.EmployeeId;
                var now = DateTime.Now;

                var current = _unitOfWork.MerchandiseOrderRepository.Query(track: true)
                    .FirstOrDefault(m => m.MerchandiseOrderId == query.MerchandiseOrderId && m.IsActive);

                if (current == null)
                {
                    return OperationResult<PatchPauseDeliveryOrder>.Fail("Không tìm thấy đơn hàng.");
                }

                await _unitOfWork.BeginTransactionAsync();

                PatchHelper.SetIf(query.IsDeliveryPaused, () => current.IsDeliveryPaused, v => current.IsDeliveryPaused = v);
                PatchHelper.SetIfNullable(query.DeliveryPausedFrom, () => current.DeliveryPausedFrom, v => current.DeliveryPausedFrom = v);
                PatchHelper.SetIfNullable(query.DeliveryPausedTo, () => current.DeliveryPausedTo, v => current.DeliveryPausedTo = v);
                PatchHelper.SetIfRef(query.DeliveryPauseReason, () => current.DeliveryPauseReason, v => current.DeliveryPauseReason = v);
                PatchHelper.SetIfRef(query.DeliveryPauseType, () => current.DeliveryPauseType, v => current.DeliveryPauseType = v);
               
                
                PatchHelper.SetIfNullable(userId, () => current.DeliveryPausedBy, v => current.DeliveryPausedBy = v);
                
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                if (IsAccountingUser() && current.ManagerById != Guid.Empty && current.ManagerById != userId)
                {
                    await NotifySaleAboutPausedDeliveryAsync(current, now, ct);
                }

                var response = new PatchPauseDeliveryOrder
                {
                    MerchandiseOrderId = current.MerchandiseOrderId,
                    IsDeliveryPaused = current.IsDeliveryPaused,
                    DeliveryPausedFrom = current.DeliveryPausedFrom,
                    DeliveryPausedTo = current.DeliveryPausedTo,
                    DeliveryPauseReason = current.DeliveryPauseReason,
                    DeliveryPauseType = current.DeliveryPauseType
                };

                return OperationResult<PatchPauseDeliveryOrder>.Ok(
                    response,
                    "Cập nhật trạng thái tạm dừng giao hàng thành công"
                );
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult<PatchPauseDeliveryOrder>.Fail($"Lỗi khi cập nhật trạng thái tạm dừng giao hàng: {ex.Message}");
            }
        }

        private bool IsAccountingUser()
        {
            return _CurrentUser.IsInRole(AppRoles.ACUser)
                || _CurrentUser.IsInRole(AppRoles.President)
                || _CurrentUser.IsInRole(AppRoles.Admin)
                || _CurrentUser.IsInRole(AppRoles.Purchaser);
        }

        private async Task NotifySaleAboutPausedDeliveryAsync(
            MerchandiseOrder merchandiseOrder,
            DateTime now,
            CancellationToken ct)
        {
            try
            {
                var pauseStatus = merchandiseOrder.IsDeliveryPaused ? "Tạm dừng giao hàng" : "Mở lại giao hàng";
                var pauseRange = BuildDeliveryPauseRangeText(merchandiseOrder);
                var leaderIds = await GetResponsibleTeamManagerIdsAsync(merchandiseOrder, ct);

                var targetUserIds = leaderIds
                    .Append(merchandiseOrder.ManagerById)
                    .Where(x => x != Guid.Empty)
                    .Distinct()
                    .ToList();

                await _notificationService.PublishAsync(new PublishNotificationRequest
                {
                    CompanyId = merchandiseOrder.CompanyId,
                    CreatedBy = _CurrentUser.EmployeeId,
                    CreatedByNameSnapshot = _CurrentUser.personName,
                    Topic = TopicNotifications.MerchandiseOrderUpdated,
                    Severity = merchandiseOrder.IsDeliveryPaused
                        ? NotificationSeverity.Error
                        : NotificationSeverity.Info,
                    Title = $"{pauseStatus} {merchandiseOrder.ExternalId}",
                    Message = $"{_CurrentUser.personName} đã cập nhật {pauseStatus} cho đơn hàng {merchandiseOrder.ExternalId}{pauseRange}.",
                    Link = $"/sales/merchandise-orders?q={merchandiseOrder.ExternalId}",
                    PayloadJson = JsonSerializer.Serialize(new
                    {
                        merchandiseOrderId = merchandiseOrder.MerchandiseOrderId,
                        merchandiseOrderCode = merchandiseOrder.ExternalId,
                        customerId = merchandiseOrder.CustomerId,
                        customerCode = merchandiseOrder.CustomerExternalIdSnapshot,
                        customerName = merchandiseOrder.CustomerNameSnapshot,
                        isDeliveryPaused = merchandiseOrder.IsDeliveryPaused,
                        deliveryPausedFrom = merchandiseOrder.DeliveryPausedFrom,
                        deliveryPausedTo = merchandiseOrder.DeliveryPausedTo,
                        deliveryPauseReason = merchandiseOrder.DeliveryPauseReason,
                        deliveryPauseType = merchandiseOrder.DeliveryPauseType,
                        updatedAt = now,
                        updatedBy = _CurrentUser.EmployeeId
                    }),
                    TargetUserIds = targetUserIds,
                    TargetRoles = new List<string>
                    {
                        AppRoles.President,
                        AppRoles.ACUser,
                        AppRoles.DispatchUser
                    }
                }, ct);
            }
            catch
            {
            }
        }

        private static string BuildDeliveryPauseRangeText(MerchandiseOrder merchandiseOrder)
        {
            if (!merchandiseOrder.DeliveryPausedFrom.HasValue && !merchandiseOrder.DeliveryPausedTo.HasValue)
                return string.Empty;

            if (merchandiseOrder.DeliveryPausedFrom.HasValue && merchandiseOrder.DeliveryPausedTo.HasValue)
                return $" từ {merchandiseOrder.DeliveryPausedFrom.Value:dd/MM/yyyy} đến {merchandiseOrder.DeliveryPausedTo.Value:dd/MM/yyyy}";

            if (merchandiseOrder.DeliveryPausedFrom.HasValue)
                return $" từ {merchandiseOrder.DeliveryPausedFrom.Value:dd/MM/yyyy}";

            return $" đến {merchandiseOrder.DeliveryPausedTo!.Value:dd/MM/yyyy}";
        }

        /// <summary>
        /// Gets the leader and SaleAdmin employee IDs belonging to the team responsible
        /// for the merchandise order's customer.
        /// </summary>
        private async Task<List<Guid>> GetResponsibleTeamManagerIdsAsync(
            MerchandiseOrder merchandiseOrder,
            CancellationToken ct)
        {
            var groupIds = await _unitOfWork.CustomerAssignmentRepository
                .Query(track: false)
                .Where(x =>
                    x.IsActive &&
                    x.CompanyId == merchandiseOrder.CompanyId &&
                    x.CustomerId == merchandiseOrder.CustomerId &&
                    x.EmployeeId == merchandiseOrder.ManagerById)
                .Select(x => x.GroupId)
                .Distinct()
                .ToListAsync(ct);

            if (groupIds.Count == 0)
                return new List<Guid>();

            //var teamMemberIds = await _unitOfWork.MemberInGroupRepository
            //    .Query()
            //    .Where(x =>
            //        x.IsActive &&
            //        x.Profile.HasValue &&
            //        groupIds.Contains(x.GroupId))
            //    .Select(x => x.Profile!.Value)
            //    .Distinct()
            //    .ToListAsync(ct);

            var leaderIds = await _unitOfWork.MemberInGroupRepository
                .Query()
                .Where(x =>
                    x.IsActive &&
                    x.IsAdmin == true &&
                    x.Profile.HasValue &&
                    groupIds.Contains(x.GroupId))
                .Select(x => x.Profile!.Value)
                .Distinct()
                .ToListAsync(ct);

            //var saleAdminIds = await (
            //    from role in _unitOfWork.ApplicationRoleRepository.Query(track: false)
            //    join userRole in _unitOfWork.ApplicationUserRoleRepository.Query(track: false)
            //        on role.Id equals userRole.RoleId
            //    join user in _unitOfWork.ApplicationUserRepository.Query(track: false)
            //        on userRole.UserId equals user.Id
            //    where role.NormalizedName == AppRoles.SaleAdmin.ToUpper()
            //          && userRole.IsActive
            //          && user.EmployeeId.HasValue
            //          && teamMemberIds.Contains(user.EmployeeId.Value)
            //    select user.EmployeeId!.Value
            //)
            //.Distinct()
            //.ToListAsync(ct);

            return leaderIds
                //.Concat(saleAdminIds)
                .Distinct()
                .ToList();
        }
    }
}
