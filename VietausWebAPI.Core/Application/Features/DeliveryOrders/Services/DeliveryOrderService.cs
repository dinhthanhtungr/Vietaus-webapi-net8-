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
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs.Deliverers;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs.ExcelBuilds;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Queries;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Core.Domain.Enums.Category;
using VietausWebAPI.Core.Domain.Enums.Deliveries;
using VietausWebAPI.Core.Domain.Enums.Merchadises;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using static QuestPDF.Helpers.Colors;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Services
{
    public class DeliveryOrderService : IDeliveryOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExternalIdService _idService; 
        private readonly ICurrentUser _currentUser;
        private readonly IWarehouseReservationService _warehouseReservationService;

        public DeliveryOrderService(IUnitOfWork unitOfWork
            , IMapper mapper
            , IExternalIdService idService
            , ICurrentUser currentUser
            , IWarehouseReservationService warehouseReservationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _idService = idService;
            _currentUser = currentUser;
            _warehouseReservationService = warehouseReservationService;
        }

        // ======================================================================== Get ======================================================================== 
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
                    .AsNoTracking()
                    .Where(x => x.IsActive);

                if (query.CompanyId.HasValue && query.CompanyId.Value != Guid.Empty)
                    baseQ = baseQ.Where(x => x.CompanyId == query.CompanyId.Value);

                if (query.DeliveryOrderId.HasValue && query.DeliveryOrderId.Value != Guid.Empty)
                    baseQ = baseQ.Where(x => x.Id == query.DeliveryOrderId.Value);

                if (query.From.HasValue)
                {
                    var fromDate = query.From.Value.Date;
                    baseQ = baseQ.Where(x => x.CreatedDate >= fromDate);
                }

                if (query.To.HasValue)
                {
                    var toDateExclusive = query.To.Value.Date.AddDays(1);
                    baseQ = baseQ.Where(x => x.CreatedDate < toDateExclusive);
                }

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var kw = query.Keyword.Trim();

                    baseQ = baseQ.Where(x =>
                        (x.Customer.CustomerName ?? "").Contains(kw) ||
                        (x.Customer.ExternalId ?? "").Contains(kw) ||
                        (x.ExternalId ?? "").Contains(kw) ||

                        x.DeliveryOrderPOs.Any(l =>
                            (l.MerchandiseOrder.ExternalId ?? "").Contains(kw)) ||

                        x.Details.Any(d =>
                            d.IsActive &&
                            (
                                (d.ProductExternalIdSnapShot ?? "").Contains(kw) ||
                                (d.ProductNameSnapShot ?? "").Contains(kw) ||
                                (d.PONo ?? "").Contains(kw) ||
                                (d.LotNoList ?? "").Contains(kw)
                            ))
                    );
                }

                var totalCount = await baseQ.CountAsync(ct);

                var items = await baseQ
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(x => new GetSampleDelivery
                    {
                        Id = x.Id,
                        ExternalId = x.ExternalId,
                        CustomerExternalIdSnapShot = x.CustomerExternalIdSnapShot,
                        CustomerName = x.Customer.CustomerName,
                        CreatedDate = x.CreatedDate,
                        Note = x.Note,

                        MerchandiseOrderExternalIdList = string.Join(", ",
                            x.DeliveryOrderPOs
                                .Where(dop => dop.MerchandiseOrder != null)
                                .Select(dop => dop.MerchandiseOrder!.ExternalId)
                                .Where(v => !string.IsNullOrWhiteSpace(v))
                                .Distinct()
                        ),

                        DelivererList = string.Join(", ",
                            x.Deliverers
                                .Where(d => d.DelivererInfor != null && !string.IsNullOrWhiteSpace(d.DelivererInfor.Name))
                                .Select(d => d.DelivererInfor.Name)
                                .Distinct()
                        ),

                        PaymentDeadline = x.PaymentDeadline,

                        Details = x.Details
                            .Where(d => d.IsActive)
                            .Select(d => new GetSampleDeliveryDetail
                            {
                                Id = d.Id,
                                MerchandiseOrderDetailId = d.MerchandiseOrderDetailId,
                                ProductId = d.ProductId,
                                ProductExternalIdSnapShot = d.ProductExternalIdSnapShot,
                                ProductNameSnapShot = d.ProductNameSnapShot,
                                LotNoList = d.LotNoList,
                                PONo = d.PONo,
                                Quantity = d.Quantity,
                                NumOfBags = d.NumOfBags,
                                IsAttach = d.IsAttach
                            })
                            .ToList()
                    })
                    .ToListAsync(ct);

                return new PagedResult<GetSampleDelivery>(items, totalCount, query.PageNumber, query.PageSize);
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
        public async Task<OperationResult<PagedResult<GetPODeliveryOrder>>> GetSelectableLinesAsync(DeliveryOrderQuery query, CancellationToken ct = default)
        {
            if (query == null)
                return OperationResult<PagedResult<GetPODeliveryOrder>>.Fail("Query is null");

            // Áp dụng paging
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 15;

            // Base query: chỉ lấy PO active của company
            var baseQ = _unitOfWork.MerchandiseOrderRepository.Query()
                .Where(mo => mo.IsActive);

            // Filter theo Customer (nếu có)
            if (query.CustomerId.HasValue && query.CustomerId.Value != Guid.Empty)
            {
                baseQ = baseQ.Where(mo => mo.CustomerId == query.CustomerId.Value);
            }

            // Filter keyword (ExternalId, PONo, CustomerNameSnapshot, PhoneSnapshot)
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var kw = query.Keyword.Trim();

                baseQ = baseQ.Where(mo =>
                    (mo.ExternalId != null && mo.ExternalId.Contains(kw)) ||
                    (mo.PONo != null && mo.PONo.Contains(kw)) ||
                    (mo.CustomerNameSnapshot != null && mo.CustomerNameSnapshot.Contains(kw)) ||
                    (mo.PhoneSnapshot != null && mo.PhoneSnapshot.Contains(kw)) ||

                    // tìm theo detail
                    mo.MerchandiseOrderDetails.Any(d =>
                        d.IsActive &&
                        (
                            (d.ProductExternalIdSnapshot != null && d.ProductExternalIdSnapshot.Contains(kw)) ||
                            (d.ProductNameSnapshot != null && d.ProductNameSnapshot.Contains(kw)) ||

                            // tìm theo Product
                            (d.Product.Code != null && d.Product.Code.Contains(kw)) ||
                            (d.Product.ColourCode != null && d.Product.ColourCode.Contains(kw)) ||

                            // tìm theo SampleRequest.ExternalId của Product đó
                            d.Product.SampleRequests.Any(sr =>
                                sr.IsActive &&
                                sr.ExternalId != null &&
                                sr.ExternalId.Contains(kw))
                        ))
                );
            }

            // Tìm tồn kho của sản phẩm
            var wrl = _unitOfWork.WarehouseShelfStockRepository.Query();

            // Sắp xếp: mới nhất trước (tùy bạn đổi)
            baseQ = baseQ.OrderByDescending(mo => mo.CreateDate);

            var items = await baseQ.Skip((query.PageNumber - 1) * query.PageSize)
                            .Take(query.PageSize)
                .Select(mo => new GetPODeliveryOrder
                {
                    MerchandiseOrderId = mo.MerchandiseOrderId,
                    ExternalId = mo.ExternalId,

                    CustomerId = mo.CustomerId,
                    CustomerNameSnapshot = mo.CustomerNameSnapshot,
                    CustomerExternalIdSnapshot = mo.CustomerExternalIdSnapshot,
                    PhoneSnapshot = mo.PhoneSnapshot,

                    Note = mo.Note,
                    ShippingMethod = null, // TODO: nếu bạn có field ShippingMethod
                    PONo = mo.PONo,

                    Receiver = mo.Receiver,
                    DeliveryAddress = mo.DeliveryAddress,
                    PaymentType = mo.PaymentType,

                    Status = mo.Status,
                    Currency = mo.Currency,

                    PODetailDeliveryOrder = mo.MerchandiseOrderDetails
                        .Where(d => d.IsActive)
                        .Select(d => new GetPODetailDeliveryOrder
                        {
                            MerchandiseOrderDetailId = d.MerchandiseOrderDetailId,
                            ProductId = d.ProductId,
                            ProductExternalIdSnapShot = d.ProductExternalIdSnapshot,
                            ProductNameSnapShot = d.ProductNameSnapshot,

                            Note = d.Comment ?? string.Empty,
                            getQuantityAndLots = wrl
                                // Match tồn kho theo mã thành phẩm
                                .Where(w => w.Code == d.ProductExternalIdSnapshot)
                                .Where(w => w.QtyKg > 0)
                                .GroupBy(w => new
                                {
                                    w.LotNo,
                                    w.StockType,
                                    CompanyName = w.Company.Name
                                })
                                .Select(g => new GetQuantityAndLots
                                {
                                    companyName = g.Key.CompanyName ?? string.Empty,
                                    lotcode = g.Key.LotNo ?? string.Empty,
                                    quantity = g.Sum(x => x.QtyKg),
                                    stockType = g.Key.StockType
                                })
                                .ToList(),

                            PONo = mo.PONo,

                            // Số lượng còn có thể giao = Expected - đã giao
                            RequestQuantity =
                                d.ExpectedQuantity
                                - (
                                    d.DeliveryOrderDetails
                                        .Where(x => x.IsActive && !x.IsAttach)
                                        .Sum(x => (decimal?)x.Quantity) ?? 0m
                                  ),

                            //StockQuantity = 0m
                        })
                        // Chỉ giữ những dòng còn RequestQuantity > 0
                        .Where(x => x.RequestQuantity > 0)
                        .OrderBy(x => x.ProductExternalIdSnapShot)
                        .ToList()
                })
                .Where(po => po.PODetailDeliveryOrder.Any())
                .ToListAsync(ct);

            var pagedResult = new PagedResult<GetPODeliveryOrder>(
                items,
                items.Count,
                query.PageNumber,
                query.PageSize);

            return OperationResult<PagedResult<GetPODeliveryOrder>>.Ok(pagedResult, "Tạo phiếu giao hàng thành công");
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

        // ======================================================================== Post ======================================================================== 

        // Tạm thời cmt thay thế chức năng mới
        /// <summary>
        /// Tạo mới đơn giao hàng (Delivery Order)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<OperationResult<GetDeliveryOrder>> CreateAsync(PostDeliveryOrder request, CancellationToken ct = default)
        {

            var companyId = _currentUser.CompanyId;
            var userId = _currentUser.EmployeeId;
            var now = DateTime.Now;

            var externalId = await _idService.NextAsync(DocumentPrefix.PGH.ToString(), ct: ct);

            // 1. Validate cơ bản
            if (request == null)
                return OperationResult<GetDeliveryOrder>.Fail("Request is null");

            if (request.CompanyId == Guid.Empty)
                return OperationResult<GetDeliveryOrder>.Fail("CompanyId is required");

            if (request.CustomerId == Guid.Empty)
                return OperationResult<GetDeliveryOrder>.Fail("CustomerId is required");

            if (request.postDeliveryOrderDetails == null || !request.postDeliveryOrderDetails.Any())
                return OperationResult<GetDeliveryOrder>.Fail("Phiếu giao hàng phải có ít nhất 1 dòng chi tiết");


            // 3. Tạo entity DeliveryOrder
            var deliveryOrder = new DeliveryOrder
            {
                Id = Guid.CreateVersion7(), // hoặc GuidV7 nếu bạn đang dùng
                ExternalId = externalId,
                Status = string.IsNullOrWhiteSpace(request.Status) ? DeliveryOrderStatus.Pending.ToString() : request.Status,

                CustomerId = request.CustomerId,
                CustomerExternalIdSnapShot = request.CustomerExternalIdSnapShot,

                Receiver = request.Receiver,
                DeliveryAddress = request.DeliveryAddress,
                PaymentType = request.PaymentType,
                PaymentDeadline = request.PaymentDeadline,
                TaxNumber = request.TaxNumber,
                PhoneSnapshot = request.PhoneSnapshot,

                RequiresUnloading = request.RequiresUnloading,

                Note = request.Note,
                IsActive = request.IsActive,

                CreatedBy = userId,
                CreatedDate = request.CreateDate ?? now,
                CompanyId = companyId,

                HasPrinted = false
            };

            // 4. Map Details từ PostDeliveryOrderDetail
            deliveryOrder.Details = request.postDeliveryOrderDetails
                .Select(d => new DeliveryOrderDetail
                {
                    Id = Guid.CreateVersion7(),
                    DeliveryOrderId = deliveryOrder.Id,

                    MerchandiseOrderDetailId = d.MerchandiseOrderDetailId,
                    ProductId = d.ProductId,

                    ProductExternalIdSnapShot = d.ProductExternalIdSnapShot,
                    ProductNameSnapShot = d.ProductNameSnapShot,

                    LotNoList = d.LotNoList,
                    PONo = d.PONo,

                    Quantity = d.Quantity,
                    NumOfBags = d.NumOfBags,

                    IsAttach = d.IsAttach,
                    IsActive = true
                })
                .ToList();

            // 5. Map DeliveryOrderPO (DO <-> PO)
            //    - Lấy từ SelectedPOIds
            //    - Và từ từng detail.MerchandiseOrderId (nếu có)
            var poIds = new HashSet<Guid>();

            if (request.SelectedPOIds != null)
            {
                foreach (var poId in request.SelectedPOIds.Where(x => x != Guid.Empty))
                {
                    poIds.Add(poId);
                }
            }


            deliveryOrder.DeliveryOrderPOs = poIds
                .Select(poId => new DeliveryOrderPO
                {
                    DeliveryOrderId = deliveryOrder.Id,
                    MerchandiseOrderId = poId,
                    IsActive = true
                })
                .ToList();

            // 6. Map Deliverers (người giao)
            //    Giả định: bảng join tên DeliveryOrderDeliverer
            if (request.Deliverers != null && request.Deliverers.Any())
            {
                deliveryOrder.Deliverers = request.Deliverers
                    .Where(id => id != Guid.Empty)
                    .Select(delivererId => new Deliverer
                    {
                        DeliveryOrderId = deliveryOrder.Id,
                        DelivererInforId = delivererId
                    })
                    .ToList();
            }

            await _warehouseReservationService.EnsureWarehouseRequestForDOAsync(deliveryOrder, now, userId, companyId, ct);

            // 8. Lưu vào DB trong cùng UnitOfWork
            await _unitOfWork.DeliveryOrderRepository.AddAsync(deliveryOrder, ct);
            //await _unitOfWork.WarehouseRequestRepository.AddAsync(warehouseRequest, ct);

            await _unitOfWork.SaveChangesAsync();

            // 8. Re-query lại bằng GetAsync để trả về DTO đầy đủ (Details + Deliverers + POExternalIds...)
            var dto = await GetAsync(deliveryOrder.Id, ct);

            return OperationResult<GetDeliveryOrder>.Ok(dto, "Tạo phiếu giao hàng thành công");
        }

        public async Task<OperationResult> CreateDelivererAsync(PostDelivererDTO request, CancellationToken ct = default)
        {
            if (request == null)
                return OperationResult.Fail("Request is null");
            if (string.IsNullOrWhiteSpace(request.Name))
                return OperationResult.Fail("Tên người giao hàng không được để trống");
            var entity = new DelivererInfor
            {
                Id = Guid.CreateVersion7(),
                Name = request.Name,
                DelivererType = request.DelivererType,
                Phone = request.Phone,
                Note = request.Note,
                IsActive = true
            };
            await _unitOfWork.DelivererInforRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return OperationResult.Ok("Tạo người giao hàng thành công");    
        }

        // ======================================================================== Patch ======================================================================== 

        /// <summary>
        /// Thay đổi thông tin đơn giao hàng (Delivery Order)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateAsync(PatchDeliveryOrder req, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;

                var existingDO = await _unitOfWork.DeliveryOrderRepository
                    .Query(track: true)
                    .FirstOrDefaultAsync(p => p.Id == req.Id && p.IsActive, ct);

                if (existingDO == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return OperationResult.Fail("Đơn giao hàng không tồn tại.");
                }

                existingDO.UpdatedDate = now;
                existingDO.UpdatedBy = userId;

                PatchHelper.SetIfRef(req.Note, () => existingDO.Note, v => existingDO.Note = v);
                PatchHelper.SetIfRef(req.Status, () => existingDO.Status, v => existingDO.Status = v ?? string.Empty);

                // ===== SYNC Deliverers (replace set) =====
                if (req.Deliverers != null)
                {
                    var incomingIds = req.Deliverers.Where(x => x != Guid.Empty).Distinct().ToList();

                    // 1) validate DelivererInfor tồn tại
                    var validInforIds = await _unitOfWork.DelivererInforRepository
                        .Query(track: false)
                        .Where(x => incomingIds.Contains(x.Id))
                        .Select(x => x.Id)
                        .ToListAsync(ct);

                    if (validInforIds.Count != incomingIds.Count)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return OperationResult.Fail("Có DelivererInforId không hợp lệ.");
                    }

                    // 2) lấy existing set từ DB (no tracking)
                    var existingIds = await _unitOfWork.DelivererRepository
                        .Query(track: false)
                        .Where(d => d.DeliveryOrderId == existingDO.Id)
                        .Select(d => d.DelivererInforId)
                        .ToListAsync(ct);

                    var toAdd = validInforIds.Except(existingIds).ToList();
                    var toRemove = existingIds.Except(validInforIds).ToList();

                    // 3) delete theo query => KHÔNG dính concurrency kiểu Remove(entity)
                    if (toRemove.Count > 0)
                    {
                        await _unitOfWork.DelivererRepository
                            .Query(track: false)
                            .Where(d => d.DeliveryOrderId == existingDO.Id && toRemove.Contains(d.DelivererInforId))
                            .ExecuteDeleteAsync(ct);
                    }

                    // 4) add missing
                    foreach (var id in toAdd)
                    {
                        await _unitOfWork.DelivererRepository.AddAsync(new Deliverer
                        {
                            Id = Guid.CreateVersion7(),
                            DeliveryOrderId = existingDO.Id,
                            DelivererInforId = id,
                            // nếu Deliverer có audit fields thì set luôn như Customer:
                            // CreatedDate = now, CreatedBy = userId, UpdatedDate = now, UpdatedBy = userId
                        }, ct);
                    }
                }

                await _unitOfWork.SaveChangesAsync(ct);
                await _unitOfWork.CommitTransactionAsync();
                return OperationResult.Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail("Dữ liệu đã bị thay đổi bởi người khác. Vui lòng tải lại và thử lại.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail(ex.InnerException?.Message ?? ex.Message);
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
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;
            var now = DateTime.Now;
            try
            {
                var existingDO = await _unitOfWork.DeliveryOrderRepository.Query(track: true)
                    .Where(p => p.Id == id && p.IsActive)
                    .Include(x => x.Details)                 // cần entity để set IsActive=false
                    .Include(x => x.DeliveryOrderPOs)        // cần PO Id + WarehouseRequestId
                    .AsSplitQuery()                          // giảm bùng nổ join
                    .FirstOrDefaultAsync(ct);

                if (existingDO == null)
                    return OperationResult.Fail("Đơn giao hàng không tồn tại.");

                // 2) Gộp số lượng rollback theo MODetailId
                var minusByMoDetailId = existingDO.Details
                    .Where(d => d.IsActive && d.MerchandiseOrderDetailId.HasValue)
                    .GroupBy(d => d.MerchandiseOrderDetailId!.Value)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

                var moDetailIds = minusByMoDetailId.Keys.ToList();

                // lấy detail cần rollback
                var moDetails = await _unitOfWork.MerchandiseOrderRepository.QueryDetail(track: true)
                    .Where(d => moDetailIds.Contains(d.MerchandiseOrderDetailId))
                    .ToListAsync(ct);

                // rollback detail
                foreach (var d in moDetails)
                {
                    var minus = minusByMoDetailId.GetValueOrDefault(d.MerchandiseOrderDetailId, 0m);
                    if (minus <= 0) continue;

                    var after = Math.Max(0m, (d.RealQuantity ?? 0m) - minus);
                    d.RealQuantity = after;

                    d.Status = (after >= d.ExpectedQuantity && d.ExpectedQuantity > 0)
                        ? MerchadiseStatus.Delivered.ToString()
                        : MerchadiseStatus.Delivering.ToString();

                }

                // affected PO lấy từ detail + bảng nối
                var affectedPoIdsFromDetails = moDetails
                    .Select(x => x.MerchandiseOrderId)
                    .Distinct()
                    .ToList();

                var affectedPoIds = existingDO.DeliveryOrderPOs
                    .Select(x => x.MerchandiseOrderId)
                    .Union(affectedPoIdsFromDetails)
                    .Distinct()
                    .ToList();

                // load header thật
                var poHeaders = await _unitOfWork.MerchandiseOrderRepository.Query(track: true)
                    .Include(po => po.MerchandiseOrderDetails.Where(dd => dd.IsActive))
                    .Where(po => affectedPoIds.Contains(po.MerchandiseOrderId))
                    .ToListAsync(ct);

                foreach (var po in poHeaders)
                {
                    var ds = po.MerchandiseOrderDetails.Where(x => x.IsActive).ToList();

                    bool allCompleted = ds.Count > 0 &&
                                        ds.All(d => (d.RealQuantity ?? 0m) >= d.ExpectedQuantity && d.ExpectedQuantity > 0m);

                    bool anyHasQty = ds.Any(d => (d.RealQuantity ?? 0m) > 0m);

                    po.Status = allCompleted
                        ? MerchadiseStatus.Delivered.ToString()
                        : MerchadiseStatus.Delivering.ToString();

                    po.UpdatedBy = userId;
                    po.UpdatedDate = now;
                }

                // 5) Soft delete DO + Details (đang có entity → set trực tiếp)
                existingDO.IsActive = false;
                existingDO.Status = "Cancelled";
                existingDO.UpdatedBy = userId;
                existingDO.UpdatedDate = now;

                foreach (var dd in existingDO.Details.Where(x => x.IsActive))
                    dd.IsActive = false;

                var wrIds = new List<int>();
                if (!string.IsNullOrWhiteSpace(existingDO.ExternalId))
                {
                    wrIds = await _unitOfWork.WarehouseRequestRepository.Query(track: false)
                        .Where(wr => wr.IsActive
                                  && wr.codeFromRequest == existingDO.ExternalId)
                        .Select(wr => wr.RequestId)
                        .Distinct()
                        .ToListAsync(ct);
                }

                if (wrIds.Count > 0)
                {
                    // Parent
                    await _unitOfWork.WarehouseRequestRepository.Query(track: false)
                        .Where(wr => wrIds.Contains(wr.RequestId) && wr.IsActive)
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(x => x.IsActive, _ => false)
                            .SetProperty(x => x.ReqStatus, _ => WarehouseRequestStatus.Cancelled)
                            .SetProperty(x => x.UpdatedBy, _ => userId)
                            .SetProperty(x => x.UpdatedDate, _ => now), ct);

                    // Details
                    await _unitOfWork.WarehouseRequestDetailRepository.Query(track: false)
                        .Where(wrd => wrIds.Contains(wrd.RequestId) && wrd.IsActive)
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(x => x.IsActive, _ => false));

                    // (Optional) Release temp stock nếu bạn có
                    // await ReleaseTempStockByWRIdsAsync(wrIds, ct);
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



        // ======================================================================== Helper ======================================================================== 

        /// <summary>
        /// Record lưu thông tin phân bổ tồn kho trên kệ
        /// </summary>
        /// <param name="SlotId"></param>
        /// <param name="ProductCode"></param>
        /// <param name="LotKey"></param>
        /// <param name="Qty"></param>
        /// <param name="StockType"></param>
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




        public async Task<List<DeliveryPlanRow>> BuildRowsAsync(DateTime from, DateTime to, CancellationToken ct)
        {
            var q = _unitOfWork.MfgOrderPORepository.Query()
                .Where(x => x.IsActive)
                .Where(x => x.Detail.IsActive)
                .Where(x => x.Detail.DeliveryRequestDate >= from && x.Detail.DeliveryRequestDate < to.AddDays(1))
                .Select(x => new
                {
                    Order = x.Detail.MerchandiseOrder,
                    Detail = x.Detail,
                    Prod = x.ProductionOrder,
                    Customer = x.Detail.MerchandiseOrder.Customer,

                    // ✅ Lấy list ExternalId của ManufacturingFormula đang chọn
                    VAExternalIds = x.ProductionOrder.ProductionSelectVersions
                        .Where(v => v.ValidTo == null && v.ManufacturingFormula != null)
                        .Select(v => v.ManufacturingFormula!.ExternalId)
                        .Distinct()
                        .ToList()
                });

            var data = await q.ToListAsync(ct);

            var rows = new List<DeliveryPlanRow>();
            var stt = 1;

            foreach (var it in data)
            {
                var prod = it.Prod;

                var pending = prod.TotalQuantity == null;
                var qtyText = pending ? $"{prod.TotalQuantityRequest}*" : prod.TotalQuantityRequest.ToString();

                // join list VA externalId -> chuỗi
                var vaList = (it.VAExternalIds ?? new List<string>())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .ToList();

                var vaText = vaList.Count == 0 ? "" : string.Join(", ", vaList);

                rows.Add(new DeliveryPlanRow
                {
                    Stt = stt++,
                    CustomerName = it.Order.CustomerNameSnapshot,
                    Code = it.Detail.ProductExternalIdSnapshot,

                    // ✅ thay LotNo bằng danh sách VA externalId
                    LotNo = vaText,

                    QuantityText = qtyText,
                    QuantityIsPending = pending,

                    Factory = "",
                    PickupTimeText = "",
                    Driver = "",
                    Note = it.Detail.Comment ?? "",
                    Address = it.Order.DeliveryAddress
                });
            }

            return rows;
        }



        public async Task<List<DeliveryFinishRow>> BuildDeliveryFinishRowsAsync(
            DateTime fromDate,
            DateTime toDate,
            CancellationToken ct)
        {
            var from = fromDate.Date;
            var to = toDate.Date.AddDays(1); // [from, to)

            var deliveryData = await _unitOfWork.DeliveryOrderRepository.Query()
                .Where(d => d.IsActive == true)
                .Where(d => d.CreatedDate >= from && d.CreatedDate < to)
                .SelectMany(
                    d => d.Details.Where(det => det.IsActive),
                    (d, det) => new
                    {
                        Delivery = d,
                        Det = det,

                        // MO Detail (optional)
                        MODetail = det.MerchandiseOrderDetail,

                        // MO (optional)
                        MO = det.MerchandiseOrderDetail != null
                            ? det.MerchandiseOrderDetail.MerchandiseOrder
                            : null,

                        // người giao đầu tiên
                        DelivererNames = d.Deliverers
                            .Select(x => x.DelivererInfor.Name)
                            .Where(n => n != null && n != "")
                            .Distinct()
                            .ToList()
                    })
                .ToListAsync(ct);

            // Lấy các MerchandiseOrderDetailId hợp lệ để query MfgOrderPO
            var moDetailIds = deliveryData
                .Where(x => x.MODetail != null)
                .Select(x => x.MODetail!.MerchandiseOrderDetailId)
                .Distinct()
                .ToList();

            // Lookup: MerchandiseOrderDetailId -> Max(ExpectedDate)
            var maxExpectedDateByDetailId = moDetailIds.Count == 0
                ? new Dictionary<Guid, DateTime?>()
                : await _unitOfWork.MfgOrderPORepository.Query()
                    .Where(x => x.IsActive)
                    .Where(x => moDetailIds.Contains(x.MerchandiseOrderDetailId))
                    .Where(x => x.ProductionOrder != null && x.ProductionOrder.IsActive)
                    .GroupBy(x => x.MerchandiseOrderDetailId)
                    .Select(g => new
                    {
                        MerchandiseOrderDetailId = g.Key,
                        ExpectedDeliveryDate = g.Max(x => x.ProductionOrder.ExpectedDate)
                    })
                    .ToDictionaryAsync(
                        x => x.MerchandiseOrderDetailId,
                        x => x.ExpectedDeliveryDate,
                        ct);

            var rows = new List<DeliveryFinishRow>(capacity: deliveryData.Count);

            foreach (var it in deliveryData)
            {
                var mo = it.MO;
                var md = it.MODetail;

                var deliverer = (it.DelivererNames != null && it.DelivererNames.Count > 0)
                    ? it.DelivererNames[0]
                    : "";

                DateTime? expectedDeliveryDate = null;

                if (md != null &&
                    maxExpectedDateByDetailId.TryGetValue(md.MerchandiseOrderDetailId, out var maxExpected))
                {
                    expectedDeliveryDate = maxExpected;
                }

                rows.Add(new DeliveryFinishRow
                {
                    DeliveryExternalId = it.Delivery.ExternalId,
                    OrderExternalId = mo?.ExternalId,

                    OrderCreatedDate = mo != null ? mo.CreateDate : (DateTime?)null,
                    DeliveryRequestDate = md != null ? md.DeliveryRequestDate : (DateTime?)null,

                    // lấy từ MfgProductionOrder.ExpectedDate lớn nhất
                    ExpectedDeliveryDate = expectedDeliveryDate,

                    DeliveryActualDate = it.Delivery.CreatedDate,

                    CustomerName = mo != null
                        ? mo.CustomerNameSnapshot
                        : it.Delivery.CustomerExternalIdSnapShot,

                    DelivererName = deliverer,

                    ProductDisplay =
                        (it.Det.ProductExternalIdSnapShot ?? "")
                        + " - "
                        + (it.Det.ProductNameSnapShot ?? ""),

                    WarehouseDisplay = "",
                    LotNoOrBatch = it.Det.LotNoList,
                    QuantityKg = it.Det.Quantity,
                    NumOfBags = it.Det.NumOfBags,
                    PoNo = it.Det.PONo,

                    Note = it.Delivery.Note
                });
            }

            return rows;
        }


        public async Task<DeliveryTransportWorkbookData> BuildTransportWorkbookDataAsync(
    DateTime fromDate,
    DateTime toDate,
    CancellationToken ct)
        {
            var from = fromDate.Date;
            var to = toDate.Date.AddDays(1); // [from, to)

            var orders = await _unitOfWork.DeliveryOrderRepository.Query()
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Where(x => x.CreatedDate >= from && x.CreatedDate < to)
                .Include(x => x.Customer)
                .Include(x => x.Details)
                .Include(x => x.Deliverers)
                    .ThenInclude(x => x.DelivererInfor)
                .ToListAsync(ct);

            var detailRows = new List<DeliveryTransportReportRow>();

            foreach (var order in orders)
            {
                var activeDetails = order.Details
                    .Where(x => x.IsActive)
                    .ToList();

                if (!activeDetails.Any())
                    continue;

                var delivererNames = order.Deliverers?
                    .Where(x => x.DelivererInfor != null && x.DelivererInfor.IsActive)
                    .Select(x => x.DelivererInfor.Name)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList()
                    ?? new List<string>();

                if (!delivererNames.Any())
                    delivererNames.Add("Chưa phân người giao");

                var totalQuantity = activeDetails.Sum(x => x.Quantity);
                var totalBags = activeDetails.Sum(x => x.NumOfBags);

                var productDisplay = string.Join("; ",
                    activeDetails
                        .Select(x =>
                            $"{(x.ProductExternalIdSnapShot ?? "").Trim()} - {(x.ProductNameSnapShot ?? "").Trim()}".Trim(' ', '-'))
                        .Where(x => !string.IsNullOrWhiteSpace(x) && x != "-")
                        .Distinct());

                var lotNoDisplay = string.Join(", ",
                    activeDetails
                        .Select(x => x.LotNoList)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Distinct());

                var poNoDisplay = string.Join(", ",
                    activeDetails
                        .Select(x => x.PONo)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Distinct());

                var splitPrice = delivererNames.Count > 0
                    ? (order.DeliveryPrice ?? 0m) / delivererNames.Count
                    : (order.DeliveryPrice ?? 0m);

                foreach (var delivererName in delivererNames)
                {
                    detailRows.Add(new DeliveryTransportReportRow
                    {
                        DeliveryOrderId = order.Id,
                        DeliveryExternalId = order.ExternalId,
                        DeliveryDate = order.CreatedDate,

                        DelivererName = delivererName,

                        CustomerCode = order.CustomerExternalIdSnapShot,
                        CustomerName = order.Customer?.CustomerName ?? order.CustomerExternalIdSnapShot,
                        Address = order.DeliveryAddress,
                        Receiver = order.Receiver,
                        Phone = order.PhoneSnapshot,

                        ProductDisplay = productDisplay,
                        LotNoDisplay = lotNoDisplay,
                        PoNoDisplay = poNoDisplay,

                        TotalQuantity = totalQuantity,
                        TotalBags = totalBags,
                        DeliveryPrice = splitPrice,

                        Note = order.Note
                    });
                }
            }

            var grouped = detailRows
                .GroupBy(x => x.DelivererName.Trim(), StringComparer.OrdinalIgnoreCase)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(x => x.DeliveryDate)
                          .ThenBy(x => x.DeliveryExternalId)
                          .ToList(),
                    StringComparer.OrdinalIgnoreCase);

            var summaryRows = grouped
                .Select((g, index) => new DeliveryTransportSummaryRow
                {
                    Stt = index + 1,
                    DelivererName = g.Key,
                    TotalTrips = g.Value.Count,
                    TotalQuantity = g.Value.Sum(x => x.TotalQuantity),
                    TotalAmount = g.Value.Sum(x => x.DeliveryPrice),
                    AdvanceAmount = 0,
                    Note = null
                })
                .ToList();

            return new DeliveryTransportWorkbookData
            {
                SummaryRows = summaryRows,
                DetailByDeliverer = grouped
            };
        }
    }

}
