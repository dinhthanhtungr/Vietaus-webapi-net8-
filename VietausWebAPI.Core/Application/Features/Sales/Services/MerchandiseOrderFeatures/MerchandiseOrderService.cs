using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Application.Shared.Models.SaleAndMfgs;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;
using VietausWebAPI.Core.Domain.Enums.Merchadises;
using VietausWebAPI.Core.Repositories_Contracts;
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

        public MerchandiseOrderService(IUnitOfWork unitOfWork
                                    , IExternalIdService idService
                                    , ITimelineService timelineService
                                    , IMapper mapper
                                    , IMfgProductionOrderService mfgProductionOrderService
                                    , ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _externalId = idService;
            _TimelineService = timelineService;
            _mapper = mapper;
            _IMfgProductionOrderService = mfgProductionOrderService;
            _CurrentUser = currentUser;
        }

        /// <summary>
        /// Tạo đơn hàng mới kèm theo chi tiết và tự động tạo lệnh sản xuất nếu đơn hàng được duyệt 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<OperationResult<Guid>> CreateAsync(PostMerchandiseOrder req, CancellationToken ct = default)
        {
            if (req.CreatedBy == Guid.Empty) throw new ArgumentNullException(nameof(req.CreatedBy), "CreatedBy cannot be empty.");

            int affected = 0;
            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                var merchandiseOrder = _mapper.Map<MerchandiseOrder>(req);

                merchandiseOrder.CreatedBy = _CurrentUser.EmployeeId;
                merchandiseOrder.CreateDate = now;
                merchandiseOrder.UpdatedBy = _CurrentUser.EmployeeId;
                merchandiseOrder.UpdatedDate = now;

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
                merchandiseOrder.ExternalId = await _externalId.NextAsync(req.CompanyId.GetValueOrDefault(), "DHG", now, ct: ct);

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

                    var addOrders = new List<MfgProductionOrder>();
                    //var addFormulas = new List<ManufacturingFormula>();
                    //var addMaterials = new List<ManufacturingFormulaMaterial>();
                    //var addSelects = new List<ProductStandardFormula>();
                    var addLinks = new List<MfgOrderPO>();

                    foreach (var detail in orderSlim.Details    )
                    {
                        //var (order, mfgFormula, materials) =
                        //    await _IMfgProductionOrderService.CreateOneMfgBundleAsync(merchandiseOrder, detail, ctx, req.CreatedBy, now, ct);

                        // Tạo đầy đủ: Order + VA + Materials + SelectVersion + Link Order<->Detail
                        //var (order, mfgFormula, materials, select, link) =
                        //    await _IMfgProductionOrderService.CreateOneMfgBundleAsync(
                        //        orderSlim, detail, ctx, req.CreatedBy, DateTime.Now, ct);
                        var (order, link) =
                            await _IMfgProductionOrderService.CreateOneMfgBundleAsync(
                                orderSlim, detail, ctx, req.CreatedBy, DateTime.Now, ct);

                        //addOrders.Add(order);
                        //addFormulas.Add(mfgFormula);
                        //if (materials.Count > 0) addMaterials.AddRange(materials);
                        //addSelects.Add(select);
                        //addLinks.Add(link);

                        addOrders.Add(order);
                        addLinks.Add(link);

                        // Log MFG (tuỳ bạn Add trước rồi log sau)
                        await _TimelineService.AddEventLogAsync(new EventLogModels
                        {
                            employeeId = req.CreatedBy,
                            eventType = EventType.ManufacturingProductOrder,
                            sourceCode = order.ExternalId,
                            sourceId = order.MfgProductionOrderId,
                            status = order.Status,
                            note = $"Created Manufacturing Order {order.ExternalId}"
                        }, ct);
                    }

                    // 4) AddRange đúng thứ tự (FK không phụ thuộc vì đã gán Id ngay trong code)
                    await _unitOfWork.MfgProductionOrderRepository.AddRangeAsync(addOrders, ct);
                    //await _unitOfWork.ManufacturingFormulaRepository.AddRangeAsync(addFormulas, ct);
                    //if (addMaterials.Count > 0)
                    //    await _unitOfWork.ManufacturingFormulaMaterialRepository.AddRangeAsync(addMaterials, ct);

                    // ⚠️ Đảm bảo bạn có các repo dưới; nếu tên khác, đổi cho khớp
                    //await _unitOfWork.ProductStandardFormulaRepository.AddRangeAsync(addSelects, ct);
                    await _unitOfWork.MfgOrderPORepository.AddRangeAsync(addLinks, ct);
                }

                await _TimelineService.AddEventLogAsync(new EventLogModels
                {
                    employeeId = req.CreatedBy,
                    eventType = EventType.MerchadiseStatus,
                    sourceCode = merchandiseOrder.ExternalId ?? string.Empty,
                    sourceId = merchandiseOrder.MerchandiseOrderId,
                    status = merchandiseOrder.Status,
                    note = $"Created Purchase Order {merchandiseOrder.ExternalId}"
                }, ct);

                

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
                .Select(x => new GetOldProductInformation {
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

        /// <summary>
        /// Xóa mềm đơn hàng
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> SoftDelete(PatchMerchandiseOrderInformation query, CancellationToken ct = default)
        {

            throw new ApplicationException("An error occurred while fetching manufacturing formulas.");
            //var now = DateTime.Now;

            //// 1) Chặn từ sớm (ngoài transaction) để giảm thời gian giữ tx
            //var hasLockedDelivery = await _unitOfWork.DeliveryOrderPORepository.Query(false)
            //    .AnyAsync(dop => dop.MerchandiseOrderId == query.MerchandiseOrderId
            //                     && dop.IsActive
            //                     && dop.DeliveryOrder.Status == "Completed", ct);
            //if (hasLockedDelivery)
            //    return OperationResult.Fail("Đơn đã có phiếu giao hoàn tất, không thể xoá mềm.");

            //await using var tx = await _unitOfWork.BeginTransactionAsync();

            //// 2) Lấy đơn cần xoá (tracking)
            //var mo = await _unitOfWork.MerchandiseOrderRepository.Query(track: true)
            //    .FirstOrDefaultAsync(o => o.MerchandiseOrderId == query.MerchandiseOrderId, ct);
            //if (mo == null) return OperationResult.Fail("Không tìm thấy đơn hàng.");
            //if (mo.IsActive == false)
            //{
            //    await tx.CommitAsync(ct);
            //    return OperationResult.Ok("Đơn đã bị vô hiệu hóa trước đó.");
            //}

            //// 3) Lấy mfgMini một lần (để ghi log)
            //var mfgMini = await _unitOfWork.MfgProductionOrderRepository.Query(false)
            //    .Where(m => m.MerchandiseOrderId == query.MerchandiseOrderId && m.IsActive == true)
            //    .Select(m => new { m.MfgProductionOrderId, m.ExternalId })
            //    .ToListAsync(ct);
            //var mfgIds = mfgMini.Select(x => x.MfgProductionOrderId).ToList();

            //// 4) Bulk updates KHÔNG cần Id list ở Details / dùng JOIN ở các bảng con
            //await _unitOfWork.MerchandiseOrderRepository.QueryDetail(true)
            //    .Where(d => d.MerchandiseOrderId == query.MerchandiseOrderId && d.IsActive == true)
            //    .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsActive, false), ct);

            //if (mfgIds.Count > 0)
            //{
            //    await _unitOfWork.MfgProductionOrderRepository.Query(true)
            //        .Where(m => m.MerchandiseOrderId == query.MerchandiseOrderId && m.IsActive == true)
            //        .ExecuteUpdateAsync(s => s
            //            .SetProperty(x => x.IsActive, false)
            //            .SetProperty(x => x.Status, _ => ManufacturingProductOrder.Cancelled.ToString())
            //            .SetProperty(x => x.UpdatedBy, _ => query.UpdatedBy), ct);

            //    await _unitOfWork.ManufacturingFormulaRepository.Query(true)
            //        .Where(f => f.MfgProductionOrder.MerchandiseOrderId == query.MerchandiseOrderId && f.IsActive)
            //        .ExecuteUpdateAsync(s => s
            //            .SetProperty(x => x.IsActive, false)
            //            .SetProperty(x => x.Status, _ => "Cancelled")
            //            .SetProperty(x => x.UpdatedBy, _ => query.UpdatedBy)
            //            .SetProperty(x => x.UpdatedDate, _ => now), ct);

            //    await _unitOfWork.ManufacturingFormulaMaterialRepository.Query(true)
            //        .Where(mm => mm.ManufacturingFormula.MfgProductionOrder.MerchandiseOrderId == query.MerchandiseOrderId && mm.IsActive)
            //        .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsActive, false), ct);
            //}

            //// 5) Cập nhật MerchandiseOrder (tracked entity)
            //mo.IsActive = false;
            //mo.Status = MerchadiseStatus.Cancelled.ToString();
            //mo.UpdatedBy = query.UpdatedBy;
            //mo.UpdatedDate = now;

            //// 6) Logs: AddRange 1 lần
            //var logs = mfgMini.Select(m => new EventLogModels
            //{
            //    employeeId = query.UpdatedBy,
            //    eventType = EventType.ManufacturingProductOrder,
            //    sourceId = m.MfgProductionOrderId,
            //    sourceCode = m.ExternalId ?? string.Empty,
            //    status = ManufacturingProductOrder.Cancelled.ToString(),
            //    note = $"Cascade soft delete from Merchandise {mo.ExternalId}"
            //}).ToList();

            //logs.Add(new EventLogModels
            //{
            //    employeeId = query.UpdatedBy,
            //    eventType = EventType.MerchadiseStatus,
            //    sourceId = mo.MerchandiseOrderId,
            //    sourceCode = mo.ExternalId ?? string.Empty,
            //    status = "SoftDeleted",
            //    note = $"Soft delete Merchandise {mo.ExternalId}, deleted reason: {query.DeletedReason}"
            //});

            //await _TimelineService.AddEventLogRangeAsync(logs, ct); // tạo hàm batch nếu chưa có

            //await _unitOfWork.SaveChangesAsync();
            //await tx.CommitAsync(ct);

            //return OperationResult.Ok($"Đã soft delete đơn {mo.ExternalId} và dữ liệu liên quan.");


        }

        /// <summary>
        /// Cập nhật trang thái duyệt đơn hàng
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateApproveStatus(PatchMerchandiseOrderInformation query, CancellationToken ct = default)
        {

            throw new ApplicationException("An error occurred while fetching manufacturing formulas.");
            //await using var tx = await _unitOfWork.BeginTransactionAsync();

            //try
            //{
            //    var now = DateTime.Now;

            //    // 1) Lấy đơn + details (tracking để cập nhật)
            //    var mo = await _unitOfWork.MerchandiseOrderRepository.Query(track: true)
            //        .Include(o => o.MerchandiseOrderDetails)
            //        .FirstOrDefaultAsync(o => o.MerchandiseOrderId == query.MerchandiseOrderId && o.IsActive == true);

            //    if (mo == null) return OperationResult.Fail("Không tìm thấy đơn hàng.");

            //    // 2) Nếu đã Approved và đã có MFG thì coi như xong
            //    var alreadyApproved = mo.Status == MerchadiseStatus.Approved.ToString();
            //    var hasMfg = await _unitOfWork.MfgProductionOrderRepository.Query(false)
            //        .AnyAsync(m => m.IsActive == true && m.MerchandiseOrderId == query.MerchandiseOrderId);

            //    // 3) Cập nhật trạng thái duyệt
            //    mo.Status = MerchadiseStatus.Approved.ToString();
            //    mo.UpdatedBy = query.UpdatedBy;
            //    mo.UpdatedDate = now;           // nếu có trường ApprovedBy/ApprovedDate thì set ở đây

            //    // 4) Nếu chưa có MFG, tạo ngay bằng context
            //    if (!hasMfg)
            //    {
            //        var ctx = await _IMfgProductionOrderService.BuildMfgContextAsync(mo);

            //        var orders = new List<MfgProductionOrder>();
            //        var formulas = new List<ManufacturingFormula>();
            //        var materials = new List<ManufacturingFormulaMaterial>();

            //        foreach (var d in mo.MerchandiseOrderDetails.Where(x => x.IsActive && (x.Status ?? "") != "Cancelled"))
            //        {
            //            var (order, formula, mats) =
            //                await _IMfgProductionOrderService.CreateOneMfgBundleAsync(mo, d, ctx, query.UpdatedBy, now, ct);

            //            orders.Add(order);
            //            formulas.Add(formula);
            //            materials.AddRange(mats);

            //            // Log cho từng MFG
            //            await _TimelineService.AddEventLogAsync(new EventLogModels
            //            {
            //                employeeId = query.UpdatedBy,
            //                eventType = EventType.ManufacturingProductOrder,
            //                sourceCode = order.ExternalId,
            //                sourceId = order.MfgProductionOrderId,
            //                status = order.Status, // New
            //                note = $"Created Manufacturing Order {order.ExternalId}"
            //            });

            //            if (formula.IsStandard)
            //            {
            //                //await _unitOfWork.ManufacturingFormulaLogRepository.AddAsync(new ManufacturingFormulaLog
            //                //{
            //                //    ManufacturingFormulaId = formula.ManufacturingFormulaId,
            //                //    Action = ManufacturingFormulaLogAction.SetStandard,
            //                //    Comment = "Tự động đặt công thức này là chuẩn vì là công thức đầu tiên của VU.",
            //                //    PerformedDate = now,
            //                //    PerformedBy = query.UpdatedBy,
            //                //    PerformedByNameSnapshot = "Created by system"
            //                //});
            //            }
            //        }

            //        await _unitOfWork.MfgProductionOrderRepository.AddRangeAsync(orders);
            //        await _unitOfWork.ManufacturingFormulaRepository.AddRangeAsync(formulas);
            //        if (materials.Count > 0)
            //            await _unitOfWork.ManufacturingFormulaMaterialRepository.AddRangeAsync(materials);
            //    }

            //    // 5) Timeline cho Merchandise: chỉ log Approved trong flow duyệt
            //    await _TimelineService.AddEventLogAsync(new EventLogModels
            //    {
            //        employeeId = query.UpdatedBy,
            //        eventType = EventType.MerchadiseStatus,
            //        sourceCode = mo.ExternalId,
            //        sourceId = mo.MerchandiseOrderId,
            //        status = MerchadiseStatus.Approved.ToString(),
            //        note = $"Approved Merchandise Order {mo.ExternalId}"
            //    });

            //    await _unitOfWork.SaveChangesAsync();
            //    await tx.CommitAsync();

            //    return OperationResult.Ok(hasMfg
            //        ? "Đơn đã duyệt (MFG đã tồn tại)."
            //        : "Đã duyệt & tạo lệnh sản xuất.");
            //}

            //catch (Exception ex)
            //{
            //    await _unitOfWork.RollbackTransactionAsync();
            //    return OperationResult.Fail(ex.Message);    
            //}
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
