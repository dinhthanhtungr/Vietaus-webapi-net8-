using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Shared.DTO.PriceDTOs;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums;

namespace VietausWebAPI.Core.Application.Features.Shared.Service.StaticCurrentPriceHelpers
{
    /// <summary>
    /// Helper dùng để truy vấn "giá vật tư hiệu lực" theo quy tắc:
    /// ưu tiên lấy từ Purchase Order gần nhất, nếu không có giá hợp lệ thì fallback sang MaterialsSupplier.
    /// </summary>
    public static class MaterialPriceQueryHelper
    {
        /// <summary>
        /// Tải dictionary thông tin giá vật tư theo danh sách MaterialId.
        /// </summary>
        /// <remarks>
        /// Rule lấy giá:
        /// 1. Ưu tiên lấy giá từ PurchaseOrderDetail gần nhất theo MaterialId.
        /// 2. Nếu PO không có, hoặc giá PO null/0, thì fallback sang MaterialsSupplier.
        /// 3. Nếu cả 2 nguồn đều không có giá hợp lệ, trả về giá = 0 và nguồn = Unknown.
        ///
        /// Hàm này dùng khi không cần ràng buộc theo Supplier cụ thể.
        /// </remarks>
        /// <param name="purchaseOrderSource">
        /// IQueryable nguồn từ bảng PurchaseOrderDetail.
        /// Nên truyền từ repository query, ví dụ: _unitOfWork.PurchaseOrderDetailRepository.Query()
        /// </param>
        /// <param name="materialSupplierSource">
        /// IQueryable nguồn từ bảng MaterialsSupplier.
        /// Nên truyền từ repository query, ví dụ: _unitOfWork.MaterialsSupplierRepository.Query()
        /// </param>
        /// <param name="materialIds">
        /// Danh sách MaterialId cần lấy giá.
        /// Có thể truyền IEnumerable&lt;Guid?&gt;; helper sẽ tự loại null, Guid.Empty và trùng lặp.
        /// </param>
        /// <param name="ct">CancellationToken.</param>
        /// <returns>
        /// Dictionary với key là MaterialId, value là <see cref="LatestMaterialPriceDto"/>
        /// chứa giá, ngày giá và nguồn giá.
        /// </returns>
        public static async Task<Dictionary<Guid, LatestMaterialPriceDto>> LoadLatestMaterialPriceInfoDictAsync(IQueryable<PurchaseOrderDetail> purchaseOrderSource, IQueryable<MaterialsSupplier> materialSupplierSource,
            IEnumerable<Guid?> materialIds,
            CancellationToken ct = default)
        {
            var ids = NormalizeIds(materialIds);

            if (ids.Count == 0)
                return new Dictionary<Guid, LatestMaterialPriceDto>();

            // 1) Giá gần nhất từ PO
            var latestPoRows = await purchaseOrderSource
                .AsNoTracking()
                .Where(x =>
                    ids.Contains(x.MaterialId) &&
                    x.PurchaseOrder != null &&
                    (x.PurchaseOrder.IsActive ?? true))
                .GroupBy(x => x.MaterialId)
                .Select(g => g
                    .OrderByDescending(x => x.PurchaseOrder.CreateDate)
                    .ThenByDescending(x => x.LineNo)
                    .Select(x => new
                    {
                        x.MaterialId,
                        Price = x.UnitPriceAgreed,
                        PriceDate = (DateTime?)x.PurchaseOrder.CreateDate
                    })
                    .FirstOrDefault()!)
                .ToListAsync(ct);

            var poDict = latestPoRows.ToDictionary(
                x => x.MaterialId,
                x => new LatestMaterialPriceDto
                {
                    MaterialId = x.MaterialId,
                    CurrentPrice = x.Price ?? 0m,
                    PriceDate = x.PriceDate,
                    PriceSource = MaterialPriceSource.PurchaseOrder
                });

            // 2) Giá fallback từ MaterialSupplier
            var supplierRows = await materialSupplierSource
                .AsNoTracking()
                .Where(s =>
                    (s.IsActive ?? true) &&
                    ids.Contains(s.MaterialId))
                .GroupBy(s => s.MaterialId)
                .Select(g => g
                    .OrderByDescending(x => x.UpdatedDate ?? x.CreateDate)
                    .ThenByDescending(x => x.IsPreferred ?? false)
                    .Select(x => new
                    {
                        x.MaterialId,
                        CurrentPrice = x.CurrentPrice ?? 0m,
                        PriceDate = x.UpdatedDate ?? x.CreateDate
                    })
                    .FirstOrDefault()!)
                .ToListAsync(ct);

            var supplierDict = supplierRows.ToDictionary(
                x => x.MaterialId,
                x => new LatestMaterialPriceDto
                {
                    MaterialId = x.MaterialId,
                    CurrentPrice = x.CurrentPrice,
                    PriceDate = x.PriceDate,
                    PriceSource = MaterialPriceSource.MaterialSupplier
                });

            return MergePriceInfo(ids, poDict, supplierDict);
        }

