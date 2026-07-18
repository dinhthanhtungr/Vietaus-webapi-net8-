using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulaAdjustments;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulaAdjustments;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public partial class MfgAdjustmenService
    {
        private IQueryable<ManufacturingFormulaAdjustment> ApplyQuery(
            IQueryable<ManufacturingFormulaAdjustment> query,
            ManufacturingFormulaAdjustmentQuery request)
        {
            query = query.Where(x => x.CompanyId == _currentUser.CompanyId);

            if (!request.IncludeInactive)
                query = query.Where(x => x.IsActive);

            if (request.MfgProductionOrderId.HasValue && request.MfgProductionOrderId.Value != Guid.Empty)
                query = query.Where(x => x.MfgProductionOrderId == request.MfgProductionOrderId.Value);

            if (request.ManufacturingFormulaId.HasValue && request.ManufacturingFormulaId.Value != Guid.Empty)
                query = query.Where(x => x.ManufacturingFormulaId == request.ManufacturingFormulaId.Value);

            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(x => x.Status == NormalizeStatus(request.Status));

            if (request.BatchNo.HasValue)
                query = query.Where(x => x.Batches.Any(b => b.BatchNo == request.BatchNo.Value));

            if (request.TrialNo.HasValue)
                query = query.Where(x => x.Batches.Any(b => b.TrialNo == request.TrialNo.Value));

            if (!string.IsNullOrWhiteSpace(request.BatchStatus))
                query = query.Where(x => x.Batches.Any(b => b.Status == NormalizeStatus(request.BatchStatus)));

            if (request.IsAdditionalBatch.HasValue)
                query = query.Where(x => x.Batches.Any(b => b.IsAdditionalBatch == request.IsAdditionalBatch.Value));

            return query;
        }

        private static IQueryable<GetManufacturingFormulaAdjustment> ProjectToDto(
            IQueryable<ManufacturingFormulaAdjustment> query)
        {
            return query.Select(x => new GetManufacturingFormulaAdjustment
            {
                ManufacturingFormulaAdjustmentId = x.ManufacturingFormulaAdjustmentId,
                MfgProductionOrderId = x.MfgProductionOrderId,
                MfgProductionOrderExternalId = x.MfgProductionOrder.ExternalId,
                ManufacturingFormulaId = x.ManufacturingFormulaId,
                ManufacturingFormulaExternalId = x.ManufacturingFormula != null ? x.ManufacturingFormula.ExternalId : null,
                Status = x.Status,
                Note = x.Note,
                CompanyId = x.CompanyId,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                UpdatedDate = x.UpdatedDate,
                UpdatedBy = x.UpdatedBy,
                Batches = x.Batches
                    .Where(b => b.IsActive)
                    .OrderBy(b => b.BatchNo)
                    .ThenBy(b => b.TrialNo)
                    .Select(b => new GetManufacturingFormulaAdjustmentBatch
                    {
                        ManufacturingFormulaAdjustmentBatchId = b.ManufacturingFormulaAdjustmentBatchId,
                        ManufacturingFormulaAdjustmentId = b.ManufacturingFormulaAdjustmentId,
                        BatchNo = b.BatchNo,
                        TrialNo = b.TrialNo,
                        Status = b.Status,
                        AdjustmentType = b.AdjustmentType,
                        IsAdditionalBatch = b.IsAdditionalBatch,
                        BatchQuantity = b.BatchQuantity,
                        TakenQuantityGram = b.TakenQuantityGram,
                        FinalQuantityGram = b.FinalQuantityGram,
                        ApplyToNextBatches = b.ApplyToNextBatches,
                        Note = b.Note,
                        IsActive = b.IsActive,
                        CreatedDate = b.CreatedDate,
                        CreatedBy = b.CreatedBy,
                        UpdatedDate = b.UpdatedDate,
                        UpdatedBy = b.UpdatedBy,
                        Items = b.Items
                            .OrderBy(i => i.LineNo)
                            .Select(i => new GetManufacturingFormulaAdjustmentItem
                            {
                                ManufacturingFormulaAdjustmentItemId = i.ManufacturingFormulaAdjustmentItemId,
                                ManufacturingFormulaAdjustmentBatchId = i.ManufacturingFormulaAdjustmentBatchId,
                                MaterialId = i.MaterialId,
                                ProductId = i.ProductId,
                                CategoryId = i.CategoryId,
                                itemType = i.itemType,
                                BaseQuantity = i.BaseQuantity,
                                AdjustedQuantity = i.AdjustedQuantity,
                                DeltaQuantity = i.DeltaQuantity,
                                Unit = i.Unit,
                                LotNo = i.LotNo,
                                Note = i.Note,
                                LineNo = i.LineNo,
                                MaterialNameSnapshot = i.Material == null
                                    ? i.MaterialNameSnapshot
                                    : i.Material.Name,
                                MaterialExternalIdSnapshot = i.Material == null
                                    ? i.MaterialExternalIdSnapshot
                                    : i.Material.ExternalId,
                                ProductNameSnapshot = i.Product == null
                                    ? i.ProductNameSnapshot
                                    : i.Product.Name,
                                ProductExternalIdSnapshot = i.Product == null
                                    ? i.ProductExternalIdSnapshot
                                    : i.Product.ColourCode
                            })
                            .ToList()
                    })
                    .ToList()
            });
        }
    }
}
