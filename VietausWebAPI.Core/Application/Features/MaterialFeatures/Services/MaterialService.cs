using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Material;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Formats.Asn1;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Products;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.GetDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.PostDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.PatchDtos;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IExternalIdService _idService;

        public MaterialService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser, IExternalIdService externalIdService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
            _idService = externalIdService;
        }


        // ======================================================================== Get ======================================================================== 

        public async Task<PagedResult<GetMaterialSummary>> GetAllAsync(MaterialQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var skip = (query.PageNumber - 1) * query.PageSize;

                // Base query: chỉ lấy active luôn cho thống nhất với total
                IQueryable<Material> result = _unitOfWork.MaterialRepository
                    .Query(track: false)
                    .Where(x => x.IsActive == true);

                // Filter
                if (query.MaterialId.HasValue)
                    result = result.Where(x => x.MaterialId == query.MaterialId.Value);

                if (query.CategoryId.HasValue)
                    result = result.Where(x => x.Category.CategoryId == query.CategoryId.Value);

                if (query.From.HasValue)
                    result = result.Where(x => x.CreatedDate >= query.From.Value);

                if (query.To.HasValue)
                    result = result.Where(x => x.CreatedDate <= query.To.Value);

                if (query.SupplierId.HasValue)
                {
                    result = result.Where(x =>
                        x.MaterialsSuppliers.Any(ms =>
                            ms.SupplierId == query.SupplierId.Value
                        )
                    );
                }

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    result = result.Where(x =>
                        EF.Functions.ILike(x.ExternalId, $"%{keyword}%")
                        || EF.Functions.ILike(x.Name, $"%{keyword}%") // nếu có tên vật tư
                    );
                }

                if (!string.IsNullOrWhiteSpace(query.Category))
                {
                    var categoryKeyword = query.Category.Trim();
                    result = result.Where(x =>
                        EF.Functions.ILike(x.Category.ExternalId, $"%{categoryKeyword}%")
                        || EF.Functions.ILike(x.Category.Name, $"%{categoryKeyword}%") // nếu có
                    );
                }
                // Query lấy list + map sang DTO, có lấy Price từ MaterialsSupplier
                var supplierFilter = query.SupplierId; // capture để EF translate

                // Đếm total sau khi filter
                var total = await result.CountAsync(ct);

                // Sắp xếp + phân trang + map sang DTO
                var items = await result
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip(skip)
                    .Take(query.PageSize)
                    .Select(x => new GetMaterialSummary
                    {
                        // TODO: map đúng field theo DTO của anh
                        MaterialId = x.MaterialId,
                        ExternalId = x.ExternalId,
                        Name = x.Name,
                        CategoryId = x.CategoryId,
                        Category = x.Category.ExternalId,

                        Unit = x.Unit,

                        Weight = x.Weight,
                        Package =
    string.IsNullOrWhiteSpace(x.Package)
        ? ""
        : x.Weight != null
            ? $"{x.Package} {x.Weight}{(string.IsNullOrWhiteSpace(x.Unit) ? "" : $" ({x.Unit})")}"
            : x.Package,
                        ItemType = ItemType.Material,

                        // Lấy Price từ MaterialsSupplier
                        // Nếu có SupplierId trong query -> lấy giá của supplier đó
                        // Nếu không -> lấy giá của supplier active mới nhất
                        Price = supplierFilter.HasValue
                            ? x.MaterialsSuppliers
                                .Where(ms => ms.SupplierId == supplierFilter.Value && ms.IsActive == true)
                                .OrderByDescending(ms => ms.UpdatedDate ?? ms.CreateDate)
                                .Select(ms => ms.CurrentPrice)
                                .FirstOrDefault()
                            : x.MaterialsSuppliers
                                .Where(ms => ms.IsActive == true)
                                .OrderByDescending(ms => ms.UpdatedDate ?? ms.CreateDate)
                                .Select(ms => ms.CurrentPrice)
                                .FirstOrDefault()
                    })
                    .ToListAsync(ct);

                return new PagedResult<GetMaterialSummary>(items, total, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                // nếu muốn giữ stack trace thì log rồi throw; còn nếu trả message cho client thì dùng OperationResult thay vì throw Exception
                throw new Exception($"Lỗi khi lấy danh sách vật tư: {ex.Message}", ex);
            }
        }

        public async Task<PagedResult<GetMaterialSummary>> GetAllMPAsync(MaterialQuery query, CancellationToken ct = default)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 15;

            var skip = (query.PageNumber - 1) * query.PageSize;
            var supplierFilter = query.SupplierId;

            // -------------------- MATERIALS base --------------------
            var materialsBase = _unitOfWork.MaterialRepository
                .Query(track: false)
                .Where(x => x.IsActive == true);

            if (query.MaterialId.HasValue)
                materialsBase = materialsBase.Where(x => x.MaterialId == query.MaterialId.Value);

            if (query.CategoryId.HasValue)
                materialsBase = materialsBase.Where(x => x.CategoryId == query.CategoryId.Value);

            if (query.From.HasValue)
                materialsBase = materialsBase.Where(x => x.CreatedDate >= query.From.Value);

            if (query.To.HasValue)
                materialsBase = materialsBase.Where(x => x.CreatedDate <= query.To.Value);

            if (query.SupplierId.HasValue)
            {
                materialsBase = materialsBase.Where(x =>
                    x.MaterialsSuppliers.Any(ms => ms.SupplierId == query.SupplierId.Value));
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                materialsBase = materialsBase.Where(x =>
                    EF.Functions.ILike(x.ExternalId, $"%{keyword}%")
                    || EF.Functions.ILike(x.Name, $"%{keyword}%")
                    || EF.Functions.ILike(x.CustomCode, $"%{keyword}%"));
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                var categoryKeyword = query.Category.Trim();
                materialsBase = materialsBase.Where(x =>
                    EF.Functions.ILike(x.Category.ExternalId, $"%{categoryKeyword}%")
                    || EF.Functions.ILike(x.Category.Name, $"%{categoryKeyword}%"));
            }

            // Project material -> DTO
            var materialsQ = materialsBase.Select(x => new
            {
                Dto = new GetMaterialSummary
                {
                    MaterialId = x.MaterialId,
                    ExternalId = x.ExternalId,
                    CustomCode = x.CustomCode,
                    Name = x.Name,
                    ItemType = ItemType.Material,

                    CategoryId = x.CategoryId,
                    Category = x.Category.ExternalId,

                    Weight = x.Weight,
                    Package = x.Package,
                    Unit = x.Unit,

                    Price = supplierFilter.HasValue
                        ? x.MaterialsSuppliers
                            .Where(ms => ms.SupplierId == supplierFilter.Value && ms.IsActive == true)
                            .OrderByDescending(ms => ms.UpdatedDate ?? ms.CreateDate)
                            .Select(ms => ms.CurrentPrice)
                            .FirstOrDefault()
                        : x.MaterialsSuppliers
                            .Where(ms => ms.IsActive == true)
                            .OrderByDescending(ms => ms.UpdatedDate ?? ms.CreateDate)
                            .Select(ms => ms.CurrentPrice)
                            .FirstOrDefault()
                },
                CreatedDateSort = x.CreatedDate
            });


            // -------------------- PRODUCTS base --------------------
            const string khCategoryCode = "KH";
            var sampleSentStatus = SampleRequestStatus.SampleSent.ToString();
            var completedStatus = SampleRequestStatus.Completed.ToString();

            var productsBase = _unitOfWork.ProductRepository
                .Query(track: false)
                .Where(p => p.IsActive == true && p.Name != null && p.ColourCode != null);

            // Filter tương ứng với MaterialId = ProductId
            if (query.MaterialId.HasValue)
                productsBase = productsBase.Where(p => p.ProductId == query.MaterialId.Value);

            if (query.From.HasValue)
                productsBase = productsBase.Where(p => p.CreatedDate >= query.From.Value);

            if (query.To.HasValue)
                productsBase = productsBase.Where(p => p.CreatedDate <= query.To.Value);

            // Product không có supplier => nếu filter supplier thì loại hết product
            if (query.SupplierId.HasValue)
                productsBase = productsBase.Where(_ => false);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                productsBase = productsBase.Where(p =>
                    EF.Functions.ILike(p.ColourCode!, $"%{keyword}%")
                    || EF.Functions.ILike(p.Name!, $"%{keyword}%")
                    || EF.Functions.ILike(p.Code!, $"%{keyword}%"));
            }

            // Category text filter: product category = "KH"
            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                var categoryKeyword = query.Category.Trim();
                if (!khCategoryCode.Contains(categoryKeyword, StringComparison.OrdinalIgnoreCase))
                    productsBase = productsBase.Where(_ => false);
            }

            // Chỉ lấy product nào có SampleRequest + Formula status = samplesent
            productsBase = productsBase.Where(p =>
                p.SampleRequests.Any(sr =>
                    sr.IsActive
                    && (
                        sr.Status == sampleSentStatus ||
                        sr.Status == completedStatus
                    )
                ));

            // Project product -> DTO
            var productsQ = productsBase.Select(p => new
            {
                Dto = new GetMaterialSummary
                {
                    MaterialId = p.ProductId,

                    ExternalId = p.SampleRequests
                        .Where(sr =>
                            sr.IsActive &&
                            (sr.Status == sampleSentStatus || sr.Status == completedStatus))
                        .OrderByDescending(sr => sr.CreatedDate)
                        .Select(sr => sr.ExternalId)
                        .FirstOrDefault(),

                    CustomCode = p.Code,

                    Name = "[" + (p.ColourCode ?? "") + "] " + (p.Name ?? ""),

                    ItemType = ItemType.Product,

                    CategoryId = null,
                    Category = khCategoryCode,

                    Price = null,
                    Weight = null,
                    Package = null,
                    Unit = null
                },
                CreatedDateSort = p.CreatedDate
            });


            // -------------------- CONCAT AFTER FILTER --------------------
            var q = materialsQ.Concat(productsQ);
            var total = await q.CountAsync(ct);

            var items = await q
                .OrderByDescending(x => x.CreatedDateSort)
                .Skip(skip)
                .Take(query.PageSize)
                .Select(x => x.Dto)
                .ToListAsync(ct);

            return new PagedResult<GetMaterialSummary>(items, total, query.PageNumber, query.PageSize);
        }

        public async Task<OperationResult<GetMaterial>> GetMaterialByIdAsync(Guid Id, CancellationToken ct = default)
        {
            try
            {
                if (Id == Guid.Empty)
                    return OperationResult<GetMaterial>.Fail("Id vật tư không hợp lệ.");

                // 1) Lấy thông tin vật tư chính
                var material = await _unitOfWork.MaterialRepository
                    .Query(track: false)
                    .Where(m => m.IsActive == true && m.MaterialId == Id)
                    .Select(m => new GetMaterial
                    {
                        MaterialId = m.MaterialId,
                        ExternalId = m.ExternalId,
                        CustomCode = m.CustomCode,
                        Name = m.Name,
                        CategoryId = m.CategoryId,
                        Weight = m.Weight,
                        Unit = m.Unit,
                        Package = m.Package,
                        Comment = m.Comment,
                        MinQuantity = m.MinQuantity,
                        CompanyId = m.CompanyId,
                        IsActive = m.IsActive,
                        Barcode = m.Barcode
                    })
                    .FirstOrDefaultAsync(ct);

                if (material == null)
                    return OperationResult<GetMaterial>.Fail($"Không tìm thấy vật tư với ID {Id}.");

                // 2) Lấy danh sách nhà cung cấp cho vật tư này
                var supplierRows = await _unitOfWork.MaterialsSupplierRepository
                    .Query(track: false)
                    .Where(ms => ms.MaterialId == Id && ms.IsActive == true && ms.Supplier.IsActive == true)
                    .Select(ms => new
                    {
                        ms.MaterialsSuppliersId,
                        SupplierName = ms.Supplier.SupplierName,
                        SupplierExternalId = ms.Supplier.ExternalId,
                        ms.CurrentPrice,
                        ms.Currency,
                        ms.MinDeliveryDays,
                        ms.IsPreferred,
                        ms.IsActive,
                        ms.UpdatedDate
                    })
                    .ToListAsync(ct);

                //// 3) Lấy lịch sử giá cho material này theo từng supplier
                material.materialSuppliers = supplierRows
                    .Select(s => new GetMaterialSupplier
                    {
                        MaterialSupplierId = s.MaterialsSuppliersId,
                        SupplierName = s.SupplierName,
                        ExternalId = s.SupplierExternalId,
                        CurrentPrice = s.CurrentPrice,
                        Currency = s.Currency,
                        MinDeliveryDays = s.MinDeliveryDays,
                        IsPreferred = s.IsPreferred,
                        isActive = s.IsActive,
                    })
                    .ToList();

                return OperationResult<GetMaterial>.Ok(material);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin vật tư: {ex.Message}", ex);
            }
        }

        public async Task<PagedResult<GetMaterialSupplier>> GetMaterialSupplierAsync(MaterialQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                // Base Query
                IQueryable<MaterialsSupplier> result = _unitOfWork.MaterialsSupplierRepository.Query();

                result = result.OrderByDescending(x => x.CreateDate);

                // Filter
                if (query.MaterialId.HasValue)
                    result = result.Where(x => x.MaterialId == query.MaterialId);

                int total = await result.CountAsync(ct);

                var items = await result
                    .Where(c => c.IsActive == true)
                    .ProjectTo<GetMaterialSupplier>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetMaterialSupplier>(items, total, query.PageNumber, query.PageSize);
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}");
            }
        }

        public async Task<OperationResult<PagedResult<GetPriceHistory>>> GetMaterialPriceHistoryByIdAsync(MaterialQuery query, CancellationToken ct = default)
        {
            try
            {
                if (!query.MaterialSupplierId.HasValue || query.MaterialSupplierId == Guid.Empty)
                    return OperationResult<PagedResult<GetPriceHistory>>.Fail("MaterialId không hợp lệ.");

                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var skip = (query.PageNumber - 1) * query.PageSize;

                // 1) Base query: lịch sử giá theo Material
                var baseQ = _unitOfWork.PriceHistorieRepository
                    .Query(track: false)
                    .Where(ph => ph.MaterialsSuppliersId == query.MaterialSupplierId);

                // 2) Đếm tổng cho PagedResult (dùng cho infinite scroll)
                var totalCount = await baseQ.CountAsync(ct);

                // 3) Lấy page hiện tại
                var items = await baseQ
                    .OrderByDescending(ph => ph.CreateDate ?? DateTime.MinValue)   // mới nhất trước
                    .Skip(skip)
                    .Take(query.PageSize)
                    .Select(ph => new GetPriceHistory
                    {
                        OldPrice = ph.OldPrice ?? 0m,
                        UpdatedDate = ph.CreateDate ?? DateTime.MinValue
                    })
                    .ToListAsync(ct);

                var result = new PagedResult<GetPriceHistory>(items, totalCount, query.PageNumber, query.PageSize);
                return OperationResult<PagedResult<GetPriceHistory>>.Ok(result, "Lấy giá lịch sử thành công");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy lịch sử giá vật tư.", ex);
            }
        }

        // ======================================================================== Post ======================================================================== 

        /// <summary>
        /// Tạo mới vật tư theo nghiệp vụ:
        /// 1) Nếu thiếu ExternalId -> sinh theo NVL_<CategoryExternalId>_<NNN>.
        /// 2) Map trường scalar từ DTO -> entity (nav sẽ xử lý thủ công).
        /// 3) Gắn quan hệ Materials_Suppliers (đảm bảo duy nhất 1 IsPreferred).
        /// 4) (Tuỳ chọn) Seed 1 bản PriceHistory active đầu tiên + đồng bộ CurrentPrice ở bảng nối.
        /// Tất cả chạy trong transaction. Trả OperationResult.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddNewMaterialAsync(PostMaterial material, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {

                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;
                var companyId = _currentUser.CompanyId;

                // 1) Generate ExternalId nếu thiếu (NVL_<catExt>_xyz)
                if (string.IsNullOrWhiteSpace(material.ExternalId))
                {
                    var categoryExternalID = await _unitOfWork.CategoryRepository.Query()
                        .Where(m => m.CategoryId == material.CategoryId)
                        .Select(m => m.ExternalId)
                        .FirstOrDefaultAsync();


                    material.ExternalId = await ExternalIdGenerator.GenerateCode(
                        "NVL",
                        categoryExternalID,
                        prefix => _unitOfWork.MaterialRepository.GetLatestExternalIdStartsWithAsync(prefix)
                    );
                }

                // 2) Map scalar -> Material
                var materialEntity = new Material
                {
                    MaterialId = Guid.CreateVersion7(),             // nếu DB không tự sinh
                    ExternalId = material.ExternalId,
                    CustomCode = material.CustomCode,
                    Name = material.Name,
                    CategoryId = material.CategoryId,
                    Weight = material.Weight,
                    Unit = material.Unit,
                    Package = material.Package,
                    Comment = material.Comment,
                    MinQuantity = material.MinQuantity,
                    Barcode = material.Barcode,
                    CompanyId = companyId,
                    IsActive = true,
                    CreatedDate = now,
                    CreatedBy = userId,
                    UpdatedDate = now,
                    UpdatedBy = userId,

                    // các collection để EF có thể gắn FK tự động khi Add
                    MaterialsSuppliers = new List<MaterialsSupplier>(),
                    PriceHistories = new List<PriceHistory>()
                };


                // 3) Gắn Suppliers (bảng nối)
                if (material.Suppliers?.Any() == true)
                {
                    // đảm bảo chỉ 1 preferred
                    var preferredCount = material.Suppliers.Count(s => s.IsPreferred.GetValueOrDefault());
                    if (preferredCount > 1)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return OperationResult.Fail("Chỉ được chọn 1 nhà cung cấp ưu tiên.");
                    }

                    // distinct theo SupplierId để tránh trùng NCC cho cùng material
                    foreach (var svm in material.Suppliers
                             .Where(s => s?.SupplierId != null)
                             .DistinctBy(s => s!.SupplierId)
                    )
                    {
                        var ms = new MaterialsSupplier
                        {
                            MaterialsSuppliersId = Guid.CreateVersion7(),
                            SupplierId = svm!.SupplierId!.Value,
                            // MaterialId sẽ được EF gắn qua navigation khi add vào materialEntity.MaterialsSuppliers
                            MinDeliveryDays = svm.MinDeliveryDays ?? 1,
                            CurrentPrice = svm.CurrentPrice,
                            Currency = string.IsNullOrWhiteSpace(svm.Currency) ? "VND" : svm.Currency,
                            IsPreferred = svm.IsPreferred ?? false,
                            IsActive = true,
                            CreateDate = now,
                            CreatedBy = userId,
                            UpdatedDate = now,
                            UpdatedBy = userId
                        };

                        materialEntity.MaterialsSuppliers.Add(ms);

                    }
                }

                await _unitOfWork.MaterialRepository.AddAsync(materialEntity, ct);

                var affected = await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Thêm vật tư mới thành công")
                    : OperationResult.Fail("Thất bại");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi thêm vật tư mới: {ex.Message}");
            }
        }


        // ======================================================================== Update ======================================================================== 

        public async Task<OperationResult> UpsertMaterialAsync(PatchMaterial req, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;
                var companyId = _currentUser.CompanyId;

                var mat = await _unitOfWork.MaterialRepository.Query(track: true)
                    .Include(m => m.MaterialsSuppliers)
                    .FirstOrDefaultAsync(m => m.MaterialId == req.MaterialId, ct);

                if (mat == null) return OperationResult.Fail("Không tìm thấy vật tư");

                // Kiểm tra nếu có nhà cung cấp nào mà CurrentPrice là null
                if (req.Suppliers?.Any(x => x.CurrentPrice == null) == true)
                    return OperationResult.Fail("Không được để trống giá (CurrentPrice) trong danh sách NCC.");

                // 2) Patch scalar fields của Material
                PatchHelper.SetIfRef(req.CustomCode, () => mat.CustomCode, v => mat.CustomCode = v);
                PatchHelper.SetIfRef(req.Name, () => mat.Name, v => mat.Name = v);

                if (req.CategoryId != Guid.Empty && req.CategoryId != mat.CategoryId)
                    mat.CategoryId = req.CategoryId;

                PatchHelper.SetIfNullable(req.Weight, () => mat.Weight, v => mat.Weight = v);
                PatchHelper.SetIfRef(req.Unit, () => mat.Unit, v => mat.Unit = v);
                PatchHelper.SetIfRef(req.Package, () => mat.Package, v => mat.Package = v);
                PatchHelper.SetIfRef(req.Comment, () => mat.Comment, v => mat.Comment = v);
                PatchHelper.SetIfNullable(req.MinQuantity, () => mat.MinQuantity, v => mat.MinQuantity = v);
                PatchHelper.SetIfRef(req.Barcode, () => mat.Barcode, v => mat.Barcode = v);

                mat.UpdatedDate = now;
                mat.UpdatedBy = userId;

                // 2) Upsert Suppliers
                if (req.Suppliers?.Any() == true)
                {
                    // Map nhanh id -> entity hiện có
                    var existingByMsId = mat.MaterialsSuppliers.ToDictionary(x => x.MaterialsSuppliersId, x => x);
                    Guid? preferredWinner = null;

                    foreach (var s in req.Suppliers)
                    {
                        // --- CASE A: UPDATE link đã tồn tại theo MaterialSupplierId ---
                        if (s.MaterialSupplierId.HasValue && s.MaterialSupplierId.Value != Guid.Empty)
                        {
                            if (!existingByMsId.TryGetValue(s.MaterialSupplierId.Value, out var link))
                                continue; // không có link tương ứng -> bỏ qua (hoặc return Fail nếu muốn)

                            // MinDeliveryDays, Currency, IsActive, IsPreferred
                            PatchHelper.SetIfNullable(s.MinDeliveryDays, () => link.MinDeliveryDays, v => link.MinDeliveryDays = v);
                            PatchHelper.SetIfRef(s.Currency, () => link.Currency, v => link.Currency = v);
                            PatchHelper.SetIfNullable(s.IsActive, () => link.IsActive, v => link.IsActive = v);
                            PatchHelper.SetIfNullable(s.IsPreferred, () => link.IsPreferred, v => link.IsPreferred = v);

                            // Giá: chỉ ghi lịch sử khi đổi
                            if (s.CurrentPrice.HasValue && s.CurrentPrice.Value != (link.CurrentPrice ?? 0m))
                            {
                                var hist = new PriceHistory
                                {
                                    PriceHistoryId = Guid.CreateVersion7(),
                                    MaterialsSuppliersId = link.MaterialsSuppliersId,
                                    OldPrice = link.CurrentPrice,
                                    Currency = link.Currency,
                                    CreateDate = now,
                                    CreatedBy = userId
                                };
                                await _unitOfWork.PriceHistorieRepository.AddAsync(hist, ct);

                                link.CurrentPrice = s.CurrentPrice.Value;
                            }

                            link.UpdatedBy = userId;
                            link.UpdatedDate = now;

                            if (s.IsPreferred == true && (s.IsActive ?? link.IsActive ?? true))
                                preferredWinner ??= link.MaterialsSuppliersId;

                            continue;
                        }

                        // --- CASE B: THÊM MỚI link theo Supplier (khi chưa có MaterialSupplierId) ---
                        if (s.SupplierId.HasValue && s.SupplierId.Value != Guid.Empty)
                        {
                            var supplierId = s.SupplierId.Value;

                            // Chặn trùng (MaterialId + SupplierId) – nếu có rồi thì chuyển sang update thay vì tạo mới
                            var dup = mat.MaterialsSuppliers.FirstOrDefault(x => x.SupplierId == supplierId);
                            if (dup != null)
                            {
                                // Treat as update
                                PatchHelper.SetIfNullable(s.MinDeliveryDays, () => dup.MinDeliveryDays, v => dup.MinDeliveryDays = v);
                                PatchHelper.SetIfRef(s.Currency, () => dup.Currency, v => dup.Currency = v);
                                PatchHelper.SetIfNullable(s.IsActive, () => dup.IsActive, v => dup.IsActive = v);
                                PatchHelper.SetIfNullable(s.IsPreferred, () => dup.IsPreferred, v => dup.IsPreferred = v);

                                if (s.CurrentPrice.HasValue && s.CurrentPrice.Value != (dup.CurrentPrice ?? 0m))
                                {
                                    var hist = new PriceHistory
                                    {
                                        PriceHistoryId = Guid.CreateVersion7(),
                                        MaterialsSuppliersId = dup.MaterialsSuppliersId,
                                        OldPrice = dup.CurrentPrice,
                                        Currency = dup.Currency,
                                        CreateDate = now,
                                        CreatedBy = userId
                                    };
                                    await _unitOfWork.PriceHistorieRepository.AddAsync(hist, ct);

                                    dup.CurrentPrice = s.CurrentPrice.Value;
                                }

                                dup.UpdatedBy = userId;
                                dup.UpdatedDate = now;

                                if (s.IsPreferred == true && (s.IsActive ?? dup.IsActive ?? true))
                                    preferredWinner ??= dup.MaterialsSuppliersId;

                                continue;
                            }

                            // Tạo mới link
                            var ms = new MaterialsSupplier
                            {
                                MaterialsSuppliersId = Guid.CreateVersion7(),
                                MaterialId = mat.MaterialId,
                                SupplierId = supplierId,
                                MinDeliveryDays = s.MinDeliveryDays ?? 1,
                                CurrentPrice = s.CurrentPrice, // KHÔNG ghi PriceHistory khi tạo mới
                                Currency = string.IsNullOrWhiteSpace(s.Currency) ? "VND" : s.Currency,
                                IsPreferred = s.IsPreferred ?? false,
                                IsActive = s.IsActive ?? true,
                                CreateDate = now,
                                CreatedBy = userId,
                                UpdatedDate = now,
                                UpdatedBy = userId
                            };

                            await _unitOfWork.MaterialsSupplierRepository.AddAsync(ms, ct);
                            mat.MaterialsSuppliers.Add(ms); // để EF track collection

                            if (ms.IsPreferred == true && (ms.IsActive ?? true))
                                preferredWinner ??= ms.MaterialsSuppliersId;

                            continue;
                        }

                        // Không có MaterialSupplierId & cũng không có Supplier -> không biết định danh -> bỏ qua
                    }
                    if (preferredWinner.HasValue)
                    {
                        foreach (var ms in mat.MaterialsSuppliers.Where(x => x.IsActive == true))
                            ms.IsPreferred = (ms.MaterialsSuppliersId == preferredWinner.Value);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return OperationResult.Ok("Cập nhật vật tư thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> DeleteMaterialAsync(Guid Id, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.MaterialRepository.DeleteMaterialAsync(Id, ct);  
                var affected = await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Xoá vật tư thành công")
                    : OperationResult.Fail("Thất bại");
            }

            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi xoá vật tư: {ex.Message}");   
            }
        }

        // ======================================================================== Helper ======================================================================== 
        public async Task ChangePriceHelper(Guid MaterialsSupplierId, Decimal newPrice, CancellationToken ct = default)
        {
            if (MaterialsSupplierId == Guid.Empty)
                throw new ArgumentException("MaterialsSuppliersId không hợp lệ", nameof(MaterialsSupplierId));

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId; // hoặc UserId tuỳ anh đang lưu gì

            // 1) Lấy MaterialsSupplier hiện tại
            var ms = await _unitOfWork.MaterialsSupplierRepository
                .Query(track: true)
                .FirstOrDefaultAsync(x => x.MaterialsSuppliersId == MaterialsSupplierId, ct);

            if (ms == null)
                throw new InvalidOperationException($"Không tìm thấy MaterialsSupplier với Id = {MaterialsSupplierId}");

            var currentPrice = ms.CurrentPrice ?? 0m;
            var currentCurr = ms.Currency ?? "VND";
            var newCurr = currentCurr; // nếu anh chưa cho đổi currency, giữ nguyên

            var priceChanged = currentPrice != newPrice;

            if (!priceChanged)
                return; // không đổi giá thì thôi, khỏi ghi history

            // 2) Ghi lịch sử giá (log giá cũ)
            var hist = new PriceHistory
            {
                MaterialsSuppliersId = ms.MaterialsSuppliersId,
                OldPrice = ms.CurrentPrice,  // giá cũ
                Currency = ms.Currency,      // currency cũ
                CreateDate = now,
                CreatedBy = userId
            };

            await _unitOfWork.PriceHistorieRepository.AddAsync(hist, ct);

            // 3) Cập nhật giá mới
            ms.CurrentPrice = newPrice;
            ms.Currency = newCurr;
            ms.UpdatedBy = userId;
            ms.UpdatedDate = now;
            ms.IsActive = true;

            // caller save chung với các thay đổi khác 
            return;
        }


    }
}
