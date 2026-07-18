using System.Globalization;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulaAdjustments;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public partial class MfgAdjustmenService
    {
        public async Task<GetManufacturingFormulaAdjustmentBatchGroups?> GetBatchGroupsAsync(
            Guid id,
            CancellationToken ct = default)
        {
            var adjustment = await GetByIdAsync(id, ct);
            if (adjustment == null)
                return null;

            return ToBatchGroupsResponse(adjustment);
        }

        public async Task<GetManufacturingFormulaAdjustmentBatchGroups?> GetBatchGroupsByProductionOrderIdAsync(
            Guid mfgProductionOrderId,
            CancellationToken ct = default)
        {
            if (mfgProductionOrderId == Guid.Empty)
                return null;

            var adjustment = await ProjectToDto(_repository.Query())
                .Where(x =>
                    x.MfgProductionOrderId == mfgProductionOrderId &&
                    x.CompanyId == _currentUser.CompanyId &&
                    x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync(ct);

            return adjustment == null
                ? null
                : ToBatchGroupsResponse(adjustment);
        }

        private static GetManufacturingFormulaAdjustmentBatchGroups ToBatchGroupsResponse(
            GetManufacturingFormulaAdjustment adjustment)
        {
            return new GetManufacturingFormulaAdjustmentBatchGroups
            {
                ManufacturingFormulaAdjustmentId = adjustment.ManufacturingFormulaAdjustmentId,
                MfgProductionOrderId = adjustment.MfgProductionOrderId,
                MfgProductionOrderExternalId = adjustment.MfgProductionOrderExternalId,
                ManufacturingFormulaId = adjustment.ManufacturingFormulaId,
                ManufacturingFormulaExternalId = adjustment.ManufacturingFormulaExternalId,
                Status = adjustment.Status,
                Note = adjustment.Note,
                CompanyId = adjustment.CompanyId,
                IsActive = adjustment.IsActive,
                CreatedDate = adjustment.CreatedDate,
                CreatedBy = adjustment.CreatedBy,
                UpdatedDate = adjustment.UpdatedDate,
                UpdatedBy = adjustment.UpdatedBy,
                BatchGroups = GroupBatchesBySignature(adjustment.Batches)
            };
        }

        private static List<GetManufacturingFormulaAdjustmentBatchGroup> GroupBatchesBySignature(
            List<GetManufacturingFormulaAdjustmentBatch> batches)
        {
            var result = new List<GetManufacturingFormulaAdjustmentBatchGroup>();

            foreach (var batch in batches.OrderBy(x => x.BatchNo).ThenBy(x => x.TrialNo))
            {
                var signature = BuildBatchSignature(batch);
                var last = result.LastOrDefault();

                var canMerge =
                    last != null &&
                    last.Signature == signature &&
                    last.BatchTo + 1 == batch.BatchNo;

                if (canMerge)
                {
                    last.BatchTo = batch.BatchNo;
                    last.Batches.Add(batch);
                    continue;
                }

                result.Add(new GetManufacturingFormulaAdjustmentBatchGroup
                {
                    BatchFrom = batch.BatchNo,
                    BatchTo = batch.BatchNo,
                    Signature = signature,
                    BatchTemplate = batch,
                    Batches = new List<GetManufacturingFormulaAdjustmentBatch> { batch }
                });
            }

            return result;
        }

        private static string BuildBatchSignature(GetManufacturingFormulaAdjustmentBatch batch)
        {
            var items = batch.Items
                .OrderBy(x => x.LineNo)
                .ThenBy(x => x.MaterialId)
                .ThenBy(x => x.ProductId)
                .Select(x => string.Join("|",
                    x.itemType.ToString(),
                    NormalizeGuid(x.MaterialId),
                    NormalizeGuid(x.ProductId),
                    NormalizeGuid(x.CategoryId),
                    NormalizeDecimal(x.BaseQuantity),
                    NormalizeDecimal(x.AdjustedQuantity),
                    NormalizeDecimal(x.DeltaQuantity),
                    NormalizeText(x.Unit),
                    NormalizeText(x.LotNo)
                ));

            return string.Join("::",
                batch.TrialNo,
                NormalizeText(batch.Status),
                batch.AdjustmentType.ToString(),
                batch.IsAdditionalBatch,
                NormalizeDecimal(batch.BatchQuantity),
                NormalizeDecimal(batch.TakenQuantityGram),
                NormalizeDecimal(batch.FinalQuantityGram),
                batch.ApplyToNextBatches,
                string.Join(";;", items)
            );
        }

        private static List<ManufacturingFormulaAdjustmentBatch> FindContiguousBatchGroup(
            List<ManufacturingFormulaAdjustmentBatch> batches,
            ManufacturingFormulaAdjustmentBatch target)
        {
            var ordered = batches
                .OrderBy(x => x.BatchNo)
                .ThenBy(x => x.TrialNo)
                .ToList();

            var targetIndex = ordered.FindIndex(x =>
                x.ManufacturingFormulaAdjustmentBatchId == target.ManufacturingFormulaAdjustmentBatchId);

            if (targetIndex < 0)
                return new List<ManufacturingFormulaAdjustmentBatch> { target };

            var signature = BuildBatchSignature(target);
            var result = new List<ManufacturingFormulaAdjustmentBatch> { target };

            var expectedBatchNo = target.BatchNo - 1;
            for (var i = targetIndex - 1; i >= 0; i--)
            {
                var current = ordered[i];
                if (current.BatchNo != expectedBatchNo || BuildBatchSignature(current) != signature)
                    break;

                result.Insert(0, current);
                expectedBatchNo--;
            }

            expectedBatchNo = target.BatchNo + 1;
            for (var i = targetIndex + 1; i < ordered.Count; i++)
            {
                var current = ordered[i];
                if (current.BatchNo != expectedBatchNo || BuildBatchSignature(current) != signature)
                    break;

                result.Add(current);
                expectedBatchNo++;
            }

            return result;
        }

        private static string BuildBatchSignature(ManufacturingFormulaAdjustmentBatch batch)
        {
            var items = batch.Items
                .OrderBy(x => x.LineNo)
                .ThenBy(x => x.MaterialId)
                .ThenBy(x => x.ProductId)
                .Select(x => string.Join("|",
                    x.itemType.ToString(),
                    NormalizeGuid(x.MaterialId),
                    NormalizeGuid(x.ProductId),
                    NormalizeGuid(x.CategoryId),
                    NormalizeDecimal(x.BaseQuantity),
                    NormalizeDecimal(x.AdjustedQuantity),
                    NormalizeDecimal(x.DeltaQuantity),
                    NormalizeText(x.Unit),
                    NormalizeText(x.LotNo)
                ));

            return string.Join("::",
                batch.TrialNo,
                NormalizeText(batch.Status),
                batch.AdjustmentType.ToString(),
                batch.IsAdditionalBatch,
                NormalizeDecimal(batch.BatchQuantity),
                NormalizeDecimal(batch.TakenQuantityGram),
                NormalizeDecimal(batch.FinalQuantityGram),
                batch.ApplyToNextBatches,
                string.Join(";;", items)
            );
        }

        private static string NormalizeGuid(Guid? value)
        {
            return value.HasValue && value.Value != Guid.Empty
                ? value.Value.ToString("N")
                : string.Empty;
        }

        private static string NormalizeGuid(Guid value)
        {
            return value != Guid.Empty
                ? value.ToString("N")
                : string.Empty;
        }

        private static string NormalizeDecimal(decimal? value)
        {
            return value.HasValue
                ? value.Value.ToString("0.##########", CultureInfo.InvariantCulture)
                : string.Empty;
        }

        private static string NormalizeText(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim().ToLowerInvariant();
        }
    }
}
