using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Queries;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Services
{
    public class DeliveryOrderService : IDeliveryOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExternalIdService _idService; 

        public DeliveryOrderService(IUnitOfWork unitOfWork, IMapper mapper, IExternalIdService idService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _idService = idService;
        }

        /// <summary>
        /// Tạo mới đơn giao hàng (Delivery Order)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<OperationResult> CreateAsync(PostDeliveryOrder req, CancellationToken ct = default)
        {
            if (req is null) throw new ArgumentNullException(nameof(req));
            if (req.postDeliveryOrderDetails is null || req.postDeliveryOrderDetails.Count == 0) throw new ArgumentNullException("MerchandiseOrderId is required");
            if (req.SelectedPOIds is null || req.SelectedPOIds.Count == 0) throw new ArgumentNullException("SelectedPOIds is required");
            if (req.CreateBy == Guid.Empty) throw new ArgumentNullException("CreateBy is required");

            // Bảo đảm tất cả postDeliveryOrderDetails.MerchadiseOrderId có trong SelectedPOIds (Ý là đều thuộc trong PO)
            // Chỉ kiểm tra mapping PO cho dòng non-attach
            var invalidLines = req.postDeliveryOrderDetails
                .Where(l => l.IsAttach == false)
                .Where(l => !l.MerchandiseOrderId.HasValue || l.MerchandiseOrderId.Value == Guid.Empty || !req.SelectedPOIds.Contains(l.MerchandiseOrderId.Value))
                .ToList();

            if (invalidLines.Count > 0)
                throw new ArgumentNullException("SelectedPOIds does not match with non-attach details' MerchandiseOrderId");

            // (Khuyến nghị) kiểm tra CustomerId đồng nhất giữa DO và các PO (nếu chính sách yêu cầu 1 DO/1 Customer)
            // var poCustomers = await _db.MerchandiseOrders
            //   .Where(po => cmd.SelectedPOIds.Contains(po.Id))
            //   .Select(po => po.CustomerId).Distinct().ToListAsync(ct);
            // if (poCustomers.Count > 1) throw new InvalidOperationException("Các PO thuộc nhiều khách hàng khác nhau.");

            var now = DateTime.Now;

            using var tx = await _unitOfWork.BeginTransactionAsync();

            // B1: DO header
            var deliveryOrder = new DeliveryOrder
            {
                Id = Guid.NewGuid(),
                ExternalId = await _idService.NextAsync(req.CompanyId, "PGH", now, ct: ct),
                CustomerId = req.CustomerId,
                CustomerExternalIdSnapShot = req.CustomerExternalIdSnapShot,

                Receiver = req.Receiver,
                DeliveryAddress = req.DeliveryAddress,
                PaymentType = req.PaymentType,
                PaymentDeadline = req.PaymentDeadline,
                TaxNumber = req.TaxNumber,

                Note = req.Note,
                CreatedBy = req.CreateBy,
                CreatedDate = now,
                CompanyId = req.CompanyId,
                Status = DeliveryOrderStatus.Pending.ToString(),
                Deliverers = new List<Deliverer>(), // sẽ gắn sau
                DeliveryOrderPOs = new List<DeliveryOrderPO>(),
                Details = new List<DeliveryOrderDetail>()
            };

            await _unitOfWork.DeliveryOrderRepository.AddAsync(deliveryOrder);

            // B2: Bridge DO–PO
            var linkByPo = new Dictionary<Guid, DeliveryOrderPO>();
            var linkByDeliverer = new Dictionary<Guid, Deliverer>();
            foreach (var poId in req.SelectedPOIds.Distinct())
            {
                var link = new DeliveryOrderPO
                {
                    DeliveryOrderId = deliveryOrder.Id,
                    MerchandiseOrderId = poId,
                    WarehouseRequestId = null
                };
                deliveryOrder.DeliveryOrderPOs.Add(link);
                linkByPo[poId] = link;
            }

            foreach(var deliverer in req.Deliverers.Distinct())
            {
                var link = new Deliverer
                {
                    DeliveryOrderId = deliveryOrder.Id,
                    DelivererInforId = deliverer,
                };
                deliveryOrder.Deliverers.Add(link);
                linkByDeliverer[deliverer] = link;
            }

            // B3: DO Details
            foreach (var line in req.postDeliveryOrderDetails)
            {
                deliveryOrder.Details.Add(new DeliveryOrderDetail
                {
                    Id = Guid.NewGuid(),
                    DeliveryOrderId = deliveryOrder.Id,
                    MerchandiseOrderId = line.MerchandiseOrderId,
                    MerchandiseOrderDetailId = line.MerchandiseOrderDetailId,
                    ProductId = line.ProductId,                 // optional
                    Quantity = line.Quantity,
                    NumOfBags = line.NumOfBags,
                    PONo = line.PONo,
                    ProductExternalIdSnapShot = line.ProductExternalIdSnapShot,
                    ProductNameSnapShot = line.ProductNameSnapShot,
                    //ManufacturingFormulaExternalIdSnapShot = line.ManufacturingFormulaExternalIdSnapShot,
                    LotNoList = line.LocationExternalIdSnapShot,
                    MerchandiseOrderExternalIdSnapShot = line.MerchandiseOrderExternalIdSnapShot,
                    IsActive = true,
                    IsAttach = line.IsAttach                     // <--- NEW
                });
            }

            // ====== (B4) Cập nhật RealQuantity cho MerchandiseOrderDetail ======
            // Gom tổng số lượng DO theo MerchandiseOrderDetailId (chỉ lấy dòng có map vào PO detail, bỏ attach)

            //var sumByMoDetailId = req.postDeliveryOrderDetails
            //    .Where(x => x.IsAttach == false && x.MerchandiseOrderDetailId.HasValue)
            //    .GroupBy(x => x.MerchandiseOrderDetailId!.Value)
            //    .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));




            // Nếu không có dòng nào cần cấp nhật thì bỏ qua
            //if (sumByMoDetailId.Count > 0)
            //{
            //    var moDetailIds = sumByMoDetailId.Keys.ToList();

            //    // Lấy các MerchandiseOrderDetail cần cập nhật (track: true)
            //    var moDetails = await _unitOfWork.MerchandiseOrderRepository
            //        .QueryDetail(track: true)
            //        .Where(d => moDetailIds.Contains(d.MerchandiseOrderDetailId))
            //        .Select(d => new { Entity = d, d.MerchandiseOrderDetailId, d.RealQuantity, d.ExpectedQuantity, d.Status })
            //        .ToListAsync(ct);

            //    // Cập nhật RealQuantity
            //    foreach (var moDetail in moDetails)
            //    {
            //        var add = sumByMoDetailId[moDetail.MerchandiseOrderDetailId];
            //        var current = moDetail.Entity.RealQuantity ?? 0m;

            //        moDetail.Entity.RealQuantity = current + add;
            //        // Cập nhật trạng thái nếu cần (ví dụ: nếu RealQuantity >= ExpectedQuantity thì chuyển sang "Completed")
            //        moDetail.Entity.Status = (moDetail.Entity.RealQuantity >= moDetail.Entity.ExpectedQuantity)
            //            ? MerchadiseStatus.Completed.ToString()
            //            : MerchadiseStatus.Delivering.ToString();
            //    }





            //    var poIds = req.SelectedPOIds.Distinct().ToArray();

            //    // 1) Lấy tất cả detail của các PO liên quan (chỉ lấy POId + DetailId + Status)
            //    //    => lấy từ DB nhưng nhẹ (projection). Track false để rẻ.
            //    var dbDetails = await _unitOfWork.MerchandiseOrderRepository
            //        .QueryDetail(track: false)
            //        .Where(d => poIds.Contains(d.MerchandiseOrderId))
            //        .Select(d => new { d.MerchandiseOrderDetailId, d.MerchandiseOrderId, d.Status })
            //        .ToListAsync(ct);

            //    // 2) Ghi đè status bằng các thay đổi mới nhất trong memory (moDetails bạn vừa set)
            //    var updated = moDetails.ToDictionary(
            //        x => x.MerchandiseOrderDetailId,
            //        x => x.Entity.Status
            //    );


            //    // Hợp nhất: dùng status đã update nếu có, còn không thì dùng status từ DB
            //    var merged = dbDetails.Select(d =>
            //    {
            //        var status = updated.TryGetValue(d.MerchandiseOrderDetailId, out var s)
            //            ? s
            //            : d.Status;
            //        return new { d.MerchandiseOrderId, Status = status };
            //    }).ToList();




            //    var existedDeliveryStatuses = await _unitOfWork.MerchandiseOrderLogRepository
            //        .Query(track: false)
            //        .Where(log => poIds.Contains(log.MerchandiseOrderId)
            //                      && log.Status == MerchadiseStatus.Delivering.ToString())
            //        .Select(log => log.MerchandiseOrderId)
            //        .Distinct()
            //        .ToListAsync(ct);

            //    var needUpdatePoIds = poIds.Except(existedDeliveryStatuses).ToArray();

            //    if (needUpdatePoIds.Length > 0)
            //    {
            //        var allLogs = needUpdatePoIds.Select(id => new MerchandiseOrderLog
            //        {
            //            LogId = Guid.NewGuid(),
            //            MerchandiseOrderId = id,
            //            Status = MerchadiseStatus.Delivering.ToString(),
            //            Note = $"From DO {deliveryOrder.ExternalId}",
            //            CreatedBy = req.CreateBy,
            //            CreatedDate = now
            //        }).ToList();

            //        // (tuỳ bạn) nếu muốn set PO → Delivering (idempotent, nhanh):
            //        await _unitOfWork.MerchandiseOrderRepository.Query(track: false)
            //            .Where(po => poIds.Contains(po.MerchandiseOrderId))
            //            .ExecuteUpdateAsync(s => s
            //                .SetProperty(p => p.Status, _ => MerchadiseStatus.Delivering.ToString())
            //                .SetProperty(p => p.UpdatedBy, _ => req.CreateBy)
            //                .SetProperty(p => p.UpdatedDate, _ => now), ct);

            //        await _unitOfWork.MerchandiseOrderLogRepository.AddRangeAsync(allLogs, ct);
            //    }

            //    // 2) Tính các PO hoàn tất trong memory
            //    var completed = MerchadiseStatus.Completed.ToString();
            //    var completedPoIds = merged
            //        .GroupBy(x => x.MerchandiseOrderId)
            //        .Where(g => g.All(x => x.Status == completed))
            //        .Select(g => g.Key)
            //        .ToList();


            //    // 3) Overwrite các PO này sang Completed
            //    var existedCompletedPoIds = await _unitOfWork.MerchandiseOrderLogRepository.Query(track: false)
            //        .Where(l => completedPoIds.Contains(l.MerchandiseOrderId)
            //                 && l.Status == MerchadiseStatus.Completed.ToString())
            //        .Select(l => l.MerchandiseOrderId)
            //        .Distinct()
            //        .ToListAsync(ct);

            //    var needCompletedLogPoIds = completedPoIds.Except(existedCompletedPoIds).ToArray();

            //    if (needCompletedLogPoIds.Length > 0)
            //    {
            //        var completedLogs = needCompletedLogPoIds.Select(id => new MerchandiseOrderLog
            //        {
            //            LogId = Guid.NewGuid(),
            //            MerchandiseOrderId = id,
            //            Status = MerchadiseStatus.Completed.ToString(),
            //            Note = $"Auto-completed from DO {deliveryOrder.ExternalId}",
            //            CreatedBy = req.CreateBy,
            //            CreatedDate = now
            //        }).ToList();


            //        // (tuỳ bạn) nếu muốn set PO → Completed (idempotent, nhanh):
            //        await _unitOfWork.MerchandiseOrderRepository.Query(track: false)
            //            .Where(po => poIds.Contains(po.MerchandiseOrderId))
            //            .ExecuteUpdateAsync(s => s
            //                .SetProperty(p => p.Status, _ => MerchadiseStatus.Completed.ToString())
            //                .SetProperty(p => p.UpdatedBy, _ => req.CreateBy)
            //                .SetProperty(p => p.UpdatedDate, _ => now), ct);
            //        await _unitOfWork.MerchandiseOrderLogRepository.AddRangeAsync(completedLogs, ct);
            //    }

            //}




            // ====== (B) Tạo 1 WR/PO + WRD từ cấp phát FIFO theo ProductCode & Lot ======
            // Gom nhu cầu theo PO + ProductCode
            var needsByPoProduct = req.postDeliveryOrderDetails.Where(x => x.IsAttach == false)
                .GroupBy(x => new { x.MerchandiseOrderId, x.ProductExternalIdSnapShot }) // <-- bạn nói "lấy ProductCode" để trừ FIFO
                .ToDictionary(g => (g.Key.MerchandiseOrderId, g.Key.ProductExternalIdSnapShot),
                              g => g.Sum(x => x.Quantity));

            // Lấy nhanh map PO→ExternalId để điền RequestName
            var poExternals = await _unitOfWork.MerchandiseOrderRepository
                .Query()
                .Where(po => req.SelectedPOIds.Contains(po.MerchandiseOrderId))
                .Select(po => new { po.MerchandiseOrderId, po.ExternalId })
                .ToDictionaryAsync(x => x.MerchandiseOrderId, x => x.ExternalId, ct);


            var wrByPo = new Dictionary<Guid, WarehouseRequest>();
            foreach (var poId in req.SelectedPOIds.Distinct())
            {
                // Chỉ tạo WR nếu PO này có dòng tính toán (non-attach)
                var poHasCalc = needsByPoProduct.Keys.Any(k => k.MerchandiseOrderId == poId);
                if (!poHasCalc) continue;

                var wr = new WarehouseRequest
                {
                    RequestCode = await _idService.NextAsync(req.CompanyId, "WR", now, ct),
                    ReqStatus = WarehouseRequestStatus.Pending,
                    RequestName = $"WR cho DO {deliveryOrder.ExternalId} – PO {poExternals.GetValueOrDefault(poId)}",
                    IsActive = true,
                    //codeFromRequest = deliveryOrder.ExternalId,
                    ReqType = WareHouseRequestType.ExportForSales,
                    CompanyId = req.CompanyId,
                    CreatedBy = req.CreateBy,
                    CreatedDate = now,
                    WarehouseRequestDetails = new List<WarehouseRequestDetail>()
                };

                // set qua nav để EF tự set WarehouseRequestId vào link
                linkByPo[poId].WarehouseRequest = wr;
                wrByPo[poId] = wr;

                // Tập nhu cầu của PO này theo ProductCode
                // With this block to ensure the key is not null:
                var needs = needsByPoProduct
                    .Where(kv => kv.Key.MerchandiseOrderId == poId)
                    .GroupBy(kv => kv.Key.ProductExternalIdSnapShot ?? string.Empty)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Value));

                // Tập nhu cầu của PO này theo ProductCode (đã có 'needs')
                var needsWithCode = needs
                    .Where(kv => !string.IsNullOrWhiteSpace(kv.Key))
                    .ToDictionary(kv => kv.Key, kv => kv.Value);

                var needsNoCode = needs
                    .Where(kv => string.IsNullOrWhiteSpace(kv.Key))
                    .ToDictionary(kv => kv.Key, kv => kv.Value);

                // 2.1) CẤP PHÁT FIFO CHO NHÓM CÓ PRODUCT CODE (dùng kho thật)
                if (needsWithCode.Count > 0)
                {
                    var allocations = await AllocateReserveFifoAsync(req.CompanyId, deliveryOrder.ExternalId, req.CreateBy,needsWithCode, ct);

                    // Sinh WRD từ allocations (mỗi lô = 1 dòng)
                    foreach (var (productCode, lots) in allocations)
                    {
                        foreach (var a in lots)
                        {
                            wr.WarehouseRequestDetails.Add(new WarehouseRequestDetail
                            {

                                ProductCode = productCode,
                                ProductName = deliveryOrder.Details.FirstOrDefault(s => !string.IsNullOrEmpty(s.ProductNameSnapShot))?.ProductNameSnapShot, // snapshot tên nếu cần, hoặc tra theo master
                                LotNumber = a.LotKey,
                                WeightKg = a.Qty,
                                StockStatus = WareHouseRequestType.ExportForSales.ToString(),
                            });
                        }
                    }
                }

                // 2.2) DÒNG KHÔNG CÓ PRODUCT CODE (hàng ngoài danh mục) từ calcLines (non-attach)
                if (needsNoCode.Count > 0)
                {
                    var snapQty = needsNoCode.Values.Sum();
                    if (snapQty > 0)
                    {
                        var firstLine = req.postDeliveryOrderDetails
                            .FirstOrDefault(x => x.MerchandiseOrderId == poId
                                                 && string.IsNullOrWhiteSpace(x.ProductExternalIdSnapShot));

                        wr.WarehouseRequestDetails.Add(new WarehouseRequestDetail
                        {

                            ProductCode = "",
                            ProductName = firstLine?.ProductNameSnapShot ?? "(No Code Item)",
                            LotNumber = null,
                            WeightKg = snapQty,
                            StockStatus = WareHouseRequestType.ExportForSales.ToString(),
                        });
                    }
                }
            }
            // ====== (C) Lưu & Commit ======
            // Lưu ý: bạn đã AddAsync(deliveryOrder) từ trước; các nav (DOPO/WR/WRD/DODetail)
            // đều bám trong graph deliveryOrder, EF sẽ tự insert đúng thứ tự.
            await _unitOfWork.SaveChangesAsync();
            await tx.CommitAsync(ct);

            return OperationResult.Ok(deliveryOrder.Status);

        }

        /// <summary>
        /// Lấy danh sách đơn giao hàng (Delivery Order) có phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<GetSampleDelivery>> GetAllAsync(DeliveryOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;


                var baseQ = _unitOfWork.DeliveryOrderRepository.Query()
                    .Where(po => po.IsActive == true);

                if (query.CompanyId.HasValue && query.CompanyId.Value != Guid.Empty)
                    baseQ = baseQ.Where(po => po.CompanyId == query.CompanyId);

                if (query.DeliveryOrderId.HasValue && query.DeliveryOrderId.Value != Guid.Empty)
                    baseQ = baseQ.Where(po => po.Id == query.DeliveryOrderId);

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var kw = query.Keyword.Trim();
                    baseQ = baseQ.Where(po =>
                        (po.Customer.CustomerName ?? "").Contains(kw) ||
                        (po.Customer.ExternalId ?? "").Contains(kw) ||
                        (po.ExternalId ?? "").Contains(kw) ||
                        po.Details.Any(p => (p.MerchandiseOrderExternalIdSnapShot ?? "").Contains(kw))

                    );
                }

                // Đếm tổng
                var totalCount = await baseQ.CountAsync(ct);

                // Trang hiện tại: chỉ lấy field cần thiết của header
                var headers = await baseQ
                    .OrderByDescending(po => po.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(po => new GetSampleDelivery
                    {
                        Id = po.Id,
                        ExternalId = po.ExternalId,
                        CustomerExternalIdSnapShot = po.CustomerExternalIdSnapShot,
                        CustomerName = po.Customer.CustomerName,
                        CreatedDate = po.CreatedDate,
                        Note = po.Note,

                        // Gom chuỗi ExtternalId các PO đã link
                        MerchandiseOrderExternalIdList = string.Join(", ",
                            po.DeliveryOrderPOs
                              .Where(dop => dop.MerchandiseOrder != null)
                              .Select(dop => dop.MerchandiseOrder!.ExternalId)
                              .Distinct()
                        ),

                        // Gom chuỗi tên các Deliverer

                        DelivererList = string.Join(", ",
                            po.Deliverers
                              .Where(d => !string.IsNullOrEmpty(d.DelivererInfor.Name))
                              .Select(d => d.DelivererInfor.Name)
                              .Distinct()
                        ),

                        // Deadline thanh toán có thể lấy từ PO nếu cần
                        PaymentDeadline = po.DeliveryOrderPOs
                            .Select(dop => dop.MerchandiseOrder.PaymentType)
                            .FirstOrDefault()
                    })
                    .ToListAsync(ct);

                if (headers.Count == 0)
                    return new PagedResult<GetSampleDelivery>(headers, totalCount, query.PageNumber, query.PageSize);


                return new PagedResult<GetSampleDelivery>(headers, totalCount, query.PageNumber, query.PageSize);

            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách đơn giao hàng.", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách người giao hàng (Deliverer) có phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<GetDeliverer>> GetAllDelivererAsync(DelivererQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var repository = _unitOfWork.DelivererInforRepository.Query();

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();

                    repository = repository.Where(d =>
                        d.Name.Contains(keyword)
                    );
                }

                int totalItems = await repository.CountAsync(ct);

                var items = await repository
                    .OrderBy(d => d.Name)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(d => new GetDeliverer
                    {
                        Id = d.Id,
                        Name = d.Name,
                        DelivererType = d.DelivererType
                    })
                    .ToListAsync(ct);

                var pagedResult = new PagedResult<GetDeliverer>(
                    items,
                    totalItems,
                    query.PageNumber,
                    query.PageSize
                );

                return pagedResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách người giao hàng.", ex);
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy danh sách đơn hàng (PO) có thể chọn để giao hàng, kèm theo tồn kho
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<GetPODeliveryOrder>> GetSelectableLinesAsync(DeliveryOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var baseQ = _unitOfWork.MerchandiseOrderRepository.Query()
                    .Where(po => po.IsActive == true);

                if (query.CompanyId.HasValue && query.CompanyId.Value != Guid.Empty)
                    baseQ = baseQ.Where(po => po.CompanyId == query.CompanyId);

                if (query.MerchandiseOrderId.HasValue && query.MerchandiseOrderId.Value != Guid.Empty)
                    baseQ = baseQ.Where(po => po.MerchandiseOrderId == query.MerchandiseOrderId.Value);

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var kw = query.Keyword.Trim();
                    baseQ = baseQ.Where(po =>
                        (po.CustomerNameSnapshot ?? "").Contains(kw) ||
                        (po.CustomerExternalIdSnapshot ?? "").Contains(kw) ||
                        (po.ExternalId ?? "").Contains(kw) ||
                        (po.PONo ?? "").Contains(kw)
                    );
                }

                // Đếm tổng
                var totalCount = await baseQ.CountAsync(ct);

                // Trang hiện tại: chỉ lấy field cần thiết của header
                var headers = await baseQ
                    .OrderByDescending(po => po.CreateDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(po => new GetPODeliveryOrder
                    {
                        MerchandiseOrderId = po.MerchandiseOrderId,
                        ExternalId = po.ExternalId,

                        CustomerId = po.CustomerId,
                        CustomerNameSnapshot = po.CustomerNameSnapshot,
                        CustomerExternalIdSnapshot = po.CustomerExternalIdSnapshot,
                        PhoneSnapshot = po.PhoneSnapshot,

                        Note = po.Note,
                        ShippingMethod = po.ShippingMethod,
                        PONo = po.PONo,

                        Receiver = po.Receiver,
                        DeliveryAddress = po.DeliveryAddress,
                        PaymentType = po.PaymentType,

                        Status = po.Status,
                        Currency = po.Currency,

                        PODetailDeliveryOrder = new List<GetPODetailDeliveryOrder>() // sẽ gắn sau
                    })
                    .ToListAsync(ct);

                if (headers.Count == 0)
                    return new PagedResult<GetPODeliveryOrder>(headers, totalCount, query.PageNumber, query.PageSize);

                var pagePoIds = headers.Select(h => h.MerchandiseOrderId).ToList();

                // -------- Subquery: TỒN kho theo Product (đổi DbSet/cột cho khớp hệ thống của bạn) ----------

                //var onHandPerProduct = _unitOfWork.WarehouseShelfStockRepository.Query()
                //        .Where(b => !string.IsNullOrWhiteSpace(b.Code))           // bỏ code rỗng
                //        .GroupBy(b => b.Code.Trim())                              // chuẩn hoá
                //        .Select(g => new {
                //            ProductCode = g.Key,
                //            OnHandQty = g.Sum(x => (decimal?)x.QtyKg)
                //        });

                var onHandPerProduct = _unitOfWork.WarehouseShelfStockRepository.Query()
                        .Where(b => !string.IsNullOrWhiteSpace(b.Code)
                                    && (!query.CompanyId.HasValue || b.CompanyId == query.CompanyId.Value))
                        .GroupBy(b => b.Code.Trim().ToUpper())
                        .Select(g => new
                        {
                            ProductCode = g.Key,
                            OnHandQty = g.Sum(x => (decimal?)x.QtyKg) ?? 0m
                        });


                // -------- Subquery: OPEN RESERVATIONS (trừ kho ảo) theo Product Code ----------
                var openReservePerProduct =
                    _unitOfWork.WarehouseTempStockRepository.Query()
                        .Where(t => t.ReserveStatus == ReserveStatus.Open.ToString()
                                    && !string.IsNullOrWhiteSpace(t.Code)
                                    && (!query.CompanyId.HasValue || t.CompanyId == query.CompanyId.Value))
                        .GroupBy(t => t.Code!.Trim().ToUpper())
                        .Select(g => new
                        {
                            ProductCode = g.Key,
                            OpenQty = g.Sum(x => (decimal?)x.QtyRequest) ?? 0m
                        });



                // -------- Lấy detail + JOIN cả on-hand và open-reserve ----------
                var detailRowsFlat = await (
                    from d in _unitOfWork.MerchandiseOrderRepository.QueryDetail().AsNoTracking()
                    join po in _unitOfWork.MerchandiseOrderRepository.Query().AsNoTracking()
                        on d.MerchandiseOrderId equals po.MerchandiseOrderId
                    where pagePoIds.Contains(d.MerchandiseOrderId) && ((bool?)d.IsActive ?? false)

                    // JOIN theo ProductExternalIdSnapshot (string) đã chuẩn hoá
                    join bal in onHandPerProduct
                        on (d.ProductExternalIdSnapshot ?? "").Trim().ToUpper() equals bal.ProductCode into jBal
                    from balance in jBal.DefaultIfEmpty()

                    join rsv in openReservePerProduct
                        on (d.ProductExternalIdSnapshot ?? "").Trim().ToUpper() equals rsv.ProductCode into jRsv
                    from reserve in jRsv.DefaultIfEmpty()

                    select new
                    {
                        d.MerchandiseOrderId,
                        ExternalId = po.ExternalId,
                        PONo = po.PONo,

                        MerchandiseOrderDetailId = d.MerchandiseOrderDetailId,
                        ProductId = d.ProductId,
                        ProductExternalIdSnapshot = d.ProductExternalIdSnapshot,
                        ProductNameSnapshot = d.ProductNameSnapshot,
                        FormulaExternalIdSnapshot = d.FormulaExternalIdSnapshot,

                        Quantity = (decimal?)d.ExpectedQuantity ?? 0m,
                        IsActiveDetail = (bool?)d.IsActive ?? false,

                        OnHandQty = (decimal?)balance.OnHandQty ?? 0m,
                        OpenQty = (decimal?)reserve.OpenQty ?? 0m,

                        // clamp về 0 để không âm nếu dữ liệu lệch
                        AvailableQty =
                            (((decimal?)balance.OnHandQty ?? 0m) - ((decimal?)reserve.OpenQty ?? 0m)) < 0m
                            ? 0m
                            : (((decimal?)balance.OnHandQty ?? 0m) - ((decimal?)reserve.OpenQty ?? 0m))
                    }
                ).OrderBy(x => x.MerchandiseOrderId)
                 .ToListAsync(ct);

                // ===== Gắn detail vào headers =====
                var headerMap = headers.ToDictionary(h => h.MerchandiseOrderId, h => h);

                // ===== Gắn detail vào headers =====
                foreach (var r in detailRowsFlat)
                {
                    if (!headerMap.TryGetValue(r.MerchandiseOrderId, out var hdr)) continue;

                    hdr.PODetailDeliveryOrder.Add(new GetPODetailDeliveryOrder
                    {
                        MerchandiseOrderId = r.MerchandiseOrderId,
                        MerchandiseOrderExternalIdSnapShot = r.ExternalId,

                        MerchandiseOrderDetailId = r.MerchandiseOrderDetailId,
                        ProductId = r.ProductId,
                        ProductExternalIdSnapShot = r.ProductExternalIdSnapshot,
                        ProductNameSnapShot = r.ProductNameSnapshot,

                        ManufacturingFormulaExternalIdSnapShot = r.FormulaExternalIdSnapshot,
                        LocationNameSnapShot = null,
                        PONo = r.PONo,

                        Quantity = r.Quantity,
                        StockQuantity = r.AvailableQty,   // <-- OnHand - Open (đã clamp >= 0)
                        IsActive = r.IsActiveDetail
                    });
                }

                // ===== Trả về PagedResult =====
                return new PagedResult<GetPODeliveryOrder>(headers, totalCount, query.PageNumber, query.PageSize);
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy chi tiết đơn giao hàng (Delivery Order) theo Id
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<GetDeliveryOrder> GetAsync(Guid query, CancellationToken ct = default)
        {
            // Validate tối thiểu
            if (query == Guid.Empty) throw new ArgumentNullException(nameof(query));

            var baseQ = _unitOfWork.DeliveryOrderRepository.Query()
                .Where(po => po.IsActive && po.Id == query);


            // Nếu không truyền Id, lấy bản mới nhất theo CreateDate (tùy nhu cầu)
            //baseQ = baseQ.OrderByDescending(po => po.CreatedDate);

            var dto = await baseQ
                .Select(po => new GetDeliveryOrder
                {
                    Id = po.Id,
                    ExternalId = po.ExternalId,
                    CompanyId = po.CompanyId,
                    CustomerId = po.CustomerId,
                    CustomerExternalIdSnapShot = po.CustomerExternalIdSnapShot,
                    Note = po.Note,
                    IsActive = po.IsActive,
                    CreateBy = po.CreatedBy,
                    CreateDate = po.CreatedDate,
                    

                    Receiver = po.Receiver, 
                    DeliveryAddress = po.DeliveryAddress,
                    PaymentType = po.PaymentType,
                    PaymentDeadline = po.PaymentDeadline,
                    TaxNumber = po.TaxNumber,
                    Status = po.Status,

                    // Lấy danh sách ExternalId của các PO gán cho DO (distinct) rồi join
                    MerchandiseOrderExternalIdList = string.Join(", ",
                        po.DeliveryOrderPOs
                          .Select(x => x.MerchandiseOrder.ExternalId)
                          .Where(s => s != null && s != "")
                          .Distinct()),

                    // Details
                    Details = po.Details
                        .OrderBy(d => d.Id) // hoặc theo PONo, tùy ý
                        .Select(d => new GetDeliveryOrderDetail
                        {
                            ProductId = d.ProductId,
                            ProductExternalIdSnapShot = d.ProductExternalIdSnapShot,
                            ProductNameSnapShot = d.ProductNameSnapShot,
                            LotNoList = d.LotNoList,
                            PONo = d.PONo,
                            Quantity = d.Quantity,
                            NumOfBags = d.NumOfBags
                        }).ToList(),

                    // Deliverers (many-to-many)
                    Deliverers = po.Deliverers
                        .Select(del => new GetDeliverer
                        {
                            Id = del.DelivererInforId,
                            Name = del.DelivererInfor.Name,
                            DelivererType = del.DelivererInfor.DelivererType
                        }).ToList(),


                })
                .FirstOrDefaultAsync(ct);

            return dto; // null nếu không tìm thấy
        }

        /// <summary>
        /// Thay đổi thông tin đơn giao hàng (Delivery Order)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateAsync(PatchDeliveryOrder req, CancellationToken ct = default)
        {
            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                var existingDO = await _unitOfWork.DeliveryOrderRepository
                    .Query(track: true)
                    .Where(p => p.Id == req.Id && p.IsActive == true)
                    .FirstOrDefaultAsync(ct);

                if (existingDO == null)
                    return OperationResult.Fail("Đơn giao hàng không tồn tại.");

                existingDO.UpdatedDate = now;
                existingDO.UpdatedBy = req.UpdateBy;

                PatchHelper.SetIfRef(req.Note, () => existingDO.Note, v => existingDO.Note = v);
                PatchHelper.SetIfRef(req.Status, () => existingDO.Status, v => existingDO.Status = v ?? string.Empty);

                //PatchHelper.SetIf(req.IsActive, () => existingDO.IsActive, v => existingDO.IsActive = v);
                // ===== Đồng bộ Deliverers =====
                // Quy ước PATCH:
                // - req.Deliverers == null  => KHÔNG đụng vào danh sách Deliverers (giữ nguyên)
                // - req.Deliverers != null  => xem như "set" lại toàn bộ danh sách: add mới những cái có trong req mà chưa có, xóa cái không còn trong req
                if (req.Deliverers != null)
                {
                    // Chuẩn hoá input: Distinct & loại Guid.Empty
                    var incomingIds = req.Deliverers
                        .Where(id => id != Guid.Empty)
                        .Distinct()
                        .ToList();

                    // (Khuyến nghị) Kiểm tra tồn tại DelivererInfor + đúng Company
                    // Nếu bạn không có cột CompanyId trong DelivererInfor thì bỏ filter CompanyId.
                    var validInforIds = await _unitOfWork.DelivererInforRepository
                        .Query(track: false)
                        .Where(x => incomingIds.Contains(x.Id))
                        .Select(x => x.Id)
                        .ToListAsync(ct);

                    if (validInforIds.Count != incomingIds.Count)
                    {
                        var missing = incomingIds.Except(validInforIds).ToList();
                        return OperationResult.Fail($"DelivererInforId không hợp lệ hoặc không thuộc công ty: {string.Join(", ", missing)}");
                    }

                    // Tập hiện có trong DB
                    var existingIds = existingDO.Deliverers.Select(d => d.DelivererInforId).ToList();

                    // Tính chênh lệch
                    var toAddIds = validInforIds.Except(existingIds).ToList();
                    var toRemoveIds = existingIds.Except(validInforIds).ToList();

                    // Xoá những Deliverer không còn trong req
                    if (toRemoveIds.Count > 0)
                    {
                        var removeEntities = existingDO.Deliverers
                            .Where(d => toRemoveIds.Contains(d.DelivererInforId))
                            .ToList();

                        foreach (var del in removeEntities)
                            await _unitOfWork.DelivererRepository.RemoveAsync(del);
                    }

                    // Thêm mới những DelivererInforId chưa có
                    foreach (var id in toAddIds)
                    {
                        existingDO.Deliverers.Add(new Deliverer
                        {
                            Id = Guid.NewGuid(),
                            DeliveryOrderId = existingDO.Id,
                            DelivererInforId = id
                        });
                    }
                }
                return OperationResult.Ok();

            }

            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult.Fail($"DelivererInforId không hợp lệ hoặc không thuộc công ty: {string.Join(", ", ex.InnerException?.Message)}");
            }

        }

        /// <summary>
        /// Xóa mềm đơn giao hàng (Delivery Order), cùng với các WR/WRD và giải phóng TempStock liên quan
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> SoftDeleteAsync(Guid id, CancellationToken ct = default)
        {
            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var existingDO = await _unitOfWork.DeliveryOrderRepository
                    .Query(track: true)
                    .Include(x => x.Details)
                    .Include(x => x.DeliveryOrderPOs)
                    .Include(x => x.Deliverers)
                    .Where(p => p.Id == id && p.IsActive == true)
                    .FirstOrDefaultAsync(ct);

                if (existingDO == null)
                    return OperationResult.Fail("Đơn giao hàng không tồn tại.");


                // 2) Guard nghiệp vụ: đã xuất kho thật?
                //var hasPostedIssue = await _unitOfWork.WarehouseRequestRepository.Query(track: false)
                //    .AnyAsync(wr => wr.codeFromRequest == existingDO.ExternalId && wr.ReqStatus == WarehouseRequestStatus.Approved, ct);

                var hasPostedIssue = existingDO.DeliveryOrderPOs
                    .Any(dop => dop.WarehouseRequest != null && dop.WarehouseRequest.ReqStatus == WarehouseRequestStatus.Approved);

                if (hasPostedIssue)
                    return OperationResult.Fail("Không thể xoá đơn giao hàng đã được xuất kho.");

                // 3) lấy tất cả các WR liên quan để xoá mềm
                var wrIds = existingDO.DeliveryOrderPOs
                    .Where(p => p.WarehouseRequestId.HasValue)
                    .Select(p => p.WarehouseRequestId!.Value)
                    .Distinct()
                    .ToList();

                var wrExternalId = existingDO.DeliveryOrderPOs
                    .Where(p => !string.IsNullOrEmpty(p.DeliveryOrder.ExternalId))
                    .Select(p => p.DeliveryOrder.ExternalId)
                    .Distinct()
                    .ToList();

                // Release tất cả các TempStock liên quan
                if(wrIds.Count > 0)
                {
                    await ReleaseTempStockByWRIdsAsync(wrExternalId, ct);

                    // Soft delete các WR + WRD liên quan
                    var wrs = await _unitOfWork.WarehouseRequestRepository.Query(track: true)
                        .Where(wr => wrIds.Contains(wr.RequestId))
                        .Include(wr => wr.WarehouseRequestDetails)
                        .ToListAsync(ct);

                    foreach (var wr in wrs)
                    {
                        wr.IsActive = false;
                        wr.ReqStatus = WarehouseRequestStatus.Cancelled;
                        wr.UpdatedDate = DateTime.Now;
                        wr.UpdatedBy = existingDO.UpdatedBy;
                    }

                    var wrDetail = await _unitOfWork.WarehouseRequestDetailRepository.Query(track: true)
                        .Where(wrd => wrIds.Contains(wrd.RequestId))
                        .ToListAsync(ct);

                    foreach (var wd in wrDetail)
                    {
                        wd.IsActive = false;
                    }
                }

                existingDO.IsActive = false;
                existingDO.UpdatedDate = DateTime.Now;
                existingDO.Status = "Cancelled";
                existingDO.UpdatedBy = existingDO.UpdatedBy;

                foreach (var dd in existingDO.Details.Where(x => x.IsActive))
                {
                    dd.IsActive = false;
                }

                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult.Fail($"Lỗi khi xoá đơn giao hàng: {ex.InnerException?.Message}");
            }
        }

        // Bạn có thể đổi record cho hợp style dự án
        private sealed record ShelfAlloc(
            int SlotId,
            string ProductCode,
            string LotKey,
            decimal Qty,
            StockType StockType
        );

        /// <summary>
        /// Tìm và cấp phát tồn kho FIFO cho các sản phẩm trong needsByProductCode,
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="vaCode"></param>
        /// <param name="createdBy"></param>
        /// <param name="needsByProductCode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, List<ShelfAlloc>>> AllocateReserveFifoAsync(
            Guid companyId,
            string vaCode,
            Guid createdBy,
            Dictionary<string, decimal> needsByProductCode,
            CancellationToken ct)
        {
            static string Norm(string? s) => (s ?? "").Trim().ToUpperInvariant();
            // Kết quả: { Code -> [các phân bổ theo lô] }
            var result = needsByProductCode.Keys.ToDictionary(k => Norm(k), _ => new List<ShelfAlloc>());

            var codes = needsByProductCode
                .Where(kv => kv.Value > 0 && !string.IsNullOrWhiteSpace(kv.Key))
                .Select(kv => kv.Key)
                .Distinct()
                .ToList();
            if (codes.Count == 0) return result;
            var vaCodeNorm = Norm(vaCode);

            // 1) Tổng Open hiện tại (đang giữ chỗ) theo Code + Lot
            var openByLot = await _unitOfWork.WarehouseTempStockRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.VaCode == vaCode
                            && codes.Contains(x.Code))
                .GroupBy(t => new { t.Code, t.LotKey })
                .Select(g => new {
                    g.Key.Code,
                    g.Key.LotKey,
                    QtyOpen = g.Sum(x => (decimal?)x.QtyRequest) ?? 0m
                })
                .ToListAsync(ct);

            var openLookup = openByLot
                .GroupBy(x => (Code: Norm(x.Code), Lot: x.LotKey ?? ""))
                .ToDictionary(g => g.Key, g => g.Sum(x => x.QtyOpen));

            // 2) Cấp phát FIFO in-memory + commit bằng UPDATE có điều kiện (atomic)
            var rawLots = await _unitOfWork.WarehouseShelfStockRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && codes.Contains(x.Code)
                            && x.QtyKg > 0)
                .OrderBy(x => x.Code)
                .ThenBy(x => x.UpdatedDate) // có ReceivedDate thì thay vào đây
                .ThenBy(x => x.LotKey)
                .Select(x => new
                {
                    x.SlotId,
                    Code = x.Code,
                    LotKey = x.LotKey,
                    OnHand = x.QtyKg,
                    x.StockType
                })
                .ToListAsync(ct);

            var lots = new List<(int SlotId, string Code, string LotKey, decimal Available, StockType StockType)>();
            foreach (var lot in rawLots)
            {
                var code = Norm(lot.Code);
                var lotKey = lot.LotKey ?? "";
                var open = openLookup.TryGetValue((code, lotKey), out var o) ? o : 0m;
                var available = lot.OnHand - open;
                if (available > 0)
                    lots.Add((lot.SlotId, code, lotKey, available, lot.StockType));
            }

            // 4) Phân bổ FIFO theo Available (KHÔNG trừ tồn thật) → CHÈN Open theo từng lô
            //await using var tx = await _unitOfWork.BeginTransactionAsync();

            var needLeft = needsByProductCode
                .Where(kv => kv.Value > 0 && !string.IsNullOrWhiteSpace(kv.Key))
                .ToDictionary(kv => Norm(kv.Key), kv => kv.Value);

            var toInsert = new List<WarehouseTempStock>();

            foreach (var lot in lots)
            {
                if (!needLeft.TryGetValue(lot.Code, out var need) || need <= 0) continue;

                var take = Math.Min(need, lot.Available);
                if (take <= 0) continue;

                toInsert.Add(new WarehouseTempStock
                {
                    CompanyId = companyId,
                    VaCode = vaCodeNorm,
                    Code = lot.Code,
                    LotKey = lot.LotKey,          // giữ theo lô đã cấp phát
                    QtyRequest = take,
                    ReserveStatus = ReserveStatus.Open.ToString(),  // trừ ảo
                    //LinkedIssueId = lot.SlotId,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now
                });

                result[lot.Code].Add(new ShelfAlloc(lot.SlotId, lot.Code, lot.LotKey, take, lot.StockType));
                needLeft[lot.Code] = need - take;
            }

            // 5) Nếu còn thiếu → báo lỗi (hoặc backorder tùy chính sách)
            //var shortage = needLeft.Where(kv => kv.Value > 0).ToList();
            //if (shortage.Count > 0)
            //{
            //    await tx.RollbackAsync(ct);
            //    var msg = string.Join(", ", shortage.Select(s => $"{s.Key} thiếu {s.Value}"));
            //    throw new ApplicationException($"Không đủ tồn khả dụng (OnHand - Open) để reserve FIFO: {msg}");
            //}

            // 6) Commit: CHỈ INSERT các dòng Open; idempotent theo VaCode + unique index
            await _unitOfWork.WarehouseTempStockRepository.AddRangeAsync(toInsert, ct);
            //await _unitOfWork.SaveChangesAsync();
            //await tx.CommitAsync(ct);

            return result;
        }

        /// <summary>
        /// Xoá các bản ghi TempStock liên quan đến các WarehouseRequestId đã cho, chỉ những bản còn Open
        /// </summary>
        /// <param name="wrIds"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task ReleaseTempStockByWRIdsAsync(List<string> wrIds, CancellationToken ct)
        {
            // wrIds thực chất là các VaCode => đổi tên cho dễ hiểu
            if (wrIds == null || wrIds.Count == 0) return;

            // Chuẩn hoá input: trim, bỏ rỗng, distinct (không phân biệt hoa thường)
            var vaCodes = wrIds
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (vaCodes.Length == 0) return;

            // So sánh không phân biệt HOA/thường (PostgreSQL dịch LOWER(...))
            var vaCodesLower = vaCodes.Select(s => s.ToLower()).ToArray();
            var rows = await _unitOfWork.WarehouseTempStockRepository.Query(track: true)
                .Where(t => t.ReserveStatus == ReserveStatus.Open.ToString()
                         && t.VaCode != null
                         && vaCodesLower.Contains(t.VaCode.ToLower()))
                .ToListAsync(ct);

            foreach (var r in rows)
                r.ReserveStatus = ReserveStatus.Cancelled.ToString();
        }
    }

}
