using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Shared.Helper.PriceHelpers
{
    public sealed class MaterialsSupplierPriceProvider : IPriceProvider
    {
        private readonly IUnitOfWork _uow;
        public MaterialsSupplierPriceProvider(IUnitOfWork uow) => _uow = uow;

        public async Task<decimal> CalculatePriceAsync(Guid formulaId, FormulaSource source, CancellationToken ct = default)
        => source switch
        {
            FormulaSource.FromVA => await CalcFromVA(formulaId, ct),
            FormulaSource.FromVU => await CalcFromVU(formulaId, ct),
            _ => throw new ArgumentOutOfRangeException(nameof(source))
        };


        // ===================== VA (ManufacturingFormula) =====================
        private async Task<decimal> CalcFromVA(Guid mfId, CancellationToken ct)
        {
            // Lấy tenant & (option) SourceVU để fallback tìm Product
            var mf = await _uow.ManufacturingFormulaRepository.Query()
                .Where(m => m.ManufacturingFormulaId == mfId && m.IsActive)
                .Select(m => new { m.ManufacturingFormulaId, m.CompanyId, m.SourceVUFormulaId })
                .FirstOrDefaultAsync(ct);

            if (mf == null)
                throw new InvalidOperationException("Công thức VA không tồn tại hoặc đã bị vô hiệu.");

            // BOM VA
            var bom = await _uow.ManufacturingFormulaMaterialRepository.Query()
                .Where(x => x.ManufacturingFormulaId == mfId)
                .Select(x => new { x.MaterialId, x.Quantity })
                .ToListAsync(ct);

            if (bom.Count == 0)
                throw new InvalidOperationException("Công thức VA không có vật tư.");

            // Giá mới nhất theo MaterialId trong tenant
            var matIds = bom.Select(b => b.MaterialId).Distinct().ToList();

            var msRaw = await _uow.MaterialsSupplierRepository.Query()
                .Where(ms => matIds.Contains(ms.MaterialId))
                .Select(ms => new { ms.MaterialId, Stamp = (DateTime?)(ms.UpdatedDate ?? ms.CreateDate), ms.CurrentPrice })
                .ToListAsync(ct);

            var priceMap = msRaw
                .GroupBy(x => x.MaterialId)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(r => r.Stamp).First().CurrentPrice ?? 0m);

            var materialCost = bom.Sum(row => row.Quantity * (priceMap.TryGetValue(row.MaterialId, out var unit) ? unit : 0m));

            // Suy ra Product để áp phụ phí:
            // 1) Ưu tiên ProductStandardFormula còn hiệu lực
            var now = DateTime.Now;
            Guid? productId = await _uow.ProductStandardFormulaRepository.Query()
                .Where(psf => psf.CompanyId == mf.CompanyId
                           && psf.ManufacturingFormulaId == mfId
                           && psf.ValidFrom <= now
                           && (psf.ValidTo == null || psf.ValidTo >= now))
                .OrderByDescending(psf => psf.ValidFrom)
                .Select(psf => (Guid?)psf.ProductId)
                .FirstOrDefaultAsync(ct);

            // 2) Fallback: ProductionSelectVersion → MfgProductionOrder → ProductId
            if (productId == null)
            {
                productId = await _uow.ProductionSelectVersionRepository.Query()
                    .Where(v => v.ManufacturingFormulaId == mfId
                             && v.ValidFrom <= now
                             && (v.ValidTo == null || v.ValidTo >= now))
                    .OrderByDescending(v => v.ValidFrom)
                    .Join(_uow.MfgProductionOrderRepository.Query(),
                          v => v.MfgProductionOrderId,
                          o => o.MfgProductionOrderId,
                          (v, o) => (Guid?)o.ProductId)
                    .FirstOrDefaultAsync(ct);
            }

            // 3) (Tùy chọn) Nếu VA có SourceVU, thử lấy Product từ đơn hàng VU gần nhất
            if (productId == null && mf.SourceVUFormulaId != null)
            {
                productId = await _uow.MerchandiseOrderRepository.Query()
                    .Where(o => o.IsActive)
                    .SelectMany(o => o.MerchandiseOrderDetails, (o, d) => new { o, d })
                    .Where(x => x.d.FormulaId == mf.SourceVUFormulaId && x.d.IsActive)
                    .OrderByDescending(x => x.o.CreateDate)
                    .Select(x => (Guid?)x.d.ProductId)
                    .FirstOrDefaultAsync(ct);
            }

            var addOn = await ComputeAddOnAsync(productId, ct);
            return Math.Round(materialCost + addOn, 2, MidpointRounding.AwayFromZero);
        }

        // ===================== VU (Formula / SampleRequests) =====================
        private async Task<decimal> CalcFromVU(Guid vuId, CancellationToken ct)
        {
            // Lấy tenant từ Formula (nullable), nếu null sẽ cố suy ra từ đơn hàng gần nhất
            var vu = await _uow.FormulaRepository.Query()
                .Where(f => f.FormulaId == vuId && f.IsActive)
                .Select(f => new { f.FormulaId, f.CompanyId })
                .FirstOrDefaultAsync(ct);

            if (vu == null)
                throw new InvalidOperationException("Công thức VU không tồn tại hoặc đã bị vô hiệu.");

            // BOM VU
            var bom = await _uow.FormulaMaterialRepository.Query()
                .Where(x => x.FormulaId == vuId)
                .Select(x => new { x.MaterialId, x.Quantity })
                .ToListAsync(ct);

            if (bom.Count == 0)
                throw new InvalidOperationException("Công thức VU không có vật tư.");

            var matIds = bom.Select(b => b.MaterialId).Distinct().ToList();

            Guid? tenantId = vu.CompanyId;

            if (tenantId == null || tenantId == Guid.Empty)
            {
                // suy ra tenant từ đơn hàng gần nhất dùng VU này
                tenantId = await _uow.MerchandiseOrderRepository.Query()
                    .Where(o => o.IsActive)
                    .SelectMany(o => o.MerchandiseOrderDetails, (o, d) => new { o, d })
                    .Where(x => x.d.FormulaId == vuId && x.d.IsActive)
                    .OrderByDescending(x => x.o.CreateDate)
                    .Select(x => (Guid?)x.o.CompanyId)
                    .FirstOrDefaultAsync(ct);
            }

            // Lấy giá mới nhất theo tenant nếu có, nếu không có tenant thì đành không filter tenant
            IQueryable<dynamic> priceQ = _uow.MaterialsSupplierRepository.Query()
                .Where(ms => matIds.Contains(ms.MaterialId))
                .Select(ms => new { ms.MaterialId, Stamp = (DateTime?)(ms.UpdatedDate ?? ms.CreateDate), ms.CurrentPrice });


            var msRaw = await priceQ.ToListAsync(ct);

            var priceMap = msRaw
                .GroupBy(x => (Guid)x.MaterialId)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(r => (DateTime?)r.Stamp).First().CurrentPrice ?? 0m);

            var materialCost = bom.Sum(row => row.Quantity * (priceMap.TryGetValue(row.MaterialId, out var unit) ? unit : 0m));

            // Suy product từ đơn hàng gần nhất dùng VU này → áp phụ phí
            Guid? productId = await _uow.MerchandiseOrderRepository.Query()
                .Where(o => o.IsActive)
                .SelectMany(o => o.MerchandiseOrderDetails, (o, d) => new { o, d })
                .Where(x => x.d.FormulaId == vuId && x.d.IsActive)
                .OrderByDescending(x => x.o.CreateDate)
                .Select(x => (Guid?)x.d.ProductId)
                .FirstOrDefaultAsync(ct);

            var addOn = await ComputeAddOnAsync(productId, ct);
            return Math.Round(materialCost + addOn, 2, MidpointRounding.AwayFromZero);
        }

        // ===================== Add-on theo Product =====================
        private async Task<decimal> ComputeAddOnAsync(Guid? productId, CancellationToken ct)
        {
            if (productId == null) return 0m;

            var prod = await _uow.ProductRepository.Query()
                .Where(p => p.ProductId == productId && p.IsActive)
                .Select(p => new { p.IsRecycle, p.ColourCode })
                .FirstOrDefaultAsync(ct);

            if (prod == null) return 0m;

            if (prod.IsRecycle) return 5_000m;
            if (!string.IsNullOrWhiteSpace(prod.ColourCode)
                && prod.ColourCode.EndsWith("C", StringComparison.OrdinalIgnoreCase))
                return 10_000m;

            return 20_000m;
        }

        public async Task<Dictionary<Guid, decimal>> GetLatestUnitPricesAsync(
            IEnumerable<Guid> materialIds,
            CancellationToken ct = default)
        {
            var ids = materialIds?.Distinct().ToList() ?? new();
            if (ids.Count == 0) return new();

            var raw = await _uow.MaterialsSupplierRepository.Query()
                .Where(s => ids.Contains(s.MaterialId) && (s.IsActive ?? true))
                .Select(s => new
                {
                    s.MaterialId,
                    s.CurrentPrice,
                    s.IsPreferred,
                    Stamp = (DateTime?)(s.UpdatedDate ?? s.CreateDate)
                })
                .ToListAsync(ct);

            return raw
                .GroupBy(x => x.MaterialId)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .OrderByDescending(x => x.Stamp)               // ngày mới nhất
                        .ThenByDescending(x => x.IsPreferred ?? false) // ưu tiên preferred nếu cùng ngày
                        .Select(x => x.CurrentPrice ?? 0m)
                        .FirstOrDefault()
                );
        }

        public async Task<decimal?> GetTargetPriceByMpoAsync(
         Guid mfgProductionOrderId,
         Guid productId,
         Guid companyId,
         CancellationToken ct = default)
        {
            // từ MPO -> MfgOrderPO -> MerchandiseOrderDetail -> MerchandiseOrder
            var price = await _uow.MfgOrderPORepository.Query()
                .Where(x => x.MfgProductionOrderId == mfgProductionOrderId && x.IsActive)
                .Select(x => x.Detail)
                .Where(d =>
                    d.ProductId == productId &&
                    d.IsActive &&
                    d.MerchandiseOrder.IsActive &&
                    d.MerchandiseOrder.CompanyId == companyId
                )
                .OrderByDescending(d => d.MerchandiseOrder.CreateDate)
                .Select(d => (decimal?)d.UnitPriceAgreed) // GIÁ BÁN CHỐT
                .FirstOrDefaultAsync(ct);

            return price; // có thể null nếu chưa có đơn sale liên kết
        }
    }

}