        /// <summary>
        /// Tải dictionary thông tin giá vật tư theo danh sách MaterialId, có ràng buộc theo SupplierId.
        /// </summary>
        /// <remarks>
        /// Rule lấy giá:
        /// 1. Ưu tiên lấy giá từ PurchaseOrderDetail gần nhất của đúng supplier.
        /// 2. Nếu PO của supplier đó không có giá hợp lệ, fallback sang MaterialsSupplier của đúng supplier.
        /// 3. Nếu cả 2 nguồn đều không có giá hợp lệ, trả về giá = 0 và nguồn = Unknown.
        ///
        /// Hàm này phù hợp khi cùng một vật tư có thể có giá khác nhau theo từng nhà cung cấp.
        /// </remarks>
        /// <param name="purchaseOrderSource">
        /// IQueryable nguồn từ bảng PurchaseOrderDetail.
        /// </param>
        /// <param name="materialSupplierSource">
        /// IQueryable nguồn từ bảng MaterialsSupplier.
        /// </param>
        /// <param name="supplierId">
        /// Id nhà cung cấp cần lọc.
        /// Nếu Guid.Empty thì hàm trả về dictionary rỗng.
        /// </param>
        /// <param name="materialIds">
        /// Danh sách MaterialId cần lấy giá.
        /// </param>
        /// <param name="ct">CancellationToken.</param>
        /// <returns>
        /// Dictionary với key là MaterialId, value là <see cref="LatestMaterialPriceDto"/>.
        /// </returns>
        public static async Task<Dictionary<Guid, LatestMaterialPriceDto>> LoadLatestMaterialPriceInfoBySupplierDictAsync(
            IQueryable<PurchaseOrderDetail> purchaseOrderSource,
            IQueryable<MaterialsSupplier> materialSupplierSource,
            Guid supplierId,
            IEnumerable<Guid?> materialIds,
            CancellationToken ct = default)
        {
            var ids = NormalizeIds(materialIds);

            if (ids.Count == 0 || supplierId == Guid.Empty)
                return new Dictionary<Guid, LatestMaterialPriceDto>();

            // 1) Giá gần nhất từ PO của đúng supplier
            var latestPoRows = await purchaseOrderSource
                .AsNoTracking()
                .Where(x =>
                    ids.Contains(x.MaterialId) &&
                    x.PurchaseOrder != null &&
                    x.PurchaseOrder.SupplierId == supplierId &&
                    (x.PurchaseOrder.IsActive ?? true))
                .GroupBy(x => x.MaterialId)
                .Select(g => g
                    .OrderByDescending(x => x.PurchaseOrder.CreateDate)
                    .ThenByDescending(x => x.LineNo)
                    .Select(x => new
                    {
                        x.MaterialId,
                        Price = x.UnitPriceAgreed,
                        PriceDate = (DateTime?)x.PurchaseOrder.CreateDate
                    })
                    .FirstOrDefault()!)
                .ToListAsync(ct);

            var poDict = latestPoRows.ToDictionary(
                x => x.MaterialId,
                x => new LatestMaterialPriceDto
                {
                    MaterialId = x.MaterialId,
                    CurrentPrice = x.Price ?? 0m,
                    PriceDate = x.PriceDate,
                    PriceSource = MaterialPriceSource.PurchaseOrder
                });

            // 2) Giá fallback từ MaterialSupplier của đúng supplier
            var supplierRows = await materialSupplierSource
                .AsNoTracking()
                .Where(s =>
                    (s.IsActive ?? true) &&
                    s.SupplierId == supplierId &&
                    ids.Contains(s.MaterialId))
                .GroupBy(s => s.MaterialId)
                .Select(g => g
                    .OrderByDescending(x => x.UpdatedDate ?? x.CreateDate)
                    .ThenByDescending(x => x.IsPreferred ?? false)
                    .Select(x => new
                    {
                        x.MaterialId,
                        CurrentPrice = x.CurrentPrice ?? 0m,
                        PriceDate = x.UpdatedDate ?? x.CreateDate
                    })
                    .FirstOrDefault()!)
                .ToListAsync(ct);

            var supplierDict = supplierRows.ToDictionary(
                x => x.MaterialId,
                x => new LatestMaterialPriceDto
                {
                    MaterialId = x.MaterialId,
                    CurrentPrice = x.CurrentPrice,
                    PriceDate = x.PriceDate,
                    PriceSource = MaterialPriceSource.MaterialSupplier
                });

            return MergePriceInfo(ids, poDict, supplierDict);
        }

