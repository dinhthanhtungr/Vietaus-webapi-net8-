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
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.MerchandiseOrderFeatures
{
    public class MerchandiseOrderService : IMerchandiseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExternalIdService _externalId;
        private readonly IMapper _mapper;

        public MerchandiseOrderService(IUnitOfWork unitOfWork, IMapper mapper, IExternalIdService externalIdService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _externalId = externalIdService;
        }


        public async Task<OperationResult> CreateAsync(PostMerchandiseOrder req, CancellationToken ct = default)
        {
            if (req.CreatedBy == Guid.Empty) throw new ArgumentNullException(nameof(req.CreatedBy), "CreatedBy cannot be empty.");

            int affected = 0;
            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                var merchandiseOrder = _mapper.Map<MerchandiseOrder>(req);

                merchandiseOrder.MerchandiseOrderDetails = (merchandiseOrder.MerchandiseOrderDetails ?? new List<MerchandiseOrderDetail>())
                    .Where(d => d != null
                            && d.FormulaId != Guid.Empty
                            && d.ProductId != Guid.Empty
                            && d.ExpectedQuantity > 0) // Lọc bỏ các chi tiết có Quantity <= 0
                    .ToList();

                foreach (var detail in merchandiseOrder.MerchandiseOrderDetails)
                {
                    detail.TotalPriceAgreed = Math.Round(detail.UnitPriceAgreed * detail.ExpectedQuantity, 2, MidpointRounding.AwayFromZero);
                }


                merchandiseOrder.TotalPrice = Math.Round(merchandiseOrder.MerchandiseOrderDetails.Sum(d => d.TotalPriceAgreed), 2, MidpointRounding.AwayFromZero);


                // 2) ExternalId DHG (ddMMyy-#####)
                merchandiseOrder.ExternalId = await _externalId.NextAsync(req.CompanyId.GetValueOrDefault(), "DHG", now, ct: ct);


                await _unitOfWork.MerchandiseOrderRepository.AddAsync(merchandiseOrder, ct);


                // Tạo lệnh sản xuất mỗi lần đơn hàng ở trạng thái duyệt
                if (merchandiseOrder.Status == "Approved")
                {
                    var details = merchandiseOrder.MerchandiseOrderDetails;
                    // =========================
                    // (A) GOM DỮ LIỆU TRƯỚC
                    // =========================
                    var productIds = details.Select(d => d.ProductId).Distinct().ToList();
                    var vuIds = details.Select(d => d.FormulaId).Distinct().ToList();



                    // Products (READ-ONLY)
                    var products = await _unitOfWork.ProductRepository.Query()
                        .Where(p => productIds.Contains(p.ProductId))
                        .Select(p => new { p.ProductId, p.ColourCode, p.Name, p.ProductType })
                        .ToDictionaryAsync(x => x.ProductId, ct);

                    // VA chuẩn mới nhất per VU (READ-ONLY)
                    var standardVaByVu = await _unitOfWork.ManufacturingFormulaRepository.Query()
                        .Where(f => f.IsActive == true && f.IsStandard == true && vuIds.Contains(f.VUFormulaId))
                        .OrderByDescending(f => f.UpdatedDate ?? f.createdDate)
                        .Select(f => new
                        {
                            f.VUFormulaId,
                            f.ManufacturingFormulaId,
                            ExternalId = f.ExternalId,                // mã VA
                            VuExternalIdSnapshot = f.FormulaExternalIdSnapshot // snapshot mã VU
                        })
                        .GroupBy(x => x.VUFormulaId)
                        .ToDictionaryAsync(g => g.Key, g => g.First(), ct);

                    // VU đã từng có VA chưa (READ-ONLY)
                    var vuHasAnyVa = await _unitOfWork.ManufacturingFormulaRepository.Query()
                        .Where(f => f.IsActive == true && vuIds.Contains(f.VUFormulaId))
                        .GroupBy(f => f.VUFormulaId)
                        .Select(g => new { VU = g.Key, Cnt = g.Count() })
                        .ToDictionaryAsync(x => x.VU, x => x.Cnt > 0, ct);

                    // Vật tư theo VU (READ-ONLY)
                    var fmItemsByVu = await _unitOfWork.FormulaMaterialRepository.Query()
                        .Where(x => vuIds.Contains(x.FormulaId))
                        .Select(x => new {
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

                    // Vật tư theo VA chuẩn (READ-ONLY)
                    var standardVaIds = standardVaByVu.Values.Select(v => v.ManufacturingFormulaId).ToList();
                    var fmItemsByVa = standardVaIds.Count == 0
                        ? new Dictionary<Guid, List<FmItemRow>>() // rỗng
                        : await _unitOfWork.ManufacturingFormulaMaterialRepository.Query()
                            .Where(m => standardVaIds.Contains(m.ManufacturingFormulaId))
                            .Select(m => new {
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


                    // Gom toàn bộ MaterialId để lấy GIÁ hiện hành 1 lần (READ-ONLY)
                    var allMatIds = new HashSet<Guid>();
                    foreach (var list in fmItemsByVu.Values) foreach (var it in list) allMatIds.Add(it.MaterialId);
                    foreach (var list in fmItemsByVa.Values) foreach (var it in list) allMatIds.Add(it.MaterialId);

                    var msQuery = _unitOfWork.MaterialsSupplierRepository.Query()
                        .Where(ms => allMatIds.Contains(ms.MaterialId));

                    // Lấy toàn bộ supplier record cần
                    var msRaw = await _unitOfWork.MaterialsSupplierRepository.Query().AsNoTracking()
                        .Where(ms => allMatIds.Contains(ms.MaterialId))
                        .ToListAsync(ct);

                    // GroupBy trên memory (LINQ to Objects)
                    var lastPerMaterial = msRaw
                        .GroupBy(ms => ms.MaterialId)
                        .Select(g => new {
                            MaterialId = g.Key,
                            MaxStamp = g.Max(x => x.UpdatedDate ?? x.CreateDate)
                        })
                        .ToList();

                    // Sau đó join tiếp cũng bằng LINQ to Objects
                    var priceMap = (
                        from ms in msRaw
                        join last in lastPerMaterial
                          on new { ms.MaterialId, Stamp = (ms.UpdatedDate ?? ms.CreateDate) }
                          equals new { last.MaterialId, Stamp = last.MaxStamp }
                        select new { ms.MaterialId, ms.CurrentPrice }
                    ).ToDictionary(x => x.MaterialId, x => x.CurrentPrice ?? 0m);

                    foreach (var detail in details)
                    {

                        // a) Product từ dictionary
                        if (!products.TryGetValue(detail.ProductId, out var product))
                            throw new InvalidOperationException($"Product {detail.ProductId} không tồn tại.");

                        // b) Chọn nguồn VA chuẩn / VU
                        var hasStandard = standardVaByVu.TryGetValue(detail.FormulaId, out var stdVa);
                        var hasAnyVa = vuHasAnyVa.TryGetValue(detail.FormulaId, out var any) && any;

                        // c) Lấy dòng vật tư nguồn
                        List<FmItemRow>? fmItems = null;
                        if (hasStandard)
                            fmItemsByVa.TryGetValue(stdVa!.ManufacturingFormulaId, out fmItems);
                        else
                            fmItemsByVu.TryGetValue(detail.FormulaId, out fmItems);

                        fmItems ??= new List<FmItemRow>();   // fallback rỗng


                        // === [M-STEP 5] TẠO MFG PRODUCTION ORDER ===
                        var MfgProductionOrderId = Guid.NewGuid();
                        var mfgExternalId = await _externalId.NextAsync(req.CompanyId.GetValueOrDefault(), "MFG", now, ct: ct);

                        var order = new MfgProductionOrder
                        {
                            MfgProductionOrderId = MfgProductionOrderId,
                            ExternalId = mfgExternalId,

                            MerchandiseOrderId = merchandiseOrder.MerchandiseOrderId,
                            MerchandiseOrderExternalId = merchandiseOrder.ExternalId,

                            ProductId = product.ProductId,
                            ProductionType = product.ProductType,
                            ProductExternalIdSnapshot = product.ColourCode,
                            ProductNameSnapshot = product.Name,

                            UnitPriceAgreed = detail.UnitPriceAgreed, // GÍA SALE BÁN CHO KHÁCH

                            CustomerId = merchandiseOrder?.CustomerId,
                            CustomerExternalIdSnapshot = merchandiseOrder?.CustomerExternalIdSnapshot,
                            CustomerNameSnapshot = merchandiseOrder?.CustomerNameSnapshot,

                            requiredDate = detail?.DeliveryRequestDate,
                            Requirement = detail.Comment,
                            BagType = detail.BagType,

                            FormulaId = detail.FormulaId,
                            FormulaExternalIdSnapshot = detail.FormulaExternalIdSnapshot,

                            Status = ManufacturingProductOrder.New.ToString(),
                            CompanyId = req.CompanyId,   // nếu lấy từ token thì thay tại đây
                            IsActive = true,

                            CreateDate = now,
                            CreatedBy = req.CreatedBy
                        };
                        await _unitOfWork.MfgProductionOrderRepository.AddAsync(order, ct);


                        // === [M-STEP 6] TẠO VA MỚI (ghi dấu vết nguồn + chuẩn theo VU) ===
                        // useStandard: có VA chuẩn đúng VU => copy từ VA chuẩn; ngược lại copy từ VU
                        // vuHasAnyVa: VU này đã từng có VA nào chưa?
                        var mfgFormula = new ManufacturingFormula
                        {
                            ManufacturingFormulaId = Guid.NewGuid(),
                            mfgProductionOrderId = order.MfgProductionOrderId,
                            MfgProductionOrderExternalId = order.ExternalId,

                            Name = await ExternalIdGenerator.GenerateFormulaCode(
                                "F",
                                prefix => _unitOfWork.ManufacturingFormulaRepository.GetLatestExternalIdStartsWithAsync(prefix, MfgProductionOrderId)
                            ),

                            ExternalId = await _externalId.NextAsync(req.CompanyId.GetValueOrDefault(), "VA", now, ct: ct),

                            Status = "New",

                            // CHUẨN THEO VU: luôn gán VU mà sale chọn
                            VUFormulaId = detail.FormulaId,
                            FormulaExternalIdSnapshot = detail.FormulaExternalIdSnapshot ?? "",

                            SourceType = hasStandard ? "FromVA" : "FromVU",
                            SourceManufacturingFormulaId = hasStandard ? stdVa!.ManufacturingFormulaId : null,
                           
                            SourceManufacturingExternalIdSnapshot = hasStandard ? stdVa!.ExternalId : null, // mã VA chuẩn
                            SourceVUFormulaId = hasStandard ? null : detail.FormulaId,
                            SourceVUExternalIdSnapshot = hasStandard ? null : detail.FormulaExternalIdSnapshot,

                            // ĐẶT CHUẨN: VA đầu tiên của VU => chuẩn
                            IsStandard = (!hasStandard && !hasAnyVa),
                            IsSelect = false,
                            IsActive = true,

                            createdDate = now,
                            CreatedBy = req.CreatedBy,
                            companyId = req.CompanyId
                        };
                        await _unitOfWork.ManufacturingFormulaRepository.AddAsync(mfgFormula, ct);


                        // Nếu vừa auto set chuẩn thì ngầm hiểu là VA đầu tiên của VU này và ghi log công thức VA này là chuẩn
                        if (mfgFormula.IsStandard)
                        {
                            await _unitOfWork.ManufacturingFormulaLogRepository.AddAsync(new ManufacturingFormulaLog
                            {
                                ManufacturingFormulaId = mfgFormula.ManufacturingFormulaId,
                                Action = ManufacturingFormulaLogAction.SetStandard,
                                Comment = "Tự động đặt công thức này là chuẩn vì là công thức đầu tiên của VU.",
                                PerformedDate = now,
                                PerformedBy = req.CreatedBy,
                                PerformedByNameSnapshot = "Created by system (ID from sale)"
                            }, ct);
                        }

                        // === [M-STEP 7] CLONE VẬT TƯ + SCALE THEO SỐ LƯỢNG ĐẶT ===
                        var materialLines = new List<ManufacturingFormulaMaterial>(fmItems.Count);
                        decimal grandTotal = 0m;

                        foreach (var fm in fmItems)
                        {
                            // QUAN TRỌNG: chỉnh luật scale số lượng đúng mô hình của bạn
                            // Nếu Quantity trong công thức là định mức cho 1 đơn vị SP:
                            decimal qty = (fm.Quantity) * (detail.ExpectedQuantity);

                            // Giá hiện hành, fallback về giá trong công thức nếu thiếu
                            var unitPrice = priceMap.TryGetValue(fm.MaterialId, out var p) ? p : (fm.UnitPrice);
                            var lineTotal = unitPrice * qty;
                            grandTotal += lineTotal;

                            materialLines.Add(new ManufacturingFormulaMaterial
                            {
                                ManufacturingFormulaMaterialId = Guid.NewGuid(),
                                ManufacturingFormulaId = mfgFormula.ManufacturingFormulaId,

                                MaterialId = fm.MaterialId,
                                CategoryId = fm.CategoryId, // để null nếu schema cho phép null
                                Quantity = fm.Quantity,
                                Unit = fm.Unit,

                                UnitPrice = unitPrice,
                                TotalPrice = lineTotal,

                                MaterialNameSnapshot = fm.MaterialNameSnapshot,
                                MaterialExternalIdSnapshot = fm.MaterialExternalIdSnapshot,
                                LotNo = string.Empty
                            });
                        }

                        mfgFormula.TotalPrice = grandTotal;
                        order.TotalQuantityRequest = (int?)detail.ExpectedQuantity;

                        await _unitOfWork.ManufacturingFormulaMaterialRepository.AddRangeAsync(materialLines);


                    }
                }

                affected = await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);

                return affected > 0
                    ? OperationResult.Ok("Tạo đơn hàng thành công")
                    : OperationResult.Fail("Thất bại.");
            }

            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult.Fail($"Lỗi khi tạo đơn hàng: {ex.Message}");

            }
        }

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

        public async Task<GetMerchadiseOrderWithId?> GetByIdAsync(Guid merchandiseOrderId, CancellationToken ct = default)
        {
            return await _unitOfWork.MerchandiseOrderRepository.Query()
                .Where(m => m.MerchandiseOrderId == merchandiseOrderId && m.IsActive == true)
                .ProjectTo<GetMerchadiseOrderWithId>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<GetOldProductInformation?> GetLastMerchandiseOrderByCustomerIdAsync(Guid customerId, Guid productId, CancellationToken ct = default)
        {
            return await _unitOfWork.MerchandiseOrderRepository.Query()
                .Where(o => o.CustomerId == customerId && (o.IsActive ?? true) == true)
                .SelectMany(o => o.MerchandiseOrderDetails, (o, d) => new { o, d }) 
                .Where(x => x.d.ProductId == productId && (x.d.IsActive ?? true) == true && (x.d.Status == null || x.d.Status != "Cancelled"))
                .OrderByDescending(x => x.d.DeliveryRequestDate ?? x.o.PaymentDate ?? x.o.UpdatedDate ?? x.o.CreateDate)
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
                PatchHelper.SetIf(req.UpdatedBy, () => MerchandiseOrder.UpdatedBy.GetValueOrDefault(), v => MerchandiseOrder.UpdatedBy = v);

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






    public class FmItemRow
    {
        public Guid MaterialId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public string MaterialNameSnapshot { get; set; } = "";
        public string MaterialExternalIdSnapshot { get; set; } = "";
    }

}
