using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulaAdjustments;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public partial class MfgAdjustmenService
    {
        private ManufacturingFormulaAdjustment MapAdjustment(
            PostManufacturingFormulaAdjustment request,
            Guid adjustmentId,
            DateTime now,
            Guid userId)
        {
            return new ManufacturingFormulaAdjustment
            {
                ManufacturingFormulaAdjustmentId = adjustmentId,
                MfgProductionOrderId = request.MfgProductionOrderId,
                ManufacturingFormulaId = NormalizeNullableId(request.ManufacturingFormulaId),
                Status = NormalizeStatus(request.Status),
                Note = Clean(request.Note),
                CompanyId = _currentUser.CompanyId,
                CreatedDate = now,
                CreatedBy = userId,
                IsActive = true,
                Batches = request.Batches
                    .Select((batch, index) => MapBatch(batch, adjustmentId, index + 1, now, userId))
                    .ToList()
            };
        }

        private void ApplyHeaderPatch(
            ManufacturingFormulaAdjustment entity,
            PatchManufacturingFormulaAdjustment request)
        {
            if (request.ManufacturingFormulaId.HasValue)
                entity.ManufacturingFormulaId = NormalizeNullableId(request.ManufacturingFormulaId);

            if (request.Status != null)
                entity.Status = NormalizeStatus(request.Status);

            if (request.Note != null)
                entity.Note = Clean(request.Note);
        }

        private void ReplaceBatches(
            ManufacturingFormulaAdjustment entity,
            List<PostManufacturingFormulaAdjustmentBatch> batches)
        {
            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;

            foreach (var batch in entity.Batches.Where(x => x.IsActive))
            {
                batch.IsActive = false;
                batch.UpdatedDate = now;
                batch.UpdatedBy = userId;
            }

            var newBatches = batches
                .Select((batch, index) => MapBatch(batch, entity.ManufacturingFormulaAdjustmentId, index + 1, now, userId))
                .ToList();

            foreach (var batch in newBatches)
                entity.Batches.Add(batch);
        }

        private async Task ApplyBatchPatchAsync(
            ManufacturingFormulaAdjustmentBatch batch,
            PostManufacturingFormulaAdjustmentBatch request,
            CancellationToken ct)
        {
            batch.BatchNo = request.BatchNo;
            batch.TrialNo = request.TrialNo;
            batch.Status = NormalizeStatus(request.Status);
            //batch.AdjustmentType = request.AdjustmentType;
            batch.IsAdditionalBatch = request.IsAdditionalBatch;
            batch.BatchQuantity = request.BatchQuantity;
            batch.TakenQuantityGram = request.TakenQuantityGram;
            batch.FinalQuantityGram = request.FinalQuantityGram;
            batch.ApplyToNextBatches = request.ApplyToNextBatches;
            batch.Note = Clean(request.Note);
            batch.UpdatedDate = DateTime.Now;
            batch.UpdatedBy = _currentUser.EmployeeId;

            await _repository.DeleteItemsByBatchIdAsync(batch.ManufacturingFormulaAdjustmentBatchId, ct);
            batch.Items.Clear();

            var newItems = request.Items
                .Select((item, index) => MapItem(item, batch.ManufacturingFormulaAdjustmentBatchId, index + 1))
                .ToList();

            foreach (var item in newItems)
                batch.Items.Add(item);
        }

        private static bool HasDuplicateBatch(
            ManufacturingFormulaAdjustment entity,
            int batchNo,
            int trialNo,
            Guid? exceptBatchId = null)
        {
            return entity.Batches.Any(x =>
                x.IsActive &&
                x.BatchNo == batchNo &&
                x.TrialNo == trialNo &&
                (!exceptBatchId.HasValue || x.ManufacturingFormulaAdjustmentBatchId != exceptBatchId.Value));
        }

        private static ManufacturingFormulaAdjustmentBatch MapBatch(
            PostManufacturingFormulaAdjustmentBatch request,
            Guid adjustmentId,
            int fallbackBatchNo,
            DateTime now,
            Guid userId)
        {
            var batchId = Guid.CreateVersion7();

            return new ManufacturingFormulaAdjustmentBatch
            {
                ManufacturingFormulaAdjustmentBatchId = batchId,
                ManufacturingFormulaAdjustmentId = adjustmentId,
                BatchNo = request.BatchNo > 0 ? request.BatchNo : fallbackBatchNo,
                TrialNo = request.TrialNo > 0 ? request.TrialNo : 1,
                Status = NormalizeStatus(request.Status),
                AdjustmentType = request.AdjustmentType,
                IsAdditionalBatch = request.IsAdditionalBatch,
                BatchQuantity = request.BatchQuantity,
                TakenQuantityGram = request.TakenQuantityGram,
                FinalQuantityGram = request.FinalQuantityGram,
                ApplyToNextBatches = request.ApplyToNextBatches,
                Note = Clean(request.Note),
                IsActive = true,
                CreatedDate = now,
                CreatedBy = userId,
                Items = request.Items
                    .Select((item, index) => MapItem(item, batchId, index + 1))
                    .ToList()
            };
        }

        private static ManufacturingFormulaAdjustmentItem MapItem(
            PostManufacturingFormulaAdjustmentItem request,
            Guid batchId,
            int fallbackLineNo)
        {
            return new ManufacturingFormulaAdjustmentItem
            {
                ManufacturingFormulaAdjustmentItemId = Guid.CreateVersion7(),
                ManufacturingFormulaAdjustmentBatchId = batchId,
                MaterialId = NormalizeNullableId(request.MaterialId),
                ProductId = NormalizeNullableId(request.ProductId),
                CategoryId = request.CategoryId,
                itemType = request.itemType,
                BaseQuantity = request.BaseQuantity,
                AdjustedQuantity = request.AdjustedQuantity,
                DeltaQuantity = request.DeltaQuantity,
                Unit = Clean(request.Unit),
                LotNo = Clean(request.LotNo) ?? string.Empty,
                Note = Clean(request.Note),
                LineNo = request.LineNo > 0 ? request.LineNo : fallbackLineNo,
                MaterialNameSnapshot = Clean(request.MaterialNameSnapshot),
                MaterialExternalIdSnapshot = Clean(request.MaterialExternalIdSnapshot),
                ProductNameSnapshot = Clean(request.ProductNameSnapshot),
                ProductExternalIdSnapshot = Clean(request.ProductExternalIdSnapshot)
            };
        }

        private static PostManufacturingFormulaAdjustmentBatch ToBatchRequest(
            PostManufacturingFormulaAdjustmentBatchRange request,
            int batchNo)
        {
            return new PostManufacturingFormulaAdjustmentBatch
            {
                BatchNo = batchNo,
                TrialNo = request.TrialNo,
                Status = request.Status,
                AdjustmentType = request.AdjustmentType,
                IsAdditionalBatch = request.IsAdditionalBatch,
                BatchQuantity = request.BatchQuantity,
                TakenQuantityGram = request.TakenQuantityGram,
                FinalQuantityGram = request.FinalQuantityGram,
                ApplyToNextBatches = request.ApplyToNextBatches,
                Note = request.Note,
                Items = request.Items
                    .Select(CloneItemRequest)
                    .ToList()
            };
        }

        private static PostManufacturingFormulaAdjustmentItem CloneItemRequest(
            PostManufacturingFormulaAdjustmentItem item)
        {
            return new PostManufacturingFormulaAdjustmentItem
            {
                MaterialId = item.MaterialId,
                ProductId = item.ProductId,
                CategoryId = item.CategoryId,
                itemType = item.itemType,
                BaseQuantity = item.BaseQuantity,
                AdjustedQuantity = item.AdjustedQuantity,
                DeltaQuantity = item.DeltaQuantity,
                Unit = item.Unit,
                LotNo = item.LotNo,
                Note = item.Note,
                LineNo = item.LineNo,
                MaterialNameSnapshot = item.MaterialNameSnapshot,
                MaterialExternalIdSnapshot = item.MaterialExternalIdSnapshot,
                ProductNameSnapshot = item.ProductNameSnapshot,
                ProductExternalIdSnapshot = item.ProductExternalIdSnapshot
            };
        }
    }
}
