using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.FormulaFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.FormulaFeatures
{
    public class ManufacturingVUFormulaService : IManufacturingVUFormulaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IVUFormulaPDF _FormulaPDF;
        public ManufacturingVUFormulaService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IVUFormulaPDF formulaPDF)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _FormulaPDF = formulaPDF;
        }

        // ======================================================================== Get ======================================================================== 
        public async Task<OperationResult<GetManufacturingVUFormula>> GetAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                if (id == Guid.Empty)
                    return OperationResult<GetManufacturingVUFormula>.Fail("Invalid Manufacturing VU Formula ID.");

                var formula = await _unitOfWork.ManufacturingVUFormulaRepository.Query()
                    .Where(x => x.ManufacturingVUFormulaId == id)
                    .Select(x => new
                    {
                        x.ManufacturingVUFormulaId,
                        x.FormulaId,
                        x.TotalProductionQuantity,
                        x.NumOfBatches,
                        x.QcCheck,
                        x.Formula,
                        x.LabNote,

                        ProductId = x.Formula.Product.ProductId,
                        ColourCode = x.Formula.Product.ColourCode,
                        Name = x.Formula.Product.Name,




                    })
                    .FirstOrDefaultAsync(ct);

                if (formula == null)
                    return OperationResult<GetManufacturingVUFormula>.Fail("Manufacturing VU Formula not found.");

                // chọn SR mới nhất (tuỳ bạn đổi logic chọn)
                var customer = await _unitOfWork.SampleRequestRepository.Query()
                    .Where(sr => sr.ProductId == formula.ProductId && sr.IsActive)
                    .OrderByDescending(sr => sr.CreatedDate)
                    .Select(sr => new
                    {
                        sr.Customer.ExternalId,
                        sr.Customer.CustomerName,
                        sr.SaleComment
                    })
                    .FirstOrDefaultAsync(ct);

                return OperationResult<GetManufacturingVUFormula>.Ok(new GetManufacturingVUFormula
                {
                    ManufacturingVUFormulaId = formula.ManufacturingVUFormulaId,
                    FormulaId = formula.FormulaId,

                    TotalProductionQuantity = formula.TotalProductionQuantity,
                    NumOfBatches = formula.NumOfBatches,
                    QcCheck = formula.QcCheck,

                    ColourCode = formula.ColourCode,
                    Name = formula.Name,

                    CustomerCode = customer?.ExternalId,
                    CustomerName = customer?.CustomerName,

                    LabNote = formula.LabNote,
                    Requirement = customer?.SaleComment
                });
            }
            catch (Exception ex)
            {
                return OperationResult<GetManufacturingVUFormula>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<OperationResult<PagedResult<GetSummaryManufacturingVUFormula>>> GetAllAsync(ManufacturingVUFormulaQuery req, CancellationToken ct = default)
        {
            // 1) Guard
            if (req.ManufacturingVUFormulaId.HasValue)
            {
                return OperationResult<PagedResult<GetSummaryManufacturingVUFormula>>
                    .Fail("Filtering by ManufacturingVUFormulaId is not supported in this method.");
            }

            var pageNumber = req.PageNumber <= 0 ? 1 : req.PageNumber;
            var pageSize = req.PageSize <= 0 ? 15 : req.PageSize;
            if (pageSize > 200) pageSize = 200;

            // 2) Base query (join qua navigation)
            // Repository.Query() thường đã trả IQueryable<T> rồi.
            IQueryable<ManufacturingVUFormula> baseQuery =
                _unitOfWork.ManufacturingVUFormulaRepository.Query();

            // Filter by ProductId
            if (req.ProductId.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.Formula.ProductId == req.ProductId.Value);
            }

            // Keyword (search trên formula code/name, product name, colour code)
            if (!string.IsNullOrWhiteSpace(req.Keyword))
            {
                var kw = req.Keyword.Trim();
                baseQuery = baseQuery.Where(x =>
                    x.Formula.ExternalId.Contains(kw) ||
                    x.Formula.Name.Contains(kw) ||
                    (x.Formula.Product != null && x.Formula.Product.Name != null && x.Formula.Product.Name.Contains(kw)) ||
                    (x.Formula.Product != null && x.Formula.Product.ColourCode != null && x.Formula.Product.ColourCode.Contains(kw)));
            }


            baseQuery = baseQuery.OrderByDescending(x => x.CreatedDate);

            // 5) Count + Paging
            var totalCount = await baseQuery.CountAsync(ct);

            var items = await baseQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new GetSummaryManufacturingVUFormula
                {
                    ManufacturingVUFormulaId = x.ManufacturingVUFormulaId,
                    VUFormula = x.Formula.ExternalId,

                    ProductName = x.Formula.Product.Name,
                    ColourCode = x.Formula.Product.ColourCode,

                    TotalProductionQuantity = x.TotalProductionQuantity,
                    NumOfBatches = x.NumOfBatches,
                    CreatedByName = x.CreatedByNavigation != null
                        ? x.CreatedByNavigation.FullName
                        : null,

                    CreatedDate = x.CreatedDate
                })
                .ToListAsync(ct);

            var paged = new PagedResult<GetSummaryManufacturingVUFormula>(items, totalCount, pageNumber, pageSize);
            return OperationResult<PagedResult<GetSummaryManufacturingVUFormula>>.Ok(paged);
        }

        // ======================================================================== Post =======================================================================
        public async Task<OperationResult> CreateAsync(PostManufacturingVUFormula req, CancellationToken ct = default)
        {
            try
            {
                if (req == null)
                {
                    return OperationResult.Fail("Request object is null.");
                }

                var user = _currentUser;
                var now = DateTime.Now;

                var manufacturingVUFormula = new ManufacturingVUFormula
                {
                    ManufacturingVUFormulaId = Guid.CreateVersion7(),
                    FormulaId = req.FormulaId,
                    TotalProductionQuantity = req.TotalProductionQuantity,
                    NumOfBatches = req.NumOfBatches,
                    LabNote = req.LabNote,
                    Requirement = req.Requirement,
                    QcCheck = req.QcCheck,
                    CreatedBy = user.EmployeeId,
                    UpdatedBy = user.EmployeeId,
                    CreatedDate = now
                };

                var 


                await _unitOfWork.ManufacturingVUFormulaRepository.AddAsync(manufacturingVUFormula, ct);
                await _unitOfWork.SaveChangesAsync();

                return OperationResult.Ok("Manufacturing VU Formula created successfully.");
            }

            catch(Exception ex)
            {
                return OperationResult.Fail($"An error occurred: {ex.Message}");
            }

        }


        // ====================================================================== Patch ======================================================================
        public async Task<OperationResult> UpdateAsync(PatchManufacturingVUFormula req, CancellationToken ct = default)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var user = _currentUser.EmployeeId;
                var now = DateTime.Now;

                var existingFormula = await _unitOfWork.ManufacturingVUFormulaRepository.Query(track: true)
                    .FirstOrDefaultAsync(x => x.ManufacturingVUFormulaId == req.ManufacturingVUFormulaId, ct);

                if (existingFormula == null) return OperationResult.Fail("Manufacturing VU Formula not found.");

                var changed = false;

                // Fix: Handle nullable decimal? to decimal conversion and null check
                changed |= PatchHelper.SetIf<decimal>(
                    req.TotalProductionQuantity,
                    current: () => existingFormula.TotalProductionQuantity ?? default,
                    apply: v => existingFormula.TotalProductionQuantity = v
                );

                // Fix: Handle nullable int? to int conversion and null check
                changed |= PatchHelper.SetIf<int>(
                    req.NumOfBatches,
                    current: () => existingFormula.NumOfBatches ?? default,
                    apply: v => existingFormula.NumOfBatches = v
                );

                // Patch string refs (chỉ set khi req.* != null)
                changed |= PatchHelper.SetIfRef<string>(req.QcCheck,
                    current: () => existingFormula.QcCheck,
                    apply: v => existingFormula.QcCheck = v);

                changed |= PatchHelper.SetIfRef<string>(req.LabNote,
                    current: () => existingFormula.LabNote,
                    apply: v => existingFormula.LabNote = v);

                // Add missing update logic (if needed)
                if (changed)
                {
                    existingFormula.UpdatedBy = user;
                    existingFormula.UpdatedDate = now;
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync(ct);
                    return OperationResult.Ok("Manufacturing VU Formula updated successfully.");
                }
                else
                {
                    await transaction.RollbackAsync(ct);
                    return OperationResult.Ok("No changes detected.");
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                return OperationResult.Fail($"An error occurred: {ex.Message}");
            }
        }

        // ====================================================================== Export PDF ======================================================================
        public async Task<byte[]> ExportToPdfAsync(Guid manufacturingVUFormulaId, CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;

            using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (manufacturingVUFormulaId == Guid.Empty)
                    throw new ArgumentException("Id rỗng.", nameof(manufacturingVUFormulaId));

                var formula = await _unitOfWork.ManufacturingVUFormulaRepository.Query()
                    .Where(x => x.ManufacturingVUFormulaId == manufacturingVUFormulaId /* + filter company/isActive nếu có */)
                    .Select(x => new
                    {
                        x.ManufacturingVUFormulaId,
                        x.FormulaId,
                        x.TotalProductionQuantity,
                        x.NumOfBatches,
                        x.QcCheck,
                        x.LabNote,
                        x.Formula,

                        ProductId = x.Formula.Product.ProductId,
                        ColourCode = x.Formula.Product.ColourCode,
                        Name = x.Formula.Product.Name,
                        BatchNo = x.Formula.ExternalId,

                        UserRate = x.Formula.Product.UsageRate,
                        x.Requirement,

                        RequestDate = x.Formula.Product.SampleRequests
                            .Where(sr => sr.IsActive)
                            .OrderByDescending(sr => sr.CreatedDate)
                            .Select(sr => sr.RequestDeliveryDate)
                            .FirstOrDefault(),

                        Materials = x.Formula.FormulaMaterials
                            .Where(m => m.IsActive)
                            .OrderBy(m => m.MaterialExternalIdSnapshot ?? m.Material.ExternalId)
                            .Select(m => new FormulaPDFMaterialDTOs
                            {
                                ExternalId = (m.MaterialExternalIdSnapshot ?? m.Material.ExternalId) ?? "",
                                MaterialName = (m.MaterialNameSnapshot ?? m.Material.Name) ?? "",
                                Quantity = m.Quantity,
                                CategoryId = m.CategoryId
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync(ct);

                if (formula == null)
                    throw new KeyNotFoundException($"Không tìm thấy ManufacturingVUFormula: {manufacturingVUFormulaId} (CompanyId={companyId}).");

                // ✅ “chốt” ProductId ra biến value-type trước khi đi query khác
                var productId = formula.ProductId;

                // chọn SR mới nhất
                var customer = await _unitOfWork.SampleRequestRepository.Query()
                    .Where(sr => sr.ProductId == formula.ProductId && sr.IsActive)
                    .OrderByDescending(sr => sr.CreatedDate)
                    .Select(sr => new
                    {
                        sr.Customer.ExternalId,
                        sr.Customer.CustomerName,
                        sr.SaleComment
                    })
                    .FirstOrDefaultAsync(ct);

                var dto = new ManufacturingVUPDF
                {
                    BatchNo = formula.BatchNo,
                    RequestDate = formula.RequestDate,


                    getManufacturingVUFormula = new GetManufacturingVUFormula
                    {
                        ManufacturingVUFormulaId = formula.ManufacturingVUFormulaId,
                        FormulaId = formula.FormulaId,

                        TotalProductionQuantity = formula.TotalProductionQuantity,
                        NumOfBatches = formula.NumOfBatches,
                        QcCheck = formula.QcCheck,

                        ColourCode = formula.ColourCode,
                        Name = formula.Name,

                        CustomerCode = customer?.ExternalId,
                        CustomerName = customer?.CustomerName,

                        userRate = formula.UserRate,
                        LabNote = formula.LabNote,
                        Requirement = formula.Requirement
                    },

                    materials = formula.Materials ?? new List<FormulaPDFMaterialDTOs>()
                };

                var pdfBytes = _FormulaPDF.Render(dto);

                await tx.CommitAsync(ct);
                return pdfBytes;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }



    }
}