        /// <summary>
        /// Lấy ra giá hiện tại từ dictionary giá đã load trước đó.
        /// </summary>
        /// <param name="priceDict">
        /// Dictionary giá đã được tạo từ helper.
        /// </param>
        /// <param name="materialId">
        /// MaterialId cần tra cứu.
        /// </param>
        /// <param name="fallback">
        /// Giá mặc định nếu không tìm thấy MaterialId trong dictionary.
        /// Mặc định là 0.
        /// </param>
        /// <returns>Giá vật tư nếu tìm thấy, ngược lại trả về fallback.</returns>
        public static decimal ResolveLatestPrice(
            IReadOnlyDictionary<Guid, LatestMaterialPriceDto> priceDict,
            Guid? materialId,
            decimal? fallback = 0m)
        {
            if (materialId.HasValue &&
                materialId.Value != Guid.Empty &&
                priceDict.TryGetValue(materialId.Value, out var info))
            {
                return info.CurrentPrice;
            }

            return fallback ?? 0m;
        }

        /// <summary>
        /// Lấy ra ngày của giá hiện tại từ dictionary giá đã load trước đó.
        /// </summary>
        /// <param name="priceDict">Dictionary giá đã được tạo từ helper.</param>
        /// <param name="materialId">MaterialId cần tra cứu.</param>
        /// <returns>
        /// Ngày giá nếu tìm thấy; null nếu không có dữ liệu.
        /// </returns>
        public static DateTime? ResolveLatestPriceDate(
            IReadOnlyDictionary<Guid, LatestMaterialPriceDto> priceDict,
            Guid? materialId)
        {
            if (materialId.HasValue &&
                materialId.Value != Guid.Empty &&
                priceDict.TryGetValue(materialId.Value, out var info))
            {
                return info.PriceDate;
            }

            return null;
        }

        /// <summary>
        /// Lấy ra nguồn giá hiện tại từ dictionary giá đã load trước đó.
        /// </summary>
        /// <param name="priceDict">Dictionary giá đã được tạo từ helper.</param>
        /// <param name="materialId">MaterialId cần tra cứu.</param>
        /// <returns>
        /// Nguồn giá nếu tìm thấy; <see cref="MaterialPriceSource.Unknown"/> nếu không có dữ liệu.
        /// </returns>
        public static MaterialPriceSource ResolveLatestPriceSource(
            IReadOnlyDictionary<Guid, LatestMaterialPriceDto> priceDict,
            Guid? materialId)
        {
            if (materialId.HasValue &&
                materialId.Value != Guid.Empty &&
                priceDict.TryGetValue(materialId.Value, out var info))
            {
                return info.PriceSource;
            }

            return MaterialPriceSource.Unknown;
        }

