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
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Repositories_Contracts;

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

    }
}
