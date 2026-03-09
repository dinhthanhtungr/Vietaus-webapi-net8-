using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Queries;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.Material_warehouse;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Queries;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.Category;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.Orders;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExternalIdService _idService;
        private readonly ITimelineService _TimelineService;
        private readonly IMaterialService _materialService;
        private readonly ICurrentUser _CurrentUser;
        private readonly IWarehouseReservationService _WarehouseReservationService;

        public PurchaseOrderService(IUnitOfWork unitOfWork
                                  , IExternalIdService idService
                                  , ITimelineService timelineService
                                  , ICurrentUser currentUser
                                  , IMaterialService materialService
                                  , IWarehouseReservationService warehouseReservationService)   
        {
            _unitOfWork = unitOfWork;
            _idService = idService;
            _TimelineService = timelineService;
            _CurrentUser = currentUser;
            _materialService = materialService;
            _WarehouseReservationService = warehouseReservationService; 
        }

        // ======================================================================== Get ======================================================================== 

        /// <summary>
        /// Lấy danh sách đơn mua hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OperationResult<PagedResult<GetSamplePurchaseOrder>>> GetAllAsync(PurchaseOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                // Guard + default
                query ??= new PurchaseOrderQuery();
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var skip = (query.PageNumber - 1) * query.PageSize;

                // PHÂN TRANG THEO PURCHASE ORDER (header)
                var poQ = _unitOfWork.PurchaseOrderRepository.Query()
                    .AsNoTracking()
                    .Where(po => po.IsActive == true);

                // ---- Filters ----
                if (query.SupplierId.HasValue)
                    poQ = poQ.Where(po => po.SupplierId == query.SupplierId.Value);

                if (!string.IsNullOrWhiteSpace(query.Status))
                    poQ = poQ.Where(po => po.Status == query.Status);

                if (!string.IsNullOrWhiteSpace(query.OrderType))
                    poQ = poQ.Where(po => po.OrderType == query.OrderType);

                if (query.From.HasValue)
                    poQ = poQ.Where(po => po.CreateDate >= query.From.Value);

                if (query.To.HasValue)
                    poQ = poQ.Where(po => po.CreateDate <= query.To.Value);

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var kw = query.Keyword.Trim();
                    poQ = poQ.Where(po =>
                        ((po.Supplier.SupplierName ?? "").Contains(kw)) ||
                        ((po.Supplier.ExternalId ?? "").Contains(kw)) ||
                        ((po.ExternalId ?? "").Contains(kw)) ||
                        // tìm theo MerchandiseOrder.ExternalId qua bảng link
                        po.PurchaseOrderLinks.Any(l => (l.MerchandiseOrder.ExternalId ?? "").Contains(kw)) ||
                        // tìm theo chi tiết
                        po.PurchaseOrderDetails.Any(d => (d.MaterialExternalIDSnapshot ?? "").Contains(kw))
                    );
                }

                // Count trước khi phân trang
                var totalCount = await poQ.CountAsync(ct);

                // Lấy page các PO (header)
                var poPage = await poQ
                    .OrderByDescending(po => po.CreateDate)
                    .Skip(skip)
                    .Take(query.PageSize)
                    .Select(po => new
                    {
                        po.PurchaseOrderId,
                        po.ExternalId,
                        po.Status,
                        po.OrderType,
                        po.Comment,
                        po.RequestDeliveryDate,
                        po.RealDeliveryDate,
                        po.CreateDate,

                        SupplierName = po.Supplier != null ? po.Supplier.SupplierName : null,
                        SupplierExternalId = po.Supplier != null ? po.Supplier.ExternalId : null,

                        MerchandiseExternalIds = po.PurchaseOrderLinks
                            .Where(l => l.MerchandiseOrder != null)
                            .Select(l => l.MerchandiseOrder.ExternalId)
                            .Distinct()
                            .ToList(),

                        TotalAmount = po.PurchaseOrderDetails.Sum(d => d.TotalPriceAgreed),

                        Details = po.PurchaseOrderDetails.Select(d => new GetSamplePurchaseOrderDetail
                        {
                            LineNo = d.LineNo,
                            MaterialExternalIDSnapshot = d.MaterialExternalIDSnapshot,
                            MaterialNameSnapshot = d.MaterialNameSnapshot,
                            UnitPriceAgreed = d.UnitPriceAgreed,
                            RequestQuantity = d.RequestQuantity,
                            RealQuantity = d.RealQuantity
                        }).OrderBy(d => d.LineNo).ToList()
                    })
                    .ToListAsync(ct);

                // Map ra DTO cuối + string.Join ở memory (không làm trong SQL)
                var items = poPage.Select(x => new GetSamplePurchaseOrder
                {
                    PurchaseOrderId = x.PurchaseOrderId,
                    ExternalId = x.ExternalId,
                    MerchediseListExternalId = string.Join(", ", x.MerchandiseExternalIds),
                    Status = x.Status,
                    OrderType = x.OrderType,
                    SupplierName = x.SupplierName,
                    SupplierExternalId = x.SupplierExternalId,
                    TotalAmount = x.TotalAmount,
                    Comment = x.Comment,
                    RequestDeliveryDate = x.RequestDeliveryDate,
                    RealDeliveryDate = x.RealDeliveryDate,
                    CreateDate = x.CreateDate,
                    Details = x.Details
                }).ToList();

                var paged = new PagedResult<GetSamplePurchaseOrder>(items, totalCount, query.PageNumber, query.PageSize);

                // BỌC TRONG OperationResult
                return OperationResult<PagedResult<GetSamplePurchaseOrder>>.Ok(paged);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<GetSamplePurchaseOrder>>.Fail(
                    $"Lỗi khi lấy danh sách đơn mua hàng. {ex.Message}");
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
        public async Task<OperationResult<GetPurchaseOrder>> GetPurchaseOrderByIdAsync(Guid purchaseOrderId, CancellationToken ct = default)
        {

            try
            {
                var po = _unitOfWork.PurchaseOrderRepository.Query()
                    .Where(PO => PO.IsActive == true && PO.PurchaseOrderId == purchaseOrderId);

                // The following check is not correct, as 'po' is an IQueryable, not null.
                // Instead, check if the result is null after querying.
                var result = await po
                    .Select(po => new
                    {
                        PO = po,
                        MerchExternalIds = po.PurchaseOrderLinks
                            .Where(link => link.MerchandiseOrder != null)
                                .Select(link => link.MerchandiseOrder != null ? link.MerchandiseOrder.ExternalId : null)
                            .Distinct()
                            .ToList(),
                        Details = po.PurchaseOrderDetails
                            .Select(d => new GetPurchaseOrderDetail
                            {
                                LineNo = d.LineNo,
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
                            })
                            .OrderBy(d => d.LineNo)
                            .ToList(),

                        PurchaseOrderSnapshot = po.PurchaseOrderSnapshot != null ? new PurchaseOrderSnapshot
                        {
                            EmployeeExternalIdSnapshot = po.PurchaseOrderSnapshot.EmployeeExternalIdSnapshot,
                            EmployeeFullNameSnapshot = po.PurchaseOrderSnapshot.EmployeeFullNameSnapshot,
                            PhoneNumberSnapshot = po.PurchaseOrderSnapshot.PhoneNumberSnapshot,
                            EmailSnapshot = po.PurchaseOrderSnapshot.EmailSnapshot,
                            SupplierExternalIdSnapshot = po.PurchaseOrderSnapshot.SupplierExternalIdSnapshot,
                            SupplierNameSnapshot = po.PurchaseOrderSnapshot.SupplierNameSnapshot,
                            SupplierContactSnapshot = po.PurchaseOrderSnapshot.SupplierContactSnapshot,
                            SupplierPhoneNumberSnapshot = po.PurchaseOrderSnapshot.SupplierPhoneNumberSnapshot,
                            SupplierAddressSnapshot = po.PurchaseOrderSnapshot.SupplierAddressSnapshot,
                            TotalPrice = po.PurchaseOrderSnapshot.TotalPrice,
                            DeliveryAddress = po.PurchaseOrderSnapshot.DeliveryAddress,
                            PaymentTypes = po.PurchaseOrderSnapshot.PaymentTypes,
                            Vat = po.PurchaseOrderSnapshot.Vat
                        } : null
                    })
                    .FirstOrDefaultAsync(ct);

                if (result == null)
                    throw new Exception($"Purchase Order with ID {purchaseOrderId} not found.");

                var dto = new GetPurchaseOrder
                {
                    PurchaseOrderId = result.PO.PurchaseOrderId,
                    ExternalId = result.PO.ExternalId,
                    OrderType = result.PO.OrderType,
                    SupplierId = result.PO.SupplierId,
                    PLPUComment = result.PO.PLPUComment,
                    Comment = result.PO.Comment,
                    Status = result.PO.Status,
                    RequestDeliveryDate = result.PO.RequestDeliveryDate,
                    RealDeliveryDate = result.PO.RealDeliveryDate,
                    CompanyId = result.PO.CompanyId,
                    CreateDate = result.PO.CreateDate,
                    CreatedBy = result.PO.CreatedBy,
                    UpdatedDate = result.PO.UpdatedDate,
                    UpdatedBy = result.PO.UpdatedBy,
                    MerchadiseOrderList = string.Join(",", result.MerchExternalIds),
                    PurchaseOrderDetails = result.Details,
                    PurchaseOrderSnapshot = result.PurchaseOrderSnapshot    
                };

                return OperationResult<GetPurchaseOrder>.Ok(dto);
            }
            catch (Exception ex)
            {
                return OperationResult<GetPurchaseOrder>.Fail("Lỗi khi lấy đơn mua hàng.");
            }
        }

        /// <summary>
        /// Lấy các dòng có thể chọn từ đơn giao hàng
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<OperationResult<PagedResult<GetPOPurchaseOrder>>> GetSelectableLinesAsync(DeliveryOrderQuery query, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }


        // ======================================================================== Post ======================================================================== 

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
                if (req.PurchaseOrderDetails is null || req.PurchaseOrderDetails.Count == 0)
                    throw new InvalidOperationException("PO phải có ít nhất 1 dòng chi tiết.");
                if (req.PurchaseOrderDetails.Any(d => d.UnitPriceAgreed is null))
                    throw new InvalidOperationException("Thiếu UnitPriceAgreed trong chi tiết.");

                var now = DateTime.Now;
                var userId = _CurrentUser.EmployeeId;
                var companyId = _CurrentUser.CompanyId;

                await using var transaction = await _unitOfWork.BeginTransactionAsync();

                var dup = req.PurchaseOrderDetails
                    .GroupBy(x => x.MaterialId)
                    .FirstOrDefault(g => g.Count() > 1);

                if (dup != null)
                {
                    throw new InvalidOperationException($"Vật tư bị trùng trong PO: {dup.Key}");
                }

                // 1) Snapshot
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

                // 2) Create PO header
                var po = new PurchaseOrder
                {
                    PurchaseOrderId = Guid.CreateVersion7(),
                    ExternalId = await _idService.NextAsync(DocumentPrefix.DDH.ToString(), ct: ct),
                    SupplierId = req.SupplierId,
                    OrderType = req.OrderType,
                    Comment = req.Comment,
                    PLPUComment = req.PLPUComment,
                    RequestDeliveryDate = req.RequestDeliveryDate,
                    RealDeliveryDate = req.RealDeliveryDate,
                    CompanyId = companyId,
                    Status = PurchaseOrderStatus.Pending.ToString(),
                    CreateDate = now,
                    CreatedBy = userId,
                    PurchaseOrderSnapshot = snapshot,
                    
                };
                await _unitOfWork.PurchaseOrderRepository.AddAsync(po, ct);

                // 3) Links
                foreach (var merchId in req.MerchadiseOrderIds.Distinct())
                {
                    po.PurchaseOrderLinks.Add(new PurchaseOrderLink
                    {
                        PurchaseOrderId = po.PurchaseOrderId,
                        MerchandiseOrderId = merchId
                    });
                }

                // 4) Prepare: load MaterialsSupplier existing
                var materialIds = req.PurchaseOrderDetails.Select(x => x.MaterialId).Distinct().ToList();

                var msList = await _unitOfWork.MaterialsSupplierRepository.Query()
                    .Where(ms => ms.SupplierId == req.SupplierId && materialIds.Contains(ms.MaterialId))
                    .ToListAsync(ct);

                // Key theo MaterialId (vì supplier đã cố định)
                var msDict = msList.ToDictionary(x => x.MaterialId);

                // (Optional) nếu bạn cần snapshot MaterialName/ExternalId chính xác từ DB:
                // load Materials theo materialIds để fallback khi request thiếu snapshot
                var materialList = await _unitOfWork.MaterialRepository.Query()
                    .Where(m => materialIds.Contains(m.MaterialId))
                    .Select(m => new { m.MaterialId, m.Name, m.ExternalId })
                    .ToListAsync(ct);
                var materialDict = materialList.ToDictionary(x => x.MaterialId);

                // 5) Create details + ensure MaterialsSupplier
                var currency = "VND"; // TODO: lấy từ Supplier hoặc Company setting nếu có

                // (Optional) gom các update giá để xử lý batch
                var priceUpdates = new List<(Guid materialsSuppliersId, decimal newPrice, decimal oldPrice, string currency)>();
                var lineNo = 1; 
                foreach (var detailReq in req.PurchaseOrderDetails)
                {
                    // 5.1 create PO detail
                    var unitPrice = detailReq.UnitPriceAgreed!.Value;

                    var detail = new PurchaseOrderDetail
                    {
                        PurchaseOrderId = po.PurchaseOrderId,

                        Package = detailReq.Package,
                        RequestQuantity = detailReq.RequestQuantity,
                        LineNo = lineNo++,

                        BaseCostSnapshot = detailReq.BaseCostSnapshot,
                        BaseDateSnapshot = detailReq.BaseDateSnapshot,

                        UnitPriceAgreed = unitPrice,
                        TotalPriceAgreed = unitPrice * detailReq.RequestQuantity,

                        MaterialId = detailReq.MaterialId,
                        MaterialExternalIDSnapshot = detailReq.MaterialExternalIDSnapshot
                            ?? materialDict.GetValueOrDefault(detailReq.MaterialId)?.ExternalId,
                        MaterialNameSnapshot = detailReq.MaterialNameSnapshot
                            ?? materialDict.GetValueOrDefault(detailReq.MaterialId)?.Name,

                        DeliveryDate = detailReq.DeliveryDate,
                        Note = detailReq.Note,
                    };
                    await _unitOfWork.PurchaseOrderDetailRepository.AddAsync(detail, ct);

                    // 5.2 ensure MaterialsSupplier exists (auto create if first-time)
                    if (!msDict.TryGetValue(detailReq.MaterialId, out var ms))
                    {
                        ms = new MaterialsSupplier
                        {
                            MaterialsSuppliersId = Guid.CreateVersion7(),
                            SupplierId = req.SupplierId,
                            MaterialId = detailReq.MaterialId,

                            CurrentPrice = unitPrice,
                            Currency = currency,

                            CreateDate = now,
                            CreatedBy = userId,
                            IsActive = true,
                            IsPreferred = false,
                        };

                        await _unitOfWork.MaterialsSupplierRepository.AddAsync(ms, ct);
                        msDict[detailReq.MaterialId] = ms;

                        // tạo PriceHistory cho lần đầu (tuỳ business, bạn có thể coi OldPrice = null)
                        var ph = new PriceHistory
                        {
                            PriceHistoryId = Guid.CreateVersion7(),
                            MaterialsSuppliersId = ms.MaterialsSuppliersId,
                            OldPrice = null,
                            Currency = currency,
                            CreateDate = now,
                            CreatedBy = userId
                        };
                        await _unitOfWork.PriceHistorieRepository.AddAsync(ph, ct);

                        continue; // đã set current price rồi
                    }

                    // 5.3 price change handling
                    var oldPrice = ms.CurrentPrice ?? 0m;
                    var oldCurrency = ms.Currency ?? currency;

                    var changed = oldPrice != unitPrice
                                  || !string.Equals(oldCurrency, currency, StringComparison.OrdinalIgnoreCase);

                    if (changed)
                    {
                        priceUpdates.Add((ms.MaterialsSuppliersId, unitPrice, oldPrice, currency));
                    }
                }

                // 6) Apply price updates (batch-friendly)
                foreach (var u in priceUpdates)
                {
                    // Nếu ChangePriceHelper của bạn đã lo PriceHistory + update ms:
                    await _materialService.ChangePriceHelper(u.materialsSuppliersId, u.newPrice);

                    // Nếu helper chỉ update ms mà KHÔNG add history, thì bạn tự add PriceHistory ở đây.
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync(ct);

                return OperationResult.Ok("Purchase Order created successfully.");
            }
            catch (Exception)
            {
                // (nên log ex ở đây)
                return OperationResult.Fail("Có lỗi xảy ra khi tạo Đơn mua hàng. Vui lòng thử lại hoặc liên hệ IT.");
            }
        }

        // ======================================================================== Update ========================================================================

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

                var existingPO = await _unitOfWork.PurchaseOrderRepository.Query(track: true)
                    .FirstOrDefaultAsync(po => po.PurchaseOrderId == patchPurchaseOrder.PurchaseOrderId && po.IsActive == true, ct);

                if (existingPO == null)
                {
                    return OperationResult.Fail($"Purchase Order with ID {patchPurchaseOrder.PurchaseOrderId} not found.");
                }

                existingPO.UpdatedDate = now;
                existingPO.UpdatedBy = patchPurchaseOrder.UpdatedBy;

                PatchHelper.SetIfRef(existingPO.Comment, () => patchPurchaseOrder.Comment, v => existingPO.Comment = v);
                PatchHelper.SetIfRef(existingPO.PLPUComment, () => patchPurchaseOrder.PLPUComment, v => existingPO.PLPUComment = v);

                if (patchPurchaseOrder.IsActive == false)
                {
                    existingPO.IsActive = false;

                    await _unitOfWork.PurchaseOrderLinkRepository.Query(track: false)
                        .Where(x => x.PurchaseOrderId == existingPO.PurchaseOrderId && x.IsActive)
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(x => x.IsActive, false),
                            ct);

                    await _unitOfWork.PurchaseOrderDetailRepository.Query(track: false)
                        .Where(x => x.PurchaseOrderId == existingPO.PurchaseOrderId && x.IsActive)
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(x => x.IsActive, false),
                            ct);

                    if (!string.IsNullOrEmpty(existingPO.ExternalId))
                        await _WarehouseReservationService.EnsureWarehouseRequestDeletedAsync(existingPO.ExternalId, ct);
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync(ct);
                return OperationResult.Ok("Hoàn thành");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                return OperationResult.Fail($"Lỗi api: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // ======================================================================== Other Methods ========================================================================


        //public async Task<OperationResult<List<GetPurchaseOrderDetail>>> GetPurchaseOrderDetailByIdAsync (Guid id, CancellationToken ct)
        //{
        //    // Validate tối thiểu
        //    if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

        //    try
        //    {
        //        var poDetail = await _unitOfWork.PurchaseOrderDetailRepository.Query()
        //            .Where(p => p.PurchaseOrderId == id)
        //            .Select (d => new GetPurchaseOrderDetail
        //            {
        //                PurchaseOrderDetailId = d.PurchaseOrderDetailId,
        //                MaterialId = d.MaterialId,
        //                MaterialExternalIDSnapshot = d.MaterialExternalIDSnapshot,
        //                MaterialNameSnapshot = d.MaterialNameSnapshot,
        //                BaseCostSnapshot = d.BaseCostSnapshot,
        //                BaseDateSnapshot = d.BaseDateSnapshot,
        //                Package = d.Package,
        //                RequestQuantity = d.RequestQuantity,
        //                UnitPriceAgreed = d.UnitPriceAgreed,
        //                TotalPriceAgreed = d.TotalPriceAgreed,
        //                DeliveryDate = d.DeliveryDate,
        //                Note = d.Note
        //            })
        //            .ToListAsync();

        //        return OperationResult<List<GetPurchaseOrderDetail>>.Ok(poDetail);
        //    }

        //    catch (Exception ex)
        //    {
        //        throw new Exception("Lỗi khi lấy đơn mua hàng.", ex);
        //    }
        //}


        ///// <summary>
        ///// Lấy tồn kho vật tư
        ///// </summary>
        ///// <param name="materialId"></param>
        ///// <param name="ct"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //public async Task<MaterialStock> GetMaterialStockAsync(Guid materialId, CancellationToken ct = default)
        //{
        //    try
        //    {
        //        var materialStock = _unitOfWork.MaterialRepository.Query()
        //            .FirstOrDefault(m => m.MaterialId == materialId);

        //        if (materialStock == null)
        //            throw new Exception(
        //                $"Material with ID {materialId} not found.");


        //        // Lấy giá gần nhất và giá hiện tại 
        //        //var lastPriceEntry = _unitOfWork.PriceHistorieRepository.Query()
        //        //    .Where(ph => ph.MaterialId == materialId)
        //        //    .OrderByDescending(ph => ph.CreateDate)
        //        //    .Select(ph => new {ph.NewPrice, ph.CreateDate})
        //        //    .FirstOrDefault();


        //        var lastPriceEntry = await _unitOfWork.PurchaseOrderDetailRepository.Query()
        //            .Where(d => d.MaterialId == materialId
        //                        && d.PurchaseOrder != null
        //                        // Nếu muốn chỉ lấy PO ở trạng thái hợp lệ, thêm điều kiện Status ở đây:
        //                        // && new[] {"Approved","Ordered","Received"}.Contains(d.PurchaseOrder.Status)
        //                        && (
        //                             d.UnitPriceAgreed != null
        //                          || d.BaseCostSnapshot != null
        //                          || (d.TotalPriceAgreed != null && d.RequestQuantity > 0)
        //                        ))
        //            .OrderByDescending(d =>
        //                d.PurchaseOrder.UpdatedDate
        //                ?? d.PurchaseOrder.CreateDate
        //                ?? d.DeliveryDate)
        //            .Select(d => new
        //            {
        //                NewPrice = (decimal?)(
        //                      d.UnitPriceAgreed
        //                   ?? d.BaseCostSnapshot
        //                   ?? ((d.TotalPriceAgreed != null && d.RequestQuantity > 0)
        //                          ? d.TotalPriceAgreed / d.RequestQuantity
        //                          : (decimal?)null)
        //                ),
        //                CreateDate = (DateTime?)(
        //                    d.PurchaseOrder.UpdatedDate
        //                    ?? d.PurchaseOrder.CreateDate
        //                    ?? d.DeliveryDate
        //                ),
        //                SupplierId = d.PurchaseOrder.SupplierId,
        //                PurchaseOrderId = d.PurchaseOrderId,
        //                PurchaseOrderExternalId = d.PurchaseOrder.ExternalId
        //            })
        //            .FirstOrDefaultAsync();



        //        // Nếu PriceHistory không có thì lấy giá từ MaterialsSupplier
        //        //if (lastPriceEntry == null)
        //        //{
        //        //    lastPriceEntry = await _unitOfWork.MaterialsSupplierRepository.Query()  
        //        //        .Where(ms => ms.MaterialId == materialId && ms.IsActive == true)
        //        //        .OrderByDescending(ms => ms.UpdatedDate)
        //        //        .Select(ms => new { NewPrice = ms.CurrentPrice, CreateDate = ms.UpdatedDate })
        //        //        .FirstOrDefaultAsync();
        //        //}


        //        // Tính toán tồn kho hiện tại
        //        var currentStock = await _unitOfWork.WarehouseShelfStockRepository.Query()
        //            .Where(s => s.Code == materialStock.ExternalId)
        //            .SumAsync(s => (decimal?)s.QtyKg) ?? 0;

        //        //DateTime? lastUseEntry = await _unitOfWork.WarehouseRequestDetailRepository.Query()
        //        //    .Where(rd => rd.ProductCode == materialStock.ExternalId
        //        //                 && rd.WarehouseRequest != null
        //        //                 && rd.WarehouseRequest.ReqType == WareHouseRequestType.ExportForProduction) // 👈 lọc loại phiếu
        //        //    .OrderByDescending(rd => rd.WarehouseRequest!.CreatedDate)
        //        //    .Select(rd => (DateTime?)rd.WarehouseRequest!.CreatedDate)
        //        //    .FirstOrDefaultAsync();

        //        var reservedQuery = _unitOfWork.WarehouseTempStockRepository.Query()
        //            .Where(x => x.Code == materialStock.ExternalId &&
        //                        x.ReserveStatus == ReserveStatus.Open.ToString());

        //        var ReservedStock = await reservedQuery.SumAsync(x => (decimal?)x.QtyRequest) ?? 0m;
        //        var ReservedCodeList = string.Join(", ",
        //            await reservedQuery.Select(x => x.VaCode).Distinct().ToListAsync());


        //        // 5️⃣ Map sang DTO
        //        return new MaterialStock
        //        {
        //            MaterialId = materialStock.MaterialId,
        //            MaterialExternalIDSnapshot = materialStock.ExternalId,
        //            MaterialNameSnapshot = materialStock.Name,
        //            Package = materialStock.Package,
        //            BaseCostSnapshot = lastPriceEntry?.NewPrice,
        //            CurrentStock = currentStock,

        //            lastUpdatePrice = lastPriceEntry?.CreateDate,
        //            ReservedStock = ReservedStock,
        //            RequiredVaCodeList = ReservedCodeList,
        //            //lastUse = lastUseEntry ?? null,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("An error occurred while retrieving material stock.", ex);
        //    }
        //}

    }
}
