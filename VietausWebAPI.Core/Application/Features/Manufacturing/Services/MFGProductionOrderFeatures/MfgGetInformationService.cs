using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.MfgGetInformationDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.MfgGetInformationInforDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.MFGProductionOrderFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Features.ManufacturingFeature.Services
{
    /// <summary>
    /// Lấy thông tin ghi chú của MFG Production Order và thông tin công thức sản xuất,
    /// bao gồm danh sách item công thức và tồn kho khả dụng.
    /// </summary>
    public class MfgGetInformationService : IMfgGetInformationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWarehouseReadService _warehouseReadService;

        public MfgGetInformationService(
            IUnitOfWork unitOfWork,
            IWarehouseReadService warehouseReadService)
        {
            _unitOfWork = unitOfWork;
            _warehouseReadService = warehouseReadService;
        }

        //=========================================================== Get Information When New ===========================================================

        /// <summary>
        /// Lấy thông tin note của MPO (nếu có MfgProductionOrderId) và thông tin formula (nếu có mfgFormulaId).
        /// MfgProductionOrderId có thể null.
        /// </summary>
        public async Task<GetMfgProductionOrderNoteInfor> GetAsync(Guid? mfgProductionOrderId, Guid? formulaId, FormulaType? formulaType, CancellationToken ct = default)
        {
            GetMfgProductionOrderNoteInfor result;

            // 1) Lấy note từ MPO nếu có mfgProductionOrderId
            if (mfgProductionOrderId.HasValue && mfgProductionOrderId.Value != Guid.Empty)
            {
                result = await _unitOfWork.MfgProductionOrderRepository
                    .Query(false)
                    .Where(x => x.MfgProductionOrderId == mfgProductionOrderId.Value && x.IsActive)
                    .Select(x => new GetMfgProductionOrderNoteInfor
                    {
                        MfgProductionOrderId = x.MfgProductionOrderId,

                        LabNote = x.LabNote,
                        Requirement = x.Requirement,
                        PlpuNote = x.PlpuNote,
                        QcCheck = x.QcCheck,
                        StepOfProduct = x.StepOfProduct
                    })
                    .FirstOrDefaultAsync(ct)
                    ?? new GetMfgProductionOrderNoteInfor();
            }
            else
            {
                result = new GetMfgProductionOrderNoteInfor();
            }

            // 2) Nếu không có formula id thì trả luôn
            if (!formulaId.HasValue || formulaId.Value == Guid.Empty)
            {
                result.FormulaInfor = null;
                return result;
            }

            // 3) Quyết định đọc VA hay VU theo FormulaType
            if (IsVuFormulaType(formulaType))
            {
                result.FormulaInfor = await GetVuFormulaInforAsync(formulaId.Value, ct);
            }
            else
            {
                result.FormulaInfor = await GetVaFormulaInforAsync(formulaId.Value, ct);
            }

            // 4) Nếu không có formula hoặc không có item thì trả luôn
            if (result.FormulaInfor == null || result.FormulaInfor.FormulaItems.Count == 0)
                return result;

            // 5) Nếu là VA thì map tồn kho ảo
            if (!IsVuFormulaType(formulaType))
            {
                var avaDict = await _warehouseReadService.GetVaAvailabilityDictAsync(formulaId.Value, ct);

                foreach (var row in result.FormulaInfor.FormulaItems)
                {
                    if (string.IsNullOrWhiteSpace(row.MaterialExternalIdSnapshot))
                        continue;

                    var codeUpper = row.MaterialExternalIdSnapshot.Trim().ToUpperInvariant();

                    if (avaDict.TryGetValue(codeUpper, out var ava))
                    {
                        row.OnHandKg = ava.OnHandKg;
                        row.ReservedOpenAllKg = ava.ReservedOpenAllKg;
                        row.AvailableKg = ava.AvailableKg;
                    }
                    else
                    {
                        row.OnHandKg = 0m;
                        row.ReservedOpenAllKg = 0m;
                        row.AvailableKg = 0m;
                    }
                }
            }

            return result;
        }

        private async Task<GetMfgProductionOrderFormulaInfor?> GetVaFormulaInforAsync(Guid manufacturingFormulaId, CancellationToken ct)
        {
            return await _unitOfWork.ManufacturingFormulaRepository
                .Query(false)
                .Where(f => f.ManufacturingFormulaId == manufacturingFormulaId && f.IsActive)
                .Select(f => new GetMfgProductionOrderFormulaInfor
                {
                    ManufacturingFormulaId = f.ManufacturingFormulaId,
                    ExternalId = f.ExternalId,
                    FormulaItems = f.ManufacturingFormulaMaterials
                        .Where(i => i.IsActive)
                        .OrderBy(i => i.LineNo)
                        .Select(i => new GetMfgProductionOrderFormulaItemsInfor
                        {
                            ManufacturingFormulaMaterialId = i.ManufacturingFormulaMaterialId,
                            ItemId = i.itemType == ItemType.Material
                                ? (i.MaterialId ?? Guid.Empty)
                                : (i.ProductId ?? Guid.Empty),
                            itemType = i.itemType,
                            CategoryId = i.CategoryId,

                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            TotalPrice = i.TotalPrice,

                            MaterialNameSnapshot = i.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = i.MaterialExternalIdSnapshot,
                            Unit = i.Unit,
                            IsActive = i.IsActive,
                            LineNo = i.LineNo,

                            OnHandKg = 0m,
                            ReservedOpenAllKg = 0m,
                            AvailableKg = 0m
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);
        }

        private async Task<GetMfgProductionOrderFormulaInfor?> GetVuFormulaInforAsync(Guid formulaId, CancellationToken ct)
        {
            return await _unitOfWork.FormulaRepository
                .Query(false)
                .Where(f => f.FormulaId == formulaId && f.IsActive)
                .Select(f => new GetMfgProductionOrderFormulaInfor
                {
                    ManufacturingFormulaId = f.FormulaId, // giữ tạm property cũ để không vỡ DTO
                    ExternalId = f.ExternalId,
                    FormulaItems = f.FormulaMaterials
                        .Where(i => i.IsActive)
                        .OrderBy(i => i.LineNo)
                        .Select(i => new GetMfgProductionOrderFormulaItemsInfor
                        {
                            ManufacturingFormulaMaterialId = i.FormulaMaterialId, // map sang field chung
                            ItemId = i.itemType == ItemType.Material
                                ? (i.MaterialId ?? Guid.Empty)
                                : (i.ProductId ?? Guid.Empty),
                            itemType = i.itemType,
                            CategoryId = i.CategoryId,

                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            TotalPrice = i.TotalPrice,

                            MaterialNameSnapshot = i.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = i.MaterialExternalIdSnapshot,
                            Unit = i.Unit,
                            IsActive = i.IsActive,
                            LineNo = i.LineNo,

                            OnHandKg = 0m,
                            ReservedOpenAllKg = 0m,
                            AvailableKg = 0m
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);
        }

        private static bool IsVuFormulaType(FormulaType? formulaType)
        {
            return formulaType == FormulaType.FromVu
                || formulaType == FormulaType.Improvement;
        }

        private static bool IsVaFormulaType(FormulaType? formulaType)
        {
            return formulaType == FormulaType.Standard
                || formulaType == FormulaType.ProductionOld
                || formulaType == FormulaType.Production;
        }


        //=========================================================== Get Information old ===========================================================
        /// <summary>
        /// Lấy dữ liệu chi tiết của 1 MFG Production Order.
        /// 
        /// Flow:
        /// 1. Lấy header của MPO
        /// 2. Resolve ManufacturingFormula đang select:
        ///    - Standard hiện hành
        ///    - nếu không có thì Select mới nhất
        /// 3. Nếu có formula thì load formula + items
        /// 4. Gắn tồn kho cho từng dòng
        /// </summary>
        public async Task<GetMfgProductionOrderInform?> GetByIdAsync(Guid mfgProductionOrderId)
        {
            var baseData = await _unitOfWork.MfgProductionOrderRepository
                .Query(false)
                .Where(x => x.MfgProductionOrderId == mfgProductionOrderId && x.IsActive)
                .Select(x => new
                {
                    x.MfgProductionOrderId,
                    x.ExternalId,

                    x.ProductId,
                    x.ProductExternalIdSnapshot,
                    x.ProductNameSnapshot,

                    x.CustomerId,
                    x.CustomerNameSnapshot,
                    x.CustomerExternalIdSnapshot,

                    x.FormulaId,
                    x.FormulaExternalIdSnapshot,

                    x.ManufacturingDate,
                    x.ExpectedDate,
                    x.RequiredDate,

                    x.TotalQuantityRequest,
                    x.TotalQuantity,
                    x.NumOfBatches,
                    x.UnitPriceAgreed,

                    x.Status,
                    x.LabNote,
                    x.Requirement,
                    x.PlpuNote,
                    x.BagType,
                    x.QcCheck,
                    x.StepOfProduct,

                    OrderLink = _unitOfWork.MfgOrderPORepository.Query(false)
                        .Where(link => link.MfgProductionOrderId == x.MfgProductionOrderId && link.IsActive)
                        .Select(link => new
                        {
                            MerchandiseOrderId = link.Detail.MerchandiseOrderId,
                            MerchandiseOrderExternalId = link.Detail.MerchandiseOrder.ExternalId
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (baseData == null)
                return null;

            var result = new GetMfgProductionOrderInform
            {
                MfgProductionOrderId = baseData.MfgProductionOrderId,
                ExternalId = baseData.ExternalId,

                MerchandiseOrderId = baseData.OrderLink?.MerchandiseOrderId ?? Guid.Empty,
                MerchandiseOrderExternalId = baseData.OrderLink?.MerchandiseOrderExternalId,

                CustomerNameSnapshot = baseData.CustomerNameSnapshot,
                CustomerExternalIdSnapshot = baseData.CustomerExternalIdSnapshot,

                ProductId = baseData.ProductId,
                ProductExternalIdSnapshot = baseData.ProductExternalIdSnapshot,
                ProductNameSnapshot = baseData.ProductNameSnapshot,

                FormulaCustomerSelect = baseData.FormulaId ?? Guid.Empty,
                FormulaCustomerExternalIdSelect = baseData.FormulaExternalIdSnapshot ?? string.Empty,

                ManufacturingDate = baseData.ManufacturingDate,
                ExpectedDate = baseData.ExpectedDate,
                RequiredDate = baseData.RequiredDate,

                TotalQuantityRequest = baseData.TotalQuantityRequest,
                TotalQuantity = baseData.TotalQuantity,
                NumOfBatches = baseData.NumOfBatches,
                UnitPriceAgreed = baseData.UnitPriceAgreed,

                Status = baseData.Status,
                LabNote = baseData.LabNote,
                Requirement = baseData.Requirement,
                PlpuNote = baseData.PlpuNote,
                BagType = baseData.BagType,
                QcCheck = baseData.QcCheck,
                StepOfProduct = baseData.StepOfProduct
            };

            // Ưu tiên 1: Standard hiện hành
            var currentStandard = await GetCurrentStandardAsync(baseData.ProductId);

            if (currentStandard is not null)
            {
                result.ManufacturingFormulaIdIsSelect = currentStandard.Value.ManufacturingFormulaId;
                result.ManufacturingFormulaExternalIdIsSelect = currentStandard.Value.ExternalId;

                result.GetMfgFormulaInform = await BuildFormulaInforAsync(currentStandard.Value.ManufacturingFormulaId);
                return result;
            }

            // Ưu tiên 2: Formula đang select gần nhất theo Product
            var currentSelect = await GetCurrentSelectByProductAsync(baseData.ProductId);

            if (currentSelect is not null)
            {
                result.ManufacturingFormulaIdIsSelect = currentSelect.Value.ManufacturingFormulaId;
                result.ManufacturingFormulaExternalIdIsSelect = currentSelect.Value.ExternalId;

                result.GetMfgFormulaInform = await BuildFormulaInforAsync(currentSelect.Value.ManufacturingFormulaId);
                return result;
            }

            // Không có ManufacturingFormula đang chọn
            result.ManufacturingFormulaIdIsSelect = null;
            result.ManufacturingFormulaExternalIdIsSelect = string.Empty;
            result.GetMfgFormulaInform = new GetMfgProductionOrderFormulaInfor();

            return result;
        }

        /// <summary>
        /// Lấy Standard hiện hành của Product.
        /// </summary>
        private async Task<(Guid ManufacturingFormulaId, Guid? MfgProductionOrderId, string? ExternalId)?> GetCurrentStandardAsync(Guid productId)
        {
            var current = await _unitOfWork.ProductStandardFormulaRepository
                .Query(false)
                .Where(x =>
                    x.ProductId == productId &&
                    x.ManufacturingFormulaId != null &&
                    x.ValidTo == null)
                .OrderByDescending(x => x.ValidFrom)
                .Select(x => new
                {
                    ManufacturingFormulaId = x.ManufacturingFormulaId!.Value,
                    ExternalId = x.ManufacturingFormula != null ? x.ManufacturingFormula.ExternalId : null,
                    MfgProductionOrderId = _unitOfWork.ProductionSelectVersionRepository
                        .Query(false)
                        .Where(sv => sv.ManufacturingFormulaId == x.ManufacturingFormulaId)
                        .OrderByDescending(sv => sv.ValidFrom)
                        .Select(sv => (Guid?)sv.MfgProductionOrderId)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (current == null) return null;

            return (current.ManufacturingFormulaId, current.MfgProductionOrderId, current.ExternalId);
        }

        /// <summary>
        /// Lấy formula select mới nhất theo Product.
        /// </summary>
        private async Task<(Guid ManufacturingFormulaId, Guid? MfgProductionOrderId, string? ExternalId)?> GetCurrentSelectByProductAsync(Guid productId)
        {
            var current = await _unitOfWork.ProductionSelectVersionRepository
                .Query(false)
                .Where(x =>
                    x.MfgProductionOrder != null &&
                    x.MfgProductionOrder.ProductId == productId &&
                    x.ManufacturingFormulaId != null)
                .OrderByDescending(x => x.ValidFrom)
                .Select(x => new
                {
                    ManufacturingFormulaId = x.ManufacturingFormulaId!.Value,
                    MfgProductionOrderId = (Guid?)x.MfgProductionOrderId,
                    ExternalId = x.ManufacturingFormula != null ? x.ManufacturingFormula.ExternalId : null
                })
                .FirstOrDefaultAsync();

            if (current == null) return null;

            return (current.ManufacturingFormulaId, current.MfgProductionOrderId, current.ExternalId);
        }

        /// <summary>
        /// Build DTO formula + items.
        /// </summary>
        private async Task<GetMfgProductionOrderFormulaInfor> BuildFormulaInforAsync(Guid manufacturingFormulaId)
        {
            var formula = await _unitOfWork.ManufacturingFormulaRepository
                .Query(false)
                .Where(x => x.ManufacturingFormulaId == manufacturingFormulaId && x.IsActive)
                .Select(x => new
                {
                    x.ManufacturingFormulaId,
                    x.ExternalId
                })
                .FirstOrDefaultAsync();

            if (formula == null)
                return new GetMfgProductionOrderFormulaInfor();

            var items = await _unitOfWork.ManufacturingFormulaMaterialRepository
                .Query(false)
                .Where(x => x.ManufacturingFormulaId == manufacturingFormulaId && x.IsActive)
                .OrderBy(x => x.LineNo)
                .Select(x => new GetMfgProductionOrderFormulaItemsInfor
                {
                    ManufacturingFormulaMaterialId = x.ManufacturingFormulaMaterialId,
                    ItemId = x.itemType == ItemType.Material
                        ? (x.MaterialId ?? Guid.Empty)
                        : (x.ProductId ?? Guid.Empty),
                    itemType = x.itemType,
                    CategoryId = x.CategoryId,

                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice,

                    MaterialNameSnapshot = x.MaterialNameSnapshot,
                    MaterialExternalIdSnapshot = x.MaterialExternalIdSnapshot,
                    Unit = x.Unit,
                    IsActive = x.IsActive,
                    LineNo = x.LineNo,

                    // Sẽ fill ở bước sau
                    OnHandKg = 0,
                    ReservedOpenAllKg = 0,
                    AvailableKg = 0
                })
                .ToListAsync();

            // Gắn tồn kho
            await FillAvailabilityAsync(items);

            return new GetMfgProductionOrderFormulaInfor
            {
                ManufacturingFormulaId = formula.ManufacturingFormulaId,
                ExternalId = formula.ExternalId,
                FormulaItems = items
            };
        }

        /// <summary>
        /// Gắn dữ liệu tồn kho vào từng item.
        /// 
        /// Gợi ý:
        /// - Nếu itemType = Material thì map theo MaterialExternalIdSnapshot
        /// - Nếu itemType = Product thì tùy rule nghiệp vụ, có thể để 0 hoặc xử lý riêng
        /// </summary>
        private async Task FillAvailabilityAsync(List<GetMfgProductionOrderFormulaItemsInfor> items)
        {
            if (items == null || items.Count == 0)
                return;

            var materialExternalIds = items
                .Where(x => x.itemType == ItemType.Material && !string.IsNullOrWhiteSpace(x.MaterialExternalIdSnapshot))
                .Select(x => x.MaterialExternalIdSnapshot!)
                .Distinct()
                .ToList();

            if (!materialExternalIds.Any())
                return;

            // TODO:
            // Thay đoạn này bằng nguồn tồn kho thực tế của bạn.
            // Ví dụ:
            // var availabilityDict = await _warehouseService.GetVaAvailabilityDictAsync(materialExternalIds);
            //
            // Dict mẫu:
            // key   = MaterialExternalIdSnapshot
            // value = new { OnHandKg, ReservedOpenAllKg, AvailableKg }

            var availabilityDict = await BuildAvailabilityDictAsync(materialExternalIds);

            foreach (var item in items)
            {
                if (item.itemType != ItemType.Material)
                    continue;

                if (string.IsNullOrWhiteSpace(item.MaterialExternalIdSnapshot))
                    continue;

                if (availabilityDict.TryGetValue(item.MaterialExternalIdSnapshot, out var stock))
                {
                    item.OnHandKg = stock.OnHandKg;
                    item.ReservedOpenAllKg = stock.ReservedOpenAllKg;
                    item.AvailableKg = stock.AvailableKg;
                }
            }
        }

        /// <summary>
        /// Hàm mock/tạm để build dict tồn kho.
        /// Bạn thay bằng query tồn kho thật trong hệ thống.
        /// </summary>
        private Task<Dictionary<string, AvailabilityVm>> BuildAvailabilityDictAsync(List<string> materialExternalIds)
        {
            var dict = materialExternalIds.ToDictionary(
                x => x,
                x => new AvailabilityVm
                {
                    OnHandKg = 0,
                    ReservedOpenAllKg = 0,
                    AvailableKg = 0
                });

            return Task.FromResult(dict);
        }

        private sealed class AvailabilityVm
        {
            public decimal OnHandKg { get; set; }
            public decimal? ReservedOpenAllKg { get; set; }
            public decimal? AvailableKg { get; set; }
        }
    }
}