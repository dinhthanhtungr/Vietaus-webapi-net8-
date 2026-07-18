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
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.ManufacturingFeature.Services
{
    /// <summary>
    /// Lay thông tin ghi chú cua MFG Production Order và thông tin cong thuc san xuat,
    /// bao gom danh sách item cong thuc và ton kho kha dung.
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
        /// Lay thông tin note cua MPO (nu có MfgProductionOrderId) và thông tin formula (nu có mfgFormulaId).
        /// MfgProductionOrderId có th null.
        /// </summary>
        public async Task<GetMfgProductionOrderNoteInfor> GetAsync(Guid? mfgProductionOrderId, Guid? formulaId, FormulaType? formulaType, CancellationToken ct = default)
        {
            GetMfgProductionOrderNoteInfor result;

            // 1) Lay note t MPO nu có mfgProductionOrderId
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

            // 2) Nu không có formula id thì tra luôn
            if (!formulaId.HasValue || formulaId.Value == Guid.Empty)
            {
                result.FormulaInfor = null;
                return result;
            }

            // 3) Quyt dnh dc VA hay VU theo FormulaType
            if (IsVuFormulaType(formulaType))
            {
                result.FormulaInfor = await GetVuFormulaInforAsync(formulaId.Value, ct);
            }
            else
            {
                result.FormulaInfor = await GetVaFormulaInforAsync(formulaId.Value, ct);
            }

            // 4) Nu không có formula hoc không có item thì tra luôn
            if (result.FormulaInfor == null || result.FormulaInfor.FormulaItems.Count == 0)
                return result;

            await FillLotNumbersAsync(result.FormulaInfor.FormulaItems, ct);

            // 5) Nu là VA thì map ton kho o
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
                    Note = f.Note,
                    FormulaItems = f.ManufacturingFormulaMaterials
                        .Where(i => i.IsActive)
                        .OrderBy(i => i.LineNo)
                        .Select(i => new GetMfgProductionOrderFormulaItemsInfor
                        {
                            ManufacturingFormulaMaterialId = i.ManufacturingFormulaMaterialId,
                            ItemId = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                                ? (i.MaterialId ?? Guid.Empty)
                                : (i.ProductId ?? Guid.Empty),
                            itemType = i.itemType,
                            CategoryId = i.CategoryId,

                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            TotalPrice = i.TotalPrice,

                            //MaterialNameSnapshot = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                            //     (i.Material != null  i.Material.Name : i.MaterialNameSnapshot)
                            //    : (i.Product != null  i.Product.Name : i.MaterialNameSnapshot),

                            //MaterialExternalIdSnapshot = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                            //     (i.Material != null  i.Material.ExternalId : i.MaterialNameSnapshot)
                            //    : (i.Product != null  i.Product.ColourCode : i.MaterialNameSnapshot),

                            MaterialNameSnapshot = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                                ? (i.Material != null ? i.Material.Name : i.MaterialNameSnapshot)
                                : (i.Product != null
                                    ? $"{i.Product.Name}"
                                    : i.MaterialNameSnapshot),

                            MaterialExternalIdSnapshot = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                                ? (i.Material != null ? i.Material.ExternalId : i.MaterialExternalIdSnapshot)
                                : (i.Product != null
                                    ? i.Product.SampleRequests
                                        .Where(sr => sr.IsActive)
                                        .OrderByDescending(sr => sr.CreatedDate)
                                        .Select(sr => sr.ExternalId)
                                        .FirstOrDefault()
                                    : i.MaterialExternalIdSnapshot),


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
                    ManufacturingFormulaId = f.FormulaId, // Giu tam property cu de khong vo DTO.
                    ExternalId = f.ExternalId,

                    Note = !string.IsNullOrWhiteSpace(f.Note)
                        ? f.Note
                        : f.Product.Requirement,

                    FormulaItems = f.FormulaMaterials
                        .Where(i => i.IsActive)
                        .OrderBy(i => i.LineNo)
                        .Select(i => new GetMfgProductionOrderFormulaItemsInfor
                        {
                            ManufacturingFormulaMaterialId = i.FormulaMaterialId, // map sang field chung
                            ItemId = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                                ? (i.MaterialId ?? Guid.Empty)
                                : (i.ProductId ?? Guid.Empty),
                            itemType = i.itemType,
                            CategoryId = i.CategoryId,

                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            TotalPrice = i.TotalPrice,

                            //MaterialNameSnapshot = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                            //     (i.Material != null  i.Material.Name : i.MaterialNameSnapshot)
                            //    : (i.Product != null  i.Product.Name : i.MaterialNameSnapshot),

                            //MaterialExternalIdSnapshot = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                            //     (i.Material != null  i.Material.ExternalId : i.MaterialNameSnapshot)
                            //    : (i.Product != null  i.Product.ColourCode : i.MaterialNameSnapshot),

                            MaterialNameSnapshot = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                                ? (i.Material != null ? i.Material.Name : i.MaterialNameSnapshot)
                                : (i.Product != null
                                    ? $"{i.Product.Name}"
                                    : i.MaterialNameSnapshot),

                            MaterialExternalIdSnapshot = (i.itemType == ItemType.Material || i.itemType == ItemType.MaterialFailure)
                                ? (i.Material != null ? i.Material.ExternalId : i.MaterialExternalIdSnapshot)
                                : (i.Product != null
                                    ? i.Product.SampleRequests
                                        .Where(sr => sr.IsActive)
                                        .OrderByDescending(sr => sr.CreatedDate)
                                        .Select(sr => sr.ExternalId)
                                        .FirstOrDefault()
                                    : i.MaterialExternalIdSnapshot),


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
        /// Lay du lieu chi tit cua 1 MFG Production Order.
        /// 
        /// Flow:
        /// 1. Lay header cua MPO
        /// 2. Resolve ManufacturingFormula dang select:
        ///    - Standard hin hành
        ///    - nu không có thì Select moi nht
        /// 3. Nu có formula thì load formula + items
        /// 4. Gn ton kho cho tng dòng
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
                            MerchandiseOrderDetailId = link.MerchandiseOrderDetailId,
                            MerchandiseOrderId = link.Detail.MerchandiseOrderId,
                            MerchandiseOrderExternalId = link.Detail.MerchandiseOrder.ExternalId,
                            PONo = link.Detail.MerchandiseOrder.PONo

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
                MerchandiseOrderDetailId = baseData.OrderLink?.MerchandiseOrderDetailId ?? Guid.Empty,
                MerchandiseOrderExternalId = baseData.OrderLink?.MerchandiseOrderExternalId,
                PONo = baseData.OrderLink?.PONo,

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

            // Ch ly formula dang hieu luc theo chính MPO này
            var currentFormula = await GetCurrentFormulaByMpoAsync(baseData.MfgProductionOrderId);

            if (currentFormula is not null)
            {
                result.ManufacturingFormulaIdIsSelect = currentFormula.Value.ManufacturingFormulaId;
                result.ManufacturingFormulaExternalIdIsSelect = currentFormula.Value.ExternalId;
                result.GetMfgFormulaInform = await BuildFormulaInforAsync(currentFormula.Value.ManufacturingFormulaId);
            }
            else
            {
                result.ManufacturingFormulaIdIsSelect = null;
                result.ManufacturingFormulaExternalIdIsSelect = string.Empty;
                result.GetMfgFormulaInform = new GetMfgProductionOrderFormulaInfor();
            }

            return result;
        }
        private async Task<(Guid ManufacturingFormulaId, string? ExternalId)?> GetCurrentFormulaByMpoAsync(Guid mfgProductionOrderId)
        {
            var current = await (
                from v in _unitOfWork.ProductionSelectVersionRepository.Query(false)
                join mf in _unitOfWork.ManufacturingFormulaRepository.Query(false)
                    on v.ManufacturingFormulaId equals mf.ManufacturingFormulaId
                where v.MfgProductionOrderId == mfgProductionOrderId
                      && v.ValidTo == null
                      && v.ManufacturingFormulaId != null
                      && mf.IsActive
                orderby v.ValidFrom descending
                select new
                {
                    ManufacturingFormulaId = v.ManufacturingFormulaId!.Value,
                    ExternalId = mf.ExternalId
                }
            ).FirstOrDefaultAsync();

            if (current == null)
                return null;

            return (current.ManufacturingFormulaId, current.ExternalId);
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
                    x.ExternalId,
                    x.Note
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
                    ItemId = (x.itemType == ItemType.Material || x.itemType == ItemType.MaterialFailure)
                        ? (x.MaterialId ?? Guid.Empty)
                        : (x.ProductId ?? Guid.Empty),
                    itemType = x.itemType,
                    CategoryId = x.CategoryId,

                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice,

                    //MaterialNameSnapshot = (x.itemType == ItemType.Material || x.itemType == ItemType.MaterialFailure)
                    //             (x.Material != null  x.Material.Name : x.MaterialNameSnapshot)
                    //            : (x.Product != null  x.Product.Name : x.MaterialNameSnapshot),

                    //MaterialExternalIdSnapshot = (x.itemType == ItemType.Material || x.itemType == ItemType.MaterialFailure)
                    //             (x.Material != null  x.Material.ExternalId : x.MaterialNameSnapshot)
                    //            : (x.Product != null  x.Product.ColourCode : x.MaterialNameSnapshot),

                    MaterialNameSnapshot = (x.itemType == ItemType.Material || x.itemType == ItemType.MaterialFailure)
                        ? (x.Material != null ? x.Material.Name : x.MaterialNameSnapshot)
                        : (x.Product != null
                            ? $"{x.Product.Name}"
                            : x.MaterialNameSnapshot),
                    MaterialExternalIdSnapshot = (x.itemType == ItemType.Material || x.itemType == ItemType.MaterialFailure)
                        ? (x.Material != null ? x.Material.ExternalId : x.MaterialExternalIdSnapshot)
                        : (x.Product != null
                            ? x.Product.SampleRequests
                                .Where(sr => sr.IsActive)
                                .OrderByDescending(sr => sr.CreatedDate)
                                .Select(sr => sr.ExternalId)
                                .FirstOrDefault()
                            : x.MaterialExternalIdSnapshot),

                    LotNo = x.LotNo,
                    Unit = x.Unit,
                    IsActive = x.IsActive,
                    LineNo = x.LineNo,

                    // S fill  buc sau
                    OnHandKg = 0,
                    ReservedOpenAllKg = 0,
                    AvailableKg = 0
                })
                .ToListAsync();

            // Gan ton kho
            await FillAvailabilityAsync(manufacturingFormulaId, items);
            await FillLotNumbersAsync(items);

            return new GetMfgProductionOrderFormulaInfor
            {
                ManufacturingFormulaId = formula.ManufacturingFormulaId,
                ExternalId = formula.ExternalId,
                Note = formula.Note,
                FormulaItems = items
            };
        }

        /// <summary>
        /// Gan du lieu ton kho vao tung item theo manufacturingFormulaId.
        /// Chi ap dung cho itemType = Material.
        /// </summary>
        private async Task FillAvailabilityAsync(Guid manufacturingFormulaId, List<GetMfgProductionOrderFormulaItemsInfor> items, CancellationToken ct = default)
        {
            if (items == null || items.Count == 0)
                return;

            var availabilityDict = await _warehouseReadService.GetVaAvailabilityDictAsync(manufacturingFormulaId, ct);

            if (availabilityDict == null || availabilityDict.Count == 0)
                return;

            foreach (var item in items)
            {
                if (item.itemType != ItemType.Material && item.itemType != ItemType.MaterialFailure)
                    continue;

                if (string.IsNullOrWhiteSpace(item.MaterialExternalIdSnapshot))
                    continue;

                var code = item.MaterialExternalIdSnapshot.Trim().ToUpperInvariant();

                if (availabilityDict.TryGetValue(code, out var stock))
                {
                    item.OnHandKg = stock.OnHandKg;
                    item.ReservedOpenAllKg = stock.ReservedOpenAllKg;
                    item.AvailableKg = stock.AvailableKg;
                }
                else
                {
                    item.OnHandKg = 0;
                    item.ReservedOpenAllKg = 0;
                    item.AvailableKg = 0;
                }
            }
        }

        /// <summary>
        /// Gn danh sách lotNo còn tn trong kho vào tng item cong thuc d nguoi dùng chon.
        /// Áp dung cho c nguyen vat lieu và thanh pham theo MaterialExternalIdSnapshot.
        /// </summary>
        private async Task FillLotNumbersAsync(List<GetMfgProductionOrderFormulaItemsInfor> items, CancellationToken ct = default)
        {
            if (items == null || items.Count == 0)
                return;

            var productIds = items
                .Where(x => (x.itemType == ItemType.Product || x.itemType == ItemType.ProductFailure) && x.ItemId != Guid.Empty)
                .Select(x => x.ItemId)
                .Distinct()
                .ToList();

            var productCodeMap = productIds.Count == 0
                ? new Dictionary<Guid, string>()
                : await _unitOfWork.ProductRepository.Query(false)
                    .Where(x => productIds.Contains(x.ProductId))
                    .Select(x => new
                    {
                        x.ProductId,
                        Code = x.ColourCode ?? x.Code ?? string.Empty
                    })
                    .ToDictionaryAsync(x => x.ProductId, x => x.Code, ct);

            var itemCodes = items
                .Select(x => ResolveFormulaItemLotCode(x, productCodeMap))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.Trim())
                .Distinct()
                .ToList();

            if (itemCodes.Count == 0)
            {
                foreach (var item in items)
                {
                    item.LotNumber = MovePersistedLotNoToFirst(item.LotNo, new List<LotNumberOptionDto>());
                }

                return;
            }

            var lotNoListMap = await _warehouseReadService.GetLotNoListMapByCodesAsync(itemCodes, ct);

            foreach (var item in items)
            {
                var itemCode = ResolveFormulaItemLotCode(item, productCodeMap);
                if (string.IsNullOrWhiteSpace(itemCode))
                {
                    item.LotNumber = MovePersistedLotNoToFirst(item.LotNo, new List<LotNumberOptionDto>());
                    continue;
                }

                var itemCodeKey = itemCode.Trim().ToUpperInvariant();
                var lotNumbers = lotNoListMap.TryGetValue(itemCodeKey, out var warehouseLotNumbers)
                    ? warehouseLotNumbers
                    : new List<LotNumberOptionDto>();

                item.LotNumber = MovePersistedLotNoToFirst(item.LotNo, lotNumbers);
            }
        }

        /// <summary>
        /// Uư tiên giữ lại số lot đã lưu trong công thức nếu có, dù số lot đó có còn tồn tại trong kho hay không.
        /// Neu cong thuc chua luu lot thi giu thu tu lot trong kho de FE mac dinh chon lot uu tien dau tien.
        /// Neu khong co lot trong kho thi chen N/A de FE co lua chon rong ro nghia.
        /// </summary>
        private static List<LotNumberOptionDto> MovePersistedLotNoToFirst(string? persistedLotNo, List<LotNumberOptionDto> lotNumbers)
        {
            var result = lotNumbers
                .Where(x => !string.IsNullOrWhiteSpace(x.LotNo))
                .Select(x => new LotNumberOptionDto
                {
                    LotNo = x.LotNo,
                    StockType = x.StockType,
                    QualityStatus = x.QualityStatus,
                    QualityStatusName = x.QualityStatusName,
                    IsDefective = x.IsDefective,
                    QuantityKg = x.QuantityKg,
                    Bags = x.Bags
                })
                .ToList();

            if (string.IsNullOrWhiteSpace(persistedLotNo))
            {
                if (result.Count > 0)
                    return result;

                result.Add(CreatePersistedLotNoOption("N/A"));
                return result;
            }

            var selectedLotNo = persistedLotNo.Trim();

            var existingIndex = result.FindIndex(x =>
                string.Equals(x.LotNo?.Trim(), selectedLotNo, StringComparison.OrdinalIgnoreCase));

            if (existingIndex >= 0)
            {
                var selected = result[existingIndex];
                result.RemoveAt(existingIndex);
                result.Insert(0, selected);
                return result;
            }

            result.Insert(0, CreatePersistedLotNoOption(selectedLotNo));
            return result;
        }

        /// <summary>
        /// Tao option dai dien cho so lot dã luu trong cong thuc nhung không có trong danh sách ton kho hien tai.
        /// </summary>
        private static LotNumberOptionDto CreatePersistedLotNoOption(string lotNo)
        {
            return new LotNumberOptionDto
            {
                LotNo = lotNo,
                StockType = StockType.Other,
                QualityStatus = "Unknown",
                QualityStatusName = lotNo == "N/A" ? "Chua chon lot" : "Không có trong kho hien tai",
                IsDefective = false,
                QuantityKg = 0m,
                Bags = 0
            };
        }

        /// <summary>
        /// Xac dinh mã dùng d do lot trong kho cho mot item cong thuc.
        /// Nguyên vt liu dùng mã snapshot, thanh pham uu tiên Product.ColourCode vì kho luu thanh pham theo ma mau.
        /// </summary>
        private static string? ResolveFormulaItemLotCode(
            GetMfgProductionOrderFormulaItemsInfor item,
            IReadOnlyDictionary<Guid, string> productCodeMap)
        {
            if ((item.itemType == ItemType.Product || item.itemType == ItemType.ProductFailure) &&
                item.ItemId != Guid.Empty &&
                productCodeMap.TryGetValue(item.ItemId, out var productCode) &&
                !string.IsNullOrWhiteSpace(productCode))
            {
                return productCode;
            }

            return item.MaterialExternalIdSnapshot;
        }
    }
}
