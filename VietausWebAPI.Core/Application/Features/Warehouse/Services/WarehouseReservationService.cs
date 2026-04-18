using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseWriteServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Core.Domain.Enums.Category;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using static QuestPDF.Helpers.Colors;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Services
{
    public class WarehouseReservationService : IWarehouseReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IExternalIdService _externalIdService;

        public WarehouseReservationService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IExternalIdService externalIdService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _externalIdService = externalIdService;
        }


        // Hàm này củ có freshness để tồn kho vừa thay đổi, vui lòng xem lại

        //public async Task<OperationResult> ReserveAvailabilityAsync(CreateVaSnapshotAndReservations req, CancellationToken ct)
        //{
        //    static string Norm(string s) => s?.Trim().ToUpperInvariant() ?? string.Empty;

        //    req.companyId = _currentUser.CompanyId;
        //    req.createdBy = _currentUser.EmployeeId;

        //    // 0) Kiểm tra đầu vào
        //    if (req == null || req.reservations == null || req.reservations.Count == 0)
        //        return new OperationResult { Success = false, Message = "Payload trống." };


        //    // 1) Gom các NVL từ payload: bỏ null/rỗng, cộng dồn TotalQuantity theo mã
        //    var items = req.reservations
        //        .Where(r => !string.IsNullOrWhiteSpace(r.MaterialExternalIdSnapshot))
        //        .GroupBy(r => Norm(r.MaterialExternalIdSnapshot!))
        //        .Select(g => new { Code = g.Key, Required = g.Sum(x => x.TotalQuantity ?? 0m) })
        //        .Where(x => x.Required > 0m)
        //        .ToList();

        //    if (items.Count == 0) return new OperationResult() { Success = false, Message = "Nguyên vật liệu rổng"};

        //    // ------ Kiểm tra "freshness" (ngăn submit trùng)

        //    var codeList = items.Select(x => x.Code).ToArray();
        //    //var (ok, current) = await ValidateFreshnessAsync(req.companyId, codeList, req.expected ?? new(), ct);

        //    //var codeList = items.Select(x => x.Code).ToArray();
        //    (bool ok, List<VaAvailabilityVm> current) = await ValidateFreshnessAsync(
        //        req.companyId,
        //        req.expected ?? new List<VaAvailabilityVm>(),
        //        codeList,
        //        ct
        //    );
        //    if (!ok)
        //    {
        //        return new OperationResult<List<VaAvailabilityVm>>
        //        {
        //            Success = false,
        //            Message = "Nguyên vật liệu đã thay đổi, dữ liệu đã được cập nhật lại.",
        //            Data = current
        //        };
        //    }

        //    // 2) Lấy VaCode từ header công thức (fallback: dùng manufacturingFormulaId)
        //    var vaCode = await _unitOfWork.MfgProductionOrderRepository.Query()
        //        .Where(f => f.MfgProductionOrderId == req.manufacturingId)
        //        .Select(f => f.ExternalId)
        //        .FirstOrDefaultAsync(ct);
        //    vaCode ??= req.manufacturingId.ToString();
        //    var vaCodeNorm = Norm(vaCode);

        //    await using var tx = await _unitOfWork.BeginTransactionAsync();

        //    // 3) (tuỳ chọn) Hủy các RESERVE OPEN cũ của chính VA này (tránh “chồng đặt chỗ”)
        //    if (req.cancelPreviousOpen)
        //    {
        //        await _unitOfWork.WarehouseTempStockRepository.Query()
        //            .Where(t => t.CompanyId == req.companyId
        //                     && t.VaCode == vaCodeNorm
        //                     //&& t.TempType == TempType.Reserve
        //                     && t.ReserveStatus == ReserveStatus.Open.ToString())
        //            .ExecuteUpdateAsync(s => s
        //                .SetProperty(t => t.ReserveStatus, ReserveStatus.Cancelled.ToString())
        //                .SetProperty(t => t.CreatedDate, DateTime.Now), ct);
        //    }


        //    // 6) Kiểm tra SNAPSHOT đã có trong set này cho các mã (để "chỉ thêm khi chưa có")
        //    var existingSnapCodes = await _unitOfWork.WarehouseTempStockRepository.Query()
        //        .Where(t => t.CompanyId == req.companyId
        //                 && t.VaCode == vaCodeNorm
        //                 //&& t.TempType == TempType.Snapshot
        //                 && t.Code != null
        //                 && codeList.Contains(t.Code!.ToUpper()))
        //        .Select(t => t.Code!)
        //        .ToListAsync(ct);

        //    var hasSnap = new HashSet<string>(existingSnapCodes.Select(Norm));

        //    // 7) Ghi dữ liệu: SNAPSHOT (nếu chưa có) + RESERVE (luôn thêm)
        //    var toInsert = new List<WarehouseTempStock>();

        //    foreach (var it in items)
        //    {

        //            toInsert.Add(new WarehouseTempStock
        //            {
        //                CompanyId = req.companyId,

        //                VaCode = vaCodeNorm,
        //                Code = it.Code,                 // <- NVL
        //                QtyRequest = it.Required,
        //                CreatedBy = req.createdBy,
        //                CreatedDate = DateTime.Now,m
        //                ReserveStatus = ReserveStatus.Open.ToString()
        //            });

        //    }

        //    await _unitOfWork.WarehouseTempStockRepository.AddRangeAsync(toInsert);
        //    //Tạm bỏ transaction này

        //    //await _unitOfWork.SaveChangesAsync();
        //    //await tx.CommitAsync(ct);

        //    return new OperationResult() { Success = true, Message = "Reserved successfully" };
        //}


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

            if (items.Count == 0)
                return new OperationResult { Success = false, Message = "Nguyên vật liệu rỗng." };

            // 2) Lấy VaCode từ MPO
            var vaCode = await _unitOfWork.MfgProductionOrderRepository.Query()
                .Where(f => f.MfgProductionOrderId == req.manufacturingId)
                .Select(f => f.ExternalId)
                .FirstOrDefaultAsync(ct);

            vaCode ??= req.manufacturingId.ToString();
            var vaCodeNorm = Norm(vaCode);

            // 3) Hủy các RESERVE OPEN cũ của chính VA này
            if (req.cancelPreviousOpen)
            {
                await _unitOfWork.WarehouseTempStockRepository.Query()
                    .Where(t => t.CompanyId == req.companyId
                             && t.VaCode == vaCodeNorm
                             && t.ReserveStatus == ReserveStatus.Open.ToString())
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(t => t.ReserveStatus, ReserveStatus.Cancelled.ToString())
                        .SetProperty(t => t.CreatedDate, DateTime.Now), ct);
            }

            // 4) Insert RESERVE mới
            var toInsert = new List<WarehouseTempStock>();

            foreach (var it in items)
            {
                toInsert.Add(new WarehouseTempStock
                {
                    CompanyId = req.companyId,
                    VaCode = vaCodeNorm,
                    Code = it.Code,
                    QtyRequest = it.Required,
                    CreatedBy = req.createdBy,
                    CreatedDate = DateTime.Now,
                    ReserveStatus = ReserveStatus.Open.ToString()
                });
            }

            await _unitOfWork.WarehouseTempStockRepository.AddRangeAsync(toInsert);

            // KHÔNG SaveChanges ở đây
            // KHÔNG Commit ở đây
            return new OperationResult { Success = true, Message = "Reserved successfully" };
        }

        // ======================================================================== Update ========================================================================
        //public async Task EnsureWarehouseIssueRequestAsync(
        //    MfgProductionOrder existing,
        //    DateTime now,
        //    Guid userId,
        //    Guid companyId,
        //    CancellationToken ct)
        //{
        //    // --- IDENTITY / TRÙNG LẶP ---
        //    // Ưu tiên kiểm theo SourceId nếu bạn có cột trong WarehouseRequest (khuyến nghị thêm)
        //    var wrQuery = _unitOfWork.WarehouseRequestRepository.Query(track: false);

        //    // Nếu CHƯA có cột nguồn, kiểm bằng RequestCode (pattern duy nhất theo ExternalId)
        //    var requestCode = await _externalIdService.NextAsync(DocumentPrefix.PRQ.ToString(), ct: ct);
        //    var existedWR = await wrQuery.FirstOrDefaultAsync(
        //        x => x.RequestCode == requestCode && x.IsActive, ct);

        //    if (existedWR != null)
        //    {
        //        // Đã có phiếu → thôi, không tạo nữa (idempotent)
        //        return;
        //    }

        //    // --- LẤY CÔNG THỨC (VERSION HIỆN HÀNH) ---
        //    // Version hiện hành = ValidTo == null
        //    var currentVersion = await _unitOfWork.ProductionSelectVersionRepository.Query(false)
        //        .Where(v => v.MfgProductionOrderId == existing.MfgProductionOrderId && v.ValidTo == null)
        //        .Select(v => new { v.ManufacturingFormulaId })
        //        .FirstOrDefaultAsync(ct);

        //    if (currentVersion == null || currentVersion.ManufacturingFormulaId == null)
        //    {
        //        // Không có version/công thức → tuỳ business: có thể bỏ qua hoặc ném lỗi domain
        //        // Ở đây mình chọn "bỏ qua" (không tạo phiếu)
        //        return;
        //    }

        //    // Lấy NVL trong công thức
        //    var mats = await _unitOfWork.ManufacturingFormulaMaterialRepository.Query(false)
        //        .Where(m => m.ManufacturingFormulaId == currentVersion.ManufacturingFormulaId && m.IsActive)
        //        .Select(m => new
        //        {
        //            m.MaterialId,
        //            m.MaterialExternalIdSnapshot,
        //            m.MaterialNameSnapshot,
        //            m.Quantity,
        //            m.Unit
        //        })
        //        .ToListAsync(ct);

        //    if (mats.Count == 0)
        //        return;

        //    // Tính hệ số nhân (ví dụ theo số mẻ)
        //    var batches = existing.NumOfBatches ?? 1m;

        //    // --- TẠO HEADER ---
        //    var wr = new WarehouseRequest
        //    {
        //        // RequestId: auto
        //        RequestCode = requestCode,                 // unique theo LSX
        //        ReqStatus = WarehouseRequestStatus.Pending,                       // tuỳ enum của bạn
        //        RequestName = $"Xuất NVL cho LSX {existing.ExternalId}",
        //        IsActive = true,
        //        ReqType = WareHouseRequestType.ExportForProduction,
        //        codeFromRequest = existing.ExternalId ?? "", // nếu bạn vẫn muốn lưu

        //        CompanyId = companyId,
        //        CreatedDate = now,
        //        CreatedBy = userId,
        //        UpdatedDate = now,
        //        UpdatedBy = userId,
        //    };

        //    await _unitOfWork.WarehouseRequestRepository.AddAsync(wr, ct);
        //    await _unitOfWork.SaveChangesAsync(); // để có RequestId cho detail

        //    // --- TẠO DETAIL ---
        //    var details = new List<WarehouseRequestDetail>(mats.Count);
        //    foreach (var m in mats)
        //    {
        //        details.Add(new WarehouseRequestDetail
        //        {
        //            // DetailId: auto
        //            RequestId = wr.RequestId,
        //            ProductCode = m.MaterialExternalIdSnapshot ?? "",   // mã NVL
        //            ProductName = m.MaterialNameSnapshot ?? "",         // tên NVL

        //            LotNumber = null,          // để kho pick lot sau
        //            WeightKg = m.Quantity * (existing.TotalQuantity ?? 0), // KHỐI LƯỢNG YÊU CẦU
        //            StockStatus = StockType.RawMaterial.ToString(),   // tuỳ enum string của bạn
        //            IsActive = true
        //        });
        //    }

        //    await _unitOfWork.WarehouseRequestDetailRepository.AddRangeAsync(details, ct);
        //}

        public async Task EnsureWarehouseRequestForDOAsync(
            DeliveryOrder deliveryOrder,
            DateTime now,
            Guid userId,
            Guid companyId,
            CancellationToken ct)
        {
            var requestCode = await _externalIdService.NextAsync(DocumentPrefix.PRQ.ToString(), ct: ct);

            // ✅ Group theo Product + LotNoList để không mất lot
            var detailGroups = deliveryOrder.Details
                .Where(x => x.IsActive)
                .GroupBy(x => new
                {
                    x.ProductExternalIdSnapShot,
                    x.ProductNameSnapShot,
                    LotNoList = NormalizeLotList(x.LotNoList) // chuẩn hoá cho đỡ lệch do dấu cách
                })
                .Select(g => new
                {
                    g.Key.ProductExternalIdSnapShot,
                    g.Key.ProductNameSnapShot,
                    g.Key.LotNoList,
                    Quantity = g.Sum(x => x.Quantity),
                    NumOfBags = g.Sum(x => x.NumOfBags)
                })
                .ToList();

            var wr = new WarehouseRequest
            {
                RequestCode = requestCode,
                ReqStatus = WarehouseRequestStatus.Pending,
                RequestName = $"Xuất kho cho PGH {deliveryOrder.ExternalId}",
                IsActive = true,
                ReqType = WareHouseRequestType.ExportFinishedGood,
                codeFromRequest = deliveryOrder.ExternalId ?? "",

                CompanyId = companyId,
                CreatedBy = userId,
                CreatedDate = now,
                UpdatedBy = userId,
                UpdatedDate = now
            };

            wr.WarehouseRequestDetails = detailGroups.Select(d => new WarehouseRequestDetail
            {
                ProductCode = d.ProductExternalIdSnapShot ?? "",
                ProductName = d.ProductNameSnapShot ?? "",

                // ✅ Lưu đúng LotNoList theo DO
                LotNumber = d.LotNoList ?? "",

                WeightKg = d.Quantity,
                BagNumber = d.NumOfBags,
                StockStatus = VoucherDetailType.Export.ToString(),
                IsActive = true
            }).ToList();

            await _unitOfWork.WarehouseRequestRepository.AddAsync(wr, ct);

            static string? NormalizeLotList(string? s)
            {
                if (string.IsNullOrWhiteSpace(s)) return null;

                // tách theo , ; | xuống dòng… rồi trim, unique, join lại
                var lots = s.Split(new[] { ',', ';', '|', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();

                return lots.Count == 0 ? null : string.Join(", ", lots);
            }
        }

        public async Task EnsureWarehouseRequestDeletedAsync(string externalId, CancellationToken ct)
        {
            externalId = (externalId ?? "").Trim();
            if (externalId.Length == 0) return;

            await _unitOfWork.WarehouseRequestRepository.Query(track: true)
                .Where(wr => wr.codeFromRequest == externalId && wr.IsActive)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(wr => wr.IsActive, false)
                    .SetProperty(wr => wr.ReqStatus, WarehouseRequestStatus.Cancelled)
                    .SetProperty(wr => wr.UpdatedDate, DateTime.Now)
                    .SetProperty(wr => wr.UpdatedBy, _currentUser.EmployeeId),
                    ct);
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

        //=============================================================== PRIVATE - PUBLIC =================================================================

        private async Task<OperationResult> SyncReservationsCoreAsync(
            ReservationSyncContext ctx,
            decimal totalQuantity,
            IEnumerable<PatchMfgProductionOrderFormulaItemRequest> formulaItems,
            CancellationToken ct = default)
        {
            if (ctx == null)
                return OperationResult.Fail("Context giữ chỗ không hợp lệ.");

            if (ctx.MfgProductionOrderId == Guid.Empty)
                return OperationResult.Fail("MfgProductionOrderId không hợp lệ.");

            if (string.IsNullOrWhiteSpace(ctx.VaCode))
                return OperationResult.Fail("VaCode không hợp lệ.");

            if (ctx.CompanyId == Guid.Empty)
                return OperationResult.Fail("CompanyId không hợp lệ.");

            if (totalQuantity <= 0)
                return OperationResult.Fail("TotalQuantity phải lớn hơn 0.");

            if (formulaItems == null)
                return OperationResult.Fail("Danh sách công thức không hợp lệ.");

            static string Norm(string? s) => s?.Trim().ToUpperInvariant() ?? string.Empty;

            var userId = _currentUser.EmployeeId;
            var now = DateTime.Now;

            var validItems = formulaItems
                .Where(x =>
                    x.IsActive &&
                    x.Quantity > 0 &&
                    !string.IsNullOrWhiteSpace(x.MaterialExternalIdSnapshot))
                .ToList();

            if (validItems.Count == 0)
                return OperationResult.Fail("Công thức không có nguyên vật liệu hợp lệ để giữ chỗ.");

            var targetMap = validItems
                .GroupBy(x => Norm(x.MaterialExternalIdSnapshot))
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Quantity * totalQuantity)
                );

            var existingOpen = await _unitOfWork.WarehouseTempStockRepository
                .Query(track: true)
                .Where(x =>
                    x.CompanyId == ctx.CompanyId &&
                    x.VaCode == ctx.VaCode &&
                    x.ReserveStatus == "Open")
                .ToListAsync(ct);

            var existingMap = existingOpen
                .GroupBy(x => Norm(x.Code))
                .ToDictionary(g => g.Key, g => g.ToList());

            // 1) update/create theo target mới
            foreach (var kv in targetMap)
            {
                var code = kv.Key;
                var newQtyRequest = kv.Value;

                if (existingMap.TryGetValue(code, out var reserveRows) && reserveRows.Count > 0)
                {
                    var reserve = reserveRows
                        .OrderBy(x => x.LotKey == null ? 0 : 1)
                        .ThenBy(x => x.TempId)
                        .First();

                    var used = reserve.QtyUsed ?? 0m;

                    //if (newQtyRequest < used)
                    //{
                    //    return OperationResult.Fail(
                    //        $"Nguyên vật liệu '{code}' đã được sử dụng {used}, không thể giảm giữ chỗ xuống {newQtyRequest}.");
                    //}

                    reserve.QtyRequest = newQtyRequest;
                    reserve.ReserveStatus = "Open";
                }
                else
                {
                    var entity = new WarehouseTempStock
                    {
                        CompanyId = ctx.CompanyId,
                        VaCode = ctx.VaCode,
                        Code = code,
                        LotKey = null,
                        QtyRequest = newQtyRequest,
                        QtyUsed = 0m,
                        ReserveStatus = "Open",
                        CreatedBy = userId,
                        CreatedDate = now
                    };

                    await _unitOfWork.WarehouseTempStockRepository.AddAsync(entity, ct);
                }
            }

            // 2) cancel những mã cũ không còn trong target
            foreach (var kv in existingMap)
            {
                var code = kv.Key;

                if (targetMap.ContainsKey(code))
                    continue;

                foreach (var reserve in kv.Value)
                {
                    var used = reserve.QtyUsed ?? 0m;

                    //if (used > 0)
                    //{
                    //    return OperationResult.Fail(
                    //        $"Nguyên vật liệu '{code}' đã được sử dụng {used}, không thể xóa khỏi công thức hoặc hủy giữ chỗ.");
                    //}

                    reserve.ReserveStatus = "Cancelled";
                    reserve.QtyRequest = 0m;
                }
            }

            return OperationResult.Ok("Đồng bộ giữ chỗ tồn kho ảo thành công.");
        }

        public async Task<OperationResult> SyncReservationsByFormulaItemsAsync(
            Guid mfgProductionOrderId,
            decimal totalQuantity,
            IEnumerable<PatchMfgProductionOrderFormulaItemRequest> formulaItems,
            CancellationToken ct = default)
                {
                    if (mfgProductionOrderId == Guid.Empty)
                        return OperationResult.Fail("MfgProductionOrderId không hợp lệ.");

                    var mpo = await _unitOfWork.MfgProductionOrderRepository
                        .Query(track: false)
                        .Where(x => x.MfgProductionOrderId == mfgProductionOrderId && x.IsActive)
                        .Select(x => new ReservationSyncContext
                        {
                            MfgProductionOrderId = x.MfgProductionOrderId,
                            VaCode = x.ExternalId ?? string.Empty,
                            CompanyId = x.CompanyId
                        })
                        .FirstOrDefaultAsync(ct);

                    if (mpo == null)
                        return OperationResult.Fail("Không tìm thấy lệnh sản xuất.");

                    if (string.IsNullOrWhiteSpace(mpo.VaCode))
                        return OperationResult.Fail("Lệnh sản xuất chưa có mã ExternalId để đồng bộ giữ chỗ.");

                    return await SyncReservationsCoreAsync(mpo, totalQuantity, formulaItems, ct);
                }

        public async Task<OperationResult> SyncReservationsByFormulaItemsAsync(
            ReservationSyncContext ctx,
            decimal totalQuantity,
            IEnumerable<PatchMfgProductionOrderFormulaItemRequest> formulaItems,
            CancellationToken ct = default)
        {
            return await SyncReservationsCoreAsync(ctx, totalQuantity, formulaItems, ct);
        }

        public async Task<OperationResult> ReserveByFormulaMaterialsAsync(
            Guid mfgProductionOrderId,
            decimal totalQuantity,
            IEnumerable<PostManufacturingFormulaMaterial> materials,
            CancellationToken ct = default)
                {
                    if (mfgProductionOrderId == Guid.Empty)
                        return OperationResult.Fail("MfgProductionOrderId không hợp lệ.");

                    if (totalQuantity <= 0)
                        return OperationResult.Fail("TotalQuantity phải lớn hơn 0.");

                    if (materials == null)
                        return OperationResult.Fail("Danh sách nguyên vật liệu không hợp lệ.");

                    static string Norm(string? s) => s?.Trim().ToUpperInvariant() ?? string.Empty;

                    var validItems = materials
                        .Where(x =>
                            x.IsActive &&
                            x.Quantity > 0 &&
                            !string.IsNullOrWhiteSpace(x.MaterialExternalIdSnapshot))
                        .ToList();

                    if (validItems.Count == 0)
                        return OperationResult.Fail("Công thức không có nguyên vật liệu hợp lệ để giữ chỗ.");

                    var reserveReq = new CreateVaSnapshotAndReservations
                    {
                        manufacturingId = mfgProductionOrderId,
                        cancelPreviousOpen = true,
                        reservations = validItems
                            .GroupBy(x => Norm(x.MaterialExternalIdSnapshot))
                            .Select(g => new MfgFormulaMaterialWarehouse
                            {
                                MaterialExternalIdSnapshot = g.Key,
                                TotalQuantity = g.Sum(x => x.Quantity * totalQuantity)
                            })
                            .ToList()
                    };

                    return await ReserveAvailabilityAsync(reserveReq, ct);
                }
    }
}
