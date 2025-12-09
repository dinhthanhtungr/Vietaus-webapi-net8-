using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Services
{
    public class ProductStandardService : IProductStandardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        public ProductStandardService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        // ======================================================================== Get ========================================================================

        public async Task<OperationResult<GetProductStandard>> GetByIdAsync(ProductStandardQuery id, CancellationToken cancellationToken)
        {
            try
            {
                // Base query: chỉ đọc, nhẹ
                var q = _unitOfWork.ProductStandardRepository.Query();


                // Filter theo Id
                if (id.ProductStandard.HasValue)
                {
                    q = q.Where(ps => ps.Id == id.ProductStandard.Value);
                }

                if (id.ProductId.HasValue)
                {
                    q = q.Where(ps => ps.ProductId == id.ProductId.Value);
                }

                // Project thẳng sang DTO (server-side)
                var dto = await q.Select(ps => new GetProductStandard
                {
                    Id = ps.Id,
                    ProductId = ps.ProductId,
                    ProductExternalId = ps.ProductExternalId,
                    Status = ps.Status,
                    DeltaE = ps.DeltaE,
                    PelletSize = ps.PelletSize,
                    Moisture = ps.Moisture,
                    Density = ps.Density,
                    MeltIndex = ps.MeltIndex,
                    TensileStrength = ps.TensileStrength,
                    ElongationAtBreak = ps.ElongationAtBreak,
                    FlexuralStrength = ps.FlexuralStrength,
                    FlexuralModulus = ps.FlexuralModulus,
                    IzodImpactStrength = ps.IzodImpactStrength,
                    Hardness = ps.Hardness,
                    DwellTime = ps.DwellTime,
                    BlackDots = ps.BlackDots,
                    MigrationTest = ps.MigrationTest,
                    Weight = ps.Weight,
                    Shape = ps.Shape,
                    CompanyId = ps.CompanyId,
                    CreatedBy = ps.CreatedBy,
                    CreatedDate = ps.CreatedDate
                })
                .FirstOrDefaultAsync(cancellationToken);

                if (dto == null)
                    return OperationResult<GetProductStandard>.Fail("Không tìm thấy ProductStandard.");

                return OperationResult<GetProductStandard>.Ok(dto);
            }
            catch (Exception ex)
            {
                return OperationResult<GetProductStandard>.Fail($"Lỗi khi lấy ProductStandard: {ex.Message}");
            }
        }

        public async Task<OperationResult<PagedResult<ProductStandardSummaryDTO>>> GetPagedListAsync(ProductStandardQuery query, CancellationToken cancellationToken)
        {
            try
            {
                query ??= new ProductStandardQuery();
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var skip = (query.PageNumber - 1) * query.PageSize;

                var baseQ = _unitOfWork.ProductStandardRepository.Query();

                // ------- Keyword filter -------
                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var kw = query.Keyword.Trim();
                    // PostgreSQL (Npgsql): ILIKE để tìm chứa không phân biệt hoa/thường
                    baseQ = baseQ.Where(ps =>
                        EF.Functions.ILike(ps.ProductExternalId ?? string.Empty, $"%{kw}%") 
                    );
                }

                // ------- Count sau khi áp filter -------
                var total = await baseQ.CountAsync(cancellationToken);

                // ------- Page + Project sang DTO -------
                var items = await baseQ
                    .OrderByDescending(ps => ps.CreatedDate)
                    .Skip(skip)
                    .Take(query.PageSize)
                    .Select(ps => new ProductStandardSummaryDTO
                    {
                        ColourCode = ps.ProductExternalId ?? string.Empty,
                        Status = ps.Status ?? string.Empty,
                        Density = ps.Density ?? string.Empty,
                        MeltIndex = ps.MeltIndex ?? string.Empty,
                        weight = ps.Weight,                 // DTO là int?
                        CreatedDate = ps.CreatedDate,
                        Id = ps.Id
                    })
                    .ToListAsync(cancellationToken);

                var paged = new PagedResult<ProductStandardSummaryDTO>(items, total, query.PageNumber, query.PageSize);
                return OperationResult<PagedResult<ProductStandardSummaryDTO>>.Ok(paged);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<ProductStandardSummaryDTO>>.Fail(
                    $"Lỗi khi lấy danh sách ProductStandard: {ex.Message}"
                );
            }
        }

        // ======================================================================== Post ========================================================================
        public async Task<OperationResult> AddAsync(PostProductStandard productStandard, CancellationToken cancellationToken)
            {
                try
                {
                    if (productStandard == null)
                        return OperationResult.Fail("Dữ liệu trống.");

                    // Tenant / Audit
                    var companyId = _currentUser?.CompanyId ?? Guid.Empty;
                    var createdBy = _currentUser?.EmployeeId ?? Guid.Empty;

                    if (companyId == Guid.Empty)
                        return OperationResult.Fail("Thiếu CompanyId (current user).");

                    if (productStandard.ProductId == Guid.Empty)
                        return OperationResult.Fail("Thiếu ProductId.");

                    // (Khuyến nghị) kiểm tra Product có tồn tại trong tenant
                    var productExists = await _unitOfWork.ProductRepository.Query()
                        .AnyAsync(p => p.ProductId == productStandard.ProductId
                                       && (p.CompanyId == companyId || p.CompanyId == null), cancellationToken);


                if (!productExists)
                        return OperationResult.Fail("Sản phẩm không tồn tại hoặc không thuộc công ty hiện tại.");

                    // (Tùy chọn) ràng buộc: 1 tiêu chuẩn/1 sản phẩm (nếu nghiệp vụ yêu cầu)
                    // var exists = await _unitOfWork.ProductStandardRepository.Query()
                    //     .AnyAsync(x => x.ProductId == productStandard.ProductId && x.CompanyId == companyId, cancellationToken);
                    // if (exists) return OperationResult.Failure("Sản phẩm đã có ProductStandard.");

                    var entity = new ProductStandard
                    {
                        Id = Guid.CreateVersion7(),   // bỏ qua Id từ DTO để tránh client ép GUID
                        ProductId = productStandard.ProductId,
                        ProductExternalId = productStandard.ProductExternalId,
                        Status = productStandard.Status,
                        DeltaE = productStandard.DeltaE,
                        PelletSize = productStandard.PelletSize,
                        Moisture = productStandard.Moisture,
                        Density = productStandard.Density,
                        MeltIndex = productStandard.MeltIndex,
                        TensileStrength = productStandard.TensileStrength,
                        ElongationAtBreak = productStandard.ElongationAtBreak,
                        FlexuralStrength = productStandard.FlexuralStrength,
                        FlexuralModulus = productStandard.FlexuralModulus,
                        IzodImpactStrength = productStandard.IzodImpactStrength,
                        Hardness = productStandard.Hardness,
                        DwellTime = productStandard.DwellTime,
                        BlackDots = productStandard.BlackDots,
                        MigrationTest = productStandard.MigrationTest,
                        Weight = productStandard.Weight,
                        Shape = productStandard.Shape,

                        CompanyId = companyId,
                        CreatedBy = createdBy,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _unitOfWork.ProductStandardRepository.AddAsync(entity, cancellationToken);
                    await _unitOfWork.SaveChangesAsync();

                    return OperationResult.Ok("Tạo ProductStandard thành công.");
                }
                catch (Exception ex)
                {
                    return OperationResult.Fail($"Lỗi khi tạo ProductStandard: {ex.Message}");
                }
            }

        // ======================================================================== Patch ========================================================================
        public async Task<OperationResult> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                if (id == Guid.Empty)
                    return OperationResult.Fail("Id không hợp lệ.");

                var q = _unitOfWork.ProductStandardRepository.Query()
                            .Where(x => x.Id == id);


                var entity = await q.FirstOrDefaultAsync(cancellationToken);

                if (entity == null)
                    return OperationResult.Fail("Không tìm thấy ProductStandard.");

                PatchHelper.SetIf(entity.IsActive, () => entity.IsActive, v => entity.IsActive = v);

                await _unitOfWork.SaveChangesAsync();
                return OperationResult.Ok("Xóa ProductStandard thành công.");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Lỗi khi soft delete ProductStandard: {ex.Message}");
            }
        }

        public async Task<OperationResult> UpdateAsync(PatchProductStandard dto, CancellationToken ct)
        {
            try
            {
                if (dto == null || dto.Id == Guid.Empty)
                    return OperationResult.Fail("Dữ liệu cập nhật không hợp lệ.");

                var q = _unitOfWork.ProductStandardRepository.Query().Where(x => x.Id == dto.Id);

                var entity = await q.FirstOrDefaultAsync(ct);
                if (entity == null)
                    return OperationResult.Fail("Không tìm thấy ProductStandard.");

                // helper nhỏ để trim nếu không null
                static string? T(string? s) => s is null ? null : s.Trim();

                var changed = false;

                // ----- value types (nullable input) -----
                changed |= PatchHelper.SetIf(dto.ProductId, () => entity.ProductId, v => entity.ProductId = v);
                changed |= PatchHelper.SetIf(dto.Weight, () => entity.Weight, v => entity.Weight = v);

                // ----- ref types (string…) -----
                changed |= PatchHelper.SetIfRef(T(dto.ProductExternalId), () => entity.ProductExternalId, v => entity.ProductExternalId = v);
                changed |= PatchHelper.SetIfRef(T(dto.Status), () => entity.Status, v => entity.Status = v);
                changed |= PatchHelper.SetIfRef(T(dto.DeltaE), () => entity.DeltaE, v => entity.DeltaE = v);
                changed |= PatchHelper.SetIfRef(T(dto.PelletSize), () => entity.PelletSize, v => entity.PelletSize = v);
                changed |= PatchHelper.SetIfRef(T(dto.Moisture), () => entity.Moisture, v => entity.Moisture = v);
                changed |= PatchHelper.SetIfRef(T(dto.Density), () => entity.Density, v => entity.Density = v);
                changed |= PatchHelper.SetIfRef(T(dto.MeltIndex), () => entity.MeltIndex, v => entity.MeltIndex = v);
                changed |= PatchHelper.SetIfRef(T(dto.TensileStrength), () => entity.TensileStrength, v => entity.TensileStrength = v);
                changed |= PatchHelper.SetIfRef(T(dto.ElongationAtBreak), () => entity.ElongationAtBreak, v => entity.ElongationAtBreak = v);
                changed |= PatchHelper.SetIfRef(T(dto.FlexuralStrength), () => entity.FlexuralStrength, v => entity.FlexuralStrength = v);
                changed |= PatchHelper.SetIfRef(T(dto.FlexuralModulus), () => entity.FlexuralModulus, v => entity.FlexuralModulus = v);
                changed |= PatchHelper.SetIfRef(T(dto.IzodImpactStrength), () => entity.IzodImpactStrength, v => entity.IzodImpactStrength = v);
                changed |= PatchHelper.SetIfRef(T(dto.Hardness), () => entity.Hardness, v => entity.Hardness = v);
                changed |= PatchHelper.SetIfRef(T(dto.DwellTime), () => entity.DwellTime, v => entity.DwellTime = v);
                changed |= PatchHelper.SetIfRef(T(dto.BlackDots), () => entity.BlackDots, v => entity.BlackDots = v);
                changed |= PatchHelper.SetIfRef(T(dto.MigrationTest), () => entity.MigrationTest, v => entity.MigrationTest = v);
                changed |= PatchHelper.SetIfRef(T(dto.Shape), () => entity.Shape, v => entity.Shape = v);

                if (!changed)
                    return OperationResult.Ok("Không có thay đổi.");

                // (tuỳ chọn) audit cập nhật
                // entity.UpdatedBy   = _currentUser?.EmployeeId ?? _currentUser?.UserId ?? Guid.Empty;
                // entity.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync();
                return OperationResult.Ok("Cập nhật thành công.");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Lỗi khi cập nhật ProductStandard: {ex.Message}");
            }
        }
    }
}
