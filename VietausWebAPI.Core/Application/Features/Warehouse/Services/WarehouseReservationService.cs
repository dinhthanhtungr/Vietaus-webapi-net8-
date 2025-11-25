using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseWriteServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using VietausWebAPI.Core.Repositories_Contracts;
using static QuestPDF.Helpers.Colors;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Services
{
    public class WarehouseReservationService : IWarehouseReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public WarehouseReservationService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<OperationResult> ReserveAvailabilityAsync(CreateVaSnapshotAndReservations req, CancellationToken ct)
        {
            static string Norm(string s) => s?.Trim().ToUpperInvariant() ?? string.Empty;

            req.companyId = _currentUser.CompanyId;
            req.createdBy = _currentUser.EmployeeId;

            // 0) Kiểm tra đầu vào
            if (req == null || req.reservations == null || req.reservations.Count == 0)
                return new OperationResult { Success = false, Message = "Payload trống." };


            // 1) Gom các NVL từ payload: bỏ null/rỗng, cộng dồn TotalQuantity theo mã
            var items = req.reservations
                .Where(r => !string.IsNullOrWhiteSpace(r.MaterialExternalIdSnapshot))
                .GroupBy(r => Norm(r.MaterialExternalIdSnapshot!))
                .Select(g => new { Code = g.Key, Required = g.Sum(x => x.TotalQuantity ?? 0m) })
                .Where(x => x.Required > 0m)
                .ToList();

            if (items.Count == 0) return new OperationResult() { Success = false, Message = "Nguyên vật liệu rổng"};

            // ------ Kiểm tra "freshness" (ngăn submit trùng)

            var codeList = items.Select(x => x.Code).ToArray();
            //var (ok, current) = await ValidateFreshnessAsync(req.companyId, codeList, req.expected ?? new(), ct);

            //var codeList = items.Select(x => x.Code).ToArray();
            (bool ok, List<VaAvailabilityVm> current) = await ValidateFreshnessAsync(
                req.companyId,
                req.expected ?? new List<VaAvailabilityVm>(),
                codeList,
                ct
            );
            if (!ok)
            {
                return new OperationResult<List<VaAvailabilityVm>>
                {
                    Success = false,
                    Message = "Nguyên vật liệu đã thay đổi, dữ liệu đã được cập nhật lại.",
                    Data = current
                };
            }

            // 2) Lấy VaCode từ header công thức (fallback: dùng manufacturingFormulaId)
            var vaCode = await _unitOfWork.MfgProductionOrderRepository.Query()
                .Where(f => f.MfgProductionOrderId == req.manufacturingId)
                .Select(f => f.ExternalId)
                .FirstOrDefaultAsync(ct);
            vaCode ??= req.manufacturingId.ToString();
            var vaCodeNorm = Norm(vaCode);

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            // 3) (tuỳ chọn) Hủy các RESERVE OPEN cũ của chính VA này (tránh “chồng đặt chỗ”)
            if (req.cancelPreviousOpen)
            {
                await _unitOfWork.WarehouseTempStockRepository.Query()
                    .Where(t => t.CompanyId == req.companyId
                             && t.VaCode == vaCodeNorm
                             //&& t.TempType == TempType.Reserve
                             && t.ReserveStatus == ReserveStatus.Open.ToString())
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(t => t.ReserveStatus, ReserveStatus.Cancelled.ToString())
                        .SetProperty(t => t.CreatedDate, DateTime.Now), ct);
            }


            // 6) Kiểm tra SNAPSHOT đã có trong set này cho các mã (để "chỉ thêm khi chưa có")
            var existingSnapCodes = await _unitOfWork.WarehouseTempStockRepository.Query()
                .Where(t => t.CompanyId == req.companyId
                         && t.VaCode == vaCodeNorm
                         //&& t.TempType == TempType.Snapshot
                         && t.Code != null
                         && codeList.Contains(t.Code!.ToUpper()))
                .Select(t => t.Code!)
                .ToListAsync(ct);

            var hasSnap = new HashSet<string>(existingSnapCodes.Select(Norm));

            // 7) Ghi dữ liệu: SNAPSHOT (nếu chưa có) + RESERVE (luôn thêm)
            var toInsert = new List<WarehouseTempStock>();

            foreach (var it in items)
            {

                    toInsert.Add(new WarehouseTempStock
                    {
                        CompanyId = req.companyId,

                        VaCode = vaCodeNorm,
                        Code = it.Code,                 // <- NVL
                        QtyRequest = it.Required,
                        CreatedBy = req.createdBy,
                        CreatedDate = DateTime.Now,
                        ReserveStatus = ReserveStatus.Open.ToString()
                    });

            }

            await _unitOfWork.WarehouseTempStockRepository.AddRangeAsync(toInsert);
            await _unitOfWork.SaveChangesAsync();
            await tx.CommitAsync(ct);

            return new OperationResult() { Success = true, Message = "Reserved successfully" };
        }


        /// <summary>
        /// Hàm kiểm tra "freshness" của danh sách mã NVL so với expected xem có thay đổi không.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="expected"></param>
        /// <param name="codes"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<(bool IsFresh, List<VaAvailabilityVm> Current)> ValidateFreshnessAsync(Guid companyId, IEnumerable<VaAvailabilityVm> expected, IEnumerable<string> codes, CancellationToken ct)
        {
            // Chuẩn hóa input

            var codeSet = codes
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct()
                .ToArray();


            if (codeSet.Length == 0)
                return (true, new List<VaAvailabilityVm>());

            // Onhand hiện tại
            var onHandByCode = await _unitOfWork.WarehouseShelfStockRepository.Query()
                .Where(s => s.CompanyId == companyId && codeSet.Contains(s.Code))
                .GroupBy(s => s.Code)
                .Select(g => new { code = g.Key, Qty = g.Sum(x => x.QtyKg) })
                .ToDictionaryAsync(x => x.code, x => x.Qty, ct);


            // ReservedOpen hiện tại (hit partial index)

            var reservedByCode = await _unitOfWork.WarehouseTempStockRepository.Query()
                .Where(t => t.CompanyId == companyId
                            //&& t.TempType == TempType.Reserve
                            && t.ReserveStatus == ReserveStatus.Open.ToString()
                            && codeSet.Contains(t.Code))
                .GroupBy(t => t.Code)
                .Select(g => new { code = g.Key, Qty = g.Sum(x => x.QtyRequest) })
                .ToDictionaryAsync(x => x.code, x => x.Qty, ct);

            // Build current list
            var current = new List<VaAvailabilityVm>(codeSet.Length);
            
            foreach(var v in codeSet)
            {
                onHandByCode.TryGetValue(v, out var onHand);
                reservedByCode.TryGetValue(v, out var reserved);

                current.Add(new VaAvailabilityVm(
                        Code: v,
                        OnHandKg: onHand,
                        ReservedOpenAllKg: reserved ?? 0m,
                        AvailableKg: (onHand) - (reserved ?? 0m)
                ));
            }

            // So sánh với expected (dung sai nhỏ để tránh sai khác làm tròn)
            const decimal EPS = 0.0001m;
            var expectedMap = (expected ?? Enumerable.Empty<VaAvailabilityVm>())
                .GroupBy(e => e.Code?.Trim())
                .ToDictionary(g => g.Key!, g => g.First());

            bool isFresh = true;
            foreach (var cur in current)
            {
                if (!expectedMap.TryGetValue(cur.Code, out var exp))
                {
                    // UI không gửi expected cho mã này => coi như stale để bắt refresh
                    isFresh = false;
                    break;
                }

                var onHandDiff = Math.Abs(cur.OnHandKg - exp.OnHandKg);
                var reservedDiff = Math.Abs(cur.ReservedOpenAllKg - exp.ReservedOpenAllKg);

                if (onHandDiff > EPS || reservedDiff > EPS)
                {
                    isFresh = false;
                    break;
                }
            }

            return (isFresh, current);
        }

        //public async Task<int> CancelReserveAsync(WarehouseReservationServiceQuery query, CancellationToken ct)
        //{
        //    var q = _unitOfWork.WarehouseTempStockRepository.Query()
        //                .Where(x => x.CompanyId == query.companyId
        //                && x.SnapshotSetId == query.snapshotSetId
        //                && x.VaCode == query.vaCode
        //                && x.Code == query.code
        //                && x.TempType == TempType.Reserve
        //                && x.ReserveStatus == ReserveStatus.Open);

        //    if(!string.IsNullOrEmpty(query.lotKey))
        //    {
        //        q = q.Where(x => x.LotKey == query.lotKey);
        //    }

        //    var rows = await q.ToListAsync(ct);
        //    foreach(var row in rows)
        //    {
        //        row.ReserveStatus = ReserveStatus.Cancelled;
        //    }

        //    return await _unitOfWork.SaveChangesAsync();
        //}

        //public async Task<int> ConsumeReserveAsync(WarehouseReservationServiceQuery query, CancellationToken ct)
        //{
        //    var q = _unitOfWork.WarehouseTempStockRepository.Query()
        //                .Where(x => x.CompanyId == query.companyId
        //                && x.SnapshotSetId == query.snapshotSetId
        //                && x.VaCode == query.vaCode
        //                && x.Code == query.code
        //                && x.TempType == TempType.Reserve
        //                && x.ReserveStatus == ReserveStatus.Open);

        //    if (!string.IsNullOrEmpty(query.lotKey))
        //    {
        //        q = q.Where(x => x.LotKey == query.lotKey);
        //    }

        //    var rows = await q.ToListAsync();

        //    foreach( var row in rows)
        //    {
        //        row.ReserveStatus = ReserveStatus.Consumed;
        //        row.LinkedIssueId = query.issueId;
        //    }

        //    return await _unitOfWork.SaveChangesAsync();
        //}

        //public async Task<long> ReserveAsync(WarehouseReservationServiceQuery query, CancellationToken ct)
        //{
        //    var row = new WarehouseTempStock
        //    {
        //        CompanyId = query.companyId,
        //        SnapshotSetId = query.snapshotSetId,
        //        TempType = TempType.Reserve,
        //        VaCode = query.vaCode,
        //        VaLineCode = query.vaLineCode,
        //        Code = query.code,
        //        LotKey = query.lotKey,
        //        QtyRequest = query.qtyRequest,
        //        ReserveStatus = Domain.Enums.ReserveStatus.Open,
        //        CreatedBy = query.createdBy,
        //        CreatedDate = DateTime.Now
        //    };

        //    await _unitOfWork.WarehouseTempStockRepository.AddAsync(row);

        //    await _unitOfWork.SaveChangesAsync();

        //    return row.TempId;
        //}
    }
}
