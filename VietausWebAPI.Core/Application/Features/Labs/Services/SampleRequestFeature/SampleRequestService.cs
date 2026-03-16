using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Queries.CreateSampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Helper.PriceHelpers;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.Core.Domain.Enums.Products;
using VietausWebAPI.Core.Domain.Enums.Visibilitys;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature
{
    public class SampleRequestService : ISampleRequestService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly INotificationService _notificationService;
        private readonly IVisibilityHelper _visibilityHelper;
        private readonly IPriceProvider _priceProvider;

        public SampleRequestService(IUnitOfWork unitOfWork
                                  , IMapper mapper
                                  , ICurrentUser currentUser
                                  , INotificationService notificationService
                                  , IVisibilityHelper visibilityHelper
            , IPriceProvider priceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
            _notificationService = notificationService;
            _visibilityHelper = visibilityHelper;
            _priceProvider = priceProvider;
        }

        // ========================================================================= Get ========================================================================= 
        /// <summary>
        /// Trả về mẫu theo ID kèm sản phẩm và công thức (nếu có)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GetSampleWithProductRequest> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _unitOfWork.SampleRequestRepository.Query()
                    .Where(c => c.SampleRequestId == id && c.IsActive == true)
                    .Select(x => new
                    {
                        // Sample
                        x.SampleRequestId,
                        x.ExternalId,
                        x.CustomerId,
                        CustomerName = x.Customer.CustomerName,
                        CustomerCode = x.Customer.ExternalId,
                        ManagerName = x.ManagerByNavigation.FullName, // đổi sang đúng property tên thật bên Employee nếu khác
                        x.ProductId,
                        x.AttachmentCollectionId,
                        x.RealDeliveryDate,
                        x.ExpectedDeliveryDate,
                        x.RequestDeliveryDate,
                        x.RequestTestSampleDate,
                        x.ResponseDeliveryDate,
                        x.RealPriceQuoteDate,
                        x.ExpectedPriceQuoteDate,
                        x.NumberDeliverySampleDate,
                        x.RequestType,
                        x.ExpectedQuantity,
                        x.ExpectedPrice,
                        x.SampleQuantity,
                        x.OtherComment,
                        x.InfoType,
                        x.FormulaId,
                        x.SaleComment,
                        x.AdditionalComment,
                        x.CustomerProductCode,
                        x.BranchId,
                        x.Status,
                        x.Package,
                        x.Formula,

                        // Product
                        Product = new
                        {
                            x.Product.ProductId,
                            x.Product.ColourCode,
                            x.Product.Name,
                            x.Product.ColourName,
                            x.Product.Additive,
                            x.Product.UsageRate,
                            x.Product.DeltaE,
                            x.Product.Requirement,
                            x.Product.ExpiryType,
                            x.Product.StorageCondition,
                            x.Product.LabComment,
                            x.Product.Procedure,
                            x.Product.RecycleRate,
                            x.Product.TaicalRate,
                            x.Product.Application,
                            x.Product.ProductUsage,
                            x.Product.PolymerMatchedIn,
                            x.Product.Code,
                            x.Product.EndUser,
                            x.Product.FoodSafety,
                            x.Product.RohsStandard,
                            x.Product.ReachStandard,
                            x.Product.MaxTemp,
                            x.Product.WeatherResistance,
                            x.Product.LightCondition,
                            x.Product.VisualTest,
                            x.Product.ReturnSample,
                            x.Product.OtherComment,
                            x.Product.CategoryId,
                            x.Product.Weight,
                            x.Product.Unit,
                            x.Product.IsRecycle
                        }
                    })
                    .FirstOrDefaultAsync(ct);
                // Map sang DTO t   rả về
                var sample = new GetSampleRequest
                {
                    SampleRequestId = dto.SampleRequestId,
                    CustomerId = dto.CustomerId,
                    ExternalId = dto.ExternalId,
                    CustomerName = dto.CustomerName,
                    CustomerCode = dto.CustomerCode,
                    ManagerName = dto.ManagerName,
                    ProductId = dto.ProductId,
                    AttachmentCollectionId = dto.AttachmentCollectionId, // có thể null
                    RealDeliveryDate = dto.RealDeliveryDate,
                    ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
                    RequestDeliveryDate = dto.RequestDeliveryDate,
                    RequestTestSampleDate = dto.RequestTestSampleDate,
                    ResponseDeliveryDate = dto.ResponseDeliveryDate,
                    RealPriceQuoteDate = dto.RealPriceQuoteDate,
                    ExpectedPriceQuoteDate = dto.ExpectedPriceQuoteDate,
                    NumberDeliverySampleDate = dto.NumberDeliverySampleDate,
                    RequestType = dto.RequestType ?? string.Empty,
                    ExpectedQuantity = dto.ExpectedQuantity,
                    ExpectedPrice = dto.ExpectedPrice,
                    SampleQuantity = dto.SampleQuantity,
                    OtherComment = dto.OtherComment,
                    InfoType = dto.InfoType,
                    //FormulaId = dto.FormulaId,
                    // FormulaPrice sẽ set ở dưới nếu có FormulaId
                    Formula = dto.Formula != null
                        ? new GetSampleFormula
                        {
                            FormulaId = dto.Formula.FormulaId,
                            ExternalId = dto.Formula.ExternalId,
                            TotalPrice = dto.Formula.TotalPrice,
                            Status = dto.Formula.Status,
                            Note = dto.Formula.Note,
                            Name = dto.Formula.Name,
                            PresidentPrice = dto.Formula.PresidentPrice,
                            ProductionPrice = dto.Formula.ProductionPrice,
                            EffectiveDate = dto.Formula.EffectiveDate,
                            IsSelect = true
                        }
                        : null,
                    SaleComment = dto.SaleComment,
                    AdditionalComment = dto.AdditionalComment ?? string.Empty,
                    CustomerProductCode = dto.CustomerProductCode ?? string.Empty,
                    BranchId = dto.BranchId,
                    Status = string.IsNullOrWhiteSpace(dto.Status) ? "New" : dto.Status,
                    Package = dto.Package ?? string.Empty
                };

                var product = new GetProduct
                {
                    ProductId = dto.Product.ProductId,
                    ColourCode = dto.Product.ColourCode,
                    Name = dto.Product.Name ?? string.Empty,    // bắt buộc
                    ColourName = dto.Product.ColourName,
                    Additive = dto.Product.Additive,
                    UsageRate = dto.Product.UsageRate,
                    DeltaE = dto.Product.DeltaE,
                    Requirement = dto.Product.Requirement,
                    ExpiryType = dto.Product.ExpiryType,
                    StorageCondition = dto.Product.StorageCondition,
                    LabComment = dto.Product.LabComment,
                    Procedure = dto.Product.Procedure,
                    RecycleRate = dto.Product.RecycleRate,
                    TaicalRate = dto.Product.TaicalRate,
                    Application = dto.Product.Application,
                    ProductUsage = dto.Product.ProductUsage,
                    PolymerMatchedIn = dto.Product.PolymerMatchedIn,
                    Code = dto.Product.Code,
                    EndUser = dto.Product.EndUser,
                    FoodSafety = dto.Product.FoodSafety,
                    RohsStandard = dto.Product.RohsStandard,
                    ReachStandard = dto.Product.ReachStandard,
                    MaxTemp = dto.Product.MaxTemp,
                    WeatherResistance = dto.Product.WeatherResistance,
                    LightCondition = dto.Product.LightCondition,
                    VisualTest = dto.Product.VisualTest,
                    ReturnSample = dto.Product.ReturnSample,
                    OtherComment = dto.Product.OtherComment,
                    CategoryId = dto.Product.CategoryId,
                    Weight = dto.Product.Weight,
                    Unit = dto.Product.Unit,
                    IsRecycle = dto.Product.IsRecycle
                };

                // Tính FormulaPrice nếu có FormulaId (tái sử dụng CalcFromVU)
                //if (sample.Formula != null && sample.Formula.FormulaId != Guid.Empty)
                //{ 
                //    // Lưu ý: CalcFromVU trả về decimal (đã round 2). Có thể mất ~vài ms do query BOM + giá
                //    var price = await _priceProvider.CalculatePriceAsync(sample.Formula.FormulaId, FormulaSource.FromVU, ct);
                //    sample.Formula.TotalPrice = price;
                //}
                //else
                //{
                //    sample.FormulaPrice = null;
                //}

                return new GetSampleWithProductRequest
                {
                    Product = product,
                    Sample = sample
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin mẫu: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách mẫu với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<SampleRequestSummaryDTO>> GetAllAsync(SampleRequestQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var viewer = await _visibilityHelper.BuildViewerScopeAsync(ct);

                var result = _unitOfWork.SampleRequestRepository.Query()
                    .Where(x => x.IsActive == true);

                // Áp quyền xem
                result = _visibilityHelper.ApplySampleRequest(result, viewer);

                // Block customer mặc định với SalesScoped, nhưng cho phép hiện khi đang search keyword
                var blockedCustomerId = Guid.Parse("019bd983-28a1-7231-810a-14c03e090b75");
                var hasKeyword = !string.IsNullOrWhiteSpace(query.Keyword);

                if (viewer.ScopeType == ViewerScopeType.SalesScoped && !hasKeyword)
                {
                    result = result.Where(x => x.CustomerId != blockedCustomerId);
                }

                // Keyword
                if (hasKeyword)
                {
                    var keyword = query.Keyword!.Trim();

                    result = result.Where(x =>
                        (x.ExternalId ?? "").Contains(keyword)
                        || ((x.CreatedByNavigation != null ? x.CreatedByNavigation.ExternalId : "") ?? "").Contains(keyword)
                        || ((x.CreatedByNavigation != null ? x.CreatedByNavigation.FullName : "") ?? "").Contains(keyword)
                        || ((x.Product != null ? x.Product.ColourCode : "") ?? "").Contains(keyword)
                        || ((x.Customer != null ? x.Customer.ExternalId : "") ?? "").Contains(keyword)
                        || ((x.Customer != null ? x.Customer.CustomerName : "") ?? "").Contains(keyword)
                        || ((x.Product != null ? x.Product.Name : "") ?? "").Contains(keyword)
                    );
                }

                // Status
                if (query.Statuses is { Count: > 0 })
                {
                    var statuses = query.Statuses
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => s.Trim())
                        .ToList();

                    if (statuses.Count > 0)
                    {
                        result = result.Where(x => statuses.Contains(x.Status));
                    }
                }

                // Date range
                if (query.From.HasValue)
                {
                    var from = query.From.Value.Date;
                    result = result.Where(x => x.CreatedDate >= from);
                }

                if (query.To.HasValue)
                {
                    var toExclusive = query.To.Value.Date.AddDays(1);
                    result = result.Where(x => x.CreatedDate < toExclusive);
                }

                var totalCount = await result.CountAsync(ct);

                var items = await result
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ProjectTo<SampleRequestSummaryDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<SampleRequestSummaryDTO>(items, totalCount, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách mẫu: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách yêu cầu mẫu kèm sản phẩm cho VU Manufacturing, chỉ nên dùng chop lệnh sản xuất VU vì catagories này nó lấy từ công thức
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<PagedResult<GetProductSampleRequestDTO>>> GetSampleRequestForVUManufacturing(ProductQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var sampleRequestQ = _unitOfWork.SampleRequestRepository.Query();

                // 1) Ẩn product không có tên HOẶC không có colour code
                sampleRequestQ = sampleRequestQ.Where(x => !string.IsNullOrWhiteSpace(x.Product.Name)
                                                            && !string.IsNullOrWhiteSpace(x.Product.ColourCode));

                // 2) Keyword
                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    sampleRequestQ = sampleRequestQ.Where(x =>
                        (x.Product.Name ?? string.Empty).Contains(keyword) ||
                        (x.Product.ColourCode ?? string.Empty).Contains(keyword));
                }

                // 3) Filter khác
                if (query.CompanyId is Guid companyId && companyId != Guid.Empty)
                    sampleRequestQ = sampleRequestQ.Where(p => p.CompanyId == companyId);

                if (query.ProductId is Guid productId && productId != Guid.Empty)
                    sampleRequestQ = sampleRequestQ.Where(p => p.ProductId == productId);
                if (query.FormulaId is Guid formulaId && formulaId != Guid.Empty)
                {
                    sampleRequestQ = sampleRequestQ.Where(sr =>
                        sr.Product.Formulas.Any(f => f.FormulaId == formulaId && f.IsActive));
                }

                Guid? customerIdFilter = null;
                if (query.CustomerId is Guid customerId && customerId != Guid.Empty)
                {
                    customerIdFilter = customerId;
                    // chỉ lấy product nào có SR của customer đó
                    sampleRequestQ = sampleRequestQ.Where(p => sampleRequestQ
                        .Any(sr => sr.SampleRequestId == p.SampleRequestId && sr.CustomerId == customerId));
                }

                // Tổng số record (để paging)
                var totalCount = await sampleRequestQ.CountAsync(ct);

                // ==== BƯỚC 1: Lấy danh sách ProductId theo trang (nhẹ nhất) ====
                var pageIds = await sampleRequestQ
                    .OrderByDescending(p => p.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(p => p.SampleRequestId)
                    .ToListAsync(ct);

                if (pageIds.Count == 0)
                {
                    return OperationResult<PagedResult<GetProductSampleRequestDTO>>.Ok(
                        new PagedResult<GetProductSampleRequestDTO>(
                            new List<GetProductSampleRequestDTO>(), // items
                            totalCount,                            // totalCount
                            query.PageNumber,                      // page
                            query.PageSize                         // pageSize
                        )
                    );
                }


                // ==== BƯỚC 2: Lấy chi tiết các SampleRequest + Product + Formula (nặng hơn) ====
                var items = await sampleRequestQ
                    .Where(sr => pageIds.Contains(sr.SampleRequestId))
                    .OrderByDescending(sr => sr.CreatedDate) // giữ đúng thứ tự trang
                    .Select(sr => new GetProductSampleRequestDTO
                    {
                        ProductId = sr.Product.ProductId,
                        ColourCode = sr.Product.ColourCode,
                        Name = sr.Product.Name ?? string.Empty,
                        ColourName = sr.Product.ColourName,
                        UsageRate = sr.Product.UsageRate,

                        ExternalId = sr.ExternalId,
                        CustomerName = sr.Customer.CustomerName,
                        CustomerCode = sr.Customer.ExternalId,
                        SaleComment = sr.Product.Requirement ?? string.Empty,
                        AdditionalComment = sr.Product.LabComment,

                        // 1 Product -> nhiều Formula
                        SampleFormulas = sr.Product.Formulas
                            .Where(f => f.IsActive)
                            .OrderByDescending(f => f.CreatedDate)
                            .Select(f => new GetFormula
                            {
                                FormulaId = f.FormulaId,
                                ExternalId = f.ExternalId,
                                Name = f.Name,
                                Status = f.Status,

                                materialFormulas = f.FormulaMaterials
                                    .Where(mf => mf.IsActive)
                                    .OrderBy(mf => mf.LineNo)
                                    .Select(mf => new GetMaterialFormula
                                    {
                                        itemType = mf.itemType,
                                        ItemId = mf.itemType == ItemType.Material
                                            ? mf.MaterialId ?? Guid.Empty
                                            : mf.ProductId ?? Guid.Empty,

                                        CategoryId = mf.CategoryId,
                                        Quantity = mf.Quantity,
                                        UnitPrice = mf.UnitPrice,
                                        TotalPrice = mf.TotalPrice,

                                        MaterialNameSnapshot = mf.MaterialNameSnapshot,
                                        MaterialExternalIdSnapshot = mf.MaterialExternalIdSnapshot,
                                        Unit = mf.Unit
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToListAsync(ct);


                var paged = new PagedResult<GetProductSampleRequestDTO>(
                    items,
                    totalCount,
                    query.PageNumber,
                    query.PageSize
                );

                return OperationResult<PagedResult<GetProductSampleRequestDTO>>.Ok(paged);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<GetProductSampleRequestDTO>>.Fail(ex.Message);
            }
        }

        // ======================================================================== Post ========================================================================= 

        /// <summary>
        /// Tạo mới yêu cầu mẫu kèm sản phẩm 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<OperationResult<Guid>> CreateAsync(CreateSampleWithProductRequest req, CancellationToken ct = default)
        {
            // lấy info từ user đăng nhập
            var companyId = _currentUser.CompanyId;        // bạn tự xem ICurrentUser bạn đang expose thế nào
            var userId = _currentUser.EmployeeId;           // hoặc EmployeeId
            var now = DateTime.Now;

            if (companyId == Guid.Empty)
                throw new ArgumentException("CompanyId cannot be empty");

            // nếu client quên gửi sample thì báo luôn
                if (req.Sample is null)
                throw new ArgumentException("Sample payload is required", nameof(req.Sample));

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                Guid productId;

                // ====== 1. Xử lý sản phẩm trước ======
                if (req.ProductId.HasValue)
                {
                    // case 1: dùng product có sẵn
                    var exists = await _unitOfWork.ProductRepository.Query()
                        .AnyAsync(p => p.ProductId == req.ProductId.Value, ct);

                    if (!exists)
                        throw new ArgumentException("Product does not exist", nameof(req.ProductId));

                    productId = req.ProductId.Value;
                }
                else
                {
                    // case 2: tạo mới product
                    var product = _mapper.Map<Product>(req.Product);
                    product.ProductId = Guid.CreateVersion7();
                    product.CompanyId = companyId;
                    product.IsActive = true;
                    

                    await _unitOfWork.ProductRepository.AddAsync(product, ct);
                    productId = product.ProductId;
                }

                // ====== 2. Map sang entity SampleRequest ======
                var sample = _mapper.Map<SampleRequest>(req.Sample);

                sample.SampleRequestId = Guid.CreateVersion7();
                sample.NumberDeliverySampleDate = req.Sample.NumberDeliverySampleDate;
                sample.InfoType = req.Sample.InfoType;
                // ép các field hệ thống (override luôn data client gửi)
                sample.CompanyId = companyId;
                sample.CreatedBy = userId;
                sample.UpdatedBy = userId;
                sample.CreatedDate = now;
                sample.UpdatedDate = now;
                sample.IsActive = true;


                if (string.IsNullOrWhiteSpace(sample.Status))
                    sample.Status = SampleRequestStatus.New.ToString();

                // ====== 3. Tạo bucket đính kèm ======
                var bucket = new AttachmentCollection
                {
                    AttachmentCollectionId = Guid.CreateVersion7(),
                };
                await _unitOfWork.AttachmentCollectionRepository.AddAsync(bucket, ct);
                sample.AttachmentCollectionId = bucket.AttachmentCollectionId;

                // ====== 4. Sinh ExternalId (theo prefix TP) ======
                sample.ExternalId = await ExternalIdGenerator.GenerateCode(
                    "TP",
                    prefix => _unitOfWork.SampleRequestRepository.GetLatestExternalIdStartsWithAsync(prefix)
                );

                var customerName = await _unitOfWork.CustomerRepository.Query()
                    .Where(x => x.CustomerId == sample.CustomerId)
                    .Select(x => x.CustomerName)
                    .FirstOrDefaultAsync(ct);

                // gán product
                sample.ProductId = productId;


                // ====== 5. Thêm sample ======

                await _unitOfWork.SampleRequestRepository.AddAsync(sample, ct);

                var affected = await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);

                // ====== 6. Gửi notification ======
                //await _notificationService.PublishAsync(new PublishNotificationRequest
                //{
                //    Topic = TopicNotifications.ProductSampleCreated,
                //    Severity = NotificationSeverity.Info,
                //    Title = $"Yêu cầu phối mẫu mới: {sample.ExternalId}",
                //    Message = $"Khách hàng {customerName}",
                //    Link = $"/labs/product-orders/{sample.SampleRequestId}",
                //    PayloadJson = System.Text.Json.JsonSerializer.Serialize(new
                //    {
                //        ProductOrderId = sample.ProductId,
                //        CustomerId = sample.CustomerId,
                //        CreatedBy = sample.CreatedBy,
                //        CreatedDate = sample.CreatedDate
                //    }),
                //    TargetRoles = new() { RoleSets.Lab_Group }
                //}, ct);

                // mình nghĩ trả về SampleRequestId hợp lý hơn kết dính bucket
                return affected > 0
                    ? OperationResult<Guid>.Ok(sample.AttachmentCollectionId, "Tạo đơn mẫu thành công")
                    : OperationResult<Guid>.Fail("Thất bại.");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult<Guid>.Fail($"Lỗi khi tạo: {ex.Message}");
            }
        }

        // ======================================================================== Patch ======================================================================== 

        /// <summary>
        /// Xóa mềm mẫu theo điều kiện (thực chất là cập nhật IsActive = false)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OperationResult> DeleteSampleRequestAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var affected = await _unitOfWork.SampleRequestRepository.DeleteSampleRequestAsync(id);

                await _unitOfWork.CommitTransactionAsync();


                return affected > 0
                    ? OperationResult.Ok("Thay đổi thành công")
                    : OperationResult.Fail("Thay đổi thất bại");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Lỗi khi Thay đổi: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật yêu cầu phối mẫu và sản phẩm kèm theo
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateSampleRequestAsync(UpdateSampleRequest req, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                // 1) Lấy entity hiện có (track) + Product để patch
                var existing = await _unitOfWork.SampleRequestRepository.Query(track: true)
                    .Include(x => x.Product)
                    .FirstOrDefaultAsync(x => x.SampleRequestId == req.SampleRequestId, ct);

                if (existing == null)
                    return OperationResult.Fail($"Không tìm thấy yêu cầu phối mẫu với ID {req.SampleRequestId}");

                var userId = _currentUser.EmployeeId;           // hoặc EmployeeId
                existing.UpdatedDate = now;

                // 2) Nếu ColourCode là template thì phát sinh mã mới trước khi patch vào Product
                //if (req.Product?.ColourCode?.Contains('_') == true)
                //{
                //    req.Product.ColourCode = await ExternalIdGenerator.GenerateCodeFromTemplateAsync(
                //        template: req.Product.ColourCode,
                //        existingCodes: _unitOfWork.ProductRepository.Query().Select(p => p.ColourCode),
                //        getLatestCodeFunc: _unitOfWork.ProductRepository.GetLatestProductStartsWithAsync,
                //        padWidth: 3,
                //        ct: ct);
                //}
                if (req.Product?.ColourCode?.Contains('_') == true)
                {
                    req.Product.ColourCode = await ExternalIdGenerator.GenerateCodeFromTemplateAsync(
                        template: req.Product.ColourCode,
                        existingCodes: _unitOfWork.ProductRepository.Query().Select(p => p.ColourCode),
                        getMaxNumberFunc: _unitOfWork.SampleRequestRepository.GetMaxRunningNumberByLeftPrefixAsync,
                        padWidth: 3,
                        ct: ct);
                }

                // 3) Patch SampleRequest (chỉ những field cho phép)
                PatchHelper.SetIfGuid(req.CustomerId, () => existing.CustomerId, v => existing.CustomerId = v);

                PatchHelper.SetIf(req.RealDeliveryDate, () => existing.RealDeliveryDate.GetValueOrDefault(), v => existing.RealDeliveryDate = v);
                PatchHelper.SetIf(req.RequestTestSampleDate, () => existing.RequestTestSampleDate.GetValueOrDefault(), v => existing.RequestTestSampleDate = v);
                PatchHelper.SetIf(req.ExpectedDeliveryDate, () => existing.ExpectedDeliveryDate.GetValueOrDefault(), v => existing.ExpectedDeliveryDate = v);
                PatchHelper.SetIf(req.RequestDeliveryDate, () => existing.RequestDeliveryDate.GetValueOrDefault(), v => existing.RequestDeliveryDate = v);
                PatchHelper.SetIf(req.ResponseDeliveryDate, () => existing.ResponseDeliveryDate.GetValueOrDefault(), v => existing.ResponseDeliveryDate = v);
                PatchHelper.SetIf(req.RealPriceQuoteDate, () => existing.RealPriceQuoteDate.GetValueOrDefault(), v => existing.RealPriceQuoteDate = v);
                PatchHelper.SetIf(req.ExpectedPriceQuoteDate, () => existing.ExpectedPriceQuoteDate.GetValueOrDefault(), v => existing.ExpectedPriceQuoteDate = v);

                PatchHelper.SetIfRef(req.AdditionalComment, () => existing.AdditionalComment, v => existing.AdditionalComment = v);
                PatchHelper.SetIfRef(req.Status, () => existing.Status, v => existing.Status = v);
                PatchHelper.SetIfRef(req.RequestType, () => existing.RequestType, v => existing.RequestType = v);
                PatchHelper.SetIfNullable(req.ExpectedQuantity, () => existing.ExpectedQuantity, v => existing.ExpectedQuantity = v);
                PatchHelper.SetIfNullable(req.ExpectedPrice, () => existing.ExpectedPrice, v => existing.ExpectedPrice = v);
                PatchHelper.SetIfNullable(req.SampleQuantity, () => existing.SampleQuantity, v => existing.SampleQuantity = v);
                PatchHelper.SetIfRef(req.OtherComment, () => existing.OtherComment, v => existing.OtherComment = v);
                PatchHelper.SetIfRef(req.InfoType, () => existing.InfoType, v => existing.InfoType = v);
                PatchHelper.SetIfRef(req.CustomerProductCode, () => existing.CustomerProductCode, v => existing.CustomerProductCode = v);
                PatchHelper.SetIfNullable(req.FormulaId, () => existing.FormulaId, v => existing.FormulaId = v);
                PatchHelper.SetIfRef(req.SaleComment, () => existing.SaleComment, v => existing.SaleComment = v);
                PatchHelper.SetIfRef(req.Package, () => existing.Package, v => existing.Package = v);

                PatchHelper.SetIfGuid(req.BranchId, () => existing.BranchId, v => existing.BranchId = v);

                PatchHelper.SetIfNullable(req.NumberDeliverySampleDate, () => existing.NumberDeliverySampleDate, v => existing.NumberDeliverySampleDate = v);

                // 4) Patch Product (nếu có)
                var product = existing.Product;

                // fallback: nếu chưa Include được mà req có ProductId thì load riêng
                if (product == null && req.ProductId.HasValue)
                {
                    product = await _unitOfWork.ProductRepository.Query(track: true)
                        .FirstOrDefaultAsync(p => p.ProductId == req.ProductId.Value, ct);

                    if (product != null)
                        existing.Product = product;
                }

                if (product != null && req.Product != null )
                {

                    product.UpdatedBy = userId;
                    product.UpdatedDate = now;

                    // Strings / refs
                    PatchHelper.SetIfRef(req.Product.Requirement, () => product.Requirement, v => product.Requirement = v);
                    PatchHelper.SetIfRef(req.Product.Name, () => product.Name, v => product.Name = v);
                    PatchHelper.SetIfRef(req.Product.ColourCode, () => product.ColourCode, v => product.ColourCode = v);
                    PatchHelper.SetIfRef(req.Product.ColourName, () => product.ColourName, v => product.ColourName = v);
                    PatchHelper.SetIfRef(req.Product.ExpiryType, () => product.ExpiryType, v => product.ExpiryType = v);
                    PatchHelper.SetIfRef(req.Product.LabComment, () => product.LabComment, v => product.LabComment = v);
                    //PatchHelper.SetIfRef(req.Product.ProductType, () => product.ProductType, v => product.ProductType = v);
                    PatchHelper.SetIfRef(req.Product.Procedure, () => product.Procedure, v => product.Procedure = v);
                    PatchHelper.SetIfRef(req.Product.Application, () => product.Application, v => product.Application = v);
                    PatchHelper.SetIfRef(req.Product.ProductUsage, () => product.ProductUsage, v => product.ProductUsage = v);
                    PatchHelper.SetIfRef(req.Product.PolymerMatchedIn, () => product.PolymerMatchedIn, v => product.PolymerMatchedIn = v);
                    PatchHelper.SetIfRef(req.Product.Code, () => product.Code, v => product.Code = v);
                    PatchHelper.SetIfRef(req.Product.EndUser, () => product.EndUser, v => product.EndUser = v);
                    PatchHelper.SetIfRef(req.Product.OtherComment, () => product.OtherComment, v => product.OtherComment = v);
                    PatchHelper.SetIfRef(req.Product.Unit, () => product.Unit, v => product.Unit = v);

                    // Value types (nullable)
                    PatchHelper.SetIfNullable(req.Product.StorageCondition, () => product.StorageCondition, v => product.StorageCondition = v);
                    PatchHelper.SetIfNullable(req.Product.UsageRate, () => product.UsageRate, v => product.UsageRate = v);
                    PatchHelper.SetIfRef(req.Product.DeltaE, () => product.DeltaE, v => product.DeltaE = v);
                    PatchHelper.SetIfNullable(req.Product.RecycleRate, () => product.RecycleRate, v => product.RecycleRate = v);
                    PatchHelper.SetIfNullable(req.Product.TaicalRate, () => product.TaicalRate, v => product.TaicalRate = v);
                    PatchHelper.SetIfNullable(req.Product.MaxTemp, () => product.MaxTemp, v => product.MaxTemp = v);
                    PatchHelper.SetIfNullable(req.Product.Weight, () => product.Weight, v => product.Weight = v);

                    // Flags / chuẩn kiểm
                    PatchHelper.SetIfNullable(req.Product.FoodSafety, () => product.FoodSafety, v => product.FoodSafety = v);
                    PatchHelper.SetIfNullable(req.Product.RohsStandard, () => product.RohsStandard, v => product.RohsStandard = v);
                    PatchHelper.SetIfRef(req.Product.WeatherResistance, () => product.WeatherResistance, v => product.WeatherResistance = v);
                    PatchHelper.SetIfRef(req.Product.LightCondition, () => product.LightCondition, v => product.LightCondition = v);
                    PatchHelper.SetIfRef(req.Product.VisualTest, () => product.VisualTest, v => product.VisualTest = v);
                    PatchHelper.SetIfNullable(req.Product.ReturnSample, () => product.ReturnSample, v => product.ReturnSample = v);

                    // Phân loại
                    PatchHelper.SetIf(req.Product.CategoryId, () => product.CategoryId, v => product.CategoryId = v);
                    PatchHelper.SetIf(req.Product.IsRecycle, () => product.IsRecycle, v => product.IsRecycle = v);
                    PatchHelper.SetIfNullable(req.Product.ReachStandard, () => product.ReachStandard, v => product.ReachStandard = v);

                    PatchHelper.SetIfRef(req.Product.Additive, () => product.Additive, v => product.Additive = v);

                    if (!string.IsNullOrWhiteSpace(req.Product.Name) && !product.CreatedBy.HasValue && !string.IsNullOrWhiteSpace(req.Product.ColourCode))
                    {
                        product.CreatedBy = userId;
                    }
                }

                existing.UpdatedDate = now;
                existing.UpdatedBy = userId;


                if (!string.IsNullOrWhiteSpace(req.Product?.ColourCode))
                {
                    var duplicated = await _unitOfWork.ProductRepository.Query()
                        .AnyAsync(p =>
                            p.ProductId != existing.ProductId &&
                            p.ColourCode == req.Product.ColourCode, ct);

                    if (duplicated)
                        return OperationResult.Fail("Mã ColourCode vừa sinh đã tồn tại. Vui lòng nhấn Lưu lại lần nữa.");
                }

                // 5) Lưu thay đổi entity đã track
                await _unitOfWork.SaveChangesAsync();

                // 6) Xử lý chọn công thức (IsSelect) nếu có FormulaId
                if (req.FormulaId.HasValue)
                {
                    var formulaSelect = await _unitOfWork.FormulaRepository.Query()
                        .Where(f => f.FormulaId == req.FormulaId.Value)
                        .Select(f => new { f.FormulaId, f.ProductId })
                        .SingleOrDefaultAsync(ct);

                    if (formulaSelect != null)
                    {
                        await _unitOfWork.FormulaRepository.Query()
                            .Where(f => f.ProductId == formulaSelect.ProductId && f.FormulaId != formulaSelect.FormulaId)
                            .ExecuteUpdateAsync(s => s.SetProperty(f => f.IsSelect, false), ct);

                        await _unitOfWork.FormulaRepository.Query()
                            .Where(f => f.FormulaId == formulaSelect.FormulaId)
                            .ExecuteUpdateAsync(s => s.SetProperty(f => f.IsSelect, true), ct);
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
                return OperationResult.Ok("Thay đổi thành công");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi cập nhật: {ex.Message}");
            }
        }
    }
}