        /// <summary>
        /// Tạo query IQueryable lấy giá mới nhất từ riêng bảng MaterialsSupplier.
        /// </summary>
        /// <remarks>
        /// Hàm này không đọc dữ liệu từ PurchaseOrder.
        /// Chỉ dùng khi bạn muốn lấy giá hiện hành đang lưu trong MaterialsSupplier.
        /// </remarks>
        /// <param name="source">
        /// IQueryable nguồn từ bảng MaterialsSupplier.
        /// </param>
        /// <returns>
        /// IQueryable trả về mỗi MaterialId một dòng giá mới nhất trong MaterialsSupplier.
        /// </returns>
        public static IQueryable<LatestMaterialPriceDto> BuildLatestMaterialSupplierPriceQuery(
            IQueryable<MaterialsSupplier> source)
        {
            return source
                .AsNoTracking()
                .Where(s => s.IsActive ?? true)
                .GroupBy(s => s.MaterialId)
                .Select(g => g
                    .OrderByDescending(x => x.UpdatedDate ?? x.CreateDate)
                    .ThenByDescending(x => x.IsPreferred ?? false)
                    .Select(x => new LatestMaterialPriceDto
                    {
                        MaterialId = x.MaterialId,
                        CurrentPrice = x.CurrentPrice ?? 0m,
                        PriceDate = x.UpdatedDate ?? x.CreateDate,
                        PriceSource = MaterialPriceSource.MaterialSupplier
                    })
                    .FirstOrDefault()!);
        }

        /// <summary>
        /// Chuẩn hóa danh sách MaterialId đầu vào:
        /// loại null, loại Guid.Empty và loại bỏ phần tử trùng.
        /// </summary>
        /// <param name="materialIds">Danh sách MaterialId dạng nullable.</param>
        /// <returns>Danh sách Guid hợp lệ, distinct.</returns>
        private static List<Guid> NormalizeIds(IEnumerable<Guid?> materialIds)
        {
            return materialIds
                .Where(x => x.HasValue && x.Value != Guid.Empty)
                .Select(x => x!.Value)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Gộp kết quả từ 2 nguồn giá:
        /// PurchaseOrder và MaterialsSupplier.
        /// </summary>
        /// <remarks>
        /// Rule merge:
        /// 1. Nếu giá từ PO > 0 thì ưu tiên dùng PO.
        /// 2. Nếu giá PO không hợp lệ, nhưng giá từ MaterialsSupplier > 0 thì dùng MaterialsSupplier.
        /// 3. Nếu cả 2 nguồn đều không có giá hợp lệ thì trả về giá 0, nguồn Unknown.
        /// </remarks>
        /// <param name="ids">Danh sách MaterialId cần map kết quả.</param>
        /// <param name="poDict">Dictionary giá lấy từ PurchaseOrder.</param>
        /// <param name="supplierDict">Dictionary giá lấy từ MaterialsSupplier.</param>
        /// <returns>Dictionary kết quả cuối cùng sau khi merge.</returns>
        private static Dictionary<Guid, LatestMaterialPriceDto> MergePriceInfo(
            IReadOnlyCollection<Guid> ids,
            IReadOnlyDictionary<Guid, LatestMaterialPriceDto> poDict,
            IReadOnlyDictionary<Guid, LatestMaterialPriceDto> supplierDict)
        {
            var result = new Dictionary<Guid, LatestMaterialPriceDto>(ids.Count);

            foreach (var id in ids)
            {
                if (poDict.TryGetValue(id, out var poInfo) && poInfo.CurrentPrice > 0)
                {
                    result[id] = poInfo;
                    continue;
                }

                if (supplierDict.TryGetValue(id, out var supplierInfo) && supplierInfo.CurrentPrice > 0)
                {
                    result[id] = supplierInfo;
                    continue;
                }

                result[id] = new LatestMaterialPriceDto
                {
                    MaterialId = id,
                    CurrentPrice = 0m,
                    PriceDate = null,
                    PriceSource = MaterialPriceSource.Unknown
                };
            }

            return result;
        }
    }
}