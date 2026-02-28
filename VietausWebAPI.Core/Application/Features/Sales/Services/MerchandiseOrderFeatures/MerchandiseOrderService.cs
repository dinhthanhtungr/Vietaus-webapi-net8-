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

                int totalCount = await result.CountAsync(ct);

                var items = await result
                    .Where(f => f.IsActive == true)
                    .OrderByDescending(c => c.CreateDate) // "F1" -> "F0000000001"
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ProjectTo<GetMerchadiseOrder>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetMerchadiseOrder>(items, totalCount, query.PageNumber, query.PageSize);
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

                    // 1) Set khách không còn là Lead
                    customer.IsLead = false;


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

                        // Gom NOTIFY để bắn SAU COMMIT
                        //postCommitNotifies.Add(new PublishNotificationRequest
                        //{
                        //    Topic = TopicNotifications.ManufacturingOrderCreated,
                        //    Severity = NotificationSeverity.Info,
                        //    Title = $"Lệnh sản xuất: {order.ExternalId}",
                        //    Message = $"Khách hàng {order.CustomerExternalIdSnapshot}",
                        //    Link = $"/plpu/mfgproductionorders/{order.MfgProductionOrderId}",
                        //    PayloadJson = JsonSerializer.Serialize(new
                        //    {
                        //        ProductOrderId = order.MfgProductionOrderId,
                        //        CustomerId = order.CustomerId,
                        //        CreatedBy = order.CreatedBy,
                        //        CreatedDate = order.CreatedDate
                        //    }),
                        //    TargetRoles = new() { RoleSets.PLPU_Group }
                        //});


                        var reqNotify = new PublishNotificationRequest
                        {
                            CompanyId = companyId,
                            CreatedBy = userId,
                            CreatedByNameSnapshot = _CurrentUser.personName,
                            Topic = TopicNotifications.ManufacturingOrderCreated,
                            Severity = NotificationSeverity.Info,
                            Title = $"Lệnh sản xuất: {order.ExternalId}",
                            Message = $"Khách hàng {order.CustomerExternalIdSnapshot}",
                            Link = $"/plpu/mfgproductionorders/{order.MfgProductionOrderId}",
                            PayloadJson = JsonSerializer.Serialize(new
                            {
                                ProductOrderId = order.MfgProductionOrderId,
                                CustomerId = order.CustomerId,
                                CreatedBy = order.CreatedBy,
                                CreatedDate = order.CreatedDate
                            }),
                            TargetRoles = new() { RoleSets.PLPU_Group }
                        };

                        var ob = new OutboxMessage
                        {
                            Type = "Notification.Build",
                            PayloadJson = JsonSerializer.Serialize(reqNotify),
                            CreatedAt = DateTime.Now
                        };

                        outboxBuildBatch.Add(ob);    
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

                // 1) Lấy đơn + details (tracking để cập nhật)
                var mo = await _unitOfWork.MerchandiseOrderRepository.Query(track: true)
                    .Include(o => o.MerchandiseOrderDetails)
                    .FirstOrDefaultAsync(o => o.MerchandiseOrderId == query.MerchandiseOrderId && o.IsActive == true, ct);

                if (mo == null)
                    return OperationResult.Fail("Không tìm thấy đơn hàng.");

                // 2) Kiểm tra đã có MFG (dựa theo link MfgOrderPO trên các MerchandiseOrderDetail)
                var detailIds = mo.MerchandiseOrderDetails
                    .Where(d => d.IsActive == true)
                    .Select(d => d.MerchandiseOrderDetailId)
                    .ToList();

                var hasMfg = detailIds.Count == 0
                    ? false
                    : await _unitOfWork.MfgOrderPORepository.Query(track: false)
                        .AnyAsync(x => x.IsActive == true && detailIds.Contains(x.MerchandiseOrderDetailId), ct);

                // 3) Cập nhật trạng thái duyệt đơn
                mo.Status = MerchadiseStatus.Approved.ToString();
                mo.UpdatedBy = query.UpdatedBy;
                mo.UpdatedDate = now;

                // 4) Nếu chưa có MFG, tạo mới MFG + Link ngay từ context
                var createdOrders = new List<MfgProductionOrder>();
                var createdLinks = new List<MfgOrderPO>();
                var postCommitNotifies = new List<PublishNotificationRequest>();

                if (!hasMfg && detailIds.Count > 0)
                {
                    // Build OrderSlim từ MerchandiseOrder (mo)
                    var orderSlim = new OrderSlim
                    {
                        MerchandiseOrderId = mo.MerchandiseOrderId,
                        ExternalId = mo.ExternalId ?? string.Empty,
                        CompanyId = mo.CompanyId,
                        CustomerId = mo.CustomerId,
                        CustomerExternalIdSnapshot = mo.CustomerExternalIdSnapshot,
                        CustomerNameSnapshot = mo.CustomerNameSnapshot,
                        Details = mo.MerchandiseOrderDetails
                            .Where(d => d.IsActive == true)
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


                    // Build context 1 lần cho toàn đơn
                    var ctx = await _IMfgProductionOrderService.BuildMfgContextAsync(orderSlim, ct);

                    foreach (var detail in orderSlim.Details)
                    {
                        // Tạo đầy đủ MPO/Link từ service (tuần tự)
                        var (order, link) = await _IMfgProductionOrderService
                            .CreateOneMfgBundleAsync(orderSlim, detail, ctx, userId, now, ct);

                        createdOrders.Add(order);
                        createdLinks.Add(link);

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

                        // Gom NOTIFY để bắn SAU COMMIT
                        postCommitNotifies.Add(new PublishNotificationRequest
                        {
                            Topic = TopicNotifications.ManufacturingOrderCreated,
                            Severity = NotificationSeverity.Info,
                            Title = $"Lệnh sản xuất: {order.ExternalId}",
                            Message = $"Khách hàng {order.CustomerExternalIdSnapshot}",
                            Link = $"/plpu/mfgproductionorders/{order.MfgProductionOrderId}",
                            PayloadJson = JsonSerializer.Serialize(new
                            {
                                ProductOrderId = order.MfgProductionOrderId,
                                CustomerId = order.CustomerId,
                                CreatedBy = order.CreatedBy,
                                CreatedDate = order.CreatedDate
                            }),
                            TargetRoles = new() { RoleSets.PLPU_Group }
                        });
                    }

                    // AddRange vào DbSet (chưa SaveChanges)
                    if (createdOrders.Count > 0) await _unitOfWork.MfgProductionOrderRepository.AddRangeAsync(createdOrders, ct);
                    if (createdLinks.Count > 0) await _unitOfWork.MfgOrderPORepository.AddRangeAsync(createdLinks, ct);
                }

                // 5) Timeline cho Merchandise: log Approved
                await _TimelineService.AddEventLogAsync(new EventLogModels
                {
                    employeeId = query.UpdatedBy,
                    eventType = EventType.MerchadiseStatus,
                    sourceCode = mo.ExternalId,
                    sourceId = mo.MerchandiseOrderId,
                    status = MerchadiseStatus.Approved.ToString(),
                    note = $"Approved Merchandise Order {mo.ExternalId}"
                }, ct);

                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);

                var msg = (!hasMfg && createdOrders.Count > 0)
                    ? "Đã duyệt & tạo lệnh sản xuất."
                    : hasMfg
                        ? "Đơn đã duyệt (MFG đã tồn tại)."
                        : "Đơn đã duyệt (không có chi tiết hợp lệ để tạo MFG).";

                return OperationResult.Ok(msg);
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
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                var MerchandiseOrder = await _unitOfWork.MerchandiseOrderRepository.Query(track: true)
                    .FirstOrDefaultAsync(m => m.MerchandiseOrderId == req.MerchandiseOrderId && m.IsActive == true, ct);

                if (MerchandiseOrder == null) return OperationResult.Fail("Không tìm thấy đơn hàng.");

                PatchHelper.SetIfRef(req.Status, () => MerchandiseOrder.Status, v => MerchandiseOrder.Status = v);
                PatchHelper.SetIfRef(req.CustomerNameSnapshot, () => MerchandiseOrder.CustomerNameSnapshot, v => MerchandiseOrder.CustomerNameSnapshot = v);
                PatchHelper.SetIfRef(req.CustomerExternalIdSnapshot, () => MerchandiseOrder.CustomerExternalIdSnapshot, v => MerchandiseOrder.CustomerExternalIdSnapshot = v);
                PatchHelper.SetIfRef(req.PhoneSnapshot, () => MerchandiseOrder.PhoneSnapshot, v => MerchandiseOrder.PhoneSnapshot = v);

                PatchHelper.SetIfRef(req.Receiver, () => MerchandiseOrder.Receiver, v => MerchandiseOrder.Receiver = v);
                PatchHelper.SetIfRef(req.DeliveryAddress, () => MerchandiseOrder.DeliveryAddress, v => MerchandiseOrder.DeliveryAddress = v);

                PatchHelper.SetIf(req.Vat, () => MerchandiseOrder.Vat.GetValueOrDefault(), v => MerchandiseOrder.Vat = v);

                PatchHelper.SetIfRef(req.PaymentType, () => MerchandiseOrder.PaymentType, v => MerchandiseOrder.PaymentType = v);

                PatchHelper.SetIf(req.PaymentDate, () => MerchandiseOrder.PaymentDate.GetValueOrDefault(), v => MerchandiseOrder.PaymentDate = v);
                //PatchHelper.SetIf(req.DeliveryRequestDate, () => MerchandiseOrder.DeliveryRequestDate.GetValueOrDefault(), v => MerchandiseOrder.DeliveryRequestDate = v);
                //PatchHelper.SetIf(req.DeliveryActualDate, () => MerchandiseOrder.DeliveryActualDate.GetValueOrDefault(), v => MerchandiseOrder.DeliveryActualDate = v);
                //PatchHelper.SetIf(req.ExpectedDeliveryDate, () => MerchandiseOrder.ExpectedDeliveryDate.GetValueOrDefault(), v => MerchandiseOrder.ExpectedDeliveryDate = v);

                PatchHelper.SetIfRef(req.Note, () => MerchandiseOrder.Note, v => MerchandiseOrder.Note = v);
                PatchHelper.SetIfRef(req.ShippingMethod, () => MerchandiseOrder.ShippingMethod, v => MerchandiseOrder.ShippingMethod = v);
                PatchHelper.SetIfRef(req.PONo, () => MerchandiseOrder.PONo, v => MerchandiseOrder.PONo = v);

                PatchHelper.SetIf(req.UpdatedDate, () => now, v => now = v);
                PatchHelper.SetIf(req.UpdatedBy, () => MerchandiseOrder.UpdatedBy, v => MerchandiseOrder.UpdatedBy = v);

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
    }
}
