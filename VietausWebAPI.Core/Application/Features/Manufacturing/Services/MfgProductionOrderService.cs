using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Services.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.Services;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Application.Shared.Models.SaleAndMfgs;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;
using VietausWebAPI.Core.Domain.Enums.Merchadises;
using VietausWebAPI.Core.Repositories_Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static QuestPDF.Helpers.Colors;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public class MfgProductionOrderService : IMfgProductionOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IExternalIdService _externalId;
        private readonly ITimelineService _timeLineService;

        public MfgProductionOrderService(IUnitOfWork unitOfWork, IMapper mapper, IExternalIdService externalId, ICurrentUser currentUser, ITimelineService timeLineService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _externalId = externalId;
            _currentUser = currentUser;
            _timeLineService = timeLineService;
        }

        // ======================================================================== Get ======================================================================== 

        /// <summary>
        /// Lấy danh sách đơn hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<PagedResult<GetSummaryMfgProductionOrder>>> GetAllAsync(MfgProductionOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var result = _unitOfWork.MfgProductionOrderRepository.Query();

                // Link sang MerchandiseOrder qua MfgOrderPO -> MerchandiseOrderDetail -> MerchandiseOrder
                var links = _unitOfWork.MfgOrderPORepository.Query()
                    .Where(l => l.IsActive == true);

                var dets = _unitOfWork.MerchandiseOrderRepository.QueryDetail()
                    .Where(d => d.IsActive == true);
                var mos = _unitOfWork.MerchandiseOrderRepository.Query();

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim().ToLower();
                    result = result.Where(po =>
                        po.ExternalId.ToLower().Contains(keyword)
                    );

                }

                if (query.CompanyId.HasValue && query.CompanyId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.CompanyId == query.CompanyId.Value);
                }

                if (query.MfgProductionOrderId.HasValue && query.MfgProductionOrderId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.MfgProductionOrderId == query.MfgProductionOrderId.Value);
                }

                if (query.Statuses is { Count: > 0 })
                {
                    result = result.Where(p => query.Statuses.Contains(p.Status));
                }

                if (query.From.HasValue)
                {
                    result = result.Where(p => p.CreatedDate >= query.From.Value);
                }

                if (query.To.HasValue)
                {
                    result = result.Where(p => p.CreatedDate <= query.To.Value);
                }

                var vm = await result
                    .Select(o => new GetSummaryMfgProductionOrder
                    {
                        MfgProductionOrderId = o.MfgProductionOrderId,
                        ExternalId = o.ExternalId,

                        MerchandiseOrderId = (
                            from l in links
                            join d in dets
                                    on l.MerchandiseOrderDetailId equals d.MerchandiseOrderDetailId
                            join mo in mos
                                    on d.MerchandiseOrderId equals mo.MerchandiseOrderId
                            where l.IsActive && l.MfgProductionOrderId == o.MfgProductionOrderId
                            orderby mo.MerchandiseOrderId descending
                            select mo.MerchandiseOrderId
                        ).FirstOrDefault(),

                        MerchandiseOrderExternalId = (
                            from l in links
                            join d in dets
                                    on l.MerchandiseOrderDetailId equals d.MerchandiseOrderDetailId
                            join mo in mos
                                    on d.MerchandiseOrderId equals mo.MerchandiseOrderId
                            where l.IsActive && l.MfgProductionOrderId == o.MfgProductionOrderId
                            orderby mo.MerchandiseOrderId descending
                            select mo.ExternalId
                        ).FirstOrDefault(),

                        ProductNameSnapshot = o.ProductNameSnapshot,
                        ProductExternalIdSnapshot = o.ProductExternalIdSnapshot,
                        CustomerExternalIdSnapshot = o.CustomerExternalIdSnapshot,
                        CustomerNameSnapshot = o.CustomerNameSnapshot,

                        TotalQuantity = o.TotalQuantity,
                        Status = o.Status,
                        CreatedDate = o.CreatedDate,
                        BagType = o.BagType,
                    }).ToListAsync();

                int totalCount = await result.CountAsync(ct);

                var items = vm.Skip((query.PageNumber - 1) * query.PageSize)
                            .Take(query.PageSize)
                            .ToList();

                return OperationResult<PagedResult<GetSummaryMfgProductionOrder>>.Ok(
                    new PagedResult<GetSummaryMfgProductionOrder>(
                        items,
                        items.Count,
                        query.PageNumber,
                        query.PageSize
                    )
                );
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách công thức theo lệnh sản xuất với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<PagedResult<GetSampleMfgFormula>> GetAllMfgFormulaAsync(MfgProductionOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                var now = DateTime.Now;
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var q = _unitOfWork.ManufacturingFormulaRepository.Query();

                if (query.MfgFormulaId.HasValue && query.MfgFormulaId.Value != Guid.Empty)
                {
                    q = q.Where(f => f.ManufacturingFormulaId == query.MfgFormulaId.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    var productionSelectVersions = _unitOfWork.ProductionSelectVersionRepository.Query();
                    var mfgProductionOrders = _unitOfWork.MfgProductionOrderRepository.Query();

                    q =
                        from f in q
                        where
                            (from s in productionSelectVersions
                             join o in mfgProductionOrders
                                 on s.MfgProductionOrderId equals o.MfgProductionOrderId
                             where s.ManufacturingFormulaId == f.ManufacturingFormulaId
                                   && (
                                        (o.Product.ColourCode ?? "").Contains(keyword) ||
                                        (o.Product.Name ?? "").Contains(keyword)
                                      )
                             select o
                            ).Any()
                        select f;
                }


                var totalCount = await q.CountAsync(ct);

                // === Subquery: chuẩn theo Product (bản hiệu lực tại 'now') ===
                var validPSF = _unitOfWork.ProductStandardFormulaRepository.Query()
                    .Where(psf => psf.ValidFrom <= now
                               && (psf.ValidTo == null || psf.ValidTo >= now));

                var maxDatePerProduct = validPSF
                    .GroupBy(psf => psf.ProductId)
                    .Select(g => new { ProductId = g.Key, MaxValidFrom = g.Max(x => x.ValidFrom) });

                var bestPSF =
                    from psf in validPSF
                    join md in maxDatePerProduct
                      on new { psf.ProductId, psf.ValidFrom }
                      equals new { md.ProductId, ValidFrom = md.MaxValidFrom }
                    select new { psf.ProductId, psf.ManufacturingFormulaId };

                // === Left-join 1 PSV gần nhất của mỗi formula để lấy Product (nếu có) ===
                // sels & orders đã có bên trên:
                var sels = _unitOfWork.ProductionSelectVersionRepository.Query();
                var orders = _unitOfWork.MfgProductionOrderRepository.Query();

                // query chính
                var itemsQuery =
                    from f in q

                        // 1) Lấy 1 bản PSV gần nhất theo ValidFrom cho từng formula (left-join)
                    join s in sels on f.ManufacturingFormulaId equals s.ManufacturingFormulaId into gsel
                    from sel in gsel
                        .OrderByDescending(s => s.ValidFrom)
                        .ThenByDescending(s => s.ProductionSelectVersionId)
                        .Take(1)
                        .DefaultIfEmpty()

                        // 2) Left-join sang MfgProductionOrder
                    join o0 in orders on sel.MfgProductionOrderId equals o0.MfgProductionOrderId into go
                    from o in go.DefaultIfEmpty()

                        // 3) Dựng anonymous để tiếp tục xử lý
                    select new
                    {
                        f,
                        sel,
                        o,
                        productId = (Guid?)(o != null ? o.ProductId : (Guid?)null),

                        // chuẩn khi bestPSF trùng công thức hiện tại
                        isStandard =
                            bestPSF.Any(psf =>
                                psf.ProductId == o.ProductId &&
                                psf.ManufacturingFormulaId == f.ManufacturingFormulaId
                            ),

                        // đang được chọn hiện hành nếu tồn tại PSV.ValidTo == null cho công thức này
                        isSelectedNow =
                            sels.Any(s =>
                                s.ManufacturingFormulaId == f.ManufacturingFormulaId &&
                                s.ValidTo == null
                            )
                    };

                // 4) Order + map sang DTO
                var items =
                    itemsQuery
                        .OrderByDescending(z => z.isSelectedNow)
                        .ThenByDescending(z => z.isStandard)
                        .ThenBy(z => z.f.CreatedDate)
                        .Select(z => new GetSampleMfgFormula
                        {
                            ManufacturingFormulaId = z.f.ManufacturingFormulaId,
                            MfgProductionOrderExternalId = z.o != null ? z.o.ExternalId : null,
                            ExternalId = z.f.ExternalId,

                            Status = z.f.Status,
                            isStandard = z.isStandard,
                            IsSelect = z.isSelectedNow,
                            CreatedDate = z.f.CreatedDate
                        });

                var pagedItems = await items
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync(ct);  // lỗi sẽ dừng ngay đây

                return new PagedResult<GetSampleMfgFormula>(pagedItems, totalCount, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Lấy thông tin của cụ thể một lệnh sản xuất 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<GetMfgProductionOrder>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return OperationResult<GetMfgProductionOrder>.Fail("Id không hợp lệ.");

            try
            {
                // Base queies
                var orders = _unitOfWork.MfgProductionOrderRepository.Query()
                .Where(p => p.IsActive == true && p.MfgProductionOrderId == id);

                var isSelect = _unitOfWork.ProductionSelectVersionRepository.Query()
                    .Where(s => s.ValidTo == null);

                var manFormula = _unitOfWork.ManufacturingFormulaRepository.Query()
                    .Where(m => m.IsActive == true);

                var links = _unitOfWork.MfgOrderPORepository.Query()
                    .Where(l => l.IsActive == true);

                var dets = _unitOfWork.MerchandiseOrderRepository.QueryDetail()
                    .Where(d => d.IsActive == true);
                var mos = _unitOfWork.MerchandiseOrderRepository.Query();

                // Chọn một công thức đã được cho phép sản xuất nếu có trong bảng ProductionSelectVersionRepository
                // ✅ THAY BẰNG projection với correlated subqueries (EF dịch tốt)
                var vm = await orders
                    .Select(o => new GetMfgProductionOrder
                    {
                        MfgProductionOrderId = o.MfgProductionOrderId,
                        ExternalId = o.ExternalId,

                        // Link MfgOrderPO -> MODetail -> MerchandiseOrder (chọn 1 bản ghi gần nhất)
                        MerchandiseOrderId = (
                            from l in links
                            join d in dets
                                    on l.MerchandiseOrderDetailId equals d.MerchandiseOrderDetailId
                            join mo in mos
                                    on d.MerchandiseOrderId equals mo.MerchandiseOrderId
                            where l.IsActive && l.MfgProductionOrderId == o.MfgProductionOrderId
                            orderby mo.MerchandiseOrderId descending
                            select mo.MerchandiseOrderId
                        ).FirstOrDefault(),

                        MerchandiseOrderExternalId = (
                            from l in links
                            join d in dets
                                    on l.MerchandiseOrderDetailId equals d.MerchandiseOrderDetailId
                            join mo in mos
                                    on d.MerchandiseOrderId equals mo.MerchandiseOrderId
                            where l.IsActive && l.MfgProductionOrderId == o.MfgProductionOrderId
                            orderby mo.MerchandiseOrderId descending
                            select mo.ExternalId
                        ).FirstOrDefault(),

                        // PSV hiện hành -> ManufacturingFormulaId
                        ManufacturingFormulaIdIsSelect = (
                            from s in isSelect
                            where s.MfgProductionOrderId == o.MfgProductionOrderId && s.ValidTo == null
                            orderby s.ValidFrom descending, s.ProductionSelectVersionId descending
                            select (Guid?)s.ManufacturingFormulaId
                        ).FirstOrDefault(),

                        // PSV hiện hành -> ManufacturingFormula.ExternalId
                        ManufacturingFormulaExternalIdIsSelect = (
                            from s in isSelect
                            join f in manFormula
                                 on s.ManufacturingFormulaId equals f.ManufacturingFormulaId
                            where s.MfgProductionOrderId == o.MfgProductionOrderId && s.ValidTo == null
                            orderby s.ValidFrom descending, s.ProductionSelectVersionId descending
                            select (string?)f.ExternalId
                        ).FirstOrDefault(),

                        CustomerNameSnapshot = o.CustomerNameSnapshot,
                        CustomerExternalIdSnapshot = o.CustomerExternalIdSnapshot,

                        ProductId = o.ProductId,
                        ProductExternalIdSnapshot = o.ProductExternalIdSnapshot,
                        ProductNameSnapshot = o.ProductNameSnapshot,

                        // VU khách chọn (snapshot trên Order)
                        FormulaCustomerSelect = o.FormulaId ?? Guid.Empty,
                        FormulaCustomerExternalIdSelect = o.FormulaExternalIdSnapshot ?? string.Empty,

                        ManufacturingDate = o.ManufacturingDate,
                        ExpectedDate = o.ExpectedDate,
                        RequiredDate = o.RequiredDate,

                        TotalQuantityRequest = o.TotalQuantityRequest,
                        TotalQuantity = o.TotalQuantity,
                        NumOfBatches = o.NumOfBatches,
                        UnitPriceAgreed = o.UnitPriceAgreed ,

                        Status = o.Status,
                        LabNote = o.LabNote,
                        Requirement = o.Requirement,
                        PlpuNote = o.PlpuNote,
                        BagType = o.BagType,
                        QcCheck = o.QcCheck
                    })
                    .FirstOrDefaultAsync(ct);

                if (vm == null)
                    return OperationResult<GetMfgProductionOrder>.Fail("Không tìm thấy lệnh sản xuất hoặc đã bị vô hiệu hóa.");

                // (Tùy chọn) cảnh báo dữ liệu lỗi: có >1 PSV current
                var currentPsvCount = await _unitOfWork.ProductionSelectVersionRepository.Query()
                    .Where(s => s.MfgProductionOrderId == id && s.ValidTo == null)
                    .CountAsync(ct);

                if (currentPsvCount > 1)
                {
                    // chỉ log cảnh báo, vẫn trả data (đã chọn bản gần nhất theo ValidFrom)
                    return OperationResult<GetMfgProductionOrder>.Fail("Order {OrderId} có {Count} PSV.ValidTo == null. Vui lòng sửa dữ liệu (chỉ nên có 1).");
                }

                return OperationResult<GetMfgProductionOrder>.Ok(vm, "Lấy lệnh sản xuất thành công.");
            }
            catch (OperationCanceledException)
            {
                return OperationResult<GetMfgProductionOrder>.Fail("Yêu cầu đã bị hủy.");
            }
            catch (Exception ex)
            {
                return OperationResult<GetMfgProductionOrder>.Fail($"Lỗi hệ thống: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách công thức và lệnh sản xuất với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns> 
        public async Task<PagedResult<GetFormulaAndMfgFormula>> GetFormulaAndMfgFormulaAsync(FormulaAndMfgFormulaQuery query, CancellationToken ct = default)
        {
            //try
            //{
            //    if (query.PageNumber <= 0) query.PageNumber = 1;
            //    if (query.PageSize <= 0) query.PageSize = 15;

            //    var q = _unitOfWork.FormulaRepository.Query();

            //    if (query.ProductId.HasValue && query.ProductId.Value != Guid.Empty)
            //    {
            //        q = q.Where(f => f.ProductId == query.ProductId.Value);
            //    }

            //    if (!string.IsNullOrWhiteSpace(query.Keyword))
            //    {
            //        var keyword = query.Keyword.Trim();
            //        // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
            //        q = q.Where(x =>
            //            (x.Name ?? "").Contains(keyword) ||
            //            (x.Product.ColourCode ?? "").Contains(keyword) ||
            //            (x.Product.Name ?? "").Contains(keyword)
            //        );
            //    }

            //    var totalCount = await q.CountAsync(ct);

            //    var items = await q
            //        .OrderByDescending(f => f.CreatedDate)
            //        .Skip((query.PageNumber - 1) * query.PageSize)
            //        .Take(query.PageSize)
            //        .ProjectTo<GetFormulaAndMfgFormula>(_mapper.ConfigurationProvider)
            //        .ToListAsync(ct);

            //    return new PagedResult<GetFormulaAndMfgFormula>(items, totalCount, query.PageNumber, query.PageSize);

            //}

            //catch (Exception ex)
            //{
            //    throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            //}

            throw new ApplicationException("An error occurred while fetching manufacturing formulas.");
        }


        // ======================================================================== Update ========================================================================

        /// <summary>
        /// Cập nhật thông tin của lệnh sản xuất
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateInformationAsync(PatchMfgProductionOrderInformation req, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;
                var companyId = _currentUser.CompanyId;



                var existingMfgOrderPO = await _unitOfWork.MfgOrderPORepository.Query(track: true)
                    .Where(p => p.MfgProductionOrderId == req.MfgProductionOrderId && p.IsActive == true)
                    .FirstOrDefaultAsync(ct);

                if (existingMfgOrderPO == null)
                    return OperationResult.Fail($"Không tìm thấy lệnh sản xuất với ID {req.MfgProductionOrderId}");


                var existing = existingMfgOrderPO.ProductionOrder;


                // Chỉ được phép sửa những field sau:
                existing.UpdatedDate = now;
                existing.UpdatedBy = userId;

                // Lưu thay đổi
                PatchHelper.SetIfRef(req.PlpuNote, () => existing.PlpuNote, v => existing.PlpuNote = v);
                PatchHelper.SetIfRef(req.LabNote, () => existing.LabNote, v => existing.LabNote = v);
                PatchHelper.SetIfRef(req.Requirement, () => existing.Requirement, v => existing.Requirement = v);

                PatchHelper.SetIfRef(req.QcCheck, () => existing.QcCheck, v => existing.QcCheck = v);

                PatchHelper.SetIfNullable(req.TotalQuantity, () => existing.TotalQuantity, v => existing.TotalQuantity = v);
                PatchHelper.SetIfNullable(req.NumOfBatches, () => existing.NumOfBatches, v => existing.NumOfBatches = v);
                PatchHelper.SetIfNullable(req.ExpectedDate, () => existing.ExpectedDate, v => existing.ExpectedDate = v);

                var statusChanged = PatchHelper.SetIfRef(req.Status, () => existing.Status, v => existing.Status = v);

                // 1) Kiểm tra xem Status có đổi sang PLPUChecked không
                var statusChangedToPLPUChecked =
                    statusChanged && existing.Status == ManufacturingProductOrder.PLPUChecked.ToString();

                if (statusChanged)
                {
                    await _timeLineService.AddEventLogAsync(new EventLogModels
                    {
                        employeeId = userId,
                        eventType = EventType.ManufacturingProductOrder,
                        sourceCode = existing.ExternalId ?? string.Empty,
                        sourceId = existing.MfgProductionOrderId,
                        status = existing.Status,
                        note = $"Cập nhật bởi hệ thống vào {now} bởi {_currentUser.personName}"
                    }, ct);
                }

                if (statusChangedToPLPUChecked)
                {
                    // Tránh tạo trùng nếu đã có schedule cho lệnh này
                    var scheduleExists = await _unitOfWork.SchedualMfgRepository
                        .Query(track: false)
                        .AnyAsync(s => s.MfgProductionOrderId == existing.MfgProductionOrderId, ct);



                    if (!scheduleExists)
                    {
                        // ===== 1) Lấy thông tin Product =====
                        var productInfo = await _unitOfWork.ProductRepository.Query(false)
                            .Where(p => p.ProductId == existing.ProductId)
                            .Select(p => new
                            {
                                // ĐỔI TÊN PROPERTY NÀY CHO ĐÚNG VỚI ENTITY Product CỦA BẠN
                                p.ExpiryType,
                                p.RohsStandard,
                                p.ReachStandard,
                                p.RecycleRate,
                                p.Weight,
                                p.MaxTemp,
                                p.UsageRate
                            })
                            .FirstOrDefaultAsync(ct);

                        // ===== 2) Tạo bản ghi SchedualMfg =====
                        var schedual = new SchedualMfg
                        {
                            MfgProductionOrderId = existing.MfgProductionOrderId,
                            ProductId = existing.ProductId,

                            //ExternalId = existing.ExternalId,
                            ColorCode = existing.ProductExternalIdSnapshot,
                            ColorName = existing.ProductNameSnapshot,

                            Quantity = existing.TotalQuantity
                                         ?? existing.TotalQuantityRequest,

                            requirement = existing.Requirement,
                            Note = existing.LabNote ?? existing.PlpuNote,

                            // ===== 3) Map dữ liệu từ Product sang =====
                            ProductExpiryType = productInfo?.ExpiryType,
                            ProductRohsStandard = productInfo?.RohsStandard,
                            ReachStandard = productInfo?.ReachStandard,
                            ProductRecycleRate = productInfo?.RecycleRate,
                            ProductWeight = productInfo?.Weight,
                            ProductMaxTemp = productInfo?.MaxTemp,
                            ProductAddRate = productInfo?.UsageRate,

                            ExpectedCompletionDate = existing.ExpectedDate,
                            PlanDate = existing.ExpectedDate,
                            CreatedDate = now,

                            Status = ManufacturingProductOrder.PLPUChecked.ToString(),
                            Qcstatus = null,
                            Area = null,
                            BTPStatus = null,
                            StepOfProduct = null,
                            Idpk = null
                        };
                        await _unitOfWork.SchedualMfgRepository.AddAsync(schedual, ct);
                    }
                }

                // 2. Đồng bộ status sang MerchandiseOrder (nếu business yêu cầu)
                var detail = existingMfgOrderPO.Detail;
                if (detail != null)
                {
                    var relatedOrder = await _unitOfWork.MerchandiseOrderRepository
                        .Query(track: true)
                        .FirstOrDefaultAsync(
                            o => o.MerchandiseOrderId == detail.MerchandiseOrderId,
                            ct
                        );

                    if (relatedOrder != null
                        && relatedOrder.Status == MerchadiseStatus.Approved.ToString())
                    {
                        relatedOrder.Status = MerchadiseStatus.Processing.ToString();
                        relatedOrder.UpdatedDate = now;
                        relatedOrder.UpdatedBy = userId;

                        await _timeLineService.AddEventLogAsync(new EventLogModels
                        {
                            employeeId = userId,
                            eventType = EventType.MerchadiseStatus,
                            sourceCode = relatedOrder.ExternalId ?? string.Empty,
                            sourceId = relatedOrder.MerchandiseOrderId,
                            status = relatedOrder.Status,
                            note = $"Cập nhật bởi hệ thống vào {now} bởi {_currentUser.personName}"
                        }, ct);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return OperationResult.Ok("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail("Có lỗi xảy ra trong quá trình cập nhật lệnh sản xuất.");
            }
        }

        // ======================================================================== Helper ======================================================================== 

        /// <summary>
        /// Phương thưc tạo lệnh sản xuất khi đơn hàng được duyệt, nằm ở service merchadiseOrder
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<MfgContext> BuildMfgContextAsync(OrderSlim mo, CancellationToken ct = default)
        {
            var details = mo.Details;
            var productIds = details.Select(d => d.ProductId).Distinct().ToList();
            var vuIds = details.Select(d => d.FormulaId).Distinct().ToList();
            var now = DateTime.Now;
            // Products
            var products = await _unitOfWork.ProductRepository.Query()
                .Where(p => productIds.Contains(p.ProductId))
                .Select(p => new { p.ProductId, p.ColourCode, p.Name, p.CategoryId })
                .ToDictionaryAsync(
                    x => x.ProductId,
                    x => new ProductRow(x.ProductId, x.ColourCode, x.Name, x.CategoryId),
                    ct);

            // 1) Lọc các PSF đang hiệu lực tại 'now'
            var validPSF = _unitOfWork.ProductStandardFormulaRepository.Query()
                .Where(psf => productIds.Contains(psf.ProductId)
                           && psf.ValidFrom <= now
                           && (psf.ValidTo == null || psf.ValidTo >= now));

            // 2) Lấy ValidFrom mới nhất theo ProductId
            var latestPerProduct = validPSF
                .GroupBy(psf => psf.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    MaxValidFrom = g.Max(x => x.ValidFrom)
                });

            // 3) Join lại để lấy đúng hàng mới nhất
            var latestPsfRows =
                from psf in validPSF
                join lp in latestPerProduct
                  on new { psf.ProductId, psf.ValidFrom }
                  equals new { lp.ProductId, ValidFrom = lp.MaxValidFrom }
                select new
                {
                    psf.ProductId,
                    psf.ManufacturingFormulaId
                };

            // 4) Join sang ManufacturingFormula để lấy thông tin VA
            var standardVaByProduct = await
                (from x in latestPsfRows
                 join mf in _unitOfWork.ManufacturingFormulaRepository.Query()
                   on x.ManufacturingFormulaId equals mf.ManufacturingFormulaId
                 select new
                 {
                     x.ProductId,
                     VaId = mf.ManufacturingFormulaId,
                     VaCode = mf.ExternalId,
                     SourceVuId = mf.SourceVUFormulaId,
                     SourceVuCode = mf.SourceVUExternalIdSnapshot
                 })
                .ToDictionaryAsync(
                    x => x.ProductId,
                    x => (x.VaId, x.VaCode, x.SourceVuId, x.SourceVuCode),
                    ct);

            // VU đã từng có VA chưa
            var vuHasAnyVa = await _unitOfWork.ManufacturingFormulaRepository.Query()
                .Where(mf => mf.IsActive && mf.SourceVUFormulaId != null && vuIds.Contains(mf.SourceVUFormulaId.Value))
                .GroupBy(mf => mf.SourceVUFormulaId!.Value)
                .Select(g => new { VU = g.Key, Cnt = g.Count() })
                .ToDictionaryAsync(x => x.VU, x => x.Cnt > 0, ct);

            // Vật tư theo VU
            var fmItemsByVu = await _unitOfWork.FormulaMaterialRepository.Query()
                .Where(x => vuIds.Contains(x.FormulaId))
                .Select(x => new
                {
                    x.FormulaId,
                    Row = new FmItemRow
                    {
                        MaterialId = x.MaterialId,
                        CategoryId = x.CategoryId,
                        Quantity = x.Quantity,
                        Unit = x.Unit,
                        UnitPrice = x.UnitPrice,
                        MaterialNameSnapshot = x.MaterialNameSnapshot,
                        MaterialExternalIdSnapshot = x.MaterialExternalIdSnapshot
                    }
                })
                .GroupBy(x => x.FormulaId)
                .ToDictionaryAsync(g => g.Key, g => g.Select(z => z.Row).ToList(), ct);

            // Vật tư theo VA chuẩn (nếu có)
            var standardVaIds = standardVaByProduct.Values.Select(v => v.VaId).Distinct().ToList();
            var fmItemsByVa = standardVaIds.Count == 0
                ? new Dictionary<Guid, List<FmItemRow>>()
                : await _unitOfWork.ManufacturingFormulaMaterialRepository.Query()
                    .Where(m => standardVaIds.Contains(m.ManufacturingFormulaId))
                    .Select(m => new
                    {
                        m.ManufacturingFormulaId,
                        Row = new FmItemRow
                        {
                            MaterialId = m.MaterialId,
                            CategoryId = m.CategoryId,
                            Quantity = m.Quantity,
                            Unit = m.Unit,
                            UnitPrice = m.UnitPrice,
                            MaterialNameSnapshot = m.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot
                        }
                    })
                    .GroupBy(m => m.ManufacturingFormulaId)
                    .ToDictionaryAsync(g => g.Key, g => g.Select(z => z.Row).ToList(), ct);


            // Price map: lấy bản ghi giá mới nhất cho mỗi MaterialId (đúng như cách bạn đang làm)
            var allMatIds = new HashSet<Guid>();
            foreach (var list in fmItemsByVu.Values) foreach (var it in list) allMatIds.Add(it.MaterialId);
            foreach (var list in fmItemsByVa.Values) foreach (var it in list) allMatIds.Add(it.MaterialId);

            var msRaw = await _unitOfWork.MaterialsSupplierRepository.Query()
                .Where(ms => allMatIds.Contains(ms.MaterialId))
                .ToListAsync(ct);

            var lastPerMaterial = msRaw
                .GroupBy(ms => ms.MaterialId)
                .Select(g => new { MaterialId = g.Key, MaxStamp = g.Max(x => x.UpdatedDate ?? x.CreateDate) })
                .ToList();

            var priceMap = (
                from ms in msRaw
                join last in lastPerMaterial
                  on new { ms.MaterialId, Stamp = (ms.UpdatedDate ?? ms.CreateDate) }
                  equals new { last.MaterialId, Stamp = last.MaxStamp }
                select new { ms.MaterialId, ms.CurrentPrice }
            ).ToDictionary(x => x.MaterialId, x => x.CurrentPrice ?? 0m);

            return new MfgContext
            {
                Products = products,
                StandardVaByProduct = standardVaByProduct,
                FmItemsByVu = fmItemsByVu,
                FmItemsByVa = fmItemsByVa,
                PriceMap = priceMap,
                VuHasAnyVa = vuHasAnyVa
            };

        }

        /// <summary>
        /// Gom toàn bộ dữ liệu <b>read-only</b> cần thiết để tạo <b>nhiều</b> lệnh sản xuất cho một đơn hàng,
        /// nhằm tránh N+1 queries và đảm bảo nhất quán trong cùng transaction.
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="detail"></param>
        /// <param name="ctx"></param>
        /// <param name="actorId"></param>
        /// <param name="now"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //public async Task<(MfgProductionOrder order,
        //                   ManufacturingFormula mfgFormula,
        //                   List<ManufacturingFormulaMaterial> materials,
        //                   ProductStandardFormula select,
        //                   MfgOrderPO link)>
        //             CreateOneMfgBundleAsync(
        //         OrderSlim mo,
        //         OrderDetailSlim detail,
        //         MfgContext ctx,
        //         Guid actorId, DateTime now, CancellationToken ct)
        //{
        public async Task<(MfgProductionOrder order,
                           MfgOrderPO link)>
                     CreateOneMfgBundleAsync(
                 OrderSlim mo,
                 OrderDetailSlim detail,
                 MfgContext ctx,
                 Guid actorId, DateTime now, CancellationToken ct)
        {

            // a) Product
            if (!ctx.Products.TryGetValue(detail.ProductId, out var product))
                throw new InvalidOperationException($"Product {detail.ProductId} không tồn tại.");

            // b) Ưu tiên VA chuẩn theo Product (nếu đang hiệu lực)
            var hasStandardForProduct = ctx.StandardVaByProduct.TryGetValue(detail.ProductId, out var stdVaForProduct);

            // c) Items nguồn
            List<FmItemRow>? fmItems = null;
            if (hasStandardForProduct)
                ctx.FmItemsByVa.TryGetValue(stdVaForProduct!.VaId, out fmItems);
            else
                ctx.FmItemsByVu.TryGetValue(detail.FormulaId, out fmItems);
            fmItems ??= new List<FmItemRow>();

            // === Tạo MFG order ===
            var mfgId = Guid.CreateVersion7();
            var mfgExternalId = await _externalId.NextAsync(mo.CompanyId, "MFG", now, ct: ct);

            var order = new MfgProductionOrder
            {
                MfgProductionOrderId = mfgId,
                ExternalId = mfgExternalId,

                //ProductionType = null, // nếu bạn có rule thì set ở đây
                ProductId = product.ProductId,
                ProductExternalIdSnapshot = product.ColourCode,
                ProductNameSnapshot = product.Name,

                CustomerId = mo.CustomerId,
                CustomerExternalIdSnapshot = mo.CustomerExternalIdSnapshot,
                CustomerNameSnapshot = mo.CustomerNameSnapshot,

                // snapshot VU (tuỳ bạn có muốn lưu không)
                FormulaId = detail.FormulaId,
                FormulaExternalIdSnapshot = detail.FormulaExternalIdSnapshot,

                ManufacturingDate = null,
                ExpectedDate = null,
                RequiredDate = detail.DeliveryRequestDate ?? now, // RequiredDate là non-nullable

                TotalQuantityRequest = (int)detail.ExpectedQuantity,
                TotalQuantity = null,

                NumOfBatches = null,
                UnitPriceAgreed = detail.UnitPriceAgreed ?? 0m,

                Status = ManufacturingProductOrder.New.ToString(),

                LabNote = null,
                Requirement = detail.Comment,
                PlpuNote = null,

                BagType = detail.BagType ?? string.Empty,
                IsActive = true,

                QcCheck = null,

                CompanyId = mo.CompanyId,
                CreatedDate = now,
                CreatedBy = actorId,
                UpdatedDate = now,
                UpdatedBy = actorId
            };

            //// === Tạo VA mới ===
            //var vaId = Guid.CreateVersion7();
            //var vaExternalId = await _externalId.NextAsync(mo.CompanyId, "VA", now, ct: ct);

            //var mfgFormula = new ManufacturingFormula
            //{
            //    ManufacturingFormulaId = vaId,
            //    ExternalId = vaExternalId,
            //    Name = await ExternalIdGenerator.GenerateFormulaCode(
            //        "F",
            //        prefix => _unitOfWork.ManufacturingFormulaRepository
            //                    .GetLatestExternalIdStartsWithAsync(prefix, mfgId)
            //    ),
            //    Status = ManufacturingProductOrderFormula.New.ToString(),
            //    TotalPrice = null,

            //    // Nguồn gốc: nếu có VA gần nhất của VU thì link vào; luôn track VU gốc
            //    SourceManufacturingFormulaId = hasStandardForProduct ? stdVaForProduct!.VaId : (Guid?)null,
            //    SourceManufacturingExternalIdSnapshot = hasStandardForProduct ? stdVaForProduct!.VaCode : null,
            //    SourceVUFormulaId = detail.FormulaId,
            //    SourceVUExternalIdSnapshot = detail.FormulaExternalIdSnapshot,

            //    IsActive = true,
            //    Note = null,

            //    CreatedDate = now,
            //    CreatedBy = actorId,
            //    UpdatedDate = now,
            //    UpdatedBy = actorId,
            //    CompanyId = mo.CompanyId
            //};

            //// === Clone vật tư + scale theo ExpectedQuantity ===
            //var materials = new List<ManufacturingFormulaMaterial>(fmItems.Count);
            //decimal total = 0m;

            //foreach (var fm in fmItems)
            //{
            //    var qty = fm.Quantity * detail.ExpectedQuantity; // scale
            //    var unitPrice = ctx.PriceMap.TryGetValue(fm.MaterialId, out var p) ? p : (fm.UnitPrice ?? 0m);
            //    var lineTotal = unitPrice * qty;
            //    total += lineTotal;

            //    materials.Add(new ManufacturingFormulaMaterial
            //    {
            //        ManufacturingFormulaMaterialId = Guid.CreateVersion7(),
            //        ManufacturingFormulaId = mfgFormula.ManufacturingFormulaId,

            //        MaterialId = fm.MaterialId,
            //        CategoryId = fm.CategoryId.GetValueOrDefault(),
            //        Quantity = qty, // ✅ đã sửa
            //        Unit = fm.Unit,

            //        UnitPrice = unitPrice,
            //        TotalPrice = lineTotal,

            //        MaterialNameSnapshot = fm.MaterialNameSnapshot,
            //        MaterialExternalIdSnapshot = fm.MaterialExternalIdSnapshot
            //    });
            //}

            //mfgFormula.TotalPrice = total;

            //// 6) Join entity: chọn phiên bản công thức cho Order
            //var select = new ProductStandardFormula
            //{
            //    ProductStandardFormulaId = Guid.CreateVersion7(),
            //    ProductId = detail.ProductId,
            //    ManufacturingFormulaId = mfgFormula.ManufacturingFormulaId,
            //    ValidFrom = now,
            //    ValidTo = null,
            //    CreatedBy = actorId,
            //    ClosedBy = null,
            //    CompanyId = mo.CompanyId
            //};


            // 7) Liên kết Order với chi tiết đơn hàng (MfgOrderPO)
            var link = new MfgOrderPO
            {
                MerchandiseOrderDetailId = detail.MerchandiseOrderDetailId,
                MfgProductionOrderId = order.MfgProductionOrderId,
                IsActive = true
            };

            //return (order, mfgFormula, materials, select, link);
            return (order, link);
        }
    }
}
