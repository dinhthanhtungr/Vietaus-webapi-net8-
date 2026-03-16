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


            throw new Exception("Lỗi khi lấy danh sách người giao hàng.");
        }

        public async Task<OperationResult<PagedResult<GetStockAvaiable>>> GetStockAvailableAsync(WarehouseReadServiceQuery query)
        {
            try
            {
                var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
                var pageSize = query.PageSize <= 0 ? 15 : query.PageSize;

                var shelfQuery = _unitOfWork.WarehouseShelfStockRepository.Query()
                    .Where(s => s.Code != null && s.Code != "");

                if (!string.IsNullOrWhiteSpace(query.KeyWord))
                {
                    var kw = query.KeyWord.Trim();
                    shelfQuery = shelfQuery.Where(x => (x.Code ?? "").Contains(kw));
                }

                if (query.StockTypes.HasValue)
                {
                    shelfQuery = shelfQuery.Where(x => x.StockType == query.StockTypes.Value);
                }

                // 1) Header group theo Code + StockType
                var headerQuery =
                    from s in shelfQuery
                    group s by new { s.Code, s.StockType } into g
                    select new GetStockAvaiable
                    {
                        ShelfStockId = g.Min(x => x.ShelfStockId),
                        Code = g.Key.Code!,
                        StockType = g.Key.StockType,
                        CodeName = "",
                        CategoryName = "",
                        TotalOnHandKg = g.Sum(x => (decimal?)x.QtyKg) ?? 0m,
                        ReservedOpenAllKg = 0m,
                        AvailableKg = 0m,
                        StockDetailAvaiables = new List<StockDetailAvaiable>()
                    };

                var totalCount = await headerQuery.CountAsync();

                var items = await headerQuery
                    .OrderBy(x => x.Code)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (items.Count == 0)
                {
                    var empty = new PagedResult<GetStockAvaiable>(items, totalCount, pageNumber, pageSize);
                    return OperationResult<PagedResult<GetStockAvaiable>>.Ok(empty);
                }

                // 2) Load tên NVL / thành phẩm
                var materialCodes = items
                    .Where(x => x.StockType == StockType.RawMaterial || x.StockType == StockType.DefectiveRawMaterial)
                    .Select(x => x.Code)
                    .Distinct()
                    .ToList();

                var productCodes = items
                    .Where(x => x.StockType == StockType.FinishedGood || x.StockType == StockType.DefectiveFinishedGood)
                    .Select(x => x.Code)
                    .Distinct()
                    .ToList();

                var materialMap = (await _unitOfWork.MaterialRepository.Query()
                    .Where(m => m.ExternalId != null && materialCodes.Contains(m.ExternalId))
                    .Select(m => new
                    {
                        Code = m.ExternalId!,
                        Name = m.Name,
                        CategoryName = m.Category != null ? m.Category.Name : ""
                    })
                    .ToListAsync())
                    .GroupBy(x => x.Code)
                    .ToDictionary(g => g.Key, g => g.First());

                var productMap = (await _unitOfWork.ProductRepository.Query()
                    .Where(p => p.Code != null && productCodes.Contains(p.Code))
                    .Select(p => new
                    {
                        Code = p.Code!,
                        Name = p.Name,
                        CategoryName = p.Category != null ? p.Category.Name : ""
                    })
                    .ToListAsync())
                    .GroupBy(x => x.Code)
                    .ToDictionary(g => g.Key, g => g.First());

                var codes = items.Select(x => x.Code).Distinct().ToList();

                // 3) Detail chỉ group theo Code + StockType + Company + LotNo
                var detailRows = await (
                    from s in _unitOfWork.WarehouseShelfStockRepository.Query()
                    where codes.Contains(s.Code)
                          && s.ShelfStockCode != "CT.0.1"
                    group s by new
                    {
                        s.Code,
                        s.StockType,
                        s.ShelfStockCode,
                        CompanyName = s.Company != null ? s.Company.Name : "",
                        s.LotNo
                    }
                    into g
                    select new
                    {
                        Code = g.Key.Code,
                        StockType = g.Key.StockType,
                        ShelfStockCode = g.Key.ShelfStockCode,
                        CompanyName = g.Key.CompanyName ?? "",
                        LotNo = g.Key.LotNo,
                        OnHandKg = g.Sum(x => (decimal?)x.QtyKg) ?? 0m
                    }
                ).ToListAsync();

                var mixingRows = await (
                    from s in _unitOfWork.WarehouseShelfStockRepository.Query()
                    where codes.Contains(s.Code)
                          && s.ShelfStockCode == "CT.0.1"
                    group s by new
                    {
                        s.Code,
                        s.StockType,
                        s.ShelfStockCode,
                        CompanyName = s.Company != null ? s.Company.Name : "",
                        s.LotNo
                    }
                    into g
                    select new
                    {
                        Code = g.Key.Code,
                        StockType = g.Key.StockType,
                        ShelfStockCode = g.Key.ShelfStockCode,
                        CompanyName = g.Key.CompanyName ?? "",
                        LotNo = g.Key.LotNo,
                        OnHandKg = g.Sum(x => (decimal?)x.QtyKg) ?? 0m
                    }
                ).ToListAsync();

                // 4) Reserved group theo Code
                var reservedRows = await (
                    from t in _unitOfWork.WarehouseTempStockRepository.Query()
                    where codes.Contains(t.Code)
                       && t.ReserveStatus == ReserveStatus.Open.ToString()
                    group t by t.Code into g
                    select new
                    {
                        Code = g.Key,
                        ReservedOpenKg = g.Sum(x => (decimal?)((x.QtyRequest ?? 0m) - (x.QtyUsed ?? 0m))) ?? 0m
                    }
                ).ToListAsync();

                var reservedMap = reservedRows.ToDictionary(x => x.Code, x => x.ReservedOpenKg);

                // 5) Merge detail vào header
                var headerMap = items.ToDictionary(x => (x.Code, x.StockType));

                foreach (var d in detailRows)
                {
                    if (!headerMap.TryGetValue((d.Code, d.StockType), out var header))
                        continue;

                    header.StockDetailAvaiables.Add(new StockDetailAvaiable
                    {
                        LotNo = d.LotNo,
                        ShelfStockCode = d.ShelfStockCode,
                        CompanyName = d.CompanyName,
                        OnHandKg = d.OnHandKg
                    });
                }

                foreach (var m in mixingRows)
                {
                    if (!headerMap.TryGetValue((m.Code, m.StockType), out var header))
                        continue;

                    header.StockDetailAvaiables.Add(new StockDetailAvaiable
                    {
                        LotNo = m.LotNo,
                        ShelfStockCode = "CT.0.1 - KHO CÂN TRỘN",
                        CompanyName = m.CompanyName,
                        OnHandKg = m.OnHandKg
                    });
                }

                // 6) Fill Reserved / Available ở header
                foreach (var it in items)
                {
                    var isDefective =
                        it.StockType == StockType.DefectiveRawMaterial ||
                        it.StockType == StockType.DefectiveFinishedGood;

                    var reserved = 0m;

                    if (!isDefective && reservedMap.TryGetValue(it.Code, out var rv))
                    {
                        reserved = rv;
                    }

                    it.ReservedOpenAllKg = isDefective ? 0m : reserved;
                    it.AvailableKg = isDefective ? 0m : (it.TotalOnHandKg - reserved);

                    if ((it.StockType == StockType.RawMaterial || it.StockType == StockType.DefectiveRawMaterial)
                        && materialMap.TryGetValue(it.Code, out var m))
                    {
                        it.CodeName = m.Name ?? string.Empty;
                        it.CategoryName = m.CategoryName ?? string.Empty;
                    }
                    else if ((it.StockType == StockType.FinishedGood || it.StockType == StockType.DefectiveFinishedGood)
                        && productMap.TryGetValue(it.Code, out var p))
                    {
                        it.CodeName = p.Name ?? string.Empty;
                        it.CategoryName = p.CategoryName ?? string.Empty;
                    }
                }

                // 7) Filter available <= 0
                if (query.OnlyAvailableLeZero)
                {
                    items = items
                        .Where(x => (x.AvailableKg ?? 0m) <= 0m)
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

        public async Task<List<VaAvailabilityVm>> GetVaAvailabilityAsync( Guid manufacturingFormulaId, CancellationToken ct = default)
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
        public async Task<Dictionary<string, VaAvailabilityVm>> GetVaAvailabilityDictAsync(Guid manufacturingFormulaId, CancellationToken ct = default)
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

        /// <summary>
        /// Lấy dictionary mapping giữa code của NVL trong công thức sản xuất và lotNo của NVL đó trong kho
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetLotNoMapByCodesAsync(IEnumerable<string> codes, CancellationToken ct = default)
        {
            var codeList = codes
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            if (codeList.Count == 0)
                return new Dictionary<string, string>();

            var rows = await _unitOfWork.WarehouseShelfStockRepository.Query()
                .Where(x =>
                    x.Code != null &&
                    codeList.Contains(x.Code) &&
                    x.StockType == StockType.RawMaterial &&
                    x.LotNo != null &&
                    x.LotNo != "")
                .Select(x => new
                {
                    Code = x.Code!,
                    LotNo = x.LotNo!,
                    x.UpdatedDate,
                    x.ShelfStockId
                })
                .ToListAsync(ct);

            var map = rows
                .GroupBy(x => x.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .OrderByDescending(x => x.UpdatedDate)
                        .ThenByDescending(x => x.ShelfStockId)
                        .Select(x => x.LotNo.Trim())
                        .FirstOrDefault() ?? string.Empty
                );

            return map;
        }


        public async Task<OperationResult<List<StockAvailableExportRow>>> GetStockAvailableExportAsync(WarehouseReadServiceQuery query)
        {
            try
            {
                var shelfQuery = _unitOfWork.WarehouseShelfStockRepository.Query()
                    .Where(s => !string.IsNullOrWhiteSpace(s.Code));

                if (!string.IsNullOrWhiteSpace(query.KeyWord))
                {
                    var kw = query.KeyWord.Trim();
                    shelfQuery = shelfQuery.Where(x => (x.Code ?? "").Contains(kw));
                }

                if (query.StockTypes.HasValue)
                {
                    shelfQuery = shelfQuery.Where(x => x.StockType == query.StockTypes.Value);
                }

                // 1) Tổng tồn theo Code + StockType
                var headerRows = await (
                    from s in shelfQuery
                    group s by new { s.Code, s.StockType } into g
                    select new
                    {
                        Code = g.Key.Code!,
                        StockType = g.Key.StockType,
                        TotalOnHandKg = g.Sum(x => (decimal?)x.QtyKg) ?? 0m
                    }
                ).ToListAsync();

                if (headerRows.Count == 0)
                    return OperationResult<List<StockAvailableExportRow>>.Ok(new List<StockAvailableExportRow>());

                var materialCodes = headerRows
                    .Where(x => x.StockType == StockType.RawMaterial || x.StockType == StockType.DefectiveRawMaterial)
                    .Select(x => x.Code)
                    .Distinct()
                    .ToList();

                var productCodes = headerRows
                    .Where(x => x.StockType == StockType.FinishedGood || x.StockType == StockType.DefectiveFinishedGood)
                    .Select(x => x.Code)
                    .Distinct()
                    .ToList();

                var materialMap = (await _unitOfWork.MaterialRepository.Query()
                    .Where(m => m.ExternalId != null && materialCodes.Contains(m.ExternalId))
                    .Select(m => new
                    {
                        Code = m.ExternalId!,
                        Name = m.Name,
                        CategoryName = m.Category != null ? m.Category.Name : ""
                    })
                    .ToListAsync())
                    .GroupBy(x => x.Code)
                    .ToDictionary(g => g.Key, g => g.First());

                var productMap = (await _unitOfWork.ProductRepository.Query()
                    .Where(p => p.Code != null && productCodes.Contains(p.Code))
                    .Select(p => new
                    {
                        Code = p.Code!,
                        Name = p.Name,
                        CategoryName = p.Category != null ? p.Category.Name : ""
                    })
                    .ToListAsync())
                    .GroupBy(x => x.Code)
                    .ToDictionary(g => g.Key, g => g.First());

                var codes = headerRows.Select(x => x.Code).Distinct().ToList();

                // 2) Lấy detail tồn kho
                var detailRows = await (
                    from s in _unitOfWork.WarehouseShelfStockRepository.Query()
                    where codes.Contains(s.Code)
                    group s by new
                    {
                        s.Code,
                        s.StockType,
                        s.ShelfStockCode,
                        CompanyName = s.Company != null ? s.Company.Name : "",
                        s.LotNo
                    }
                    into g
                    select new
                    {
                        Code = g.Key.Code,
                        StockType = g.Key.StockType,
                        ShelfStockCode = g.Key.ShelfStockCode,
                        CompanyName = g.Key.CompanyName ?? "",
                        LotNo = g.Key.LotNo,
                        OnHandKg = g.Sum(x => (decimal?)x.QtyKg) ?? 0m
                    }
                ).ToListAsync();

                var totalMap = headerRows.ToDictionary(
                    x => (x.Code, x.StockType),
                    x => x.TotalOnHandKg
                );

                var result = new List<StockAvailableExportRow>();

                foreach (var d in detailRows.OrderBy(x => x.Code).ThenBy(x => x.ShelfStockCode).ThenBy(x => x.LotNo))
                {
                    string codeName = string.Empty;
                    string categoryName = string.Empty;

                    if ((d.StockType == StockType.RawMaterial || d.StockType == StockType.DefectiveRawMaterial)
                        && materialMap.TryGetValue(d.Code, out var m))
                    {
                        codeName = m.Name ?? string.Empty;
                        categoryName = m.CategoryName ?? string.Empty;
                    }
                    else if ((d.StockType == StockType.FinishedGood || d.StockType == StockType.DefectiveFinishedGood)
                        && productMap.TryGetValue(d.Code, out var p))
                    {
                        codeName = p.Name ?? string.Empty;
                        categoryName = p.CategoryName ?? string.Empty;
                    }

                    var shelfStockCode = d.ShelfStockCode == "CT.0.1"
                        ? "CT.0.1 - KHO CÂN TRỘN"
                        : d.ShelfStockCode ?? string.Empty;

                    result.Add(new StockAvailableExportRow
                    {
                        Code = d.Code ?? string.Empty,
                        CodeName = codeName,
                        CategoryName = categoryName,
                        StockType = d.StockType.ToString(),
                        TotalOnHandKg = totalMap.TryGetValue((d.Code, d.StockType), out var total) ? total : 0m,
                        ShelfStockCode = shelfStockCode,
                        CompanyName = d.CompanyName ?? string.Empty,
                        LotNo = d.LotNo,
                        OnHandKg = d.OnHandKg
                    });
                }

                return OperationResult<List<StockAvailableExportRow>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<List<StockAvailableExportRow>>.Fail(ex.Message);
            }
        }

    }
}
