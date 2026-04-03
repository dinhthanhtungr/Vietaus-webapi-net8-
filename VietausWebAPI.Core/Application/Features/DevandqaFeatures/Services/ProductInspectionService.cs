using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Labs.Helpers;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.Category;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Services
{
    public class ProductInspectionService : IProductInspectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExternalIdService _externalId;
        private readonly ICurrentUser _currentUser;

        public ProductInspectionService(IUnitOfWork unitOfWork
                                      , IMapper mapper
                                      , IExternalIdService externalId
                                      , ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _externalId = externalId;
            _currentUser = currentUser;
        }


        // ======================================================================== Get ========================================================================
        public async Task<OperationResult<PagedResult<GetProductCOA>>> GetProductCOAService(
            ProductInspectionQuery query,
            CancellationToken ct)
        {
            var page = query.PageNumber <= 0 ? 1 : query.PageNumber;
            var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;
            var skip = (page - 1) * pageSize;

            var productTests = _unitOfWork.ProductTestRepository.Query().AsNoTracking();
            var formulas = _unitOfWork.ManufacturingFormulaRepository.Query().AsNoTracking();
            var versions = _unitOfWork.ProductionSelectVersionRepository.Query().AsNoTracking();
            var mfgOrders = _unitOfWork.MfgProductionOrderRepository.Query().AsNoTracking();
            var products = _unitOfWork.ProductRepository.Query().AsNoTracking();

            var formulaBase =
                from mf in formulas
                where mf.IsActive
                join psv in versions on mf.ManufacturingFormulaId equals psv.ManufacturingFormulaId
                join mpo in mfgOrders on psv.MfgProductionOrderId equals mpo.MfgProductionOrderId
                join p in products on mpo.ProductId equals p.ProductId
                select new
                {
                    mf.ExternalId,
                    MfgProductionOrderId = mpo.MfgProductionOrderId,
                    mpo.TotalQuantityRequest,
                    mpo.ProductId,
                    mpo.ProductExternalIdSnapshot,
                    ProductName = p.Name,
                    p.ColourCode,
                    ProductPackage = mpo.BagType
                };

            if (!string.IsNullOrWhiteSpace(query.keyword))
            {
                var kw = query.keyword.Trim();
                var like = $"%{kw}%";

                formulaBase = formulaBase.Where(x =>
                    (x.ExternalId != null && EF.Functions.ILike(x.ExternalId, like)) ||
                    (x.ProductExternalIdSnapshot != null && EF.Functions.ILike(x.ProductExternalIdSnapshot, like)) ||
                    (x.ProductName != null && EF.Functions.ILike(x.ProductName, like)) ||
                    (x.ColourCode != null && EF.Functions.ILike(x.ColourCode, like))
                );
            }

            // Nếu bị duplicate do join thì nên Distinct theo MPO
            var total = await formulaBase.CountAsync(ct);

            var pageItems = await formulaBase
                .OrderByDescending(x => x.ExternalId)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);

            var extIds = pageItems
                .Where(x => !string.IsNullOrWhiteSpace(x.ExternalId))
                .Select(x => x.ExternalId!)
                .Distinct()
                .ToList();

            var normalizedExtIds = extIds
                .Select(x => x.StartsWith("VA") ? x.Substring(2) : x)
                .Distinct()
                .ToList();

            var allLookupIds = extIds
                .Concat(normalizedExtIds)
                .Distinct()
                .ToList();

            var ptList = await productTests
                .Where(t => t.ExternalId != null && allLookupIds.Contains(t.ExternalId))
                .ToListAsync(ct);

            var ptDict = ptList
                .GroupBy(t => t.ExternalId)
                .ToDictionary(g => g.Key!, g => g.First());

            var items = pageItems.Select(x =>
            {
                var normalizedExternalId = x.ExternalId != null && x.ExternalId.StartsWith("VA")
                    ? x.ExternalId.Substring(2)
                    : x.ExternalId;

                ptDict.TryGetValue(x.ExternalId ?? "", out var pt);
                pt ??= normalizedExternalId != null && ptDict.TryGetValue(normalizedExternalId, out var pt2) ? pt2 : null;

                return new GetProductCOA
                {
                    id = x.MfgProductionOrderId,
                    externalId = pt?.ExternalId ?? x.ExternalId,

                    manufacturingDate = pt?.ManufacturingDate,
                    expiryDate = pt?.ExpiryDate,

                    productPackage = x.ProductPackage,
                    ProductWeight = (float?)x.TotalQuantityRequest,

                    ProductId = pt?.ProductId ?? x.ProductId,
                    productExternalId = pt?.ProductExternalId ?? x.ProductExternalIdSnapshot,
                    productName = pt?.ProductName ?? x.ProductName,

                    TotalQuantityRequest = x.TotalQuantityRequest,
                    ColourCode = x.ColourCode
                };
            })
            .OrderByDescending(x => x.manufacturingDate)
            .ThenByDescending(x => x.expiryDate)
            .ToList();

            var result = new PagedResult<GetProductCOA>(items, total, page, pageSize);
            return OperationResult<PagedResult<GetProductCOA>>.Ok(result);
        }
        public async Task<OperationResult<ProductInspectionInformation>> GetProductInspectionByIdAsync(Guid id, CancellationToken ct)
        {
            if (id == Guid.Empty)
                return OperationResult<ProductInspectionInformation>.Fail("Id không hợp lệ.");

            var q = _unitOfWork.ProductInspectionRepository.Query()
                .Where(x => x.Id == id);

            var dto = await q.Select(x => new ProductInspectionInformation
            {
                ExternalId = x.ExternalId,
                BatchId = x.BatchId,
                ProductStandardId = x.ProductStandardId,
                ProductName = x.ProductName,
                ProductCode = x.ProductCode,
                Weight = x.Weight,
                ManufacturingDate = x.ManufacturingDate,
                ExpiryDate = x.ExpiryDate,

                // I. KIỂM TRA NGOẠI QUAN
                Shape = x.Shape,
                IsShapePass = x.IsShapePass,
                ParticleSize = x.ParticleSize,
                IsParticleSizePass = x.IsParticleSizePass,
                PackingSpec = x.PackingSpec,
                IsPackingSpecPass = x.IsPackingSpecPass,
                VisualCheck = x.VisualCheck,

                // II. KIỂM TRA MÀU SẮC
                ColorDeltaE = x.ColorDeltaE,
                IsColorDeltaEPass = x.IsColorDeltaEpass,   // entity: IsColorDeltaEpass

                // III. CHỈ TIÊU KỸ THUẬT
                Moisture = x.Moisture,
                IsMoisturePass = x.IsMoisturePass,

                MFR = x.Mfr,                               // entity: Mfr
                IsMFRPass = x.IsMfrpass,                   // entity: IsMfrpass

                FlexuralStrength = x.FlexuralStrength,
                IsFlexuralStrengthPass = x.IsFlexuralStrengthPass,

                Elongation = x.Elongation,
                IsElongationPass = x.IsElongationPass,

                Hardness = x.Hardness,
                IsHardnessPass = x.IsHardnessPass,

                Density = x.Density,
                IsDensityPass = x.IsDensityPass,

                TensileStrength = x.TensileStrength,
                IsTensileStrengthPass = x.IsTensileStrengthPass,

                FlexuralModulus = x.FlexuralModulus,
                IsFlexuralModulusPass = x.IsFlexuralModulusPass,

                ImpactResistance = x.ImpactResistance,
                IsImpactResistancePass = x.IsImpactResistancePass,

                Antistatic = x.Antistatic,
                IsAntistaticPass = x.IsAntistaticPass,

                StorageCondition = x.StorageCondition,
                IsStorageConditionPass = x.IsStorageConditionPass,

                MeshType = x.MeshType,
                IsMeshAttached = x.IsMeshAttached,

                DwellTime = x.DwellTime,

                // IV. NGOẠI QUAN ĐẶC BIỆT
                BlackDots = x.BlackDots,
                MigrationTest = x.MigrationTest,

                Defect_Impurity = x.DefectImpurity,
                Defect_BlackDot = x.DefectBlackDot,
                Defect_ShortFiber = x.DefectShortFiber,
                Defect_Moist = x.DefectMoist,
                Defect_Dusty = x.DefectDusty,
                Defect_WrongColor = x.DefectWrongColor,

                // V. KẾT LUẬN
                Types = x.Types,
                DeliveryAccepted = x.DeliveryAccepted,
                Notes = x.Notes,
                CreatedBy = x.CreatedBy,

                // DTO có machineId nhưng entity không có field -> null
                machineId = null
            }).FirstOrDefaultAsync();

            if (dto is null)
                return OperationResult<ProductInspectionInformation>.Fail("Không tìm thấy phiếu kiểm tra (ProductInspection).");

            return OperationResult<ProductInspectionInformation>.Ok(dto);
        }

        public async Task<OperationResult<PagedResult<ProductInspectionSummary>>> GetProductInspectionPagedAsync(ProductInspectionQuery? query, CancellationToken ct)
        {
            query ??= new ProductInspectionQuery();

            var pageIndex = query.PageNumber <= 0 ? 1 : query.PageNumber; // 1-based
            var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;
            var skip = (pageIndex - 1) * pageSize;

            var q = _unitOfWork.ProductInspectionRepository.Query();

            // ===== FILTERS =====
            if (!string.IsNullOrWhiteSpace(query.keyword))
            {
                var kw = query.keyword.Trim();
                var like = $"%{kw}%";

                q = q.Where(x =>
                    (x.ExternalId != null && EF.Functions.ILike(x.ExternalId, like)) ||
                    (x.BatchId != null && EF.Functions.ILike(x.BatchId, like)) ||
                    (x.ProductCode != null && EF.Functions.ILike(x.ProductCode, like))
                );
            }

            // ===== SORT =====
            q = q.OrderByDescending(x => x.CreateDate).ThenByDescending(x => x.ManufacturingDate);

            // ===== PAGING =====
            var total = await q.CountAsync(ct);

            var items = await q
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ProductInspectionSummary
                {
                    Id = x.Id,
                    ColourCode = x.ProductCode,                // entity không có ColourCode -> dùng ProductCode
                    ProductName = x.ProductName,
                    BatchNumber = x.BatchId,
                    Status = null,                             // entity chưa có field Status
                    Result = x.DeliveryAccepted,
                    Types = x.Types,
                    CreateDate = x.CreateDate ?? DateTime.MinValue,
                    CreatedBy = x.CreatedBy
                })
                .ToListAsync(ct);

            var result = new PagedResult<ProductInspectionSummary>(
                items: items,
                totalCount: total,
                page: pageIndex,
                pageSize: pageSize
            );

            return OperationResult<PagedResult<ProductInspectionSummary>>.Ok(result);
        }

        // ======================================================================== Post ========================================================================

        public async Task<OperationResult> PostProductInspectionServiceAsync(PostProductInspectionRequest productInspection, CancellationToken ct)
        {
            if (productInspection is null)
                return OperationResult.Fail("Dữ liệu gửi lên bị null.");

            var externalId = await _externalId.NextAsync(DocumentPrefix.KSP.ToString(), ct: ct);

            // 2) Map request -> entity
            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            var entity = new ProductInspection
            {
                Id = Guid.CreateVersion7(),

                ExternalId = externalId,
                BatchId = productInspection.BatchId?.Trim(),
                ProductStandardId = productInspection.ProductStandardId,
                ProductName = productInspection.ProductName?.Trim(),
                ProductCode = productInspection.ProductCode?.Trim(),
                Weight = productInspection.Weight,
                ManufacturingDate = productInspection.ManufacturingDate,
                ExpiryDate = productInspection.ExpiryDate,

                // I. NGOẠI QUAN
                Shape = productInspection.Shape,
                IsShapePass = productInspection.IsShapePass,
                ParticleSize = productInspection.ParticleSize,
                IsParticleSizePass = productInspection.IsParticleSizePass,
                PackingSpec = productInspection.PackingSpec,
                IsPackingSpecPass = productInspection.IsPackingSpecPass,
                VisualCheck = productInspection.VisualCheck,

                // II. MÀU SẮC
                ColorDeltaE = productInspection.ColorDeltaE,
                IsColorDeltaEpass = productInspection.IsColorDeltaEPass, // entity: IsColorDeltaEpass

                // III. KỸ THUẬT
                Moisture = productInspection.Moisture,
                IsMoisturePass = productInspection.IsMoisturePass,

                Mfr = productInspection.MFR,                 // entity: Mfr
                IsMfrpass = productInspection.IsMFRPass,     // entity: IsMfrpass

                FlexuralStrength = productInspection.FlexuralStrength,
                IsFlexuralStrengthPass = productInspection.IsFlexuralStrengthPass,

                Elongation = productInspection.Elongation,
                IsElongationPass = productInspection.IsElongationPass,

                Hardness = productInspection.Hardness,
                IsHardnessPass = productInspection.IsHardnessPass,

                Density = productInspection.Density,
                IsDensityPass = productInspection.IsDensityPass,

                TensileStrength = productInspection.TensileStrength,
                IsTensileStrengthPass = productInspection.IsTensileStrengthPass,

                FlexuralModulus = productInspection.FlexuralModulus,
                IsFlexuralModulusPass = productInspection.IsFlexuralModulusPass,

                ImpactResistance = productInspection.ImpactResistance,
                IsImpactResistancePass = productInspection.IsImpactResistancePass,

                Antistatic = productInspection.Antistatic,
                IsAntistaticPass = productInspection.IsAntistaticPass,

                StorageCondition = productInspection.StorageCondition,
                IsStorageConditionPass = productInspection.IsStorageConditionPass,

                MeshType = productInspection.MeshType,
                IsMeshAttached = productInspection.IsMeshAttached,

                DwellTime = productInspection.DwellTime,

                // IV. NGOẠI QUAN ĐẶC BIỆT
                BlackDots = productInspection.BlackDots,
                MigrationTest = productInspection.MigrationTest,

                DefectImpurity = productInspection.Defect_Impurity,
                DefectBlackDot = productInspection.Defect_BlackDot,
                DefectShortFiber = productInspection.Defect_ShortFiber,
                DefectMoist = productInspection.Defect_Moist,
                DefectDusty = productInspection.Defect_Dusty,
                DefectWrongColor = productInspection.Defect_WrongColor,

                // V. KẾT LUẬN
                Types = productInspection.Types,
                DeliveryAccepted = productInspection.DeliveryAccepted,

                Notes = productInspection.Notes,
                CreatedBy = _currentUser.personName,
                CreateDate = now
            };

            // 3) Save
            try
            {
                await _unitOfWork.ProductInspectionRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return OperationResult.Ok();
            }
            catch (DbUpdateException ex)
            {
                // Nếu bạn có logger thì log ex ở đây
                return OperationResult.Fail("Lưu ProductInspection thất bại (DbUpdateException).");
            }
            catch (Exception)
            {
                return OperationResult.Fail("Lưu ProductInspection thất bại (Exception).");
            }
        }

        // ======================================================================== Patch ========================================================================

        public Task<OperationResult> DeleteCOAService(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }


        // ======================================================================== PDF ========================================================================
        public async Task<OperationResult<byte[]>> GeneralPdfService(Guid id, CancellationToken ct)
        {
            if (id == Guid.Empty)
                return OperationResult<byte[]>.Fail("Id không hợp lệ.");

            try
            {
                // 1) Lấy inspection
                var inspection = await _unitOfWork.ProductInspectionRepository
                    .Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(ct);
                var productTests = _unitOfWork.ProductTestRepository.Query().AsNoTracking();


                if (inspection is null)
                    return OperationResult<byte[]>.Fail("Không tìm thấy ProductInspection để in COA.");

                // 2) Map -> PDFResultValue (đủ field)
                var result = new PDFResultValue
                {
                    ExternalId = inspection.ExternalId,
                    BatchId = inspection.BatchId,

                    ProductName = inspection.ProductName,
                    ProductCode = inspection.ProductCode,

                    Weight = inspection.Weight,
                    ManufacturingDate = inspection.ManufacturingDate,
                    ExpiryDate = inspection.ExpiryDate,

                    Shape = inspection.Shape,
                    ParticleSize = inspection.ParticleSize,
                    PackingSpec = inspection.PackingSpec,
                    ColorDeltaE = inspection.ColorDeltaE,
                    Moisture = inspection.Moisture,
                    MFR = inspection.Mfr, // entity là Mfr (string?), PDF là MFR

                    FlexuralStrength = inspection.FlexuralStrength,
                    Elongation = inspection.Elongation,
                    Hardness = inspection.Hardness,
                    Density = inspection.Density,
                    TensileStrength = inspection.TensileStrength,
                    FlexuralModulus = inspection.FlexuralModulus,
                    ImpactResistance = inspection.ImpactResistance,
                    Antistatic = inspection.Antistatic,
                    StorageCondition = inspection.StorageCondition,

                    DwellTime = inspection.DwellTime,
                    BlackDots = inspection.BlackDots,
                    MigrationTest = inspection.MigrationTest,

                    // cái này bạn đang đặt bagType = MeshType, mình vẫn giữ theo DB hiện có
                    bagType = productTests
                        .Where(x => x.ProductExternalId == inspection.ProductCode)
                        .Select(x => x.ProductPackage)
                        .FirstOrDefault(),

                    COAType = inspection.Types, // entity là Types (string?), PDF là COAType

                    CreateDate = inspection.CreateDate ?? DateTime.Now
                };

                // 3) Lấy ProductStandard (specs) - ưu tiên theo ProductStandardId nếu có
                //    Nếu không có, fallback theo ProductCode (hoặc ProductExternalId)
                var standardQ = _unitOfWork.ProductStandardRepository
                    .Query()
                    .Where(s => s.IsActive);

                if (inspection.ProductStandardId.HasValue)
                {
                    standardQ = standardQ.Where(s => s.Id == inspection.ProductStandardId.Value);
                }
                else if (!string.IsNullOrWhiteSpace(inspection.ProductCode))
                {
                    var code = inspection.ProductCode.Trim();
                    standardQ = standardQ.Where(s =>
                        (s.ProductExternalId != null && s.ProductExternalId == code)
                    );
                }

                var specs = await standardQ
                    .OrderByDescending(s => s.CreatedDate) // bản mới nhất
                    .Select(s => new PDFSpecificationsValue
                    {
                        Shape = s.Shape,
                        PelletSize = s.PelletSize,
                        DeltaE = s.DeltaE,
                        Moisture = s.Moisture,
                        Density = s.Density,
                        MeltIndex = s.MeltIndex, // MeltIndex -> MFR tiêu chuẩn
                        TensileStrength = s.TensileStrength,
                        ElongationAtBreak = s.ElongationAtBreak,
                        FlexuralStrength = s.FlexuralStrength,
                        FlexuralModulus = s.FlexuralModulus,
                        IzodImpactStrength = s.IzodImpactStrength,
                        Hardness = s.Hardness,
                        DwellTime = s.DwellTime,
                        BlackDots = s.BlackDots,
                        MigrationTest = s.MigrationTest
                    })
                    .FirstOrDefaultAsync(ct);

                // 4) Render PDF
                var pdfBytes = new COAPdf(result, specs).GeneratePdf();

                if (pdfBytes is null || pdfBytes.Length == 0)
                    return OperationResult<byte[]>.Fail("PDF bytes rỗng.");

                return OperationResult<byte[]>.Ok(pdfBytes);
            }
            catch (Exception ex)
            {
                return OperationResult<byte[]>.Fail($"Có lỗi khi tạo PDF: {ex.Message}");
            }
        }

        public Task<OperationResult<byte[]>> GeneralQCPdfService(StatisticalReportQuery query, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        


    }
}
