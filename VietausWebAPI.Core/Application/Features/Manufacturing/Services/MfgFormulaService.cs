using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public class MfgFormulaService : IMfgFormulaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExternalIdService _externalId;

        public MfgFormulaService(IUnitOfWork unitOfWork, IMapper mapper, IExternalIdService externalId)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _externalId = externalId;
        }

        /// <summary>
        /// Lấy thông tin công thức sản xuất theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<GetManufacturingFormula?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _unitOfWork.ManufacturingFormulaRepository.Query()
                .Where(p => p.ManufacturingFormulaId == id && p.IsActive == true)
                .ProjectTo<GetManufacturingFormula>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);


        }

        /// <summary>
        /// Lấy tất cả công thức sản xuất theo điều kiện lọc và phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<PagedResult<GetSummaryMfgFormula>> GetAllAsync(MfgFormulaQuery query, CancellationToken ct)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                // 1) Query từng nguồn
                IQueryable<ManufacturingFormula> qm = _unitOfWork.ManufacturingFormulaRepository
                    .Query().AsNoTracking().Where(p => p.IsActive);

                IQueryable<Formula> qr = _unitOfWork.FormulaRepository
                    .Query().AsNoTracking().Where(p => p.IsActive == true);

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var kw = query.Keyword.Trim().ToLower();
                    qm = qm.Where(x => x.ExternalId.ToLower().Contains(kw));
                    qr = qr.Where(x => x.ExternalId.ToLower().Contains(kw));
                    // Gợi ý: nếu DB hỗ trợ, dùng EF.Functions.Like(x.ExternalId, $"%{query.Keyword}%") + collation case-insensitive để tận dụng index tốt hơn.
                }

                if (query.CompanyId is Guid cid && cid != Guid.Empty)
                {
                    qm = qm.Where(x => x.companyId == cid);
                    qr = qr.Where(x => x.CompanyId == cid);
                }

                if (query.ProductId is Guid pid && pid != Guid.Empty)
                {
                    qm = qm.Where(x => x.MfgProductionOrder != null && x.MfgProductionOrder.ProductId == pid);
                    qr = qr.Where(x => x.ProductId == pid);
                }

                // 2) Hợp nguồn và ProjectTo DTO
                IQueryable<GetSummaryMfgFormula> unified = query.Source switch
                {
                    FormulaSource.Production => qm.ProjectTo<GetSummaryMfgFormula>(_mapper.ConfigurationProvider),
                    FormulaSource.Research => qr.ProjectTo<GetSummaryMfgFormula>(_mapper.ConfigurationProvider),
                    _ => qm.ProjectTo<GetSummaryMfgFormula>(_mapper.ConfigurationProvider)
                          .Union(qr.ProjectTo<GetSummaryMfgFormula>(_mapper.ConfigurationProvider))
                };

                // 3) Đếm + sort/paging trên DTO (an toàn vì chỉ sắp xếp theo field scalar)
                var total = await unified.CountAsync(ct);

                var items = await unified
                    .OrderBy(x => x.isStandard ?? false)
                    .ThenBy(x => x.IsSelect ?? false)
                    .ThenByDescending(x => x.ExternalId)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync(ct);

                // 4) Bơm TotalPrice cho trang hiện tại (giá mới nhất theo UpdatedDate/CreateDate)
                await HydrateUnitPricesAndTotalsAsync(items, ct);


                return new PagedResult<GetSummaryMfgFormula>(items, total, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching manufacturing formulas.", ex);
            }
        }

        /// <summary>
        /// Tính TotalPrice cho danh sách DTO: Σ(Quantity * đơn giá gần nhất của material).
        /// Đơn giá lấy từ MaterialsSupplier: bản ghi IsActive == true, ngày mới nhất (UpdatedDate ưu tiên, fallback CreateDate).
        /// Nếu có nhiều supplier cùng ngày, ưu tiên IsPreferred == true.
        /// </summary>
        private async Task HydrateUnitPricesAndTotalsAsync(List<GetSummaryMfgFormula> items, CancellationToken ct)
        {
            if (items.Count == 0) return;

            // Gom tất cả MaterialId có trong trang
            var materialIds = items
                .SelectMany(i => i.ManufacturingFormulaMaterials)
                .Select(m => m.MaterialId)
                .Distinct()
                .ToList();

            if (materialIds.Count == 0) return;

            // Lấy đơn giá mới nhất cho từng MaterialId trong MaterialsSupplier
            var raw = await _unitOfWork.MaterialsSupplierRepository.Query()
                .Where(s => materialIds.Contains(s.MaterialId) && (s.IsActive ?? true))
                .Select(s => new
                {
                    s.MaterialId,
                    s.CurrentPrice,
                    s.IsPreferred,
                    Stamp = (DateTime?)(s.UpdatedDate ?? s.CreateDate)
                })
                .ToListAsync(ct);

            var latestPriceByMaterial = raw
                .GroupBy(x => x.MaterialId)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .OrderByDescending(x => x.Stamp)               // ngày mới nhất
                        .ThenByDescending(x => x.IsPreferred ?? false) // nếu cùng ngày ưu tiên preferred
                        .Select(x => x.CurrentPrice ?? 0m)
                        .FirstOrDefault()
                );

            // Gán UnitPrice cho từng material & tính TotalPrice công thức
            foreach (var f in items)
            {
                decimal total = 0m;

                foreach (var m in f.ManufacturingFormulaMaterials)
                {
                    m.UnitPrice = latestPriceByMaterial.TryGetValue(m.MaterialId, out var unit)
                        ? unit
                        : 0m;

                    var qty = m.Quantity ?? 0m;
                    total += qty * (m.UnitPrice ?? 0m);
                }

                f.TotalPrice = total;
            }
        }

        /// <summary>
        /// Upsert (cập nhật hoặc bổ sung) **Công thức sản xuất** cho một Lệnh sản xuất.
        /// 
        /// NGHIỆP VỤ & QUY TẮC KINH DOANH
        /// 1) Cho phép cập nhật các thuộc tính meta của công thức (Note, Source*, IsActive, IsSelect, IsStandard, Status…).
        /// 2) Quản lý **duy nhất 1 công thức IsSelect** trong phạm vi **một Lệnh sản xuất** (MfgProductionOrder).
        /// 3) Quản lý **duy nhất 1 công thức IsStandard** trong phạm vi **một VUFormula** (bộ công thức chuẩn).
        /// 4) Khi công thức được chọn (IsSelect = true):
        ///    - Cập nhật Status của công thức = "IsSelect".
        ///    - Nếu Lệnh sản xuất đang ở trạng thái New/QCFail ⇒ chuyển sang QCChecked.
        /// 5) Quản lý danh sách vật tư của công thức (ManufacturingFormulaMaterials):
        ///    - Thêm mới nếu chưa có.
        ///    - Cập nhật nếu đã tồn tại (Quantity, UnitPrice, Unit, snapshots, Lot/Stock link…).
        ///    - **Soft-delete** (IsActive = false) những dòng không còn trong payload.
        /// 6) **Tổng tiền công thức** luôn tự tính lại (Sum TotalPrice các dòng còn hiệu lực) — bỏ qua giá trị do client gửi.
        /// 7) Toàn bộ thao tác được bọc trong **transaction** để đảm bảo tính nhất quán.
        /// 
        /// LƯU Ý THỰC THI:
        /// - Sử dụng PatchHelper để cập nhật từng phần (partial update) theo payload.
        /// - Chỉ load/track công thức mục tiêu + collection vật tư của nó.
        /// - Ghi log khi đánh dấu IsStandard (truy vết ai/bao giờ/vì sao).
        /// </summary>
        public async Task<OperationResult> UpsertFormulaAsync(PatchMfgFormula req, CancellationToken? cancellationToken = null)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                // 1) Load công thức sản xuất cần cập nhật (bao gồm cả materials)
                var formulaExist = await _unitOfWork.ManufacturingFormulaRepository.Query(track: true)
                    .Include(f => f.ManufacturingFormulaMaterials)
                    .FirstOrDefaultAsync(f => f.ManufacturingFormulaId == req.ManufacturingFormulaId && f.IsActive == true, cancellationToken ?? default);




                if (formulaExist == null) return OperationResult.Fail("Công thức không tồn tại");

                // 2) Cập nhật trạng thái đơn hàng

                if (formulaExist.Status == ManufacturingProductOrder.New.ToString())
                {

                    var mfgProductionOrder = formulaExist.MfgProductionOrder;
                    Guid? merchandiseOrderId = mfgProductionOrder?.MerchandiseOrderId;

                    MerchandiseOrder? merchadiseExist = null;
                    if (merchandiseOrderId != null && merchandiseOrderId != Guid.Empty)
                    {
                        merchadiseExist = await _unitOfWork.MerchandiseOrderRepository.Query(track: true)
                            .FirstOrDefaultAsync(m => m.MerchandiseOrderId == merchandiseOrderId, cancellationToken ?? default);
                    }

                    if (merchadiseExist != null)
                    {
                        merchadiseExist.Status = MerchadiseStatus.Processing.ToString();
                    }
                }


                PatchHelper.SetIfRef(req.Note, () => formulaExist.Note, v => formulaExist.Note = v);


                PatchHelper.SetIfRef(req.SourceType, () => formulaExist.SourceType, v => formulaExist.SourceType = v);
                
                PatchHelper.SetIfRef(req.SourceManufacturingExternalIdSnapshot, () => formulaExist.SourceManufacturingExternalIdSnapshot, v => formulaExist.SourceManufacturingExternalIdSnapshot = v);                
                PatchHelper.SetIf(req.SourceManufacturingFormulaId, () => formulaExist.SourceManufacturingFormulaId.GetValueOrDefault(), v => formulaExist.SourceManufacturingFormulaId = v);
                
                PatchHelper.SetIfRef(req.SourceVUExternalIdSnapshot, () => formulaExist.SourceVUExternalIdSnapshot, v => formulaExist.SourceVUExternalIdSnapshot = v);                
                PatchHelper.SetIf(req.SourceVUFormulaId, () => formulaExist.SourceVUFormulaId.GetValueOrDefault(), v => formulaExist.SourceVUFormulaId = v);
                
                
                PatchHelper.SetIf(req.IsSelect, () => formulaExist.IsSelect, v => formulaExist.IsSelect = v);

                // 3) Trạng thái IsSelect
                if (req.IsSelect)
                {
                    // Nếu được chọn làm công thức chính cho lệnh sản xuất này → cập nhật trạng thái cho công thức
                    formulaExist.Status = ManufacturingProductOrderFormula.IsSelect.ToString();

                    // Đồng thời cập nhật trạng thái của Lệnh sản xuất là đã được chọn công thức
                    var mfgOrderEntity = await _unitOfWork.MfgProductionOrderRepository.Query(track: true)
                        .FirstOrDefaultAsync(o => o.MfgProductionOrderId == formulaExist.mfgProductionOrderId, cancellationToken ?? default);

                    if (mfgOrderEntity != null /*&& (mfgOrderEntity.Status == ManufacturingProductOrder.New.ToString() || mfgOrderEntity.Status == ManufacturingProductOrder.QCFail.ToString())*/)
                    {
                        mfgOrderEntity.Status = ManufacturingProductOrder.QCChecked.ToString();
                    }
                }
                else
                {
                    // Nếu không chọn thì để trạng thái đang kiểm tra
                    formulaExist.Status = ManufacturingProductOrderFormula.Checking.ToString();
                }


                PatchHelper.SetIf(req.IsActive, () => formulaExist.IsActive, v => formulaExist.IsActive = v);
                PatchHelper.SetIf(req.IsStandard, () => formulaExist.IsStandard, v => formulaExist.IsStandard = v);

                // 5) Nếu đặt làm Standard → log lại
                if (req.IsStandard)
                {
                    await _unitOfWork.ManufacturingFormulaLogRepository.AddAsync(new ManufacturingFormulaLog
                    {
                        ManufacturingFormulaId = formulaExist.ManufacturingFormulaId,
                        Action = ManufacturingFormulaLogAction.SetStandard,
                        Comment = req.noteWhyStandardChanged,
                        PerformedDate = now,
                        PerformedBy = req.UpdatedBy.GetValueOrDefault(),
                        PerformedByNameSnapshot = "Created by system (ID from Lab)"
                    }, cancellationToken ?? default);
                }



                // đảm bảo chỉ có 1 công thức IsSelect trong mỗi ProductionOrder
                if (req.IsSelect == true)
                {
                    var ct = cancellationToken ?? default;
                    var orderId = formulaExist.mfgProductionOrderId;

                    await _unitOfWork.ManufacturingFormulaRepository.Query(track: false)
                        .Where(f => f.mfgProductionOrderId == orderId
                                    && f.ManufacturingFormulaId != formulaExist.ManufacturingFormulaId
                                    && (f.IsActive == true))
                        .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsSelect, x => false), ct);

                    formulaExist.IsSelect = true;
                }

                // đảm bảo chỉ có 1 công thức IsStandard trong mỗi VUFormula
                if (req.IsStandard == true && formulaExist.VUFormulaId != Guid.Empty)
                {
                    var vuId = formulaExist.VUFormulaId;

                    await _unitOfWork.ManufacturingFormulaRepository.Query(track: false)
                        .Where(f => f.VUFormulaId == vuId
                                    && f.ManufacturingFormulaId != formulaExist.ManufacturingFormulaId
                                    && (f.IsActive == true))
                        .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsStandard, x => false), cancellationToken ?? default);

                    formulaExist.IsStandard = true;
                }




                // 8) Quản lý danh sách vật tư (ManufacturingFormulaMaterials)

                var existingMaterials = formulaExist.ManufacturingFormulaMaterials.ToDictionary(fm => fm.MaterialId, fm => fm);

                foreach (var m in req.ManufacturingFormulaMaterials)
                {
                    if (!existingMaterials.TryGetValue(m.MaterialId, out var link))
                    {
                        // New link
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

                        await _unitOfWork.ManufacturingFormulaMaterialRepository.AddAsync(link, cancellationToken ?? default);
                    }

                    else
                    {
                        // UPDATE: so sánh và gán khi khác
                        if (m.CategoryId != link.CategoryId) link.CategoryId = m.CategoryId;

                        if (m.Quantity != link.Quantity) link.Quantity = m.Quantity.GetValueOrDefault();
                        if (m.UnitPrice != link.UnitPrice) link.UnitPrice = m.UnitPrice.GetValueOrDefault();

                        // luôn đồng bộ TotalPrice theo BE để nhất quán
                        var newTotal = decimal.Round(link.Quantity * link.UnitPrice, 2, MidpointRounding.AwayFromZero);
                        if (newTotal != link.TotalPrice) link.TotalPrice = newTotal;

                        PatchHelper.SetIfRef(m.Unit, () => link.Unit, v => link.Unit = v);
                        PatchHelper.SetIfRef(m.MaterialNameSnapshot, () => link.MaterialNameSnapshot, v => link.MaterialNameSnapshot = v);
                        PatchHelper.SetIfRef(m.MaterialExternalIdSnapshot, () => link.MaterialExternalIdSnapshot, v => link.MaterialExternalIdSnapshot = v);

                        PatchHelper.SetIfNullable(m.StockId, () => link.StockId, v => link.StockId = v);
                        PatchHelper.SetIfRef(m.LotNo, () => link.LotNo, v => link.LotNo = v);

                        if (link.IsActive == false) link.IsActive = true; // RE-activate nếu trước đó bị xóa mềm
                    }
                }

                // SOFT-DELETE: những dòng đang active nhưng không còn trong payload
                var incomingIds = req.ManufacturingFormulaMaterials.Select(x => x.MaterialId).ToHashSet();
                foreach (var old in formulaExist.ManufacturingFormulaMaterials.Where(x => x.IsActive && !incomingIds.Contains(x.MaterialId)))
                {
                    old.IsActive = false;
                }

                // Cách an toàn nhất: bỏ qua req.TotalPrice, luôn tính theo dòng còn hiệu lực
                formulaExist.TotalPrice = formulaExist.ManufacturingFormulaMaterials
                    .Where(x => x.IsActive)              // nhớ lọc IsActive
                    .Sum(x => x.TotalPrice);


                formulaExist.UpdatedDate = now;
                formulaExist.UpdatedBy = req.UpdatedBy;


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
        /// Tạo mới công thức sản xuất
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<OperationResult> CreateAsync(PostMfgFormula req, CancellationToken ct = default)
        {
            if (req.mfgProductionOrderId == Guid.Empty) throw new ArgumentException("ProductId cannot be empty", nameof(req.mfgProductionOrderId));
            if (req.CreatedBy == Guid.Empty) throw new ArgumentException("CreatedBy  cannot be empty", nameof(req.CreatedBy));
            if (req.ManufacturingFormulaMaterials is null || !req.ManufacturingFormulaMaterials.Any())
                throw new ArgumentException("Công thức phải có ít nhất 1 vật tư.", nameof(req.ManufacturingFormulaMaterials));

            int affected = 0;
            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                Guid guid = req.mfgProductionOrderId;
                var now = DateTime.Now;

                // Case1: Kiểm tra sản phẩm có tồn tại không
                var exists = await _unitOfWork.MfgProductionOrderRepository.Query()
                    .AnyAsync(p => p.MfgProductionOrderId == req.mfgProductionOrderId, ct);

                if (!exists) throw new ArgumentException("Product does not exist", nameof(req.mfgProductionOrderId));

                // đảm bảo chỉ có 1 công thức IsSelect trong mỗi ProductionOrder
                // ✅ Reset cờ chọn cho TẤT CẢ công thức của đơn hàng này, nếu công thức mới sẽ được chọn
                    if (req.IsSelect == true)
                {
                    await _unitOfWork.ManufacturingFormulaRepository.Query(track: false)
                        .Where(f => f.mfgProductionOrderId == req.mfgProductionOrderId && f.IsActive == true && f.IsSelect)
                        .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsSelect, x => false), ct);


                    // Nếu được chọn làm công thức chính cho lệnh sản xuất này → cập nhật trạng thái cho công thức
                    req.Status = ManufacturingProductOrderFormula.IsSelect.ToString();

                    // Đồng thời cập nhật trạng thái của Lệnh sản xuất là đã được chọn công thức
                    var mfgOrderEntity = await _unitOfWork.MfgProductionOrderRepository.Query(track: true)
                        .FirstOrDefaultAsync(o => o.MfgProductionOrderId == req.mfgProductionOrderId, ct);

                    if (mfgOrderEntity != null && (mfgOrderEntity.Status == ManufacturingProductOrder.New.ToString() || mfgOrderEntity.Status == ManufacturingProductOrder.QCFail.ToString()))
                    {
                        mfgOrderEntity.Status = ManufacturingProductOrder.QCChecked.ToString();
                    }
                }


                // đảm bảo chỉ có 1 công thức IsStandard trong mỗi VUFormula
                if (req.IsStandard == true)
                {
                    var q = _unitOfWork.ManufacturingFormulaRepository.Query(track: false)
                        .Where(f => f.VUFormulaId == req.VUFormulaId && f.IsActive && f.IsStandard)
                        .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsStandard, x => false), ct);
                }


                // Tạo mới công thức
                var formula = _mapper.Map<ManufacturingFormula>(req);

                // Làm sạch để tránh lỗi null
                formula.ManufacturingFormulaMaterials = (formula.ManufacturingFormulaMaterials ?? new List<ManufacturingFormulaMaterial>())
                    .Where(x => x != null
                             && x.MaterialId != Guid.Empty
                             && x.Quantity > 0
                             && x.UnitPrice >= 0)
                    .ToList();

                //    - Tính TotalPrice từng dòng (round 2 số thập phân, có thể đổi theo rule công ty)
                foreach (var i in formula.ManufacturingFormulaMaterials)
                {
                    i.TotalPrice = Math.Round(i.Quantity * i.UnitPrice, 2, MidpointRounding.AwayFromZero);
                }

                //    - Tổng cộng công thức
                formula.TotalPrice = formula.ManufacturingFormulaMaterials.Sum(i => i.TotalPrice);


                formula.ExternalId = await _externalId.NextAsync(req.companyId.GetValueOrDefault(), "VA", now, ct: ct);


                formula.Name = await ExternalIdGenerator.GenerateFormulaCode(
                    "F",
                    prefix => _unitOfWork.ManufacturingFormulaRepository.GetLatestExternalIdStartsWithAsync(prefix, guid)
                );

                formula.mfgProductionOrderId = req.mfgProductionOrderId;
                formula.createdDate = now;

                formula.CreatedBy = req.CreatedBy;



                await _unitOfWork.ManufacturingFormulaRepository.AddAsync(formula, ct);
                affected = await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);
                return affected > 0
                    ? OperationResult.Ok("Tạo thành công")
                    : OperationResult.Fail("Tạo thất bại");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult.Fail(ex.Message);
            }
        }
    }
}
