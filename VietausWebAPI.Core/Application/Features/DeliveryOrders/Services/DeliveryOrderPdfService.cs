using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums.Merchadises;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Services
{
    public class DeliveryOrderPdfService : IDeliveryOrderPdfService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeliveryOrderPdfRenderHelper _pdfRenderHelper;

        public DeliveryOrderPdfService(IUnitOfWork unitOfWork, IDeliveryOrderPdfRenderHelper pdfRenderHelper)
        {
            _unitOfWork = unitOfWork;
            _pdfRenderHelper = pdfRenderHelper;
        }
        
        // Tạm thời cmt
        //public async Task<byte[]> GenerateAsync(Guid deliveryOrderId, CancellationToken ct = default)
        //{
        //    var now = DateTime.Now;

        //    using var tx = await _unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        var vm = await _unitOfWork.DeliveryOrderRepository.Query(track: false)
        //        .Where(x => x.Id == deliveryOrderId)
        //        .Select(x => new PdfPrinterDeliveryOrder
        //        {
        //            Id = x.Id,
        //            ExternalId = x.ExternalId,
        //            CustomerExternalIdSnapShot = x.CustomerExternalIdSnapShot,
        //            DeliveryAddress = x.DeliveryAddress,

        //            CustomerAddress = x.Customer.Addresses.Select(a => a.AddressLine).FirstOrDefault(),
        //            CustomerName = x.Customer.CustomerName,

        //            TaxNumber = x.TaxNumber,
        //            PhoneSnapshot = x.PhoneSnapshot,
        //            Receiver = x.Receiver,
        //            PaymentType = x.PaymentType,
        //            PaymentDeadline = x.PaymentDeadline,
        //            Note = x.Note,

        //            HasPrinted = x.HasPrinted,
        //            CreateBy = x.CreatedBy,
        //            //Details = x.DeliveryOrderPOs
        //            //.Where(dop => dop.IsActive && dop.WarehouseRequest != null)
        //            //.SelectMany(dop => dop.WarehouseRequest!.WarehouseRequestDetails
        //            //    .Where(d => d.IsActive))
        //            //.Select(dod => new PdfPrinterDeliveryOrderDetail
        //            //{
        //            //    ProductCode = dod.ProductCode,
        //            //    ProductName = dod.ProductName,
        //            //    LotNumber = dod.LotNumber,
        //            //    WeightKg = dod.WeightKg,
        //            //    // 🔹 Lấy BagNumber từ DeliveryOrder.Details có cùng ProductCode hoặc LotNumber
        //            //    BagNumber = x.Details
        //            //        .Where(dd => dd.ProductExternalIdSnapShot == dod.ProductCode)
        //            //        .Select(dd => dd.NumOfBags)
        //            //        .FirstOrDefault(),
        //            //    StockStatus = dod.StockStatus,
        //            //    PONoNumber = x.DeliveryOrderPOs
        //            //        .Where(dop => dop.WarehouseRequest != null
        //            //                    && dop.WarehouseRequest!.RequestId == dod.RequestId
        //            //                    && dop.MerchandiseOrder != null)
        //            //        .Select(dop => dop.MerchandiseOrder!.PONo)
        //            //        .FirstOrDefault() ?? string.Empty
        //            //}).ToList(),

        //            DeliveryOrderDetails = x.Details.Select(d => new GetDeliveryOrderDetail
        //            {
        //                // Map properties according to GetDeliveryOrderDetail signature
        //                ProductId = d.ProductId,
        //                ProductExternalIdSnapShot = d.ProductExternalIdSnapShot,
        //                ProductNameSnapShot = d.ProductNameSnapShot,
        //                LotNoList = d.LotNoList,
        //                PONo = d.PONo,
        //                Quantity = d.Quantity,
        //                NumOfBags = d.NumOfBags
        //            }).ToList(),

        //            Deliverers = x.Deliverers.Select(del => new GetDeliverer
        //            {
        //                Id = del.Id,
        //                Name = del.DelivererInfor != null ? del.DelivererInfor.Name : null,
        //            }).ToList(),

        //            // Gom chuỗi ExtternalId các PO đã link
        //            MerchandiseOrderExternalIdList = string.Join(", ",
        //            x.DeliveryOrderPOs
        //                .Where(dop => dop.MerchandiseOrder != null)
        //                .Select(dop => dop.MerchandiseOrder!.ExternalId)
        //                .Distinct()
        //            ),
        //        })
        //        .FirstOrDefaultAsync(ct);


        //        if (vm == null)
        //            throw new Exception("Delivery Order not found.");



        //        // ====== (B4) Cập nhật RealQuantity cho MerchandiseOrderDetail ======
        //        // Gom tổng số lượng DO theo MerchandiseOrderDetailId (chỉ lấy dòng có map vào PO detail, bỏ attach)

        //        //var sumByMoDetailId = req.postDeliveryOrderDetails
        //        //    .Where(x => x.IsAttach == false && x.MerchandiseOrderDetailId.HasValue)
        //        //    .GroupBy(x => x.MerchandiseOrderDetailId!.Value)
        //        //    .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

        //        if (vm.HasPrinted)
        //        {
        //            var sumByMoDetailId = _unitOfWork.DeliveryOrderDetailRepository
        //           .Query(track: false)
        //           .Where(dod => dod.DeliveryOrderId == deliveryOrderId
        //                         && dod.IsAttach == false
        //                         && dod.MerchandiseOrderDetailId != null)
        //           .GroupBy(dod => dod.MerchandiseOrderDetailId!.Value)
        //           .Select(g => new
        //           {
        //               MerchandiseOrderDetailId = g.Key,
        //               TotalQuantity = g.Sum(dod => dod.Quantity)
        //           })
        //           .ToDictionary(x => x.MerchandiseOrderDetailId, x => x.TotalQuantity);

        //            //Nếu không có dòng nào cần cấp nhật thì bỏ qua
        //            if (sumByMoDetailId.Count > 0)
        //            {
        //                var moDetailIds = sumByMoDetailId.Keys.ToList();

        //                // Lấy các MerchandiseOrderDetail cần cập nhật (track: true)
        //                var moDetails = await _unitOfWork.MerchandiseOrderRepository
        //                    .QueryDetail(track: true)
        //                    .Where(d => moDetailIds.Contains(d.MerchandiseOrderDetailId))
        //                    .Select(d => new { Entity = d, d.MerchandiseOrderDetailId, d.RealQuantity, d.ExpectedQuantity, d.Status })
        //                    .ToListAsync(ct);

        //                // Cập nhật RealQuantity
        //                foreach (var moDetail in moDetails)
        //                {
        //                    var add = sumByMoDetailId[moDetail.MerchandiseOrderDetailId];
        //                    var current = moDetail.Entity.RealQuantity ?? 0m;

        //                    moDetail.Entity.RealQuantity = current + add;
        //                    // Cập nhật trạng thái nếu cần (ví dụ: nếu RealQuantity >= ExpectedQuantity thì chuyển sang "Completed")
        //                    moDetail.Entity.Status = (moDetail.Entity.RealQuantity >= moDetail.Entity.ExpectedQuantity)
        //                        ? MerchadiseStatus.Completed.ToString()
        //                        : MerchadiseStatus.Delivering.ToString();
        //                }





        //                //var poIds = req.SelectedPOIds.Distinct().ToArray();

        //                var poIds = moDetails
        //                    .Select(d => d.Entity.MerchandiseOrderId)
        //                    .Distinct()
        //                    .ToArray();

        //                // 1) Lấy tất cả detail của các PO liên quan (chỉ lấy POId + DetailId + Status)
        //                //    => lấy từ DB nhưng nhẹ (projection). Track false để rẻ.
        //                var dbDetails = await _unitOfWork.MerchandiseOrderRepository
        //                    .QueryDetail(track: false)
        //                    .Where(d => poIds.Contains(d.MerchandiseOrderId))
        //                    .Select(d => new { d.MerchandiseOrderDetailId, d.MerchandiseOrderId, d.Status })
        //                    .ToListAsync(ct);

        //                // 2) Ghi đè status bằng các thay đổi mới nhất trong memory (moDetails bạn vừa set)
        //                var updated = moDetails.ToDictionary(
        //                    x => x.MerchandiseOrderDetailId,
        //                    x => x.Entity.Status
        //                );


        //                // Hợp nhất: dùng status đã update nếu có, còn không thì dùng status từ DB
        //                var merged = dbDetails.Select(d =>
        //                {
        //                    var status = updated.TryGetValue(d.MerchandiseOrderDetailId, out var s)
        //                        ? s
        //                        : d.Status;
        //                    return new { d.MerchandiseOrderId, Status = status };
        //                }).ToList();




        //                //var existedDeliveryStatuses = await _unitOfWork.MerchandiseOrderLogRepository
        //                //    .Query(track: false)
        //                //    .Where(log => poIds.Contains(log.MerchandiseOrderId)
        //                //                  && log.Status == MerchadiseStatus.Delivering.ToString())
        //                //    .Select(log => log.MerchandiseOrderId)
        //                //    .Distinct()
        //                //    .ToListAsync(ct);

        //                //var needUpdatePoIds = poIds.Except(existedDeliveryStatuses).ToArray();

        //                //if (needUpdatePoIds.Length > 0)
        //                //{
        //                //    var allLogs = needUpdatePoIds.Select(id => new MerchandiseOrderLog
        //                //    {
        //                //        LogId = Guid.CreateVersion7(),
        //                //        MerchandiseOrderId = id,
        //                //        Status = MerchadiseStatus.Delivering.ToString(),
        //                //        Note = $"From DO {vm.ExternalId}",
        //                //        CreatedBy = vm.CreateBy,
        //                //        CreatedDate = now
        //                //    }).ToList();

        //                //    // (tuỳ bạn) nếu muốn set PO → Delivering (idempotent, nhanh):
        //                //    await _unitOfWork.MerchandiseOrderRepository.Query(track: false)
        //                //        .Where(po => poIds.Contains(po.MerchandiseOrderId))
        //                //        .ExecuteUpdateAsync(s => s
        //                //            .SetProperty(p => p.Status, _ => MerchadiseStatus.Delivering.ToString())
        //                //            .SetProperty(p => p.UpdatedBy, _ => vm.CreateBy)
        //                //            .SetProperty(p => p.UpdatedDate, _ => now), ct);

        //                //    await _unitOfWork.MerchandiseOrderLogRepository.AddRangeAsync(allLogs, ct);
        //                //}

        //                // 2) Tính các PO hoàn tất trong memory
        //                var completed = MerchadiseStatus.Completed.ToString();
        //                var completedPoIds = merged
        //                    .GroupBy(x => x.MerchandiseOrderId)
        //                    .Where(g => g.All(x => x.Status == completed))
        //                    .Select(g => g.Key)
        //                    .ToList();


        //                // 3) Overwrite các PO này sang Completed
        //                //var existedCompletedPoIds = await _unitOfWork.MerchandiseOrderLogRepository.Query(track: false)
        //                //    .Where(l => completedPoIds.Contains(l.MerchandiseOrderId)
        //                //             && l.Status == MerchadiseStatus.Completed.ToString())
        //                //    .Select(l => l.MerchandiseOrderId)
        //                //    .Distinct()
        //                //    .ToListAsync(ct);

        //                //var needCompletedLogPoIds = completedPoIds.Except(existedCompletedPoIds).ToArray();

        //                //if (needCompletedLogPoIds.Length > 0)
        //                //{
        //                //    var completedLogs = needCompletedLogPoIds.Select(id => new MerchandiseOrderLog
        //                //    {
        //                //        LogId = Guid.CreateVersion7(),
        //                //        MerchandiseOrderId = id,
        //                //        Status = MerchadiseStatus.Completed.ToString(),
        //                //        Note = $"Auto-completed from DO {vm.ExternalId}",
        //                //        CreatedBy = vm.CreateBy,
        //                //        CreatedDate = now
        //                //    }).ToList();


        //                //    // (tuỳ bạn) nếu muốn set PO → Completed (idempotent, nhanh):
        //                //    await _unitOfWork.MerchandiseOrderRepository.Query(track: false)
        //                //        .Where(po => poIds.Contains(po.MerchandiseOrderId))
        //                //        .ExecuteUpdateAsync(s => s
        //                //            .SetProperty(p => p.Status, _ => MerchadiseStatus.Completed.ToString())
        //                //            .SetProperty(p => p.UpdatedBy, _ => vm.CreateBy)
        //                //            .SetProperty(p => p.UpdatedDate, _ => now), ct);
        //                //    await _unitOfWork.MerchandiseOrderLogRepository.AddRangeAsync(completedLogs, ct);
        //                //}
        //            }

        //            // 2) Chỉ cập nhật khi CHƯA in lần nào
        //            var rows = await _unitOfWork.DeliveryOrderRepository.Query(track: false)
        //                .Where(x => x.Id == deliveryOrderId && x.HasPrinted == false)   // <<-- điều kiện 1 lần
        //                .ExecuteUpdateAsync(s => s
        //                    .SetProperty(p => p.HasPrinted, p => true)
        //                , ct);
        //            // ====== (C) Lưu & Commit ======
        //            // Lưu ý: bạn đã AddAsync(deliveryOrder) từ trước; các nav (DOPO/WR/WRD/DODetail)
        //            // đều bám trong graph deliveryOrder, EF sẽ tự insert đúng thứ tự.
        //            await _unitOfWork.SaveChangesAsync();
        //            await tx.CommitAsync(ct);
        //        }

        //        // Here you would implement the PDF generation logic using a library like iTextSharp, PdfSharp, etc.

        //        return _pdfRenderHelper.Render(vm);
        //    }
        //    catch (Exception ex)
        //    {
        //        await tx.RollbackAsync();
        //        throw new Exception("Lỗi khi lấy danh sách đơn giao hàng.", ex);
        //    }


        //}
        public async Task<byte[]> GenerateAsync(Guid deliveryOrderId, CancellationToken ct = default)
        {
            var now = DateTime.Now;

            using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var vm = await _unitOfWork.DeliveryOrderRepository.Query(track: false)
                .Where(x => x.Id == deliveryOrderId)
                .Select(x => new PdfPrinterDeliveryOrder
                {
                    Id = x.Id,
                    ExternalId = x.ExternalId,
                    CustomerExternalIdSnapShot = x.CustomerExternalIdSnapShot,
                    DeliveryAddress = x.DeliveryAddress,

                    CustomerAddress = x.Customer.Addresses.Select(a => a.AddressLine).FirstOrDefault(),
                    CustomerName = x.Customer.CustomerName,

                    TaxNumber = x.TaxNumber,
                    PhoneSnapshot = x.PhoneSnapshot,
                    Receiver = x.Receiver,
                    PaymentType = x.PaymentType,
                    PaymentDeadline = x.PaymentDeadline,
                    Note = x.Note,

                    HasPrinted = x.HasPrinted,
                    CreateBy = x.CreatedBy,

                    Details = x.Details.Select(d => new PdfPrinterDeliveryOrderDetail
                    {
                        ProductCode = d.ProductExternalIdSnapShot ?? string.Empty,
                        ProductName = d.ProductNameSnapShot ?? string.Empty,
                        LotNumber = d.LotNoList ?? string.Empty,
                        WeightKg = d.Quantity,
                        BagNumber = d.NumOfBags,
                        StockStatus = d.MerchandiseOrderDetail != null ? d.MerchandiseOrderDetail.Status : string.Empty,
                        PONoNumber = d.PONo ?? string.Empty
                    }).ToList(),

                    //Details = x.DeliveryOrderPOs
                    //.Where(dop => dop.IsActive && dop.WarehouseRequest != null)
                    //.SelectMany(dop => dop.WarehouseRequest!.WarehouseRequestDetails
                    //    .Where(d => d.IsActive))
                    //.Select(dod => new PdfPrinterDeliveryOrderDetail
                    //{
                    //    ProductCode = dod.ProductCode,
                    //    ProductName = dod.ProductName,
                    //    LotNumber = dod.LotNumber,
                    //    WeightKg = dod.WeightKg,
                    //    // 🔹 Lấy BagNumber từ DeliveryOrder.Details có cùng ProductCode hoặc LotNumber
                    //    BagNumber = x.Details
                    //        .Where(dd => dd.ProductExternalIdSnapShot == dod.ProductCode)
                    //        .Select(dd => dd.NumOfBags)
                    //        .FirstOrDefault(),
                    //    StockStatus = dod.StockStatus,
                    //    PONoNumber = x.DeliveryOrderPOs
                    //        .Where(dop => dop.WarehouseRequest != null
                    //                    && dop.WarehouseRequest!.RequestId == dod.RequestId
                    //                    && dop.MerchandiseOrder != null)
                    //        .Select(dop => dop.MerchandiseOrder!.PONo)
                    //        .FirstOrDefault() ?? string.Empty
                    //}).ToList(),

                    DeliveryOrderDetails = x.Details.Select(d => new GetDeliveryOrderDetail
                    {
                        // Map properties according to GetDeliveryOrderDetail signature
                        ProductId = d.ProductId,
                        ProductExternalIdSnapShot = d.ProductExternalIdSnapShot,
                        ProductNameSnapShot = d.ProductNameSnapShot,
                        LotNoList = d.LotNoList,
                        PONo = d.PONo,
                        Quantity = d.Quantity,
                        NumOfBags = d.NumOfBags
                    }).ToList(),

                    Deliverers = x.Deliverers.Select(del => new GetDeliverer
                    {
                        Id = del.Id,
                        Name = del.DelivererInfor != null ? del.DelivererInfor.Name : null,
                    }).ToList(),

                    // Gom chuỗi ExtternalId các PO đã link
                    MerchandiseOrderExternalIdList = string.Join(", ",
                    x.DeliveryOrderPOs
                        .Where(dop => dop.MerchandiseOrder != null)
                        .Select(dop => dop.MerchandiseOrder!.ExternalId)
                        .Distinct()
                    ),
                })
                .FirstOrDefaultAsync(ct);


                if (vm == null)
                    throw new Exception("Delivery Order not found.");



                // ====== (B4) Cập nhật RealQuantity cho MerchandiseOrderDetail ======
                // Gom tổng số lượng DO theo MerchandiseOrderDetailId (chỉ lấy dòng có map vào PO detail, bỏ attach)

                //var sumByMoDetailId = req.postDeliveryOrderDetails
                //    .Where(x => x.IsAttach == false && x.MerchandiseOrderDetailId.HasValue)
                //    .GroupBy(x => x.MerchandiseOrderDetailId!.Value)
                //    .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

                if (vm.HasPrinted)
                {
                    var sumByMoDetailId = _unitOfWork.DeliveryOrderDetailRepository
                   .Query(track: false)
                   .Where(dod => dod.DeliveryOrderId == deliveryOrderId
                                 && dod.IsAttach == false
                                 && dod.MerchandiseOrderDetailId != null)
                   .GroupBy(dod => dod.MerchandiseOrderDetailId!.Value)
                   .Select(g => new
                   {
                       MerchandiseOrderDetailId = g.Key,
                       TotalQuantity = g.Sum(dod => dod.Quantity)
                   })
                   .ToDictionary(x => x.MerchandiseOrderDetailId, x => x.TotalQuantity);

                    //Nếu không có dòng nào cần cấp nhật thì bỏ qua
                    if (sumByMoDetailId.Count > 0)
                    {
                        var moDetailIds = sumByMoDetailId.Keys.ToList();

                        // Lấy các MerchandiseOrderDetail cần cập nhật (track: true)
                        var moDetails = await _unitOfWork.MerchandiseOrderRepository
                            .QueryDetail(track: true)
                            .Where(d => moDetailIds.Contains(d.MerchandiseOrderDetailId))
                            .Select(d => new { Entity = d, d.MerchandiseOrderDetailId, d.RealQuantity, d.ExpectedQuantity, d.Status })
                            .ToListAsync(ct);

                        // Cập nhật RealQuantity
                        foreach (var moDetail in moDetails)
                        {
                            var add = sumByMoDetailId[moDetail.MerchandiseOrderDetailId];
                            var current = moDetail.Entity.RealQuantity ?? 0m;

                            moDetail.Entity.RealQuantity = current + add;
                            // Cập nhật trạng thái nếu cần (ví dụ: nếu RealQuantity >= ExpectedQuantity thì chuyển sang "Completed")
                            moDetail.Entity.Status = (moDetail.Entity.RealQuantity >= moDetail.Entity.ExpectedQuantity)
                                ? MerchadiseStatus.Completed.ToString()
                                : MerchadiseStatus.Delivering.ToString();
                        }





                        //var poIds = req.SelectedPOIds.Distinct().ToArray();

                        var poIds = moDetails
                            .Select(d => d.Entity.MerchandiseOrderId)
                            .Distinct()
                            .ToArray();

                        // 1) Lấy tất cả detail của các PO liên quan (chỉ lấy POId + DetailId + Status)
                        //    => lấy từ DB nhưng nhẹ (projection). Track false để rẻ.
                        var dbDetails = await _unitOfWork.MerchandiseOrderRepository
                            .QueryDetail(track: false)
                            .Where(d => poIds.Contains(d.MerchandiseOrderId))
                            .Select(d => new { d.MerchandiseOrderDetailId, d.MerchandiseOrderId, d.Status })
                            .ToListAsync(ct);

                        // 2) Ghi đè status bằng các thay đổi mới nhất trong memory (moDetails bạn vừa set)
                        var updated = moDetails.ToDictionary(
                            x => x.MerchandiseOrderDetailId,
                            x => x.Entity.Status
                        );


                        // Hợp nhất: dùng status đã update nếu có, còn không thì dùng status từ DB
                        var merged = dbDetails.Select(d =>
                        {
                            var status = updated.TryGetValue(d.MerchandiseOrderDetailId, out var s)
                                ? s
                                : d.Status;
                            return new { d.MerchandiseOrderId, Status = status };
                        }).ToList();




                        //var existedDeliveryStatuses = await _unitOfWork.MerchandiseOrderLogRepository
                        //    .Query(track: false)
                        //    .Where(log => poIds.Contains(log.MerchandiseOrderId)
                        //                  && log.Status == MerchadiseStatus.Delivering.ToString())
                        //    .Select(log => log.MerchandiseOrderId)
                        //    .Distinct()
                        //    .ToListAsync(ct);

                        //var needUpdatePoIds = poIds.Except(existedDeliveryStatuses).ToArray();

                        //if (needUpdatePoIds.Length > 0)
                        //{
                        //    var allLogs = needUpdatePoIds.Select(id => new MerchandiseOrderLog
                        //    {
                        //        LogId = Guid.CreateVersion7(),
                        //        MerchandiseOrderId = id,
                        //        Status = MerchadiseStatus.Delivering.ToString(),
                        //        Note = $"From DO {vm.ExternalId}",
                        //        CreatedBy = vm.CreateBy,
                        //        CreatedDate = now
                        //    }).ToList();

                        //    // (tuỳ bạn) nếu muốn set PO → Delivering (idempotent, nhanh):
                        //    await _unitOfWork.MerchandiseOrderRepository.Query(track: false)
                        //        .Where(po => poIds.Contains(po.MerchandiseOrderId))
                        //        .ExecuteUpdateAsync(s => s
                        //            .SetProperty(p => p.Status, _ => MerchadiseStatus.Delivering.ToString())
                        //            .SetProperty(p => p.UpdatedBy, _ => vm.CreateBy)
                        //            .SetProperty(p => p.UpdatedDate, _ => now), ct);

                        //    await _unitOfWork.MerchandiseOrderLogRepository.AddRangeAsync(allLogs, ct);
                        //}

                        // 2) Tính các PO hoàn tất trong memory
                        var completed = MerchadiseStatus.Completed.ToString();
                        var completedPoIds = merged
                            .GroupBy(x => x.MerchandiseOrderId)
                            .Where(g => g.All(x => x.Status == completed))
                            .Select(g => g.Key)
                            .ToList();


                        // 3) Overwrite các PO này sang Completed
                        //var existedCompletedPoIds = await _unitOfWork.MerchandiseOrderLogRepository.Query(track: false)
                        //    .Where(l => completedPoIds.Contains(l.MerchandiseOrderId)
                        //             && l.Status == MerchadiseStatus.Completed.ToString())
                        //    .Select(l => l.MerchandiseOrderId)
                        //    .Distinct()
                        //    .ToListAsync(ct);

                        //var needCompletedLogPoIds = completedPoIds.Except(existedCompletedPoIds).ToArray();

                        //if (needCompletedLogPoIds.Length > 0)
                        //{
                        //    var completedLogs = needCompletedLogPoIds.Select(id => new MerchandiseOrderLog
                        //    {
                        //        LogId = Guid.CreateVersion7(),
                        //        MerchandiseOrderId = id,
                        //        Status = MerchadiseStatus.Completed.ToString(),
                        //        Note = $"Auto-completed from DO {vm.ExternalId}",
                        //        CreatedBy = vm.CreateBy,
                        //        CreatedDate = now
                        //    }).ToList();


                        //    // (tuỳ bạn) nếu muốn set PO → Completed (idempotent, nhanh):
                        //    await _unitOfWork.MerchandiseOrderRepository.Query(track: false)
                        //        .Where(po => poIds.Contains(po.MerchandiseOrderId))
                        //        .ExecuteUpdateAsync(s => s
                        //            .SetProperty(p => p.Status, _ => MerchadiseStatus.Completed.ToString())
                        //            .SetProperty(p => p.UpdatedBy, _ => vm.CreateBy)
                        //            .SetProperty(p => p.UpdatedDate, _ => now), ct);
                        //    await _unitOfWork.MerchandiseOrderLogRepository.AddRangeAsync(completedLogs, ct);
                        //}
                    }

                    // 2) Chỉ cập nhật khi CHƯA in lần nào
                    var rows = await _unitOfWork.DeliveryOrderRepository.Query(track: false)
                        .Where(x => x.Id == deliveryOrderId && x.HasPrinted == false)   // <<-- điều kiện 1 lần
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(p => p.HasPrinted, p => true)
                        , ct);
                    // ====== (C) Lưu & Commit ======
                    // Lưu ý: bạn đã AddAsync(deliveryOrder) từ trước; các nav (DOPO/WR/WRD/DODetail)
                    // đều bám trong graph deliveryOrder, EF sẽ tự insert đúng thứ tự.
                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync(ct);
                }

                // Here you would implement the PDF generation logic using a library like iTextSharp, PdfSharp, etc.

                return _pdfRenderHelper.Render(vm);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw new Exception("Lỗi khi lấy danh sách đơn giao hàng.", ex);
            }


        }
    }
}
