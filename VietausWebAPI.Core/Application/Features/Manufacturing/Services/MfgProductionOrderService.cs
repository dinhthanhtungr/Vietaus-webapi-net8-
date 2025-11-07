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
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public class MfgProductionOrderService : IMfgProductionOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExternalIdService _externalId;

        public MfgProductionOrderService(IUnitOfWork unitOfWork, IMapper mapper, IExternalIdService externalId)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _externalId = externalId;
        }

        /// <summary>
        /// Phương thưc tạo lệnh sản xuất khi đơn hàng được duyệt, nằm ở service merchadiseOrder
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<MfgContext> BuildMfgContextAsync(MerchandiseOrder mo, CancellationToken ct = default)
        {
            var details = mo.MerchandiseOrderDetails;
            var productIds = details.Select(d => d.ProductId).Distinct().ToList();
            var vuIds = details.Select(d => d.FormulaId).Distinct().ToList();

            // Products
            var products = await _unitOfWork.ProductRepository.Query()
                .Where(p => productIds.Contains(p.ProductId))
                .Select(p => new { p.ProductId, p.ColourCode, p.Name, p.CategoryId })
                .ToDictionaryAsync(
                    x => x.ProductId,
                    x => (x.ProductId, x.ColourCode, x.Name, x.CategoryId),
                    ct);

            // VA chuẩn mới nhất per VU
            var standardVaByVu = await _unitOfWork.ManufacturingFormulaRepository.Query()
                .Where(f => f.IsActive == true && f.IsStandard == true && vuIds.Contains(f.VUFormulaId))
                .OrderByDescending(f => f.UpdatedDate)
                .Select(f => new
                {
                    f.VUFormulaId,
                    f.ManufacturingFormulaId, // VA id
                    VaCode = f.ExternalId,    // mã VA
                    VuCode = f.FormulaExternalIdSnapshot
                })
                .GroupBy(x => x.VUFormulaId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => (g.First().ManufacturingFormulaId, g.First().VaCode, g.First().VuCode!),
                    ct);

            // VU đã từng có VA chưa
            var vuHasAnyVa = await _unitOfWork.ManufacturingFormulaRepository.Query()
                .Where(f => f.IsActive == true && vuIds.Contains(f.VUFormulaId))
                .GroupBy(f => f.VUFormulaId)
                .Select(g => new { VU = g.Key, Cnt = g.Count() })
                .ToDictionaryAsync(x => x.VU, x => x.Cnt > 0, ct);

            // Vật tư theo VU
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

            // Vật tư theo VA chuẩn (nếu có)
            var standardVaIds = standardVaByVu.Values.Select(v => v.ManufacturingFormulaId).ToList();
            var fmItemsByVa = standardVaIds.Count == 0
                ? new Dictionary<Guid, List<FmItemRow>>()
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
                StandardVaByVu = standardVaByVu,
                FmItemsByVu = fmItemsByVu,
                FmItemsByVa = fmItemsByVa,
                PriceMap = priceMap,
                VuHasAnyVa = vuHasAnyVa
            };
        }

        /// <summary>
        /// Lấy danh sách đơn hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<PagedResult<GetSummaryMfgProductionOrder>> GetAllAsync(MfgProductionOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var result = _unitOfWork.MfgProductionOrderRepository.Query();

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

                int totalCount = await result.CountAsync(ct);

                var items = await result
                    .Where(f => f.IsActive == true)
                    .OrderByDescending(c => c.CreateDate) // "F1" -> "F0000000001"
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ProjectTo<GetSummaryMfgProductionOrder>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetSummaryMfgProductionOrder>(items, totalCount, query.PageNumber, query.PageSize);
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách đơn hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<PagedResult<GetSampleMfgFormula>> GetAllMfgFormulaAsync(MfgProductionOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var q = _unitOfWork.ManufacturingFormulaRepository.Query();
                var mpoQ = _unitOfWork.MfgProductionOrderRepository.Query();

                if (query.MfgProductionOrderId.HasValue && query.MfgProductionOrderId.Value != Guid.Empty)
                {
                    q = q.Where(f => f.MfgProductionOrderId == query.MfgProductionOrderId.Value);
                }

                if (query.MfgFormulaId.HasValue && query.MfgFormulaId.Value != Guid.Empty)
                {
                    q = q.Where(f => f.ManufacturingFormulaId == query.MfgFormulaId.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                    q = q.Where(x =>
                        (x.MfgProductionOrder.Product.ColourCode ?? "").Contains(keyword) ||
                        (x.MfgProductionOrder.Product.Name ?? "").Contains(keyword)
                    );
                }
                var totalCount = await q.CountAsync(ct);
                var items = q
                    // Đừng sort trước; để sau khi tính được cờ isStandard của *bản ghi hiện tại*
                    .Select(f => new
                    {
                        f,
                        std = q
                            .Join(mpoQ,
                                  fs => fs.MfgProductionOrderId,
                                  o => o.MfgProductionOrderId,
                                  (fs, o) => new { fs, o })
                            .Where(x => x.fs.IsActive == true
                                        && x.fs.IsStandard == true
                                        && x.fs.VUFormulaId == f.VUFormulaId
                                        && x.o.ProductId == f.MfgProductionOrder.ProductId)
                            // IsStandard đã lọc = true, nên 2 dòng OrderBy dưới là thừa; giữ createdDate nếu muốn
                            .OrderByDescending(x => x.fs.CreatedDate)
                            .Select(x => new { x.fs.ManufacturingFormulaId, x.fs.ExternalId })
                            .FirstOrDefault()
                    })
                    // TÍNH cờ isStandard cho *bản ghi hiện tại*
                    .Select(x => new
                    {
                        x.f,
                        x.std,
                        isStandard = x.std != null && x.std.ManufacturingFormulaId == x.f.ManufacturingFormulaId
                    })
                    // >>> SORT: chuẩn trước, rồi bản được chọn, rồi theo ngày
                    .OrderByDescending(x => x.isStandard)
                    .ThenByDescending(x => x.f.IsSelect == true)
                    .ThenBy(x => x.f.CreatedDate)
                    .Select(x => new GetSampleMfgFormula
                    {
                        ManufacturingFormulaId = x.f.ManufacturingFormulaId,
                        //MfgProductionOrderExternalId = x.f.MfgProductionOrderExternalId,
                        ExternalId = x.f.ExternalId,
                        Name = x.f.Name,
                        VUFormulaId = x.f.VUFormulaId,
                        FormulaExternalIdSnapshot = x.f.FormulaExternalIdSnapshot,

                        MfgFormulaId = x.std == null ? (Guid?)null : x.std.ManufacturingFormulaId,
                        MfgFormulaExternalIdSnapshot = x.std == null ? null : x.std.ExternalId,

                        Status = x.f.Status,
                        TotalPrice = x.f.TotalPrice,
                        isStandard = x.isStandard,                // <— dùng cờ đúng nghĩa
                        IsSelect = x.f.IsSelect,
                        CreatedDate = x.f.CreatedDate 
                    });


                return new PagedResult<GetSampleMfgFormula>(items, totalCount, query.PageNumber, query.PageSize);
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
        public async Task<GetMfgProductionOrder?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _unitOfWork.MfgProductionOrderRepository.Query()
                .Where(p => p.MfgProductionOrderId == id && p.IsActive == true)
                .ProjectTo<GetMfgProductionOrder>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Lấy danh sách công thức và lệnh sản xuất với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns> 
        public async Task<PagedResult<GetFormulaAndMfgFormula>> GetFormulaAndMfgFormulaAsync(FormulaAndMfgFormulaQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var q = _unitOfWork.FormulaRepository.Query();

                if (query.ProductId.HasValue && query.ProductId.Value != Guid.Empty)
                {
                    q = q.Where(f => f.ProductId == query.ProductId.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                    q = q.Where(x =>
                        (x.Name ?? "").Contains(keyword) ||
                        (x.Product.ColourCode ?? "").Contains(keyword) ||
                        (x.Product.Name ?? "").Contains(keyword)
                    );
                }

                var totalCount = await q.CountAsync(ct);

                var items = await q
                    .OrderByDescending(f => f.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ProjectTo<GetFormulaAndMfgFormula>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetFormulaAndMfgFormula>(items, totalCount, query.PageNumber, query.PageSize);

            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin theo đơn hàng mới
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

                var existing = await _unitOfWork.MfgProductionOrderRepository.Query(track: true)
                    .Where(p => p.MfgProductionOrderId == req.MfgProductionOrderId && p.IsActive == true)
                    .FirstOrDefaultAsync(ct);

                if (existing == null)
                    return OperationResult.Fail($"Không tìm thấy lệnh sản xuất với ID {req.MfgProductionOrderId}");

                // Chỉ được phép sửa những field sau:
                existing.UpdatedDate = now;

                existing.UpdatedBy = req.UpdatedBy;

                // Lưu thay đổi
                PatchHelper.SetIfRef(req.PlpuNote, () => existing.PlpuNote, v => existing.PlpuNote = v);
                PatchHelper.SetIfRef(req.LabNote, () => existing.LabNote, v => existing.LabNote = v);
                PatchHelper.SetIfRef(req.Requirement, () => existing.Requirement, v => existing.Requirement = v);

                PatchHelper.SetIfRef(req.Status, () => existing.Status, v => existing.Status = v);
                PatchHelper.SetIfRef(req.QcCheck, () => existing.QcCheck, v => existing.QcCheck = v);

                PatchHelper.SetIf(req.TotalQuantity, () => existing.TotalQuantity.GetValueOrDefault(), v => existing.TotalQuantity = v);
                PatchHelper.SetIf(req.NumOfBatches, () => existing.NumOfBatches.GetValueOrDefault(), v => existing.NumOfBatches = v);
                
                PatchHelper.SetIf(req.ExpectedDate, () => existing.ExpectedDate.GetValueOrDefault(), v => existing.ExpectedDate = v);
                PatchHelper.SetIf(req.requiredDate, () => existing.requiredDate, v => existing.requiredDate = v);

                //PatchHelper.SetIf(req.QualifiedQuantity, () => existing.QualifiedQuantity.GetValueOrDefault(), v => existing.QualifiedQuantity = v);
                //PatchHelper.SetIf(req.RejectedQuantity, () => existing.RejectedQuantity.GetValueOrDefault(), v => existing.RejectedQuantity = v);
                //PatchHelper.SetIf(req.WasteQuantity, () => existing.WasteQuantity.GetValueOrDefault(), v => existing.WasteQuantity = v);

                // 2) Materials của công thức
                var formulaExist = await _unitOfWork.ManufacturingFormulaRepository.Query(track: true)
                    .Include(f => f.ManufacturingFormulaMaterials) // << bắt buộc Include
                    .FirstOrDefaultAsync(f => f.ManufacturingFormulaId == req.ManufacturingFormulaId && f.IsActive == true, ct);

                if (formulaExist == null)
                    return OperationResult.Fail($"Không tìm thấy công thức sản xuất với ID {req.ManufacturingFormulaId}");

                // Đừng lọc IsActive ở đây để còn re-activate hoặc patch dòng cũ
                var existingMaterials = formulaExist.ManufacturingFormulaMaterials
                    .ToDictionary(fm => fm.MaterialId, fm => fm);

                foreach (var m in req.ManufacturingFormulaMaterials ?? Enumerable.Empty<PatchMfgFormulaMaterial>())
                {
                    if (!existingMaterials.TryGetValue(m.MaterialId, out var link) || link == null)
                    {
                        // Nếu chỉ muốn patch các dòng đã có, bỏ qua dòng không tồn tại:
                        //  -> continue;

                        // Nếu muốn tạo mới khi chưa có, dùng khối dưới:
                        link = new ManufacturingFormulaMaterial
                        {
                            ManufacturingFormulaMaterialId = Guid.NewGuid(),
                            ManufacturingFormulaId = formulaExist.ManufacturingFormulaId,
                            MaterialId = m.MaterialId,
                            CategoryId = m.CategoryId,
                            Quantity = m.Quantity.GetValueOrDefault(),
                            UnitPrice = m.UnitPrice.GetValueOrDefault(),
                            TotalPrice = decimal.Round((m.Quantity ?? 0m) * (m.UnitPrice ?? 0m), 2, MidpointRounding.AwayFromZero),
                            Unit = m.Unit,
                            MaterialNameSnapshot = m.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,
                            IsActive = true
                        };
                        await _unitOfWork.ManufacturingFormulaMaterialRepository.AddAsync(link, ct);
                        existingMaterials[m.MaterialId] = link; // nhớ add vào dictionary
                    }

                    // Tới đây link chắc chắn != null
                    PatchHelper.SetIfNullable(m.StockId, () => link.StockId, v => link.StockId = v);
                    PatchHelper.SetIfRef(m.LotNo, () => link.LotNo, v => link.LotNo = v);

                    // Nếu dòng này từng bị tắt, bật lại
                    if (link.IsActive == false) link.IsActive = true;
                }

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
        public async Task<(MfgProductionOrder order, ManufacturingFormula mfgFormula, List<ManufacturingFormulaMaterial> materials)>
             CreateOneMfgBundleAsync(
                 MerchandiseOrder mo,
                 MerchandiseOrderDetail detail,
                 MfgContext ctx,
                 Guid actorId, DateTime now, CancellationToken ct)
        {
            // a) Product
            if (!ctx.Products.TryGetValue(detail.ProductId, out var product))
                throw new InvalidOperationException($"Product {detail.ProductId} không tồn tại.");

            // b) Chọn nguồn VA chuẩn / VU
            var hasStandard = ctx.StandardVaByVu.TryGetValue(detail.FormulaId, out var stdVa);
            var hasAnyVa = ctx.VuHasAnyVa.TryGetValue(detail.FormulaId, out var any) && any;

            // c) Items nguồn
            List<FmItemRow>? fmItems = null;
            if (hasStandard)
                ctx.FmItemsByVa.TryGetValue(stdVa!.VaId, out fmItems);
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

                MerchandiseOrderId = mo.MerchandiseOrderId,
                MerchandiseOrderExternalId = mo.ExternalId,

                ProductId = product.Id,
                //ProductionType = product.ProductType,
                ProductExternalIdSnapshot = product.ColourCode,
                ProductNameSnapshot = product.Name,

                UnitPriceAgreed = detail.UnitPriceAgreed,

                CustomerId = mo.CustomerId,
                CustomerExternalIdSnapshot = mo.CustomerExternalIdSnapshot,
                CustomerNameSnapshot = mo.CustomerNameSnapshot,

                requiredDate = detail.DeliveryRequestDate,
                Requirement = detail.Comment,
                BagType = detail.BagType,

                FormulaId = detail.FormulaId,
                FormulaExternalIdSnapshot = detail.FormulaExternalIdSnapshot,

                Status = ManufacturingProductOrder.New.ToString(),
                CompanyId = mo.CompanyId,
                IsActive = true,

                CreateDate = now,
                CreatedBy = actorId
            };

            // === Tạo VA mới ===
            var vaId = Guid.CreateVersion7();
            var vaExternalId = await _externalId.NextAsync(mo.CompanyId, "VA", now, ct: ct);

            var mfgFormula = new ManufacturingFormula
            {
                ManufacturingFormulaId = vaId,
                MfgProductionOrderId = order.MfgProductionOrderId,
                //MfgProductionOrderExternalId = order.ExternalId,

                Name = await ExternalIdGenerator.GenerateFormulaCode(
                    "F",
                    prefix => _unitOfWork.ManufacturingFormulaRepository
                                .GetLatestExternalIdStartsWithAsync(prefix, mfgId)
                ),

                ExternalId = vaExternalId,
                Status = "New",

                // Chuẩn theo VU (sale chọn)
                VUFormulaId = detail.FormulaId,
                FormulaExternalIdSnapshot = detail.FormulaExternalIdSnapshot ?? string.Empty,

                SourceType = hasStandard ? FormulaSource.FromVA : FormulaSource.FromVU,
                SourceManufacturingFormulaId = hasStandard ? stdVa!.VaId : null,
                SourceManufacturingExternalIdSnapshot = hasStandard ? stdVa!.VaCode : null,
                SourceVUFormulaId = hasStandard ? null : detail.FormulaId,
                SourceVUExternalIdSnapshot = hasStandard ? null : detail.FormulaExternalIdSnapshot,

                // GIỮ LUẬT GỐC CỦA BẠN:
                IsStandard = (!hasStandard && !hasAnyVa),
                IsSelect = false,
                IsActive = true,

                CreatedDate = now,
                CreatedBy = actorId,
                CompanyId = mo.CompanyId
            };

            // === Clone vật tư + scale theo ExpectedQuantity ===
            var materials = new List<ManufacturingFormulaMaterial>(fmItems.Count);
            decimal total = 0m;

            foreach (var fm in fmItems)
            {
                var qty = fm.Quantity * detail.ExpectedQuantity;             // scale
                var unitPrice = ctx.PriceMap.TryGetValue(fm.MaterialId, out var p) ? p : fm.UnitPrice;
                var lineTotal = unitPrice * qty;
                total += lineTotal;

                materials.Add(new ManufacturingFormulaMaterial
                {
                    ManufacturingFormulaMaterialId = Guid.CreateVersion7(),
                    ManufacturingFormulaId = mfgFormula.ManufacturingFormulaId,

                    MaterialId = fm.MaterialId,
                    CategoryId = fm.CategoryId,
                    Quantity = fm.Quantity,                                        // <-- sửa đúng
                    Unit = fm.Unit,

                    UnitPrice = unitPrice,
                    TotalPrice = lineTotal,

                    MaterialNameSnapshot = fm.MaterialNameSnapshot,
                    MaterialExternalIdSnapshot = fm.MaterialExternalIdSnapshot,
                    LotNo = string.Empty
                });
            }

            mfgFormula.TotalPrice = total;
            order.TotalQuantityRequest = (int)detail.ExpectedQuantity;

            return (order, mfgFormula, materials);
        }

    }
}
