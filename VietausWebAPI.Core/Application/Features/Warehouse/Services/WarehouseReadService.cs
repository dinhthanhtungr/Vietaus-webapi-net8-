using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.Material_warehouse;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Services
{
    public class WarehouseReadService : IWarehouseReadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WarehouseReadService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        // ======================================================================== Get ======================================================================== 

        public async Task<List<ProductAvailabilityVm>> GetProductAvailabilityVmsAsync(List<string> productExternalIds, CancellationToken ct = default)
        {
            //if (productExternalIds == null || productExternalIds.Count == 0)
            //    return new();

            //var ids = productExternalIds
            //    .Where(s => !string.IsNullOrWhiteSpace(s))
            //    .Select(s => s.Trim())
            //    .Distinct()
            //    .ToList();

            //var list = await _unitOfWork.WarehouseShelfStockRepository.Query()
            //    .Where(s => !string.IsNullOrEmpty(s.Code) && ids.Contains(s.Code))
            //    .GroupBy(s => new { s.Code, s.BatchNo , s.QtyKg})
            //    .Select(g => new ProductAvailabilityVm
            //    {
            //        ProductExternalId = g.Key.Code,
            //        LotNumber = g.Key.BatchNo ?? string.Empty,
            //        AvailableQuantity = g.QtyKg ?? 0m
            //    })
            //    .ToListAsync(ct);

            //return list;


            throw new Exception("Lỗi khi lấy danh sách người giao hàng.");
        }

        public async Task<OperationResult<PagedResult<GetStockAvaiable>>> GetStockAvailableAsync(WarehouseReadServiceQuery query)
        {
            try
            {
                var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
                var pageSize = query.PageSize <= 0 ? 15 : query.PageSize;

                // 1) Base query: ShelfStock
                var shelfQuery = _unitOfWork.WarehouseShelfStockRepository.Query()
                    .Where(s => s.Code != null && s.Code != "");

                if (!string.IsNullOrWhiteSpace(query.KeyWord))
                {
                    var kw = query.KeyWord.Trim();
                    shelfQuery = shelfQuery.Where(x =>
                        (x.Code ?? "").Contains(kw)
                    );
                }

                if (query.StockTypes.HasValue)
                {
                    shelfQuery = shelfQuery.Where(x => x.StockType == query.StockTypes.Value);
                }


                // 2) Group OnHand theo Code + StockType (ra DTO trước)
                var onHandGrouped =
                    from s in shelfQuery
                    group s by new { s.Code, s.StockType } into g
                    select new GetStockAvaiable
                    {
                        ShelfStockId = g.Min(x => x.ShelfStockId),
                        Code = g.Key.Code,
                        StockType = g.Key.StockType,

                        OnHandKg = g.Sum(x => (decimal?)x.QtyKg) ?? 0m,
                        ReservedOpenAllKg = 0m, // sẽ fill sau
                        AvailableKg = 0m        // sẽ fill sau
                    };

                // 3) Paging (lấy page items trước)
                var totalCount = await onHandGrouped.CountAsync();

                var items = await onHandGrouped
                    .OrderBy(x => x.Code)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Nếu page rỗng thì khỏi query tempstock
                if (items.Count == 0)
                {
                    var empty = new PagedResult<GetStockAvaiable>(items, totalCount, pageNumber, pageSize);
                    return OperationResult<PagedResult<GetStockAvaiable>>.Ok(empty);
                }


                var materialCodes = items
                .Where(x => x.StockType == StockType.RawMaterial
                         || x.StockType == StockType.DefectiveRawMaterial)
                .Select(x => x.Code)
                .Distinct()
                .ToList();

                var productCodes = items
                    .Where(x => x.StockType == StockType.FinishedGood
                                || x.StockType == StockType.DefectiveFinishedGood)
                    .Select(x => x.Code)
                    .Distinct()
                    .ToList();


                var materialMap = await _unitOfWork.MaterialRepository.Query()
                    .Where(m => m.ExternalId != null && materialCodes.Contains(m.ExternalId))
                    .Select(m => new
                    {
                        Code = m.ExternalId!,
                        Name = m.Name,
                        CategoryName = m.Category != null ? m.Category.Name : ""
                    })
                    .ToDictionaryAsync(x => x.Code);   

                var productMap = await _unitOfWork.ProductRepository.Query()
                    .Where(p => p.Code != null && productCodes.Contains(p.Code))
                    .Select(p => new
                    {
                        Code = p.Code!,
                        Name = p.Name,
                        CategoryName = p.Category != null ? p.Category.Name : ""
                    })
                    .ToDictionaryAsync(x => x.Code);  



                // 4) Lấy danh sách Code đang có trong page để query TempStock gọn
                var codes = items.Select(x => x.Code).Distinct().ToList();

                // 5) Reserved OPEN từ TempStock (group theo Code)
                var reservedMap = await _unitOfWork.WarehouseTempStockRepository.Query()
                    .Where(t => codes.Contains(t.Code)
                             && t.ReserveStatus == ReserveStatus.Open.ToString())
                    .GroupBy(t => t.Code)
                    .Select(g => new
                    {
                        Code = g.Key,
                        ReservedOpenKg = g.Sum(x => (decimal?)x.QtyRequest) ?? 0m
                        // Nếu muốn trừ QtyUsed:
                        // ReservedOpenKg = g.Sum(x => (decimal?)((x.QtyRequest ?? 0m) - (x.QtyUsed ?? 0m))) ?? 0m
                    })
                    .ToDictionaryAsync(x => x.Code, x => x.ReservedOpenKg);

                // 6) Merge vào items
                foreach (var it in items)
                {
                    // HÀNG LỖI: không cho giữ, không cho dùng
                    if (it.StockType == StockType.DefectiveRawMaterial
                     || it.StockType == StockType.DefectiveFinishedGood)
                    {
                        it.ReservedOpenAllKg = 0m;
                        it.AvailableKg = 0m;
                    }
                    else
                    {
                        // HÀNG BÌNH THƯỜNG
                        var reserved = reservedMap.TryGetValue(it.Code, out var rv) ? rv : 0m;
                        it.ReservedOpenAllKg = reserved;
                        it.AvailableKg = it.OnHandKg - reserved;
                    }
                    // NVL
                    if ((it.StockType == StockType.RawMaterial
                      || it.StockType == StockType.DefectiveRawMaterial)
                      && materialMap.TryGetValue(it.Code, out var m))
                    {
                        it.CodeName = m.Name ?? string.Empty;
                        it.CategoryName = m.CategoryName ?? string.Empty;
                    }

                    // Thành phẩm
                    else if ((it.StockType == StockType.FinishedGood
                           || it.StockType == StockType.DefectiveFinishedGood)
                           && productMap.TryGetValue(it.Code, out var p))
                    {
                        it.CodeName = p.Name ?? string.Empty;
                        it.CategoryName = p.CategoryName ?? string.Empty;
                    }
                }

                // 7) Filter tồn kho khả dụng <= 0 (nếu yêu cầu)
                if (query.OnlyAvailableLeZero)
                {
                    items = items
                        .Where(x => x.AvailableKg <= 0m)
                        .ToList();

                    totalCount = items.Count;
                }

                var result = new PagedResult<GetStockAvaiable>(items, totalCount, pageNumber, pageSize);
                return OperationResult<PagedResult<GetStockAvaiable>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<GetStockAvaiable>>.Fail(ex.Message);
            }
        }






        //public async Task<decimal> GetAvailableAsync(WarehouseReadServiceQuery query, CancellationToken cancellationToken)
        //{
        //    var onHand = await GetOnHandAsync(query, cancellationToken);
        //    var reserved = await GetReservedOpenAsync(query, cancellationToken);
        //    return onHand - reserved;
        //}

        //public async Task<decimal> GetOnHandAsync(WarehouseReadServiceQuery query, CancellationToken cancellationToken)
        //{
        //    var q = _unitOfWork.WarehouseShelfStockRepository.Query()
        //        .Where(x => x.CompanyId == query.companyId && x.Code == query.code);

        //    if (!string.IsNullOrEmpty(query.lotkey))
        //    {
        //        q = q.Where(x => x.LotKey == query.lotkey);
        //    }

        //    return await q.SumAsync(x => (decimal?)x.QtyKg, cancellationToken) ?? 0m;
        //}

        //public async Task<decimal> GetReservedOpenAsync(WarehouseReadServiceQuery query, CancellationToken cancellationToken)
        //{
        //    var q = _unitOfWork.WarehouseTempStockRepository.Query() 
        //        .Where(x => x.CompanyId == query.companyId && x.Code == query.code
        //    && x.ReserveStatus == Domain.Enums.ReserveStatus.Open);

        //    if (!string.IsNullOrEmpty(query.lotkey))
        //    {
        //        q = q.Where(x => x.LotKey == query.lotkey);
        //    }

        //    return await q.SumAsync(x => (decimal?)x.QtyRequest, cancellationToken) ?? 0m;

        //}

        public async Task<List<VaAvailabilityVm>> GetVaAvailabilityAsync(
            Guid manufacturingFormulaId,
            CancellationToken ct = default)
        {
            // Lấy danh sách NVL trong công thức (có thể Trim/ToUpper ở C# nếu muốn),
            // NHƯNG ở SQL ta sẽ so sánh trực tiếp với cột citext, KHÔNG bọc hàm cho cột.
            var mfm = _unitOfWork.ManufacturingFormulaMaterialRepository.Query()
                .Where(x => x.ManufacturingFormulaId == manufacturingFormulaId
                         && x.IsActive
                         && x.MaterialExternalIdSnapshot != null);


            // Lấy danh sách nguyên vật liệu và lotKey từ công thức sản xuất
            var materials = await mfm
                .Select(x => x.MaterialExternalIdSnapshot!)
                .Distinct()
                .ToListAsync(ct);

            if (materials.Count == 0) return new List<VaAvailabilityVm>();


            // Chuẩn hóa danh sách materials ở dạng UPPER + TRIM
            var mats = materials.Select(m => m.Trim().ToUpperInvariant()).ToList();


            var onHand = await _unitOfWork.WarehouseShelfStockRepository.Query()
                .Where(s => s.Code != null && s.Code != "")
                .Where(s => mats.Contains(s.Code))
                .GroupBy(s => s.Code)
                .Select(g => new { Code = g.Key, OnHandKg = g.Sum(x => (decimal?)x.QtyKg) ?? 0m })
                .ToDictionaryAsync(x => x.Code, x => x.OnHandKg, ct);

            //var onHandMap = onHand.ToDictionary(x => x.Code, x => x.OnHandKg);

            // Reserved OPEN (toàn hệ thống; nếu chỉ muốn của VA này thì filter theo VaCode/SnapshotSetId)
            var reserved = await _unitOfWork.WarehouseTempStockRepository.Query()
                .Where(t => materials.Contains(t.Code)
                         //&& t.TempType == TempType.Reserve
                         && t.ReserveStatus == ReserveStatus.Open.ToString())
                .GroupBy(t => t.Code)
                .Select(g => new { Code = g.Key, ReservedOpenKg = g.Sum(x => (decimal?)x.QtyRequest) ?? 0m })
                .ToDictionaryAsync(x => x.Code, x => x.ReservedOpenKg, ct);

            //var reservedMap = reserved.ToDictionary(x => x.Code, x => x.ReservedOpenKg);

            // Ghép vào VM (lấy luôn tên NVL + định mức của công thức)
            var rows = await mfm
                .Select(x => new {
                    Code = x.MaterialExternalIdSnapshot!,
                    Name = x.MaterialNameSnapshot,
                    FormulaQty = (decimal?)x.Quantity ?? 0m
                })
                .Distinct()
                .ToListAsync(ct);

            //var ReservedCodeList = string.Join(", ",
            //    await reserved.Select(x => x.MaterialExternalIdSnapshot).Distinct().ToListAsync());


            return rows
                .OrderBy(r => r.Code)
                .Select(r =>
                {
                    var on = onHand.TryGetValue(r.Code, out var v1) ? v1 : 0m;
                    var rv = reserved.TryGetValue(r.Code, out var v2) ? v2 : 0m;
                    return new VaAvailabilityVm(
                        Code: r.Code,
                        //SnapshotQty: r.FormulaQty,
                        OnHandKg: on,
                        ReservedOpenAllKg: rv,
                        AvailableKg: on - rv
                    );
                })
                .ToList();

        }

        // ======================================================================== Helper ======================================================================== 

        /// <summary>
        /// Lấy dictionary thông tin tồn kho VA của các NVL trong công thức sản xuất
        /// </summary>
        /// <param name="manufacturingFormulaId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, VaAvailabilityVm>> GetVaAvailabilityDictAsync(
            Guid manufacturingFormulaId,
            CancellationToken ct = default)
        {
                    var mfm = _unitOfWork.ManufacturingFormulaMaterialRepository.Query()
                        .Where(x => x.ManufacturingFormulaId == manufacturingFormulaId
                                 && x.IsActive
                                 && x.MaterialExternalIdSnapshot != null);

                    var materials = await mfm
                        .Select(x => x.MaterialExternalIdSnapshot!)
                        .Distinct()
                        .ToListAsync(ct);

                    if (materials.Count == 0)
                        return new Dictionary<string, VaAvailabilityVm>();

                    // chuẩn hóa
                    var mats = materials
                        .Select(m => m.Trim().ToUpperInvariant())
                        .ToList();

                    var onHand = await _unitOfWork.WarehouseShelfStockRepository.Query()
                        .Where(s => s.Code != null && s.Code != "")
                        .Where(s => mats.Contains(s.Code!.Trim().ToUpper()))
                        .GroupBy(s => s.Code!.Trim().ToUpper())
                        .Select(g => new { Code = g.Key, OnHandKg = g.Sum(x => (decimal?)x.QtyKg) ?? 0m })
                        .ToDictionaryAsync(x => x.Code, x => x.OnHandKg, ct);

                    var reserved = await _unitOfWork.WarehouseTempStockRepository.Query()
                        .Where(t => t.Code != null && t.Code != ""
                                 && mats.Contains(t.Code!.Trim().ToUpper())
                                 && t.ReserveStatus == ReserveStatus.Open.ToString())
                        .GroupBy(t => t.Code!.Trim().ToUpper())
                        .Select(g => new { Code = g.Key, ReservedOpenKg = g.Sum(x => (decimal?)x.QtyRequest) ?? 0m })
                        .ToDictionaryAsync(x => x.Code, x => x.ReservedOpenKg, ct);

                    // ghép dict
                    var dict = new Dictionary<string, VaAvailabilityVm>();

                    foreach (var codeUpper in mats)
                    {
                        var on = onHand.TryGetValue(codeUpper, out var v1) ? v1 : 0m;
                        var rv = reserved.TryGetValue(codeUpper, out var v2) ? v2 : 0m;

                        dict[codeUpper] = new VaAvailabilityVm(
                            Code: codeUpper,
                            OnHandKg: on,
                            ReservedOpenAllKg: rv,
                            AvailableKg: on - rv
                        );
                    }

                    return dict;
                }

    }
}
