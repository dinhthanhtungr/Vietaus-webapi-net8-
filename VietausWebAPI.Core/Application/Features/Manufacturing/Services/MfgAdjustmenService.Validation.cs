using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulaAdjustments;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public partial class MfgAdjustmenService
    {
        private async Task<OperationResult> ValidateCreateAsync(
            PostManufacturingFormulaAdjustment request,
            CancellationToken ct)
        {
            if (request == null)
                return OperationResult.Fail("Request khong duoc de trong.");

            if (request.MfgProductionOrderId == Guid.Empty)
                return OperationResult.Fail("MfgProductionOrderId khong hop le.");

            if (!IsValidStatus(request.Status))
                return OperationResult.Fail("Status khong hop le.");

            var orderExists = await _mfgProductionOrderRepository.Query()
                .AnyAsync(x =>
                    x.MfgProductionOrderId == request.MfgProductionOrderId &&
                    x.CompanyId == _currentUser.CompanyId &&
                    x.IsActive, ct);

            if (!orderExists)
                return OperationResult.Fail("Khong tim thay lenh san xuat.");

            return await ValidateReferencesAndBatchesAsync(
                request.ManufacturingFormulaId,
                request.Batches,
                ct);
        }

        private async Task<OperationResult> ValidatePatchAsync(
            PatchManufacturingFormulaAdjustment request,
            CancellationToken ct)
        {
            if (request.Status != null && !IsValidStatus(request.Status))
                return OperationResult.Fail("Status khong hop le.");

            return await ValidateReferencesAndBatchesAsync(
                request.ManufacturingFormulaId,
                request.Batches,
                ct);
        }

        private async Task<OperationResult> ValidateReferencesAndBatchesAsync(
            Guid? manufacturingFormulaId,
            List<PostManufacturingFormulaAdjustmentBatch>? batches,
            CancellationToken ct)
        {
            manufacturingFormulaId = NormalizeNullableId(manufacturingFormulaId);

            if (manufacturingFormulaId.HasValue)
            {
                var formulaExists = await _manufacturingFormulaRepository.Query()
                    .AnyAsync(x =>
                        x.ManufacturingFormulaId == manufacturingFormulaId.Value &&
                        x.CompanyId == _currentUser.CompanyId &&
                        x.IsActive, ct);

                if (!formulaExists)
                    return OperationResult.Fail("Khong tim thay cong thuc san xuat.");
            }

            if (batches == null)
                return OperationResult.Ok();

            var duplicateBatch = batches
                .Where(x => x.BatchNo > 0 && x.TrialNo > 0)
                .GroupBy(x => new { x.BatchNo, x.TrialNo })
                .FirstOrDefault(x => x.Count() > 1);

            if (duplicateBatch != null)
                return OperationResult.Fail($"Bi trung me {duplicateBatch.Key.BatchNo}, lan chinh {duplicateBatch.Key.TrialNo}.");

            for (var batchIndex = 0; batchIndex < batches.Count; batchIndex++)
            {
                var batch = batches[batchIndex];
                var batchLabel = batch.BatchNo > 0 ? batch.BatchNo.ToString() : (batchIndex + 1).ToString();

                if (batch.BatchNo <= 0)
                    return OperationResult.Fail($"Me {batchLabel}: BatchNo phai lon hon 0.");

                if (batch.TrialNo <= 0)
                    return OperationResult.Fail($"Me {batchLabel}: TrialNo phai lon hon 0.");

                if (!IsValidStatus(batch.Status))
                    return OperationResult.Fail($"Me {batchLabel}: Status khong hop le.");

                if (!Enum.IsDefined(typeof(AdjustmentType), batch.AdjustmentType))
                    return OperationResult.Fail($"Me {batchLabel}: AdjustmentType khong hop le.");

                if (batch.TakenQuantityGram.HasValue && batch.TakenQuantityGram.Value <= 0)
                    return OperationResult.Fail($"Me {batchLabel}: TakenQuantityGram phai lon hon 0.");

                if (batch.FinalQuantityGram.HasValue && batch.FinalQuantityGram.Value <= 0)
                    return OperationResult.Fail($"Me {batchLabel}: FinalQuantityGram phai lon hon 0.");

                if (batch.TakenQuantityGram.HasValue &&
                    batch.FinalQuantityGram.HasValue &&
                    batch.FinalQuantityGram.Value < batch.TakenQuantityGram.Value)
                {
                    return OperationResult.Fail($"Me {batchLabel}: FinalQuantityGram khong duoc nho hon TakenQuantityGram.");
                }

                var itemValidation = await ValidateItemsAsync(batch.Items, $"Me {batchLabel}", ct);
                if (!itemValidation.Success)
                    return itemValidation;
            }

            return OperationResult.Ok();
        }

        private async Task<OperationResult> ValidateItemsAsync(
            List<PostManufacturingFormulaAdjustmentItem>? items,
            string prefix,
            CancellationToken ct)
        {
            if (items == null)
                return OperationResult.Ok();

            var categoryIds = new HashSet<Guid>();
            var materialIds = new HashSet<Guid>();
            var productIds = new HashSet<Guid>();

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var line = item.LineNo > 0 ? item.LineNo : i + 1;

                if (item.CategoryId == Guid.Empty)
                    return OperationResult.Fail($"{prefix}, dong {line}: CategoryId khong hop le.");

                var hasMaterial = item.MaterialId.HasValue && item.MaterialId.Value != Guid.Empty;
                var hasProduct = item.ProductId.HasValue && item.ProductId.Value != Guid.Empty;

                if (hasMaterial == hasProduct)
                    return OperationResult.Fail($"{prefix}, dong {line}: phai chon dung mot MaterialId hoac ProductId.");

                if (!Enum.IsDefined(typeof(ItemType), item.itemType))
                    return OperationResult.Fail($"{prefix}, dong {line}: itemType khong hop le.");

                if ((item.itemType == ItemType.Material || item.itemType == ItemType.MaterialFailure) && !hasMaterial)
                    return OperationResult.Fail($"{prefix}, dong {line}: itemType yeu cau MaterialId.");

                if ((item.itemType == ItemType.Product || item.itemType == ItemType.ProductFailure) && !hasProduct)
                    return OperationResult.Fail($"{prefix}, dong {line}: itemType yeu cau ProductId.");

                if (!item.BaseQuantity.HasValue && !item.AdjustedQuantity.HasValue && !item.DeltaQuantity.HasValue)
                    return OperationResult.Fail($"{prefix}, dong {line}: phai co it nhat mot gia tri khoi luong.");

                categoryIds.Add(item.CategoryId);

                if (hasMaterial)
                    materialIds.Add(item.MaterialId!.Value);

                if (hasProduct)
                    productIds.Add(item.ProductId!.Value);
            }

            var referenceValidation = await ValidateItemReferencesAsync(prefix, categoryIds, materialIds, productIds, ct);
            if (!referenceValidation.Success)
                return referenceValidation;

            return OperationResult.Ok();
        }

        private async Task<OperationResult> ValidateItemReferencesAsync(
            string prefix,
            HashSet<Guid> categoryIds,
            HashSet<Guid> materialIds,
            HashSet<Guid> productIds,
            CancellationToken ct)
        {
            if (categoryIds.Count > 0)
            {
                var existingCategoryIds = await _unitOfWork.CategoryRepository.Query()
                    .Where(x => categoryIds.Contains(x.CategoryId))
                    .Select(x => x.CategoryId)
                    .ToListAsync(ct);

                if (existingCategoryIds.Count != categoryIds.Count)
                    return OperationResult.Fail($"{prefix}: co CategoryId khong ton tai.");
            }

            if (materialIds.Count > 0)
            {
                var existingMaterialIds = await _unitOfWork.MaterialRepository.Query()
                    .Where(x => materialIds.Contains(x.MaterialId) && x.IsActive == true)
                    .Select(x => x.MaterialId)
                    .ToListAsync(ct);

                if (existingMaterialIds.Count != materialIds.Count)
                    return OperationResult.Fail($"{prefix}: co MaterialId khong ton tai.");
            }

            if (productIds.Count > 0)
            {
                var existingProductIds = await _unitOfWork.ProductRepository.Query()
                    .Where(x => productIds.Contains(x.ProductId) && x.IsActive == true)
                    .Select(x => x.ProductId)
                    .ToListAsync(ct);

                if (existingProductIds.Count != productIds.Count)
                    return OperationResult.Fail($"{prefix}: co ProductId khong ton tai.");
            }

            return OperationResult.Ok();
        }

        private static bool IsValidStatus(string? status)
        {
            return !string.IsNullOrWhiteSpace(status) &&
                   Enum.TryParse<AdjustmentStatus>(status.Trim(), ignoreCase: true, out _);
        }

        private static string NormalizeStatus(string status)
        {
            return Enum.Parse<AdjustmentStatus>(status.Trim(), ignoreCase: true).ToString();
        }

        private static Guid? NormalizeNullableId(Guid? id)
        {
            return id.HasValue && id.Value != Guid.Empty ? id.Value : null;
        }

        private static string? Clean(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
