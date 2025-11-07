using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Queries;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.Material_warehouse;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Queries;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExternalIdService _idService;
        private readonly ITimelineService _TimelineService;

        public PurchaseOrderService(IUnitOfWork unitOfWork, IExternalIdService idService, ITimelineService timelineService)
        {
            _unitOfWork = unitOfWork;
            _idService = idService;
            _TimelineService = timelineService;
        }

        /// <summary>
        /// Tạo mới đơn mua hàng
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> CreateAsync(PostPurchaseOrder req, CancellationToken ct = default)
        {
            try
            {
                if (req is null) throw new ArgumentNullException(nameof(req));

                if (req.PurchaseOrderDetails.Count == 0) throw new InvalidOperationException("PO phải có ít nhất 1 dòng chi tiết.");
                if (req.PurchaseOrderDetails.Any(d => d.UnitPriceAgreed is null))
                    throw new InvalidOperationException("Thiếu UnitPriceAgreed trong chi tiết.");


                var now = DateTime.Now;

                using var transaction = await _unitOfWork.BeginTransactionAsync();

                PurchaseOrderSnapshot? snapshot = null;

                if (req.PurchaseOrderSnapshot is not null)
                {
                    snapshot = new PurchaseOrderSnapshot
                    {
                        EmployeeExternalIdSnapshot = req.PurchaseOrderSnapshot.EmployeeExternalIdSnapshot,
                        EmployeeFullNameSnapshot = req.PurchaseOrderSnapshot.EmployeeFullNameSnapshot,
                        PhoneNumberSnapshot = req.PurchaseOrderSnapshot.PhoneNumberSnapshot,
                        EmailSnapshot = req.PurchaseOrderSnapshot.EmailSnapshot,
                        SupplierExternalIdSnapshot = req.PurchaseOrderSnapshot.SupplierExternalIdSnapshot,
                        SupplierNameSnapshot = req.PurchaseOrderSnapshot.SupplierNameSnapshot,
                        SupplierContactSnapshot = req.PurchaseOrderSnapshot.SupplierContactSnapshot,
                        SupplierPhoneNumberSnapshot = req.PurchaseOrderSnapshot.SupplierPhoneNumberSnapshot,
                        SupplierAddressSnapshot = req.PurchaseOrderSnapshot.SupplierAddressSnapshot,
                        TotalPrice = req.PurchaseOrderSnapshot.TotalPrice,
                        DeliveryAddress = req.PurchaseOrderSnapshot.DeliveryAddress,
                        PaymentTypes = req.PurchaseOrderSnapshot.PaymentTypes,
                        Vat = req.PurchaseOrderSnapshot.Vat,
                    };
                    await _unitOfWork.PurchaseOrderSnapshotRepository.AddAsync(snapshot, ct);
                }


                // 2 Tạo PO
                var po = new PurchaseOrder
                {
                    ExternalId = await _idService.NextAsync(req.CompanyId.GetValueOrDefault(), "DDH", now, ct: ct),
                    SupplierId = req.SupplierId,
                    OrderType = req.OrderType,

                    Comment = req.Comment,
                    PLPUComment = req.PLPUComment,

                    RequestDeliveryDate = req.RequestDeliveryDate,
                    RealDeliveryDate = req.RealDeliveryDate,

                    CompanyId = req.CompanyId,
                    Status = PurchaseOrderStatus.Pending.ToString(), // tuỳ quy ước
                    CreateDate = now,
                    CreatedBy = req.CreatedBy,
                    PurchaseOrderSnapshotId = snapshot?.PurchaseOrderSnapshotId
                };



                await _unitOfWork.PurchaseOrderRepository.AddAsync(po, ct);

                var linkByPo = new Dictionary<Guid, PurchaseOrderLink>();
                // Bridge MerchadiseOrders nếu có
                foreach (var merchId in req.MerchadiseOrderIds.Distinct())
                {
                    var link = new PurchaseOrderLink
                    {
                        PurchaseOrderId = po.PurchaseOrderId,
                        MerchandiseOrderId = merchId
                    };
                    po.PurchaseOrderLinks.Add(link);
                    linkByPo[merchId] = link;
                }


                // 3 Tạo chi tiết PO
                var materialIds = req.PurchaseOrderDetails.Select(x => x.MaterialId).Distinct().ToList();

                // Load MaterialsSupplier hiện có cho supplier này và các material nói trên
                var msList = await _unitOfWork.MaterialsSupplierRepository.Query()
                    .Where(ms => ms.SupplierId == req.SupplierId && materialIds.Contains(ms.MaterialId))
                    .ToListAsync(ct);

                // Build dictionary để tra nhanh
                var msDict = msList.ToDictionary(k => k.MaterialId, v => v);

                // 4) Tạo PurchaseOrderDetail, đồng thời xử lý giá
                foreach (var detailReq in req.PurchaseOrderDetails)
                {
                    var detail = new PurchaseOrderDetail
                    {
                        PurchaseOrderId = po.PurchaseOrderId,

                        Package = detailReq.Package,

                        RequestQuantity = detailReq.RequestQuantity,

                        BaseCostSnapshot = detailReq.BaseCostSnapshot,
                        BaseDateSnapshot = detailReq.BaseDateSnapshot,

                        UnitPriceAgreed = detailReq.UnitPriceAgreed, // != null đã validate
                        TotalPriceAgreed = (detailReq.UnitPriceAgreed ?? 0m) * detailReq.RequestQuantity,

                        MaterialId = detailReq.MaterialId,
                        MaterialExternalIDSnapshot = detailReq.MaterialExternalIDSnapshot,
                        MaterialNameSnapshot = detailReq.MaterialNameSnapshot,

                        DeliveryDate = detailReq.DeliveryDate ,
                        Note = detailReq.Note,
                        // Các snapshot của Material nếu cần: MaterialExternalIDSnapshot, MaterialNameSnapshot...
                    };
                    await _unitOfWork.PurchaseOrderDetailRepository.AddAsync(detail, ct);

                    var agreedPrice = detailReq.UnitPriceAgreed!.Value;
                    var currency = "VND"; // hoặc lấy theo Supplier/Company default

                    // Tìm MaterialsSupplier, nếu chưa có thì tạo
                    if (!msDict.TryGetValue(detailReq.MaterialId, out var ms))
                    {
                        var materialName = detailReq.MaterialNameSnapshot ?? "Unknown";
                        return OperationResult.Fail($"Nhà cung cấp không có vật tư {materialName}");
                        //ms = new MaterialsSupplier
                        //{
                        //    MaterialsSuppliersId = Guid.NewGuid(),
                        //    SupplierId = req.SupplierId.GetValueOrDefault(),
                        //    MaterialId = detail.MaterialId,
                        //    CurrentPrice = null, // sẽ set ngay dưới
                        //    Currency = currency,
                        //    CreateDate = now,
                        //    CreatedBy = req.CreatedBy,
                        //    IsActive = true,
                        //    IsPreferred = false
                        //};
                        //await _unitOfWork.MaterialsSupplierRepository.AddAsync(ms, ct);
                        //msDict[detail.MaterialId] = ms;
                    }

                    // So sánh giá: có thể dùng decimal trực tiếp, hoặc chấp nhận epsilon nếu có tỷ giá/rounding
                    var currentPrice = ms.CurrentPrice ?? 0m;
                    var priceChanged = currentPrice != agreedPrice || !string.Equals(ms.Currency, currency, StringComparison.OrdinalIgnoreCase);

                    if (priceChanged)
                    {
                        // 4.1) Đóng hiệu lực PriceHistory cũ nếu đang IsActive=true
                        var activeHist = await _unitOfWork.PriceHistorieRepository.Query()
                            .Where(ph => ph.SupplierId == req.SupplierId.GetValueOrDefault()
                                      && ph.MaterialId == detail.MaterialId
                                      && ph.IsActive == true)
                            .OrderByDescending(ph => ph.CreateDate) // an toàn
                            .FirstOrDefaultAsync(ct);

                        if (activeHist is not null)
                        {
                            activeHist.IsActive = false;
                            activeHist.EndDate = now;
                            // (OldPrice/NewPrice của bản ghi cũ giữ nguyên như lịch sử)
                        }

                        // 4.2) Tạo PriceHistory mới (active)
                        var newHist = new PriceHistory
                        {
                            SupplierId = req.SupplierId.GetValueOrDefault(),
                            MaterialId = detail.MaterialId,
                            OldPrice = ms.CurrentPrice,
                            NewPrice = agreedPrice,
                            Currency = currency,
                            CreateDate = now,
                            CreatedBy = req.CreatedBy,
                            IsActive = true
                        };
                        await _unitOfWork.PriceHistorieRepository.AddAsync(newHist, ct);

                        // 4.3) Cập nhật MaterialsSupplier
                        ms.CurrentPrice = agreedPrice;
                        ms.Currency = currency;
                        ms.UpdatedBy = req.CreatedBy;
                        ms.UpdatedDate = now;
                        ms.IsActive = true; // tuỳ logic, giả định vẫn active
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync(ct);
                return OperationResult.Ok("Purchase Order created successfully.");
            }

            catch (Exception ex)
            {
                return OperationResult.Fail($"Error creating Purchase Order: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách đơn mua hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<GetSamplePurchaseOrder>> GetAllAsync(PurchaseOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;


                var baseQ = _unitOfWork.PurchaseOrderRepository.Query()
                    .Where(po => po.IsActive == true);

                if (query.SupplierId.HasValue)
                {
                    baseQ = baseQ.Where(po => po.SupplierId == query.SupplierId.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.Status))
                {
                    baseQ = baseQ.Where(po => po.Status == query.Status);
                }

                if (!string.IsNullOrWhiteSpace(query.OrderType))
                {
                    baseQ = baseQ.Where(po => po.OrderType == query.OrderType);
                }

                if (query.From.HasValue)
                {
                    baseQ = baseQ.Where(po => po.CreateDate >= query.From.Value);
                }

                if (query.To.HasValue)
                {
                    baseQ = baseQ.Where(po => po.CreateDate <= query.To.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var kw = query.Keyword.Trim();
                    baseQ = baseQ.Where(po =>
                        (po.Supplier.SupplierName ?? "").Contains(kw) ||
                        (po.Supplier.ExternalId ?? "").Contains(kw) ||
                        (po.ExternalId ?? "").Contains(kw) ||
                        po.PurchaseOrderDetails.Any(p => (p.MaterialExternalIDSnapshot ?? "").Contains(kw))

                    );
                }

                var totalCount = await baseQ.CountAsync(ct);

                var headers = await baseQ
                    .OrderByDescending(po => po.CreateDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(po => new GetSamplePurchaseOrder
                    {
                        PurchaseOrderId = po.PurchaseOrderId,
                        ExternalId = po.ExternalId,
                        MerchediseListExternalId = string.Join(", ",
                            po.PurchaseOrderLinks
                              .Select(link => link.MerchandiseOrder != null ? link.MerchandiseOrder.ExternalId : null)
                              .Where(eid => eid != null)
                              .Distinct()
                        ),

                        Status = po.Status,
                        OrderType = po.OrderType,
                        SupplierName = po.Supplier != null ? po.Supplier.SupplierName : null,
                        SupplierExternalId = po.Supplier != null ? po.Supplier.ExternalId : null,
                        TotalAmount = po.PurchaseOrderDetails.Sum(d => d.TotalPriceAgreed) ,
                        Comment = po.Comment,
                        RequestDeliveryDate =  po.RequestDeliveryDate,
                        RealDeliveryDate = po.RealDeliveryDate,
                        CreateDate = po.CreateDate,

                        Details = po.PurchaseOrderDetails.Select(d => new GetSamplePurchaseOrderDetail
                        {
                            MaterialExternalIDSnapshot = d.MaterialExternalIDSnapshot,
                            MaterialNameSnapshot = d.MaterialNameSnapshot,
                            UnitPriceAgreed = d.UnitPriceAgreed,
                            RequestQuantity = d.RequestQuantity,
                            RealQuantity = d.RealQuantity
                        }).ToList()
                    })
                    .ToListAsync(ct);

                if (headers.Count == 0)
                    return new PagedResult<GetSamplePurchaseOrder>(headers, totalCount, query.PageNumber, query.PageSize);


                return new PagedResult<GetSamplePurchaseOrder>(headers, totalCount, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách đơn mua hàng.", ex);
            }
        }

        /// <summary>
        /// Lấy tồn kho vật tư
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<MaterialStock> GetMaterialStockAsync(Guid materialId, CancellationToken ct = default)
        {
            try
            {
                var materialStock = _unitOfWork.MaterialRepository.Query()
                    .FirstOrDefault(m => m.MaterialId == materialId);

                if (materialStock == null)
                    throw new Exception(
                        $"Material with ID {materialId} not found.");


                // Lấy giá gần nhất và giá hiện tại 
                //var lastPriceEntry = _unitOfWork.PriceHistorieRepository.Query()
                //    .Where(ph => ph.MaterialId == materialId)
                //    .OrderByDescending(ph => ph.CreateDate)
                //    .Select(ph => new {ph.NewPrice, ph.CreateDate})
                //    .FirstOrDefault();


                var lastPriceEntry = await _unitOfWork.PurchaseOrderDetailRepository.Query()
                    .Where(d => d.MaterialId == materialId
                                && d.PurchaseOrder != null
                                // Nếu muốn chỉ lấy PO ở trạng thái hợp lệ, thêm điều kiện Status ở đây:
                                // && new[] {"Approved","Ordered","Received"}.Contains(d.PurchaseOrder.Status)
                                && (
                                     d.UnitPriceAgreed != null
                                  || d.BaseCostSnapshot != null
                                  || (d.TotalPriceAgreed != null && d.RequestQuantity > 0)
                                ))
                    .OrderByDescending(d =>
                        d.PurchaseOrder.UpdatedDate
                        ?? d.PurchaseOrder.CreateDate
                        ?? d.DeliveryDate)
                    .Select(d => new
                    {
                        NewPrice = (decimal?)(
                              d.UnitPriceAgreed
                           ?? d.BaseCostSnapshot
                           ?? ((d.TotalPriceAgreed != null && d.RequestQuantity > 0)
                                  ? d.TotalPriceAgreed / d.RequestQuantity
                                  : (decimal?)null)
                        ),
                        CreateDate = (DateTime?)(
                            d.PurchaseOrder.UpdatedDate
                            ?? d.PurchaseOrder.CreateDate
                            ?? d.DeliveryDate
                        ),
                        SupplierId = d.PurchaseOrder.SupplierId,
                        PurchaseOrderId = d.PurchaseOrderId,
                        PurchaseOrderExternalId = d.PurchaseOrder.ExternalId
                    })
                    .FirstOrDefaultAsync();


                // Nếu PriceHistory không có thì lấy giá từ MaterialsSupplier
                //if (lastPriceEntry == null)
                //{
                //    lastPriceEntry = await _unitOfWork.MaterialsSupplierRepository.Query()  
                //        .Where(ms => ms.MaterialId == materialId && ms.IsActive == true)
                //        .OrderByDescending(ms => ms.UpdatedDate)
                //        .Select(ms => new { NewPrice = ms.CurrentPrice, CreateDate = ms.UpdatedDate })
                //        .FirstOrDefaultAsync();
                //}


                // Tính toán tồn kho hiện tại
                var currentStock = await _unitOfWork.WarehouseShelfStockRepository.Query()
                    .Where(s => s.Code == materialStock.ExternalId)
                    .SumAsync(s => (decimal?)s.QtyKg) ?? 0;

                //DateTime? lastUseEntry = await _unitOfWork.WarehouseRequestDetailRepository.Query()
                //    .Where(rd => rd.ProductCode == materialStock.ExternalId
                //                 && rd.WarehouseRequest != null
                //                 && rd.WarehouseRequest.ReqType == WareHouseRequestType.ExportForProduction) // 👈 lọc loại phiếu
                //    .OrderByDescending(rd => rd.WarehouseRequest!.CreatedDate)
                //    .Select(rd => (DateTime?)rd.WarehouseRequest!.CreatedDate)
                //    .FirstOrDefaultAsync();

                var reservedQuery = _unitOfWork.WarehouseTempStockRepository.Query()
                    .Where(x => x.Code == materialStock.ExternalId &&
                                x.ReserveStatus == ReserveStatus.Open.ToString());

                var ReservedStock = await reservedQuery.SumAsync(x => (decimal?)x.QtyRequest) ?? 0m;
                var ReservedCodeList = string.Join(", ",
                    await reservedQuery.Select(x => x.VaCode).Distinct().ToListAsync());


                // 5️⃣ Map sang DTO
                return new MaterialStock
                {
                    MaterialId = materialStock.MaterialId,
                    MaterialExternalIDSnapshot = materialStock.ExternalId,
                    MaterialNameSnapshot = materialStock.Name,
                    Package = materialStock.Package,
                    BaseCostSnapshot = lastPriceEntry?.NewPrice,
                    CurrentStock = currentStock,

                    lastUpdatePrice = lastPriceEntry?.CreateDate,
                    ReservedStock = ReservedStock,
                    RequiredVaCodeList = ReservedCodeList,
                    //lastUse = lastUseEntry ?? null,
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving material stock.", ex);
            }
        }

        /// <summary>
        /// Lấy đơn mua hàng theo ID
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<GetPurchaseOrder> GetPurchaseOrderByIdAsync(Guid purchaseOrderId, CancellationToken ct = default)
        {
            // Validate tối thiểu
            if (purchaseOrderId == Guid.Empty) throw new ArgumentNullException(nameof(purchaseOrderId));

            try
            {
                var po = _unitOfWork.PurchaseOrderRepository.Query()
                    .Where(PO => PO.IsActive == true && PO.PurchaseOrderId == purchaseOrderId);

                // The following check is not correct, as 'po' is an IQueryable, not null.
                // Instead, check if the result is null after querying.
                var result = await po
                    .Select(po => new GetPurchaseOrder
                    {
                        PurchaseOrderId = po.PurchaseOrderId,
                        ExternalId = po.ExternalId,
                        OrderType = po.OrderType,
                        SupplierId = po.SupplierId,
                        Comment = po.Comment,
                        Status = po.Status,
                        RequestDeliveryDate = po.RequestDeliveryDate,
                        RealDeliveryDate = po.RealDeliveryDate,
                        CompanyId = po.CompanyId,
                        CreateDate = po.CreateDate,
                        CreatedBy = po.CreatedBy,
                        UpdatedDate = po.UpdatedDate,
                        UpdatedBy = po.UpdatedBy,
                        MerchadiseOrderList = string.Join(",",
                            po.PurchaseOrderLinks
                                .Select(link => link.MerchandiseOrder != null ? link.MerchandiseOrder.ExternalId : null)
                                .Where(eid => eid != null)
                                .Distinct()
                        ),
                        PurchaseOrderDetails = po.PurchaseOrderDetails.Select(d => new GetPurchaseOrderDetail
                        {
                            PurchaseOrderDetailId = d.PurchaseOrderDetailId,
                            MaterialId = d.MaterialId,
                            MaterialExternalIDSnapshot = d.MaterialExternalIDSnapshot,
                            MaterialNameSnapshot = d.MaterialNameSnapshot,
                            BaseCostSnapshot = d.BaseCostSnapshot,
                            BaseDateSnapshot = d.BaseDateSnapshot,
                            Package = d.Package,
                            RequestQuantity = d.RequestQuantity,
                            UnitPriceAgreed = d.UnitPriceAgreed,
                            TotalPriceAgreed = d.TotalPriceAgreed,
                            DeliveryDate = d.DeliveryDate,
                            Note = d.Note
                        }).ToList(),
                        PurchaseOrderSnapshot = po.PurchaseOrderSnapshot // Fix: ensure this is not a syntax error and allow possible null assignment
                    })
                    .FirstOrDefaultAsync(ct);

                if (result == null)
                    throw new Exception($"Purchase Order with ID {purchaseOrderId} not found.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy đơn mua hàng.", ex);
            }
        }

        /// <summary>
        /// Lấy các dòng có thể chọn từ đơn giao hàng
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<PagedResult<GetPOPurchaseOrder>> GetSelectableLinesAsync(DeliveryOrderQuery query, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cập nhật lại đơn mua hàng bao gồm cả hành động hủy
        /// </summary>
        /// <param name="patchPurchaseOrder"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateAsync(PatchPurchaseOrder patchPurchaseOrder, CancellationToken ct = default)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                var existingPO = await _unitOfWork.PurchaseOrderRepository.Query()
                    .FirstOrDefaultAsync(po => po.PurchaseOrderId == patchPurchaseOrder.PurchaseOrderId && po.IsActive == true, ct);

                if (existingPO == null)
                {
                    return OperationResult.Fail($"Purchase Order with ID {patchPurchaseOrder.PurchaseOrderId} not found.");
                }

                existingPO.UpdatedDate = now;
                existingPO.UpdatedBy = patchPurchaseOrder.UpdatedBy;

                PatchHelper.SetIfRef(existingPO.Comment, () => patchPurchaseOrder.Comment, v => existingPO.Comment = v);
                PatchHelper.SetIfRef(existingPO.PLPUComment, () => patchPurchaseOrder.PLPUComment, v => existingPO.PLPUComment = v);

                if (patchPurchaseOrder.IsActive.HasValue)
                {
                    existingPO.IsActive = false;
                    foreach (var dd in existingPO.PurchaseOrderLinks.Where(x => x.IsActive))
                    {
                        dd.IsActive = false;
                    }

                    foreach (var dd in existingPO.PurchaseOrderDetails.Where(x => x.IsActive))
                    {
                        dd.IsActive = false;
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync(ct);
                return OperationResult.Ok("Hoàn thành");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                return OperationResult.Fail($"Lỗi api: {ex.InnerException?.Message}");
            }
        }
    }
}
