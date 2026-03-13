using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.CompareFormulaDTOs;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.FormulaVersion;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.Services;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Helper.PriceHelpers;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Domain.Enums.Category;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public class MfgFormulaService : IMfgFormulaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExternalIdService _externalId;
        private readonly ICurrentUser _currentUser;
        private readonly IWarehouseReadService _warehouseReadService;
        private readonly ITimelineService _TimelineService;


        private readonly INotificationService _notificationService;
        private readonly IPriceProvider _priceProvider;
        private readonly IFormulaPDF _FormulaPDF;

        public MfgFormulaService(
                                IUnitOfWork unitOfWork, 
                                IExternalIdService externalId, 
                                ICurrentUser currentUser, 
                                ITimelineService timelineService, 
                                IWarehouseReadService warehouseReadService,
                                INotificationService notificationService,
                                IPriceProvider priceProvider,
                                IFormulaPDF formulaPDF)
        {
            _unitOfWork = unitOfWork;
            _externalId = externalId;
            _currentUser = currentUser;
            _TimelineService = timelineService;
            _FormulaPDF = formulaPDF;
            _warehouseReadService = warehouseReadService;

            _notificationService = notificationService;
            _priceProvider = priceProvider;

        }

        // ======================================================================== Get ======================================================================== 

        /// <summary>
        /// Lấy thông tin công thức sản xuất theo Id lấy theo Vu nếu là công thức mới tanh và lấy theo VA nếu đã có công thức chuẩn trước đó. Lấy Id này khi trang ở New hoặc Clone
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<GetManufacturingFormula>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return OperationResult<GetManufacturingFormula>.Fail("Id không hợp lệ.");

            // 1) lấy MPO cần tìm
            var mpo = await _unitOfWork.MfgOrderPORepository.Query()
                .Where(f => f.MfgProductionOrderId == id && f.IsActive == true)
                .Select(x => new
                {
                    x.MfgProductionOrderId,
                    x.ProductionOrder.ExternalId,
                    x.ProductionOrder.ProductId,
                    ProductNameSnapshot = x.ProductionOrder.Product.Name,
                    ProductExternalIdSnapshot = x.ProductionOrder.Product.ColourCode,
                    IsRecycle = x.ProductionOrder.Product.IsRecycle,
                    x.ProductionOrder.CustomerNameSnapshot,
                    x.ProductionOrder.TotalQuantityRequest,

                    VuChosenId = x.ProductionOrder.FormulaId,
                    VuChosenExternalIdSnapshot = x.ProductionOrder.FormulaExternalIdSnapshot,

                    x.Detail.TotalPriceAgreed,
                    x.ProductionOrder.CompanyId
                })
                .FirstOrDefaultAsync(ct);

            if (mpo == null)
                return OperationResult<GetManufacturingFormula>.Fail("Không tìm thấy công thức sản xuất.");

            var now = DateTime.Now;

            // 2) Lấy công thức sản xuất được chọn là chuẩn trước đó (nếu có)
            var stdPsf = await _unitOfWork.ProductStandardFormulaRepository.Query()
                .Where(s => s.ProductId == mpo.ProductId
                          && s.CompanyId == mpo.CompanyId
                          && s.ValidFrom <= now
                          && (s.ValidTo == null || s.ValidTo > now))

                .OrderByDescending(s => s.ValidFrom)
                .Select(s => s.ManufacturingFormulaId)
                .FirstOrDefaultAsync(ct);

            Guid? vaId = null;
            Guid? vaSourceVuId = null;
            string? nameVa = string.Empty;
            string? vaCode = null;
            DateTime dateTime = DateTime.MinValue;
            string? vaSourceVuCode = null;
            string? note = null;

            if (stdPsf != Guid.Empty)
            {
                var vaInfo = await _unitOfWork.ManufacturingFormulaRepository.Query()
                    .Where(mf => mf.ManufacturingFormulaId == stdPsf)
                    .Select(mf => new
                    {
                        mf.Status,
                        mf.ManufacturingFormulaId,
                        mf.ExternalId,
                        mf.SourceVUFormulaId,
                        mf.SourceVUExternalIdSnapshot,
                        mf.Name,
                        mf.Note,
                        mf.CreatedDate
                    })
                    .FirstOrDefaultAsync(ct);

                if (vaInfo != null)
                {
                    vaId = vaInfo.ManufacturingFormulaId;
                    vaCode = vaInfo.ExternalId;
                    vaSourceVuId = vaInfo.SourceVUFormulaId;
                    vaSourceVuCode = vaInfo.SourceVUExternalIdSnapshot;
                    nameVa = vaInfo.Name;
                    dateTime = vaInfo.CreatedDate;
                    note = vaInfo.Note;
                }
            }

            // 3) Quyết định dùng VA hay VU
            var useVA =
                vaId.HasValue &&                      // có VA chuẩn
                (
                    mpo.VuChosenId == null            // không có VU sales chọn
                    || (vaSourceVuId.HasValue && mpo.VuChosenId == vaSourceVuId) // VU trùng
                );

            // 4) Lấy vật tư & build DTO
            if (useVA)
            {
                // Vật tư theo VA
                var materials = await _unitOfWork.ManufacturingFormulaMaterialRepository.Query(false)
                    .Where(m => m.ManufacturingFormulaId == vaId!.Value)
                        .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                        .ThenBy(x => x.MaterialExternalIdSnapshot)
                    .Select(m => new GetManufacturingFormulaMaterial
                    {
                        itemType = m.itemType,
                        ItemId = m.itemType == ItemType.Material
                                    ? m.MaterialId ?? Guid.Empty
                                    : m.ProductId ?? Guid.Empty,
                        CategoryId = m.CategoryId,
                        Quantity = m.Quantity,
                        Unit = m.Unit ?? "",
                        UnitPrice = m.UnitPrice,
                        TotalPrice = m.TotalPrice,
                        MaterialNameSnapshot = m.MaterialNameSnapshot ?? "",
                        MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot ?? ""
                    })
                    .ToListAsync(ct);

                var result = new GetManufacturingFormula
                {
                    ManufacturingFormulaId = Guid.Empty, // chưa có VA tương ứng
                    ExternalId = string.Empty, 

                    mfgProductionOrderId = mpo.MfgProductionOrderId,
                    MfgProductionOrderExternalId = mpo.ExternalId,
                    VUFormulaName = nameVa, // hoặc tên của VA nếu bạn muốn hiển thị Name

                    ProductId = mpo.ProductId,
                    ProductNameSnapshot = mpo.ProductNameSnapshot,
                    ProductExternalIdSnapshot = mpo.ProductExternalIdSnapshot,
                    IsRecycle = mpo.IsRecycle,
                    CustomerNameSnapshot = mpo.CustomerNameSnapshot,

                    SaleTotalPrice = mpo.TotalPriceAgreed,
                    TotalQuantityRequest = mpo.TotalQuantityRequest,

                    VUFormulaId = vaSourceVuId, // VU gốc của VA
                    FormulaExternalIdSnapshot = vaSourceVuCode,
                    FormulaSourceIdCreatedDate = dateTime,

                    Note = note,

                    SourceType = FormulaSource.FromVA,
                    FormulaSourceId = vaId!.Value,
                    FormulaSourceExternalIdSnapshot = vaCode ?? "",
                    IsSelect = true,
                    IsStandard = false,
                    ManufacturingFormulaMaterials = materials
                };


                return OperationResult<GetManufacturingFormula>.Ok(result, "Tạo công thức mới thành công");
            }
            else if (mpo.VuChosenId.HasValue)
            {
                // Lấy tên/mã VU để show
                var vuInfo = await _unitOfWork.FormulaRepository.Query(false)
                    .Where(f => f.FormulaId == mpo.VuChosenId.Value)
                    .Select(f => new { f.Name, f.ExternalId, f.CreatedDate })
                    .FirstOrDefaultAsync(ct);

                // Vật tư theo VU
                var materials = await _unitOfWork.FormulaMaterialRepository.Query(false)
                    .Where(fm => fm.FormulaId == mpo.VuChosenId.Value)
                        .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                        .ThenBy(x => x.MaterialExternalIdSnapshot)
                    .Select(fm => new GetManufacturingFormulaMaterial
                    {
                        itemType = fm.itemType,
                        ItemId = fm.itemType == ItemType.Material
                                    ? fm.MaterialId ?? Guid.Empty
                                    : fm.ProductId ?? Guid.Empty,
                        CategoryId = fm.CategoryId,
                        Quantity = fm.Quantity,
                        Unit = fm.Unit ?? "",
                        UnitPrice = fm.UnitPrice,
                        TotalPrice = fm.UnitPrice * fm.Quantity, // nếu bạn đã có TotalPrice thì dùng field đó
                        MaterialNameSnapshot = fm.MaterialNameSnapshot ?? "",
                        MaterialExternalIdSnapshot = fm.MaterialExternalIdSnapshot ?? ""
                    })
                    .ToListAsync(ct);

                var result =  new GetManufacturingFormula
                {
                    ManufacturingFormulaId = Guid.Empty, // chưa có VA tương ứng
                    ExternalId = string.Empty,

                    mfgProductionOrderId = mpo.MfgProductionOrderId,
                    MfgProductionOrderExternalId = mpo.ExternalId,
                    VersionName = string.Empty,

                    VUFormulaName = vuInfo?.Name ?? string.Empty,

                    ProductId = mpo.ProductId,
                    ProductNameSnapshot = mpo.ProductNameSnapshot,
                    ProductExternalIdSnapshot = mpo.ProductExternalIdSnapshot,
                    IsRecycle = mpo.IsRecycle,
                    CustomerNameSnapshot = mpo.CustomerNameSnapshot,

                    SaleTotalPrice = mpo.TotalPriceAgreed,
                    TotalQuantityRequest = mpo.TotalQuantityRequest,

                    VUFormulaId = mpo.VuChosenId,
                    FormulaExternalIdSnapshot = vuInfo?.ExternalId ?? mpo.VuChosenExternalIdSnapshot,
                    FormulaSourceIdCreatedDate = vuInfo?.CreatedDate ?? DateTime.MinValue, 

                    SourceType = FormulaSource.FromVU,
                    FormulaSourceId = mpo.VuChosenId.Value,
                    FormulaSourceExternalIdSnapshot = vuInfo?.ExternalId ?? mpo.VuChosenExternalIdSnapshot ?? "",
                    IsSelect = true,
                    IsStandard = false,
                    ManufacturingFormulaMaterials = materials
                };

                return OperationResult<GetManufacturingFormula>.Ok(result, "Tạo công thức mới thành công");
            }

            return OperationResult<GetManufacturingFormula>.Fail("Lấy thông tin thất bại");
        }
       
        /// <summary>
        /// Lấy thông tin công thức sản xuất theo Id. Lấy Id này khi trang ở View
        /// </summary>
        /// <param name="MfgOrderId">Truyền Id của lệnh sản xuất</param>
        /// <param name="MfgFormulaId">Truyền Id của công thức sản xuất</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<GetManufacturingFormula>> GetByIdForViewAsync(Guid MfgOrderId, Guid MfgFormulaId, CancellationToken ct = default)
        {
            if (MfgOrderId == Guid.Empty || MfgFormulaId == Guid.Empty)
                return OperationResult<GetManufacturingFormula>.Fail("Id không hợp lệ.");

            // 1) lấy MPO cần tìm
            var mpo = await _unitOfWork.MfgOrderPORepository.Query()
                .Where(f => f.MfgProductionOrderId == MfgOrderId && f.IsActive == true)
                .Select(x => new
                {
                    x.MfgProductionOrderId,
                    x.ProductionOrder.ExternalId,
                    x.ProductionOrder.ProductId,
                    ProductNameSnapshot = x.ProductionOrder.Product.Name,
                    ProductExternalIdSnapshot = x.ProductionOrder.Product.ColourCode,

                    IsRecycle = x.ProductionOrder.Product.IsRecycle,
                    x.ProductionOrder.CustomerNameSnapshot,
                    MerchadiseOrderExternalId = x.Detail.MerchandiseOrder.ExternalId,
                    x.ProductionOrder.TotalQuantityRequest,
                    x.ProductionOrder.Status,

                    VuChosenId = x.ProductionOrder.FormulaId,
                    VuChosenExternalIdSnapshot = x.ProductionOrder.FormulaExternalIdSnapshot,

                    x.Detail.UnitPriceAgreed,
                    x.ProductionOrder.CompanyId
                })
                .FirstOrDefaultAsync(ct);


            if (mpo == null)
                return OperationResult<GetManufacturingFormula>.Fail("Không tìm thấy công thức sản xuất.");

            var mfgo = await _unitOfWork.ManufacturingFormulaRepository.Query()
                .Where(f => f.ManufacturingFormulaId == MfgFormulaId && f.IsActive == true)
                .Select(f => new
                {
                    f.ManufacturingFormulaId,
                    f.ExternalId,
                    f.Name,
                    f.Status,
                    f.SourceType,
                    f.SourceManufacturingFormulaId,
                    f.SourceManufacturingExternalIdSnapshot,
                    f.SourceVUFormulaId,
                    f.SourceVUExternalIdSnapshot,
                    f.Note,
                    f.ProductionSelectVersions,
                    f.ProductStandardFormulas,
                    f.CreatedDate,
                })
                .FirstOrDefaultAsync(ct);

            if (mfgo == null)
                return OperationResult<GetManufacturingFormula>.Fail("Không tìm thấy công thức sản xuất.");

            var materials = await _unitOfWork.ManufacturingFormulaMaterialRepository.Query()
                .Where(m => m.ManufacturingFormulaId == mfgo.ManufacturingFormulaId && m.IsActive == true)
                    .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                    .ThenBy(x => x.MaterialExternalIdSnapshot)
                .Select(m => new GetManufacturingFormulaMaterial
                {
                    ManufacturingFormulaMaterialId = m.ManufacturingFormulaMaterialId,

                    itemType = m.itemType,
                    ItemId = m.itemType == ItemType.Material ? m.MaterialId ?? Guid.Empty : m.ProductId ?? Guid.Empty,

                    CategoryId = m.CategoryId,
                    Quantity = m.Quantity,
                    Unit = m.Unit ?? "",
                    UnitPrice = m.UnitPrice,
                    TotalPrice = m.TotalPrice,
                    MaterialNameSnapshot = m.MaterialNameSnapshot ?? "",
                    MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot ?? ""
                })
                .ToListAsync(ct);

            var result = new GetManufacturingFormula
            {
                ManufacturingFormulaId = mfgo != null ? mfgo.ManufacturingFormulaId : Guid.Empty,
                ExternalId = mfgo != null ? mfgo.ExternalId : string.Empty,
                status = mfgo != null ? mfgo.Status : string.Empty,

                mfgProductionOrderId = mpo.MfgProductionOrderId,
                MfgProductionOrderExternalId = mpo.ExternalId,
                VersionName = mfgo != null ? mfgo.Name : string.Empty,

                ProductId = mpo.ProductId,
                ProductNameSnapshot = mpo.ProductNameSnapshot,
                ProductExternalIdSnapshot = mpo.ProductExternalIdSnapshot,
                IsRecycle = mpo.IsRecycle,

                MerchadiseOrderExternalId = mpo.MerchadiseOrderExternalId,
                CustomerNameSnapshot = mpo.CustomerNameSnapshot,

                SaleTotalPrice = mpo.UnitPriceAgreed,
                TotalQuantityRequest = mpo.TotalQuantityRequest,

                VUFormulaId = mpo.VuChosenId,
                VUFormulaName = string.Empty,
                FormulaExternalIdSnapshot = mfgo != null ? mfgo.SourceVUExternalIdSnapshot : string.Empty,

                SourceType = mfgo != null ? mfgo.SourceType : null,

                FormulaSourceId = mfgo != null
                                ? (mfgo.SourceManufacturingFormulaId ?? mfgo.SourceVUFormulaId) ?? Guid.Empty
                                : Guid.Empty,
                FormulaSourceExternalIdSnapshot = mfgo != null
                                ? (mfgo.SourceManufacturingExternalIdSnapshot ?? mfgo.SourceVUExternalIdSnapshot) ?? string.Empty
                                : string.Empty,
                FormulaSourceIdCreatedDate = mfgo.CreatedDate,

                IsSelect = mfgo != null ? mfgo.ProductionSelectVersions.Any(psv => psv.MfgProductionOrderId == mpo.MfgProductionOrderId && (psv.ValidTo == null)) : false,
                IsStandard = mfgo != null ? mfgo.ProductStandardFormulas.Any(psf => psf.ProductId == mpo.ProductId && (psf.ValidTo == null)) : false,
                Note = mfgo != null ? mfgo.Note : string.Empty,

                ManufacturingFormulaMaterials = materials

            };

            return OperationResult<GetManufacturingFormula>.Ok(result, "Tạo công thức mới thành công");
        }

        /// <summary>
        /// Lấy danh sách nguyên vật liệu của công thức sản xuất theo Id công thức kèm thông tin có trong kho
        /// </summary>
        /// <param name="MfgFormulaId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<GetSampleManufacturingFormula>> GetMaterialsByFormulaIdAsync(Guid MfgFormulaId, CancellationToken ct = default)
        {
            // 0) Lấy thông tin công thức
            var mfgo = await _unitOfWork.ManufacturingFormulaRepository.Query()
                .Where(f => f.ManufacturingFormulaId == MfgFormulaId && f.IsActive == true)
                .Select(f => new
                {
                    f.ManufacturingFormulaId,
                    f.ExternalId,
                    f.Name,
                    f.Status,
                    f.Note,
                })
                .FirstOrDefaultAsync(ct);

            // 1) Lấy chi tiết công thức
            var mfmQuery = _unitOfWork.ManufacturingFormulaMaterialRepository.Query()
                .Where(x => x.ManufacturingFormulaId == MfgFormulaId
                         && x.IsActive);

            var materials = await mfmQuery
                .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                .ThenBy(x => x.MaterialExternalIdSnapshot)
                .Select(x => new GetManufacturingFormulaMaterial
                {
                    ManufacturingFormulaMaterialId = x.ManufacturingFormulaMaterialId,
                    ManufacturingFormulaId = x.ManufacturingFormulaId,
                    ItemId = x.itemType == ItemType.Material
                                    ? x.MaterialId ?? Guid.Empty
                                    : x.ProductId ?? Guid.Empty,
                    CategoryId = x.CategoryId,

                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice,

                    MaterialNameSnapshot = x.MaterialNameSnapshot,
                    MaterialExternalIdSnapshot = x.MaterialExternalIdSnapshot,
                    Unit = x.Unit,
                    IsActive = x.IsActive
                })
                .ToListAsync(ct);

            var result = new GetSampleManufacturingFormula
            {
                ManufacturingFormulaId = mfgo != null ? mfgo.ManufacturingFormulaId : Guid.Empty,
                ExternalId = mfgo != null ? mfgo.ExternalId : string.Empty,
                VersionName = mfgo != null ? mfgo.Name : string.Empty,
                status = mfgo != null ? mfgo.Status : string.Empty,
                Note = mfgo != null ? mfgo.Note : string.Empty,
                ManufacturingFormulaMaterials = materials
            };

            if (materials.Count == 0)
                return OperationResult<GetSampleManufacturingFormula>.Ok(result);

            // 2) Lấy tồn kho ảo theo formulaId
            var avaDict = await _warehouseReadService.GetVaAvailabilityDictAsync(MfgFormulaId, ct);

            // 3) Map tồn kho vào từng dòng NVL
            foreach (var row in materials)
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

            return OperationResult<GetSampleManufacturingFormula>.Ok(result);
        }

        /// <summary>
        /// Lấy tất cả công thức sản xuất chuẩn (hiện tại cũng như trong quá khứ) hoặc theo công thức VU theo điều kiện lọc và phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<OperationResult<PagedResult<GetSummaryMfgFormula>>> GetAllAsync(MfgFormulaQuery query, CancellationToken ct)
        {
            try
            {
                if (query.ProductId == Guid.Empty)
                    return OperationResult<PagedResult<GetSummaryMfgFormula>>.Fail("Id công thức không hợp lệ.");

                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var msp = _unitOfWork.MaterialsSupplierRepository.Query();

                // ✅ ĐỔI: lấy VA theo ProductionSelectVersion (theo lệnh sản xuất)
                var psv = _unitOfWork.ProductionSelectVersionRepository.Query();          // chọn version sản xuất (VA) theo MPO
                var mfr = _unitOfWork.ManufacturingFormulaRepository.Query();             // công thức sản xuất
                var fr = _unitOfWork.FormulaRepository.Query();                          // công thức nghiên cứu (VU)
                var mpo = _unitOfWork.MfgProductionOrderRepository.Query();


                var qva =
                    from s in psv
                    join o in mpo on s.MfgProductionOrderId equals o.MfgProductionOrderId
                    where o.ProductId == query.ProductId
                          && s.ManufacturingFormulaId != null

                    // Giữ lại duy nhất bản ProductionSelectVersion mới nhất cho từng ManufacturingFormula
                    let isLatestForThisFormula =
                        !psv.Any(s2 =>
                            s2.MfgProductionOrderId == s.MfgProductionOrderId
                            && s2.ManufacturingFormulaId == s.ManufacturingFormulaId
                            && (
                                s2.ValidFrom > s.ValidFrom
                                || (s2.ValidFrom == s.ValidFrom
                                    && s2.ProductionSelectVersionId.CompareTo(s.ProductionSelectVersionId) > 0)
                            ))

                    where isLatestForThisFormula

                    // Join sang bảng công thức sản xuất
                    join f in mfr
                        on s.ManufacturingFormulaId equals f.ManufacturingFormulaId

                    // Ưu tiên công thức hiện hành (ValidTo == null),
                    // sau đó các bản cũ, mới nhất trước
                    orderby s.ValidTo == null descending,
                            s.ValidFrom descending,
                            s.ProductionSelectVersionId descending

                    select new GetSummaryMfgFormula
                    {
                        ManufacturingFormulaId = f.ManufacturingFormulaId,
                        ExternalId = f.ExternalId,
                        Name = f.Name,
                        FormulaSourceIdCreatedDate = f.CreatedDate,

                        // TotalPrice = tổng (Quantity * UnitPrice) theo GIÁ MỚI NHẤT
                        TotalPrice = f.ManufacturingFormulaMaterials
                            .Select(m =>
                                (
                                    (
                                        from ms in msp
                                        where ms.MaterialId == m.MaterialId
                                        orderby (ms.UpdatedDate ?? ms.CreateDate) descending
                                        select (decimal?)ms.CurrentPrice
                                    ).FirstOrDefault() ?? m.UnitPrice
                                ) * m.Quantity
                            )
                            .Sum(),

                        ManufacturingFormulaMaterials = f.ManufacturingFormulaMaterials
                            .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                                .ThenBy(x => x.MaterialExternalIdSnapshot)
                            .Select(m => new GetSampleMfgFormulaMaterial
                            {
                                itemType = m.itemType,
                                ItemId = m.itemType == ItemType.Material
                                    ? m.MaterialId ?? Guid.Empty
                                    : m.ProductId ?? Guid.Empty,

                                CategoryId = m.CategoryId,
                                Quantity = m.Quantity,
                                Unit = m.Unit,
                                MaterialNameSnapshot = m.MaterialNameSnapshot,
                                MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,

                                UnitPrice = (
                                    from ms in msp
                                    where ms.MaterialId == m.MaterialId
                                    orderby (ms.UpdatedDate ?? ms.CreateDate) descending
                                    select (decimal?)ms.CurrentPrice
                                ).FirstOrDefault() ?? m.UnitPrice
                            })
                            .ToList()
                    };

                // --------- GIỮ NGUYÊN PHẦN VU + PHẦN DƯỚI ---------
                var qvu = from f in fr
                          where f.ProductId == query.ProductId
                          orderby f.CreatedDate descending,
                                  f.FormulaId descending
                          select new GetSummaryMfgFormula
                          {
                              ManufacturingFormulaId = f.FormulaId,
                              ExternalId = f.ExternalId,
                              Name = f.Name,
                              TotalPrice = f.FormulaMaterials
                                  .Select(m =>
                                      (
                                          (
                                              from ms in msp
                                              where ms.MaterialId == m.MaterialId
                                              orderby (ms.UpdatedDate ?? ms.CreateDate) descending
                                              select (decimal?)ms.CurrentPrice
                                          ).FirstOrDefault() ?? m.UnitPrice
                                      )
                                      * m.Quantity
                                  )
                                  .Sum(),
                              FormulaSourceIdCreatedDate = f.CreatedDate,
                              ManufacturingFormulaMaterials = f.FormulaMaterials
                                .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                                    .ThenBy(x => x.MaterialExternalIdSnapshot)
                                  .Select(m => new GetSampleMfgFormulaMaterial
                                  {
                                      itemType = m.itemType,
                                      ItemId = m.itemType == ItemType.Material
                                          ? m.MaterialId ?? Guid.Empty
                                          : m.ProductId ?? Guid.Empty,

                                      CategoryId = m.CategoryId,
                                      Quantity = m.Quantity,
                                      Unit = m.Unit,
                                      MaterialNameSnapshot = m.MaterialNameSnapshot,
                                      MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,
                                      UnitPrice = (
                                          from ms in msp
                                          where ms.MaterialId == m.MaterialId
                                          orderby (ms.UpdatedDate ?? ms.CreateDate) descending
                                          select (decimal?)ms.CurrentPrice
                                      ).FirstOrDefault() ?? m.UnitPrice
                                  })
                                  .ToList()
                          };

                var q = query.Source switch
                {
                    FormulaSource.FromVA => qva,
                    FormulaSource.FromVU => qvu,
                    _ => qva.Concat(qvu)
                };

                var items = await q.ToListAsync(ct);

                var result = items
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();

                return OperationResult<PagedResult<GetSummaryMfgFormula>>.Ok(
                    new PagedResult<GetSummaryMfgFormula>(
                        result,
                        items.Count,
                        query.PageNumber,
                        query.PageSize
                    )
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách các phiên bản công thức của một công thức sản xuất trong lịch sử
        /// </summary>
        /// <param name="mfgFormulaId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<PagedResult<FormulaVersionMetaDto>>> GetFormulaVersionsPagedAsync(MfgProductionOrderQuery queryable, CancellationToken ct = default)
        {
            if (queryable.MfgFormulaId == Guid.Empty)
                return OperationResult<PagedResult<FormulaVersionMetaDto>>.Fail("Id không hợp lệ.");

            if (queryable.PageNumber <= 0) queryable.PageNumber = 1;
            if (queryable.PageSize <= 0) queryable.PageSize = 20;

            var baseQuery = _unitOfWork.ManufacturingFormulaVersionRepository.Query()
                .Where(v => v.ManufacturingFormulaId == queryable.MfgFormulaId);

            var total = await baseQuery.CountAsync(ct);

            var query = baseQuery
                .OrderByDescending(v => v.VersionNo);

            var items = await query
                .Skip((queryable.PageNumber - 1) * queryable.PageSize)
                .Take(queryable.PageSize)
                .Select(v => new FormulaVersionMetaDto
                {
                    ManufacturingFormulaVersionId = v.ManufacturingFormulaVersionId,
                    VersionNo = v.VersionNo,
                    Status = v.Status,
                    EffectiveFrom = v.EffectiveFrom,
                    EffectiveTo = v.EffectiveTo,
                    Note = v.Note,

                    ManufacturingFormulaMaterials = v.Items
                        .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                            .ThenBy(x => x.MaterialExternalIdSnapshot)
                        .Select(i => new GetManufacturingFormulaMaterial
                        {
                            itemType = i.itemType,
                            ItemId = i.itemType == ItemType.Material
                                ? i.MaterialId ?? Guid.Empty
                                : i.ProductId ?? Guid.Empty,
                            CategoryId = i.CategoryId,

                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            TotalPrice = i.TotalPrice,

                            MaterialNameSnapshot = i.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = i.MaterialExternalIdSnapshot,
                            Unit = i.Unit,
                            IsActive = true
                        })
                        .ToList()
                })
                .ToListAsync(ct);

            var paged = new PagedResult<FormulaVersionMetaDto>(
                items,
                total,
                queryable.PageNumber,
                queryable.PageSize
            );

            return OperationResult<PagedResult<FormulaVersionMetaDto>>.Ok(paged);
        }

        /// <summary>
        /// Lấy dánh sách công thức đẻ so sánh
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OperationResult<PagedResult<GetCompareFormula>>> GetCompareFormulaAsync(MfgFormulaQuery query, CancellationToken ct = default)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 15;

            var keyword = (query.Keyword ?? "").Trim();
            var kw = keyword.ToLower();

            var psv = _unitOfWork.ProductStandardFormulaRepository.Query();
            var mfr = _unitOfWork.ManufacturingFormulaRepository.Query();
            var fr = _unitOfWork.FormulaRepository.Query();
            var pr = _unitOfWork.ProductRepository.Query();

            // =========================
            // 1) HEADER QUERY (KHÔNG materials)
            // =========================

            var qvaHeader =
                from s in psv
                where s.ManufacturingFormulaId.HasValue
                      && (!query.CompanyId.HasValue || s.CompanyId == query.CompanyId.Value)

                let isLatestStandard =
                    !psv.Any(s2 =>
                        s2.ManufacturingFormulaId == s.ManufacturingFormulaId
                        && (
                            s2.ValidFrom > s.ValidFrom
                            || (s2.ValidFrom == s.ValidFrom
                                && s2.ProductStandardFormulaId.CompareTo(s.ProductStandardFormulaId) > 0)
                        ))

                where isLatestStandard
                join f in mfr on s.ManufacturingFormulaId.Value equals f.ManufacturingFormulaId
                join p in pr on s.ProductId equals p.ProductId
                where f.IsActive
                      && (!query.From.HasValue || f.CreatedDate >= query.From.Value)
                      && (!query.To.HasValue || f.CreatedDate <= query.To.Value)
                      && (string.IsNullOrEmpty(kw)
                          || (f.ExternalId != null && f.ExternalId.ToLower().Contains(kw))
                          || (f.Name != null && f.Name.ToLower().Contains(kw))
                          || (p.ColourCode != null && p.ColourCode.ToLower().Contains(kw))
                          || (p.Name != null && p.Name.ToLower().Contains(kw)))
                select new CompareFormulaHeader
                {
                    Source = FormulaSource.FromVA,
                    CreatedDate = f.CreatedDate,
                    Id = f.ManufacturingFormulaId,
                    ExternalId = f.ExternalId,
                    ColourCode = p.ColourCode
                };

            var qvuHeader =
                from f in fr
                where f.IsActive
                      && (!query.CompanyId.HasValue || (f.CompanyId.HasValue && f.CompanyId.Value == query.CompanyId.Value))
                      && (!query.From.HasValue || f.CreatedDate >= query.From.Value)
                      && (!query.To.HasValue || f.CreatedDate <= query.To.Value)
                      && (string.IsNullOrEmpty(kw)
                          || (f.ExternalId != null && f.ExternalId.ToLower().Contains(kw))
                          || (f.Name != null && f.Name.ToLower().Contains(kw))
                          || (f.Product != null && (
                              (f.Product.ColourCode != null && f.Product.ColourCode.ToLower().Contains(kw)) ||
                              (f.Product.Name != null && f.Product.Name.ToLower().Contains(kw))
                          )))
                select new CompareFormulaHeader
                {
                    Source = FormulaSource.FromVU,
                    CreatedDate = f.CreatedDate ?? DateTime.MinValue,
                    Id = f.FormulaId,
                    ExternalId = f.ExternalId,
                    ColourCode = f.Product != null ? f.Product.ColourCode : null
                };

            var headerQ = query.Source switch
            {
                FormulaSource.FromVA => qvaHeader,
                FormulaSource.FromVU => qvuHeader,
                _ => qvaHeader.Concat(qvuHeader)
            };

            headerQ = headerQ.OrderByDescending(h => h.CreatedDate).ThenByDescending(h => h.Id);

            var total = await headerQ.CountAsync(ct);

            var headerPage = await headerQ
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(ct);

            // tách ids theo nguồn
            var vaIds = headerPage.Where(x => x.Source == FormulaSource.FromVA).Select(x => (Guid)x.Id).ToList();
            var vuIds = headerPage.Where(x => x.Source == FormulaSource.FromVU).Select(x => (Guid)x.Id).ToList();

            // =========================
            // 2) LOAD MATERIALS THEO PAGE (2 query)
            // =========================

            var vaMatRepo = _unitOfWork.ManufacturingFormulaMaterialRepository.Query();
            var vuMatRepo = _unitOfWork.FormulaMaterialRepository.Query();
            var vaMats = vaIds.Count == 0
                ? new List<(Guid FormulaId, GetSampleMfgFormulaMaterial Mat)>()
                : await vaMatRepo
                    .Where(m => m.IsActive && vaIds.Contains(m.ManufacturingFormulaId))
                    .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                        .ThenBy(x => x.MaterialExternalIdSnapshot)
                    .Select(m => new
                    {
                        FormulaId = m.ManufacturingFormulaId,
                        Mat = new GetSampleMfgFormulaMaterial
                        {
                            itemType = m.itemType,
                            ItemId = m.itemType == ItemType.Material 
                                ? m.MaterialId ?? Guid.Empty
                                : m.ProductId ?? Guid.Empty,

                            CategoryId = m.CategoryId,
                            Quantity = m.Quantity,
                            Unit = m.Unit,
                            MaterialNameSnapshot = m.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot
                        }
                    })
                    .ToListAsync(ct)
                    .ContinueWith(t => t.Result.Select(x => ((Guid)x.FormulaId, x.Mat)).ToList(), ct);

            var vuMats = vuIds.Count == 0
                ? new List<(Guid FormulaId, GetSampleMfgFormulaMaterial Mat)>()
                : await vuMatRepo
                    .Where(m => m.IsActive && vuIds.Contains(m.FormulaId))
                    .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                        .ThenBy(x => x.MaterialExternalIdSnapshot)
                    .Select(m => new
                    {

                        FormulaId = m.FormulaId,
                        Mat = new GetSampleMfgFormulaMaterial
                        {
                            itemType = m.itemType,
                            ItemId = m.itemType == ItemType.Material 
                                ? m.MaterialId ?? Guid.Empty
                                : m.ProductId ?? Guid.Empty,
                            CategoryId = m.CategoryId,
                            Quantity = m.Quantity,
                            Unit = m.Unit,
                            MaterialNameSnapshot = m.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot
                        }
                    })
                    .ToListAsync(ct)
                    .ContinueWith(t => t.Result.Select(x => ((Guid)x.FormulaId, x.Mat)).ToList(), ct);

            var vaGroup = vaMats
                .GroupBy(x => x.Item1)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Mat).ToList());

            var vuGroup = vuMats
                .GroupBy(x => x.Item1)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Mat).ToList());

            // =========================
            // 3) BUILD RESULT
            // =========================
            var page = new List<GetCompareFormula>(headerPage.Count);

            foreach (var h in headerPage)
            {
                var id = (Guid)h.Id;

                page.Add(new GetCompareFormula
                {
                    Id = id,
                    ExternalId = h.ExternalId,
                    ColourCode = h.ColourCode,
                    ManufacturingFormulaMaterials =
                        h.Source == FormulaSource.FromVA
                            ? (vaGroup.TryGetValue(id, out var a) ? a : new List<GetSampleMfgFormulaMaterial>())
                            : (vuGroup.TryGetValue(id, out var b) ? b : new List<GetSampleMfgFormulaMaterial>())
                });
            }

            return OperationResult<PagedResult<GetCompareFormula>>.Ok(
                new PagedResult<GetCompareFormula>(page, total, query.PageNumber, query.PageSize));
        }

        // ======================================================================== Post ======================================================================== 

        /// <summary>
        /// Tạo mới công thức sản xuất nếu đã có công thức chuẩn và đang copy từ công thức đó thì sẽ lưu lại công thức chuẩn cũ vào lịch sử
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <summary>
        /// Tạo công thức sản xuất (VA) từ màn hình lab/kế hoạch:
        /// - Tạo ManufacturingFormula + Materials (bản master SX)
        /// - Release ra ManufacturingFormulaVersion + VersionItems (bản đóng băng)
        /// - Nếu IsStandard = true  -> set làm công thức chuẩn cho Product
        /// - Nếu IsSelect  = true  -> gán version này cho lệnh sản xuất hiện tại
        /// </summary>
        public async Task<OperationResult> CreateAsync(PostMfgFormula req, CancellationToken ct = default)
        {
            // 0) Validate cơ bản
            if (req.ProductId == Guid.Empty)
                return OperationResult.Fail("ProductId không hợp lệ.");

            if (req.SourceType == null)
                return OperationResult.Fail("SourceType là bắt buộc (FromVA / FromVU).");

            if (req.ManufacturingFormulaMaterials == null || !req.ManufacturingFormulaMaterials.Any())
                return OperationResult.Fail("Công thức phải có ít nhất 1 nguyên vật liệu.");

            // Bắt buộc phải select
            if (!req.IsSelect)
                return OperationResult.Fail("Tạo công thức sản xuất bắt buộc phải chọn cho lệnh sản xuất.");

            if (!req.mfgProductionOrderId.HasValue || req.mfgProductionOrderId.Value == Guid.Empty)
                return OperationResult.Fail("Thiếu MfgProductionOrderId.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var mpoId = req.mfgProductionOrderId.Value;

                var mpo = await _unitOfWork.MfgProductionOrderRepository
                    .Query(track: true)
                    .Include(o => o.Product)
                    .FirstOrDefaultAsync(o =>
                        o.MfgProductionOrderId == mpoId &&
                        o.CompanyId == companyId &&
                        o.IsActive,
                        ct);

                if (mpo == null)
                    return OperationResult.Fail("Không tìm thấy lệnh sản xuất.");

                var oldMpoStatus = mpo.Status;
                string? nextMpoStatus = null;

                // Vì flow này luôn select nên formula mới luôn ở trạng thái Checking
                var formulaStatus = ManufacturingProductOrderFormula.Checking.ToString();

                if (string.Equals(oldMpoStatus, ManufacturingProductOrder.FormulaRequested.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    nextMpoStatus = ManufacturingProductOrder.FormulaSuccess.ToString();
                }

                // 1) Tạo ManufacturingFormula (VA)
                var mf = new ManufacturingFormula
                {
                    ManufacturingFormulaId = Guid.CreateVersion7(),
                    ExternalId = await _externalId.NextAsync(DocumentPrefix.VA.ToString(), ct: ct),
                    Name = "F001",
                    Status = formulaStatus,
                    TotalPrice = req.ManufacturingFormulaMaterials.Sum(x => x.UnitPrice * x.Quantity),

                    // Nguồn gốc
                    SourceType = req.SourceType.Value,
                    IsActive = true,
                    Note = req.Note,

                    CreatedDate = now,
                    UpdatedDate = now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CompanyId = companyId
                };

                // Mapping nguồn gốc tùy theo SourceType
                switch (req.SourceType)
                {
                    case FormulaSource.FromVA:
                        mf.SourceManufacturingFormulaId = req.FormulaSourceId;
                        mf.SourceManufacturingExternalIdSnapshot = req.FormulaSourceNameSnapshot;
                        mf.SourceVUFormulaId = req.VUFormulaId;
                        mf.SourceVUExternalIdSnapshot = req.FormulaExternalIdSnapshot;
                        break;

                    case FormulaSource.FromVU:
                        mf.SourceVUFormulaId = req.FormulaSourceId;
                        mf.SourceVUExternalIdSnapshot = req.FormulaSourceNameSnapshot;
                        break;
                }

                await _unitOfWork.ManufacturingFormulaRepository.AddAsync(mf, ct);

                // 2) Tạo ManufacturingFormulaMaterial
                var materialEntities = req.ManufacturingFormulaMaterials
                    .Select((m, index) => new ManufacturingFormulaMaterial
                    {
                        ManufacturingFormulaMaterialId = Guid.CreateVersion7(),
                        ManufacturingFormulaId = mf.ManufacturingFormulaId,

                        LineNo = index + 1,
                        itemType = m.ItemType,

                        MaterialId = m.ItemType == ItemType.Material ? m.ItemId : null,
                        ProductId = m.ItemType == ItemType.Product ? m.ItemId : null,

                        CategoryId = m.CategoryId,
                        Quantity = m.Quantity,
                        UnitPrice = m.UnitPrice,
                        TotalPrice = m.UnitPrice * m.Quantity,

                        MaterialNameSnapshot = m.MaterialNameSnapshot,
                        MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,
                        Unit = m.Unit,
                        IsActive = m.IsActive
                    })
                    .ToList();

                await _unitOfWork.ManufacturingFormulaMaterialRepository.AddRangeAsync(materialEntities, ct);

                // 3) Nếu IsStandard = true -> set làm công thức chuẩn cho Product
                if (req.IsStandard)
                {
                    var currentStd = await _unitOfWork.ProductStandardFormulaRepository
                        .Query(track: true)
                        .FirstOrDefaultAsync(x =>
                            x.CompanyId == companyId &&
                            x.ProductId == req.ProductId &&
                            x.ValidTo == null, ct);

                    if (currentStd != null)
                    {
                        currentStd.ValidTo = now;
                        currentStd.ClosedBy = userId;
                    }

                    var newStd = new ProductStandardFormula
                    {
                        ProductStandardFormulaId = Guid.CreateVersion7(),
                        ProductId = req.ProductId,
                        ManufacturingFormulaId = mf.ManufacturingFormulaId,
                        ValidFrom = now,
                        ValidTo = null,
                        CreatedBy = userId,
                        ClosedBy = null,
                        CompanyId = companyId,
                        Note = req.Note
                    };

                    await _unitOfWork.ProductStandardFormulaRepository.AddAsync(newStd, ct);
                }

                // 4) LUÔN tạo select version cho MPO
                var currentPsv = await _unitOfWork.ProductionSelectVersionRepository
                    .Query(track: true)
                    .FirstOrDefaultAsync(x =>
                        x.CompanyId == companyId &&
                        x.MfgProductionOrderId == mpoId &&
                        x.ValidTo == null, ct);

                if (currentPsv != null)
                {
                    currentPsv.ValidTo = now;
                    currentPsv.ClosedBy = userId;
                }

                var newPsv = new ProductionSelectVersion
                {
                    ProductionSelectVersionId = Guid.CreateVersion7(),
                    MfgProductionOrderId = mpoId,
                    ManufacturingFormulaId = mf.ManufacturingFormulaId,
                    ValidFrom = now,
                    ValidTo = null,
                    CreatedBy = userId,
                    ClosedBy = null,
                    CompanyId = companyId
                };

                await _unitOfWork.ProductionSelectVersionRepository.AddAsync(newPsv, ct);

                // 5) Cập nhật MPO
                mpo.UpdatedDate = now;
                mpo.UpdatedBy = userId;

                if (!string.IsNullOrWhiteSpace(nextMpoStatus) &&
                    !string.Equals(mpo.Status, nextMpoStatus, StringComparison.OrdinalIgnoreCase))
                {
                    mpo.Status = nextMpoStatus;
                }

                // 6) Timeline
                await _TimelineService.AddEventLogAsync(new EventLogModels
                {
                    employeeId = userId,
                    eventType = EventType.ManufacturingProductOrder,
                    sourceCode = mpo.ExternalId ?? string.Empty,
                    sourceId = mpo.MfgProductionOrderId,
                    status = mpo.Status,
                    note = $"Cập nhật bởi hệ thống vào {now} bởi {_currentUser.personName}"
                }, ct);

                // 7) Kiểm tra GIÁ CẢNH BÁO và bắn Notification
                decimal? targetPrice = null;
                try
                {
                    targetPrice = await _priceProvider.GetTargetPriceByMpoAsync(
                        mfgProductionOrderId: mpoId,
                        ct: ct
                    );
                }
                catch
                {
                    // không chặn flow nếu provider lỗi
                }

                var ignoreCustomerId = Guid.Parse("019bd983-28a1-7231-810a-14c03e090b75");
                var isIgnoreCustomer = mpo.CustomerId == ignoreCustomerId;

                var productDisplay = !string.IsNullOrWhiteSpace(mpo.Product?.ColourCode)
                    ? mpo.Product.ColourCode
                    : mpo.Product?.Name ?? string.Empty;

                if (!isIgnoreCustomer && targetPrice.HasValue && (mf.TotalPrice ?? 0m) > targetPrice.Value)
                {
                    await _notificationService.PublishAsync(new PublishNotificationRequest
                    {
                        Topic = TopicNotifications.PriceOverSellCreated,
                        Severity = NotificationSeverity.Warning,
                        Title = $"Cảnh báo giá: {productDisplay} - {mf.ExternalId}",
                        Message = string.IsNullOrWhiteSpace(productDisplay)
                            ? $"Tổng chi phí {(mf.TotalPrice ?? 0m):N0} > Giá bán {targetPrice.Value:N0}"
                            : $"SP {productDisplay}: Tổng chi phí {(mf.TotalPrice ?? 0m):N0} > Giá bán {targetPrice.Value:N0}",
                        Link = $"/labs/mfgformula/{mpo.MfgProductionOrderId}/{mf.ManufacturingFormulaId}",
                        PayloadJson = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            FormulaId = mf.ManufacturingFormulaId,
                            ExternalId = mf.ExternalId,
                            ProductColourCode = mpo.Product?.ColourCode,
                            ProductCode = mpo.Product?.Code,
                            ProductName = mpo.Product?.Name,
                            TotalCost = mf.TotalPrice,
                            TargetPrice = targetPrice.Value,
                            MpoId = mpoId,
                            CustomerId = mpo.CustomerId
                        }),
                        TargetRoles = new() { AppRoles.PLPUNotify, AppRoles.President }
                    }, ct);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return OperationResult.Ok("Tạo công thức sản xuất thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi tạo công thức sản xuất: {ex.Message}");
            }
        }

        // ====================================================================== Patch ========================================================================

        /// <summary>
        /// Upsert (cập nhật hoặc bổ sung) **Công thức sản xuất** cho một Lệnh sản xuất.
        /// 
        /// NGHIỆP VỤ & QUY TẮC
        /// 1) Cho phép cập nhật các thuộc tính meta của công thức (Note, Source*, IsActive, IsSelect, IsStandard, Status…).
        /// 2) Quản lý **duy nhất 1 công thức IsSelect** trong phạm vi **một Lệnh sản xuất** (MfgProductionOrder).
        /// 3) Quản lý **duy nhất 1 công thức IsStandard** trong phạm vi **một VUFormula** (bộ công thức chuẩn).
        /// 4) Khi công thức được chọn (IsSelect = true):
        ///    - Cập nhật Status của công thức = "IsSelect".
        ///    - Nếu Lệnh sản xuất đang ở trạng thái New/QCFail ⇒ chuyển sang QCChecked.
        /// 5) Quản lý danh sách vật tư của công thức (ManufacturingFormulaMaterials):
        ///    - Thêm mới nếu chưa có.
        ///    - Cập nhật nếu đã tồn tại (Quantity, UnitPrice, Unit, snapshots, Lot/Stock link…).
        ///    - **Soft-delete** (IsActive = false) những dòng không còn trong payload.
        /// 6) **Tổng tiền công thức** luôn tự tính lại (Sum TotalPrice các dòng còn hiệu lực) — bỏ qua giá trị do client gửi.
        /// 7) Toàn bộ thao tác được bọc trong **transaction** để đảm bảo tính nhất quán.
        /// 
        /// LƯU Ý THỰC THI:
        /// - Sử dụng PatchHelper để cập nhật từng phần (partial update) theo payload.
        /// - Chỉ load/track công thức mục tiêu + collection vật tư của nó.
        /// - Ghi log khi đánh dấu IsStandard (truy vết ai/bao giờ/vì sao).
        /// </summary>
        public async Task<OperationResult> UpsertFormulaAsync(PatchMfgFormula req, CancellationToken? cancellationToken = null)
        {
            var ct = cancellationToken ?? CancellationToken.None;
            Guid productId = Guid.Empty;
            string mfgExternalId = string.Empty;
            bool materialChanged = false;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;
                var companyId = _currentUser.CompanyId;

                var existing = await _unitOfWork.ManufacturingFormulaRepository
                    .Query(track: true)
                    .Include(f => f.ManufacturingFormulaMaterials)
                    .Include(f => f.ProductionSelectVersions)
                    .Include(f => f.ProductStandardFormulas)
                    .Include(f => f.ManufacturingFormulaVersions)
                        .ThenInclude(v => v.Items)
                    .FirstOrDefaultAsync(
                        f => f.ManufacturingFormulaId == req.ManufacturingFormulaId && f.IsActive,
                        ct);

                if (existing == null)
                    return OperationResult.Fail($"Không tìm thấy công thức với ID {req.ManufacturingFormulaId}");

                existing.UpdatedDate = now;
                existing.UpdatedBy = userId;

                PatchHelper.SetIfRef(req.Note, () => existing.Note, v => existing.Note = v);

                if (req.TotalPrice.HasValue)
                    existing.TotalPrice = req.TotalPrice.Value;
                else if (req.MfgTotalPrice.HasValue)
                    existing.TotalPrice = req.MfgTotalPrice.Value;

                MfgProductionOrder? mpoEntity = null;

                // =====================================================
                // 1) ALWAYS SELECT nếu có MfgProductionOrderId
                // =====================================================
                if (req.mfgProductionOrderId.HasValue && req.mfgProductionOrderId.Value != Guid.Empty)
                {
                    var orderId = req.mfgProductionOrderId.Value;

                    mpoEntity = await _unitOfWork.MfgProductionOrderRepository
                        .Query(track: true)
                        .FirstOrDefaultAsync(o => o.MfgProductionOrderId == orderId && o.IsActive, ct);

                    if (mpoEntity == null)
                        return OperationResult.Fail("Không tìm thấy lệnh sản xuất.");

                    productId = mpoEntity.ProductId;
                    mfgExternalId = mpoEntity.ExternalId ?? string.Empty;

                    var psvRepo = _unitOfWork.ProductionSelectVersionRepository.Query(track: true);

                    var currentActivePsvs = await psvRepo
                        .Where(psv =>
                            psv.MfgProductionOrderId == orderId &&
                            psv.ValidTo == null)
                        .ToListAsync(ct);

                    var activeOfThisFormula = currentActivePsvs
                        .FirstOrDefault(psv => psv.ManufacturingFormulaId == existing.ManufacturingFormulaId);

                    var activeOthers = currentActivePsvs
                        .Where(psv => psv.ManufacturingFormulaId != existing.ManufacturingFormulaId)
                        .ToList();

                    foreach (var psv in activeOthers)
                    {
                        psv.ValidTo = now;
                        psv.ClosedBy = userId;
                    }

                    if (activeOfThisFormula == null)
                    {
                        var newPsv = new ProductionSelectVersion
                        {
                            ProductionSelectVersionId = Guid.CreateVersion7(),
                            MfgProductionOrderId = orderId,
                            ManufacturingFormulaId = existing.ManufacturingFormulaId,
                            ValidFrom = now,
                            ValidTo = null,
                            CreatedBy = userId,
                            ClosedBy = null,
                            CompanyId = companyId
                        };

                        await _unitOfWork.ProductionSelectVersionRepository.AddAsync(newPsv, ct);
                    }

                    // BE tự quyết định status
                    var isMpoFormulaRequested =
                        string.Equals(mpoEntity.Status, ManufacturingProductOrder.FormulaRequested.ToString(), StringComparison.OrdinalIgnoreCase);

                    var isFormulaNew =
                        string.Equals(existing.Status, ManufacturingProductOrderFormula.New.ToString(), StringComparison.OrdinalIgnoreCase);

                    if (isMpoFormulaRequested && isFormulaNew)
                    {
                        existing.Status = ManufacturingProductOrderFormula.Checking.ToString();

                        mpoEntity.Status = ManufacturingProductOrder.FormulaSuccess.ToString();
                        mpoEntity.UpdatedBy = userId;
                        mpoEntity.UpdatedDate = now;
                    }
                    else
                    {
                        // nếu bạn không muốn dùng IsSelect như 1 status thì đổi thành Checking hoặc giữ nguyên
                        existing.Status = ManufacturingProductOrderFormula.Checking.ToString();
                    }

                    existing.UpdatedBy = userId;
                    existing.UpdatedDate = now;
                }

                // =====================================================
                // 2) STANDARD
                // =====================================================
                if (req.mfgProductionOrderId.HasValue && productId != Guid.Empty)
                {
                    if (req.IsStandard)
                    {
                        var currentStandards = await _unitOfWork.ProductStandardFormulaRepository
                            .Query(track: true)
                            .Where(psf =>
                                psf.ProductId == productId &&
                                psf.CompanyId == companyId &&
                                psf.ValidTo == null)
                            .ToListAsync(ct);

                        foreach (var psf in currentStandards)
                        {
                            psf.ValidTo = now;
                            psf.ClosedBy = userId;
                        }

                        var hasCurrentStdForThisFormula = currentStandards.Any(psf =>
                            psf.ManufacturingFormulaId == existing.ManufacturingFormulaId);

                        if (!hasCurrentStdForThisFormula)
                        {
                            var newStd = new ProductStandardFormula
                            {
                                ProductStandardFormulaId = Guid.CreateVersion7(),
                                ProductId = productId,
                                ManufacturingFormulaId = existing.ManufacturingFormulaId,
                                ValidFrom = now,
                                ValidTo = null,
                                CreatedBy = userId,
                                ClosedBy = null,
                                CompanyId = companyId,
                                Note = req.noteWhyStandardChanged ?? string.Empty
                            };

                            await _unitOfWork.ProductStandardFormulaRepository.AddAsync(newStd, ct);
                        }
                    }
                    else
                    {
                        var currentStdForThisFormula = await _unitOfWork.ProductStandardFormulaRepository
                            .Query(track: true)
                            .Where(psf =>
                                psf.ProductId == productId &&
                                psf.CompanyId == companyId &&
                                psf.ManufacturingFormulaId == existing.ManufacturingFormulaId &&
                                psf.ValidTo == null)
                            .ToListAsync(ct);

                        foreach (var psf in currentStdForThisFormula)
                        {
                            psf.ValidTo = now;
                            psf.ClosedBy = userId;
                        }
                    }
                }

                // =====================================================
                // 3) MATERIALS
                // =====================================================
                if (req.ManufacturingFormulaMaterials != null)
                {
                    CreateFormulaVersionSnapshot(existing, now, req.Note);

                    var incoming = req.ManufacturingFormulaMaterials.ToList();

                    var incomingIds = incoming
                        .Where(x => x.ManufacturingFormulaMaterialId != Guid.Empty)
                        .Select(x => x.ManufacturingFormulaMaterialId)
                        .ToHashSet();

                    foreach (var material in existing.ManufacturingFormulaMaterials.Where(m => m.IsActive))
                    {
                        if (!incomingIds.Contains(material.ManufacturingFormulaMaterialId))
                        {
                            material.IsActive = false;
                            materialChanged = true;
                        }
                    }

                    for (int idx = 0; idx < incoming.Count; idx++)
                    {
                        var patchItem = incoming[idx];
                        var lineNo = idx + 1;

                        if (patchItem.ItemId == Guid.Empty)
                            continue;

                        if (patchItem.ManufacturingFormulaMaterialId == Guid.Empty)
                        {
                            var resurrectMaterial = existing.ManufacturingFormulaMaterials.FirstOrDefault(m =>
                                m.ManufacturingFormulaId == existing.ManufacturingFormulaId &&
                                m.itemType == patchItem.ItemType &&
                                (
                                    (patchItem.ItemType == ItemType.Material && m.MaterialId == patchItem.ItemId) ||
                                    (patchItem.ItemType == ItemType.Product && m.ProductId == patchItem.ItemId)
                                ) &&
                                m.CategoryId == patchItem.CategoryId &&
                                !m.IsActive);

                            if (resurrectMaterial != null)
                            {
                                resurrectMaterial.IsActive = true;
                                resurrectMaterial.Quantity = patchItem.Quantity;
                                resurrectMaterial.UnitPrice = patchItem.UnitPrice;
                                resurrectMaterial.TotalPrice = patchItem.TotalPrice;
                                resurrectMaterial.LineNo = lineNo;
                                resurrectMaterial.Unit = patchItem.Unit ?? resurrectMaterial.Unit;
                                resurrectMaterial.MaterialNameSnapshot = patchItem.MaterialNameSnapshot ?? resurrectMaterial.MaterialNameSnapshot;
                                resurrectMaterial.MaterialExternalIdSnapshot = patchItem.MaterialExternalIdSnapshot ?? resurrectMaterial.MaterialExternalIdSnapshot;

                                materialChanged = true;
                                continue;
                            }

                            var newMaterial = new ManufacturingFormulaMaterial
                            {
                                ManufacturingFormulaMaterialId = Guid.CreateVersion7(),
                                ManufacturingFormulaId = existing.ManufacturingFormulaId,

                                itemType = patchItem.ItemType,
                                MaterialId = patchItem.ItemType == ItemType.Material ? patchItem.ItemId : null,
                                ProductId = patchItem.ItemType == ItemType.Product ? patchItem.ItemId : null,

                                CategoryId = patchItem.CategoryId,
                                LineNo = lineNo,

                                Quantity = patchItem.Quantity,
                                UnitPrice = patchItem.UnitPrice,
                                TotalPrice = patchItem.TotalPrice,

                                Unit = patchItem.Unit ?? string.Empty,
                                MaterialNameSnapshot = patchItem.MaterialNameSnapshot ?? string.Empty,
                                MaterialExternalIdSnapshot = patchItem.MaterialExternalIdSnapshot ?? string.Empty,
                                IsActive = patchItem.IsActive ?? true
                            };
                            //existing.ManufacturingFormulaMaterials.Add(newMaterial);

                            await _unitOfWork.ManufacturingFormulaMaterialRepository.AddAsync(newMaterial, ct);
                            materialChanged = true;
                            continue;
                        }

                        var existingMaterial = existing.ManufacturingFormulaMaterials
                            .FirstOrDefault(m => m.ManufacturingFormulaMaterialId == patchItem.ManufacturingFormulaMaterialId);

                        if (existingMaterial == null)
                            continue;

                        var before = new
                        {
                            existingMaterial.itemType,
                            existingMaterial.MaterialId,
                            existingMaterial.ProductId,
                            existingMaterial.CategoryId,
                            existingMaterial.LineNo,
                            existingMaterial.Quantity,
                            existingMaterial.UnitPrice,
                            existingMaterial.TotalPrice,
                            existingMaterial.Unit,
                            existingMaterial.MaterialNameSnapshot,
                            existingMaterial.MaterialExternalIdSnapshot,
                            existingMaterial.IsActive
                        };

                        existingMaterial.LineNo = lineNo;
                        existingMaterial.itemType = patchItem.ItemType;
                        existingMaterial.MaterialId = patchItem.ItemType == ItemType.Material ? patchItem.ItemId : null;
                        existingMaterial.ProductId = patchItem.ItemType == ItemType.Product ? patchItem.ItemId : null;
                        existingMaterial.CategoryId = patchItem.CategoryId;

                        PatchHelper.SetIf(patchItem.Quantity, () => existingMaterial.Quantity, v => existingMaterial.Quantity = v);
                        PatchHelper.SetIf(patchItem.UnitPrice, () => existingMaterial.UnitPrice, v => existingMaterial.UnitPrice = v);
                        PatchHelper.SetIf(patchItem.TotalPrice, () => existingMaterial.TotalPrice, v => existingMaterial.TotalPrice = v);

                        PatchHelper.SetIfRef(patchItem.Unit, () => existingMaterial.Unit, v => existingMaterial.Unit = v);
                        PatchHelper.SetIfRef(patchItem.MaterialNameSnapshot, () => existingMaterial.MaterialNameSnapshot, v => existingMaterial.MaterialNameSnapshot = v);
                        PatchHelper.SetIfRef(patchItem.MaterialExternalIdSnapshot, () => existingMaterial.MaterialExternalIdSnapshot, v => existingMaterial.MaterialExternalIdSnapshot = v);

                        if (patchItem.IsActive.HasValue)
                            existingMaterial.IsActive = patchItem.IsActive.Value;

                        var after = new
                        {
                            existingMaterial.itemType,
                            existingMaterial.MaterialId,
                            existingMaterial.ProductId,
                            existingMaterial.CategoryId,
                            existingMaterial.LineNo,
                            existingMaterial.Quantity,
                            existingMaterial.UnitPrice,
                            existingMaterial.TotalPrice,
                            existingMaterial.Unit,
                            existingMaterial.MaterialNameSnapshot,
                            existingMaterial.MaterialExternalIdSnapshot,
                            existingMaterial.IsActive
                        };

                        if (!Equals(before, after))
                            materialChanged = true;
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                // =====================================================
                // 4) NOTIFICATION SAU COMMIT
                // =====================================================
                decimal? targetPrice = null;
                try
                {
                    targetPrice = await _priceProvider.GetTargetPriceByMpoAsync(
                        mfgProductionOrderId: req.mfgProductionOrderId ?? Guid.Empty,
                        ct: ct
                    );
                }
                catch
                {
                }

                var ignoreCustomerId = Guid.Parse("019bd983-28a1-7231-810a-14c03e090b75");
                var isIgnoreCustomer = existing.ProductionSelectVersions
                    .Any(psv => psv.MfgProductionOrder.CustomerId == ignoreCustomerId);

                var productDisplay = mpoEntity != null
                    ? (!string.IsNullOrWhiteSpace(mpoEntity.Product?.ColourCode)
                        ? mpoEntity.Product.ColourCode
                        : mpoEntity.Product?.Name ?? string.Empty)
                    : string.Empty;


                if (!isIgnoreCustomer && targetPrice.HasValue && (existing.TotalPrice ?? 0m) > targetPrice.Value)
                    {
                    await _notificationService.PublishAsync(new PublishNotificationRequest
                    {
                        Topic = TopicNotifications.PriceOverSellCreated,
                        Severity = NotificationSeverity.Warning,
                        Title = $"Cảnh báo giá: {mpoEntity?.Product?.ColourCode}",
                        Message = $"Tổng chi phí {(existing.TotalPrice ?? 0m):N0} > Giá bán {targetPrice.Value:N0}",
                        Link = $"/labs/mfgformula/{req.mfgProductionOrderId}/{req.ManufacturingFormulaId}",
                        PayloadJson = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            FormulaId = req.ManufacturingFormulaId,
                            ExternalId = mfgExternalId,
                            TotalCost = existing.TotalPrice,
                            TargetPrice = targetPrice.Value,
                            MpoId = req.mfgProductionOrderId
                        }),
                        TargetRoles = new() { AppRoles.PLPUNotify, AppRoles.President }
                    }, ct);
                }


                return OperationResult.Ok("Cập nhật công thức thành công");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                var entriesInfo = ex.Entries
                    .Select(e =>
                    {
                        var entityType = e.Metadata.ClrType.Name;
                        var state = e.State.ToString();
                        var keys = e.Properties
                            .Where(p => p.Metadata.IsPrimaryKey())
                            .ToDictionary(
                                p => p.Metadata.Name,
                                p => p.CurrentValue ?? "(null)");

                        var keyStr = string.Join(", ", keys.Select(kv => kv.Key + "=" + kv.Value));
                        return $"{entityType} | State={state} | Keys={keyStr}";
                    });

                var debugMessage = "Concurrency entries: " + string.Join(" || ", entriesInfo);
                return OperationResult.Fail(debugMessage);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail(ex.Message);
            }
        }
        // ======================================================================== Helper ======================================================================== 

        /// <summary>
        /// Tạo snapshot cho phiên bản công thức sản xuất
        /// </summary>
        private void CreateFormulaVersionSnapshot(ManufacturingFormula existing, DateTime now, string? note = null)
        {
            // 1) Tính VersionNo mới
            var lastVersionNo = existing.ManufacturingFormulaVersions
                .OrderByDescending(v => v.VersionNo)
                .Select(v => (int?)v.VersionNo)
                .FirstOrDefault() ?? 0;

            // Cập nhật lại tên công thức theo version mới
            existing.Name = $"F{(lastVersionNo + 1):D3}";

            var newVersionNo = lastVersionNo + 1;

            // 2) (Optional) Close các version đang mở
            foreach (var v in existing.ManufacturingFormulaVersions.Where(v => v.EffectiveTo == null))
            {
                v.EffectiveTo = now;
            }

            // 3) Tạo version mới
            var newVersion = new ManufacturingFormulaVersion
            {
                ManufacturingFormulaId = existing.ManufacturingFormulaId,
                VersionNo = newVersionNo,
                Status = existing.Status,           // snapshot lại status hiện tại
                EffectiveFrom = now,
                EffectiveTo = null,
                Note = note ?? existing.Note        // hoặc ưu tiên note từ req
            };

            // 4) Snapshot danh sách vật tư hiện tại (chỉ IsActive)
            foreach (var m in existing.ManufacturingFormulaMaterials
                .Where(x => x.IsActive)
                    .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                    .ThenBy(x => x.MaterialExternalIdSnapshot))
            {
                var item = new ManufacturingFormulaVersionItem
                {
                    ManufacturingFormulaVersionId = newVersion.ManufacturingFormulaVersionId,
                    LineNo = m.LineNo,
                    MaterialId = m.MaterialId,
                    CategoryId = m.CategoryId,
                    Quantity = m.Quantity,
                    UnitPrice = m.UnitPrice,
                    TotalPrice = m.TotalPrice,

                    MaterialNameSnapshot = m.MaterialNameSnapshot ?? string.Empty,
                    MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot ?? string.Empty,
                    Unit = m.Unit ?? string.Empty
                };

                newVersion.Items.Add(item);
            }

            // 5) Gắn vào navigation, EF sẽ tự insert
            existing.ManufacturingFormulaVersions.Add(newVersion);
        }

        /// <summary>
        /// Tính TotalPrice cho danh sách DTO: Σ(Quantity * đơn giá gần nhất của material).
        /// Đơn giá lấy từ MaterialsSupplier: bản ghi IsActive == true, ngày mới nhất (UpdatedDate ưu tiên, fallback CreateDate).
        /// Nếu có nhiều supplier cùng ngày, ưu tiên IsPreferred == true.
        /// </summary>
        private async Task HydrateUnitPricesAndTotalsAsync(List<GetSummaryMfgFormula> items, CancellationToken ct)
        {
            if (items.Count == 0) return;

            // Gom tất cả MaterialId có trong trang
            var materialIds = items
                .SelectMany(i => i.ManufacturingFormulaMaterials)
                .Where(m => m.itemType == ItemType.Material)
                .Select(m => m.ItemId)
                .Distinct()
                .ToList();

            if (materialIds.Count == 0) return;

            // Lấy đơn giá mới nhất cho từng MaterialId trong MaterialsSupplier
            var raw = await _unitOfWork.MaterialsSupplierRepository.Query()
                .Where(s => materialIds.Contains(s.MaterialId) && (s.IsActive ?? true))
                .Select(s => new
                {
                    s.MaterialId,
                    s.CurrentPrice,
                    s.IsPreferred,
                    Stamp = (DateTime?)(s.UpdatedDate ?? s.CreateDate)
                })
                .ToListAsync(ct);

            var latestPriceByMaterial = raw
                .GroupBy(x => x.MaterialId)
                .ToDictionary(
                    g => g.Key,
                    g =>
                        g.OrderByDescending(x => x.Stamp)               // ngày mới nhất
                        .ThenByDescending(x => x.IsPreferred ?? false) // nếu cùng ngày ưu tiên preferred
                        .Select(x => x.CurrentPrice ?? 0m)
                        .FirstOrDefault()
                );

            //// Gán UnitPrice cho từng material & tính TotalPrice công thức
            //foreach (var f in items)
            //{
            //    decimal total = 0m;

            //    foreach (var m in f.ManufacturingFormulaMaterials)
            //    {
            //        m.UnitPrice = latestPriceByMaterial.TryGetValue(m.MaterialId, out var unit)
            //            ? unit
            //            : 0m;

            //        var qty = m.Quantity ?? 0m;
            //        total += qty * (m.UnitPrice ?? 0m);
            //    }

            //    f.TotalPrice = total;
            //}
        }

        // ====================================================================== Export PDF ======================================================================
        public async Task<byte[]> ExportVAFormulaToPdfAsync(Guid mfgProductionOrderId, CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (mfgProductionOrderId == Guid.Empty)
                    throw new ArgumentException("Id rỗng.", nameof(mfgProductionOrderId));

                var data = await _unitOfWork.MfgProductionOrderRepository.Query()
                    .Where(x => x.MfgProductionOrderId == mfgProductionOrderId /* && x.CompanyId == companyId && x.IsActive */)
                    .Select(x => new
                    {
                        // MPO
                        x.MfgProductionOrderId,
                        ProductionOrderNo = x.ExternalId,

                        ColourCode = x.ProductExternalIdSnapshot,   // map qua ColourCode cho hợp layout PDF hiện tại
                        Name = x.ProductNameSnapshot,
                        x.ColorName,
                        x.FormulaExternalIdSnapshot,
                        x.RequiredDate,
                        AddRate = (double)(x.Product.UsageRate ?? 0),
                        CustomerCode = x.CustomerExternalIdSnapshot,
                        CustomerName = x.CustomerNameSnapshot,

                        TotalQuantityRequest = x.TotalQuantity, // int
                        x.NumOfBatches,
                        x.QcCheck,
                        x.LabNote,
                        x.Requirement,
                        x.PlpuNote,
                        x.BagType,

                        x.CreatedDate,
                        // current selected formula (ValidTo == null)
                        Current = x.ProductionSelectVersions
                            .Where(v => v.ValidTo == null && v.ManufacturingFormulaId != null)
                            .OrderByDescending(v => v.ValidFrom)
                            .Select(v => new
                            {
                                v.ManufacturingFormulaId,

                                Formula = v.ManufacturingFormula == null ? null : new
                                {
                                    v.ManufacturingFormula.ManufacturingFormulaId,
                                    v.ManufacturingFormula.ExternalId,
                                    v.ManufacturingFormula.Name,
                                    v.MfgProductionOrder.FormulaExternalIdSnapshot,

                                    Materials = v.ManufacturingFormula.ManufacturingFormulaMaterials
                                        .Where(m => m.IsActive)
                                        .OrderBy(x => x.LineNo == 0 ? int.MaxValue : x.LineNo)
                                            .ThenBy(x => x.MaterialExternalIdSnapshot)
                                        .Select(m => new FormulaPDFMaterialDTOs
                                        {
                                            ExternalId =
                                                !string.IsNullOrWhiteSpace(m.MaterialExternalIdSnapshot)
                                                    ? m.MaterialExternalIdSnapshot!
                                                    : (m.itemType == ItemType.Material
                                                        ? (m.Material.ExternalId ?? "")
                                                        : ""), // nếu Product có code riêng thì bạn thay ở đây

                                            MaterialName =
                                                !string.IsNullOrWhiteSpace(m.MaterialNameSnapshot)
                                                    ? m.MaterialNameSnapshot!
                                                    : (m.itemType == ItemType.Material
                                                        ? (m.Material.Name ?? "")
                                                        : (m.Product.Name ?? "")),

                                            CategoryId = m.CategoryId,
                                            Quantity = m.Quantity,

                                            LotNo = m.LotNo 
                                        })
                                        .ToList()
                                }
                            })
                            .FirstOrDefault()
                    })
                    .FirstOrDefaultAsync(ct);

                if (data == null)
                    throw new KeyNotFoundException($"Không tìm thấy MfgProductionOrder: {mfgProductionOrderId} (CompanyId={companyId}).");

                if (data.Current?.ManufacturingFormulaId == null || data.Current.Formula == null)
                    throw new InvalidOperationException(
                        $"MPO {data.ProductionOrderNo} chưa chọn công thức VA hiện hành (ProductionSelectVersion.ValidTo == null).");

                var materials = data.Current.Formula.Materials ?? new List<FormulaPDFMaterialDTOs>();


                await FillAndPersistLotNoIfNeededAsync(
                    data.Current.Formula.ManufacturingFormulaId,
                    materials,
                    ct);


                // Build DTO đúng format VU để reuse _FormulaPDF
                var dto = new ManufacturingVUPDF
                {
                    BatchNo = data.ProductionOrderNo ?? string.Empty,
                    RequestDate = data.RequiredDate,
                    BagType = data.BagType ?? string.Empty,
                    CreatedDate = data.CreatedDate,
                    getManufacturingVUFormula = new GetManufacturingVUFormula
                    {
                        ManufacturingVUFormulaId = Guid.Empty,
                        SourceVUExternalId = data.Current.Formula.FormulaExternalIdSnapshot,
                        FormulaId = data.Current.Formula.ManufacturingFormulaId,
                        FormulaExternalId = data.Current.Formula.ExternalId,
                        TotalProductionQuantity = Convert.ToDecimal(data.TotalQuantityRequest),
                        userRate = data.AddRate,
                        NumOfBatches = data.NumOfBatches,
                        QcCheck = data.QcCheck,
                        ColourCode = data.ColourCode,
                        Name = data.Name,
                        CustomerCode = data.CustomerCode,
                        CustomerName = data.CustomerName,
                        LabNote = data.LabNote,
                        Requirement = data.Requirement,
                        PlpuNote = data.PlpuNote
                    },

                    materials = materials
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

        // ====================================================================== Helper ======================================================================

        /// <summary>
        /// Gắn LotNo vào DTO để in PDF nếu còn thiếu, đồng thời cập nhật ngược lại vào DB để lần sau không bị thiếu nữa.
        /// </summary>
        /// <param name="formulaId">ID của công thức sản xuất</param>
        /// <param name="materials">Danh sách nguyên liệu trong DTO</param>
        /// <param name="ct">Token hủy bỏ</param>
        /// <returns></returns>
        private async Task FillAndPersistLotNoIfNeededAsync(Guid formulaId, List<FormulaPDFMaterialDTOs> materials, CancellationToken ct = default)
        {
            var missingLotCodes = materials
                .Where(x => string.IsNullOrWhiteSpace(x.LotNo))
                .Select(x => x.ExternalId?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            if (missingLotCodes.Count == 0)
                return;

            var lotNoMap = await _warehouseReadService.GetLotNoMapByCodesAsync(missingLotCodes!, ct);

            // 1. Fill vào DTO để in PDF
            foreach (var material in materials)
            {
                if (!string.IsNullOrWhiteSpace(material.LotNo))
                    continue;

                var code = material.ExternalId?.Trim();
                if (string.IsNullOrWhiteSpace(code))
                    continue;

                if (lotNoMap.TryGetValue(code, out var lotNo))
                {
                    material.LotNo = lotNo;
                }
            }

            // 2. Fill ngược lại entity DB
            var formulaMaterialEntities = await _unitOfWork.ManufacturingFormulaMaterialRepository.Query(track: true)
                .Where(x => x.ManufacturingFormulaId == formulaId && x.IsActive)
                .Select(x => new
                {
                    Entity = x,
                    Code = !string.IsNullOrWhiteSpace(x.MaterialExternalIdSnapshot)
                        ? x.MaterialExternalIdSnapshot
                        : (x.itemType == ItemType.Material ? x.Material.ExternalId : null)
                })
                .ToListAsync(ct);

            var hasUpdated = false;

            foreach (var row in formulaMaterialEntities)
            {
                if (!string.IsNullOrWhiteSpace(row.Entity.LotNo))
                    continue;

                var code = row.Code?.Trim();
                if (string.IsNullOrWhiteSpace(code))
                    continue;

                if (lotNoMap.TryGetValue(code, out var lotNo))
                {
                    row.Entity.LotNo = lotNo;
                    hasUpdated = true;
                }
            }

            if (hasUpdated)
            {
                await _unitOfWork.SaveChangesAsync(ct);
            }
        }
    }
}
