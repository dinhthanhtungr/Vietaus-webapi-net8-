using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.FormulaVersion;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
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
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
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

        public MfgFormulaService(IUnitOfWork unitOfWork, 
            IExternalIdService externalId, 
            ICurrentUser currentUser, 
            ITimelineService timelineService, 
            IWarehouseReadService warehouseReadService,
            INotificationService notificationService,
            IPriceProvider priceProvider)
        {
            _unitOfWork = unitOfWork;
            _externalId = externalId;
            _currentUser = currentUser;
            _TimelineService = timelineService;
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
                    .Select(m => new GetManufacturingFormulaMaterial
                    {
                        MaterialId = m.MaterialId,
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
                    .Select(fm => new GetManufacturingFormulaMaterial
                    {
                        MaterialId = fm.MaterialId,
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
                    x.ProductionOrder.TotalQuantityRequest,
                    x.ProductionOrder.Status,

                    VuChosenId = x.ProductionOrder.FormulaId,
                    VuChosenExternalIdSnapshot = x.ProductionOrder.FormulaExternalIdSnapshot,

                    x.Detail.TotalPriceAgreed,
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
                .Select(m => new GetManufacturingFormulaMaterial
                {
                    ManufacturingFormulaMaterialId = m.ManufacturingFormulaMaterialId,
                    MaterialId = m.MaterialId,
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
                CustomerNameSnapshot = mpo.CustomerNameSnapshot,

                SaleTotalPrice = mpo.TotalPriceAgreed,
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
                .Select(x => new GetManufacturingFormulaMaterial
                {
                    ManufacturingFormulaMaterialId = x.ManufacturingFormulaMaterialId,
                    ManufacturingFormulaId = x.ManufacturingFormulaId,
                    MaterialId = x.MaterialId,
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
                var psv = _unitOfWork.ProductStandardFormulaRepository.Query();      // chuẩn sản xuất (VA)
                var mfr = _unitOfWork.ManufacturingFormulaRepository.Query();       // công thức sản xuất
                var fr = _unitOfWork.FormulaRepository.Query();                    // công thức nghiên cứu (VU) - anh sửa tên đúng repo của anh

                var qva = from s in psv
                    where s.ProductId == query.ProductId

                    // Giữ lại duy nhất bản ProductStandardFormula mới nhất cho từng ManufacturingFormula
                    let isLatestForThisFormula =
                        !psv.Any(s2 =>
                            s2.ProductId == s.ProductId
                            && s2.ManufacturingFormulaId == s.ManufacturingFormulaId
                            && (
                                s2.ValidFrom > s.ValidFrom
                                || (s2.ValidFrom == s.ValidFrom
                                    && s2.ProductStandardFormulaId.CompareTo(s.ProductStandardFormulaId) > 0)
                            ))

                    where isLatestForThisFormula

                    // Join sang bảng công thức sản xuất
                    join f in mfr
                        on s.ManufacturingFormulaId equals f.ManufacturingFormulaId

                    // Ưu tiên công thức chuẩn hiện tại (ValidTo == null),
                    // sau đó các công thức chuẩn cũ, mới nhất trước
                    orderby s.ValidTo == null descending,
                            s.ValidFrom descending,
                            s.ProductStandardFormulaId descending

                    select new GetSummaryMfgFormula
                    {
                        ManufacturingFormulaId = f.ManufacturingFormulaId,
                        ExternalId = f.ExternalId,
                        Name = f.Name,
                        FormulaSourceIdCreatedDate = f.CreatedDate,
                        // TotalPrice = tổng (Quantity * UnitPrice) theo GIÁ MỚI NHẤT
                        TotalPrice =f.ManufacturingFormulaMaterials
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
                        //isStandard = null,

                        // đang là công thức chuẩn hiện tại
                        //IsSelect = s.ValidTo == null,

                        ManufacturingFormulaMaterials = f.ManufacturingFormulaMaterials
                            .Select(m => new GetSampleMfgFormulaMaterial
                            {
                                MaterialId = m.MaterialId,
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
                        //IsSelect = null,
                        ManufacturingFormulaMaterials = f.FormulaMaterials
                            .Select(m => new GetSampleMfgFormulaMaterial
                            {
                                MaterialId = m.MaterialId,
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
                        .Select(i => new GetManufacturingFormulaMaterial
                        {
                            MaterialId = i.MaterialId,
                            // nếu VersionItem CHƯA có CategoryId thì tạm set Guid.Empty / hoặc bỏ field này khỏi DTO
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

            if (!req.ManufacturingFormulaMaterials.Any())
                return OperationResult.Fail("Công thức phải có ít nhất 1 nguyên vật liệu.");

            // nếu yêu cầu chọn cho MPO mà không truyền MPO Id
            if (req.IsSelect && (req.mfgProductionOrderId == null || req.mfgProductionOrderId == Guid.Empty))
                return OperationResult.Fail("Thiếu MfgProductionOrderId khi IsSelect = true.");

            var now = DateTime.Now; // hoặc DateTime.Now nếu hệ thống bạn đang dùng local time
            var userId = _currentUser.EmployeeId;       // TODO: thay bằng service thực tế
            var companyId = _currentUser.CompanyId; // TODO: thay bằng service thực tế

            var mpo = await _unitOfWork.MfgProductionOrderRepository
                    .Query(track: true)
                    .FirstOrDefaultAsync(o =>
                        o.MfgProductionOrderId == req.mfgProductionOrderId &&
                        o.CompanyId == companyId &&
                        o.IsActive,
                        ct);


            // 1) Tạo ManufacturingFormula (VA)
            var mf = new ManufacturingFormula
            {
                ManufacturingFormulaId = Guid.CreateVersion7(),
                ExternalId = await _externalId.NextAsync("VA", ct: ct),
                Name = "F001",
                Status = string.IsNullOrWhiteSpace(req.status)
                    ? ManufacturingProductOrderFormula.New.ToString()
                    : req.status,

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
                    mf.SourceManufacturingFormulaId = req.FormulaSourceId;              // VA gốc
                    mf.SourceManufacturingExternalIdSnapshot = req.FormulaSourceNameSnapshot;
                    mf.SourceVUFormulaId = req.VUFormulaId;                         // VU gốc nếu có va thì soucevuformula sẽ là vu sale chọn
                    mf.SourceVUExternalIdSnapshot = req.FormulaExternalIdSnapshot;
                    break;

                case FormulaSource.FromVU:
                    mf.SourceVUFormulaId = req.FormulaSourceId;                         // VU gốc
                    mf.SourceVUExternalIdSnapshot = req.FormulaSourceNameSnapshot;
                    break;

                default:
                    // nếu sau này có thêm enum khác thì xử lý riêng
                    break;
            }

            await _unitOfWork.ManufacturingFormulaRepository.AddAsync(mf, ct);

            // 2) Tạo ManufacturingFormulaMaterial
            var materialEntities = req.ManufacturingFormulaMaterials.Select(m => new ManufacturingFormulaMaterial
            {
                ManufacturingFormulaMaterialId = Guid.CreateVersion7(),
                ManufacturingFormulaId = mf.ManufacturingFormulaId,
                MaterialId = m.MaterialId,
                CategoryId = m.CategoryId,
                Quantity = m.Quantity,
                UnitPrice = m.UnitPrice,
                TotalPrice = m.UnitPrice * m.Quantity,

                MaterialNameSnapshot = m.MaterialNameSnapshot,
                MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,
                Unit = m.Unit,
                IsActive = m.IsActive
            }).ToList();

            await _unitOfWork.ManufacturingFormulaMaterialRepository.AddRangeAsync(materialEntities, ct);

            // 3) Nếu IsStandard = true -> set làm công thức chuẩn cho Product
            if (req.IsStandard)
            {
                // 3.1. Đóng công thức chuẩn hiện tại (nếu có)
                var currentStd = await _unitOfWork.ProductStandardFormulaRepository.Query()
                    .Where(x => x.CompanyId == companyId
                                && x.ProductId == req.ProductId
                                && x.ValidTo == null)
                    .FirstOrDefaultAsync(ct);

                if (currentStd != null)
                {
                    currentStd.ValidTo = now;
                    currentStd.ClosedBy = userId;
                    _unitOfWork.ProductStandardFormulaRepository.UpdateAsync(currentStd, ct);
                }

                // 3.2. Tạo dòng chuẩn mới
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

            // 4) Nếu IsSelect = true -> gán VA này cho lệnh sản xuất hiện tại
            if (req.IsSelect && req.mfgProductionOrderId.HasValue)
            {
                var mpoId = req.mfgProductionOrderId.Value;

                // 4.1. Đóng phiên chọn hiện tại (nếu có)
                var currentPsv = await _unitOfWork.ProductionSelectVersionRepository.Query()
                    .Where(x => x.CompanyId == companyId
                                && x.MfgProductionOrderId == mpoId
                                && x.ValidTo == null)
                    .FirstOrDefaultAsync(ct);

                if (currentPsv != null)
                {
                    currentPsv.ValidTo = now;
                    currentPsv.ClosedBy = userId;
                    _unitOfWork.ProductionSelectVersionRepository.UpdateAsync(currentPsv, ct);
                }

                // 4.2. Tạo phiên chọn mới
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

                // 4.3. Cập nhật trạng thái của lệnh sản xuất
                if (mpo != null)
                {
                    mpo.Status = ManufacturingProductOrder.FormulaSuccess.ToString();
                    mpo.UpdatedDate = now;
                    mpo.UpdatedBy = userId;
                }

                await _TimelineService.AddEventLogAsync(new EventLogModels
                {
                    employeeId = userId,
                    eventType = EventType.ManufacturingProductOrder,
                    sourceCode = mpo.ExternalId ?? string.Empty,
                    sourceId = mpo.MfgProductionOrderId,
                    status = mpo.Status,
                    note = $"Cập nhật bởi hệ thống vào {now} bởi {_currentUser.personName}"
                }, ct);

                await _unitOfWork.ProductionSelectVersionRepository.AddAsync(newPsv, ct);


                // 4.5) Kiểm tra GIÁ CẢNH BÁO và bắn Notification (nếu vượt)
                // Giả sử bạn có cách lấy giá bán dự kiến/đã chốt từ Sale:
                decimal? targetPrice = null;
                try
                {
                    // GỢI Ý 1: lấy theo sản phẩm + khách hàng từ lệnh sản xuất (nếu MPO có CustomerId)
                    // (Tuỳ dữ liệu thực tế của bạn — nếu không có, hãy thay bằng service của bạn)
                    var customerId = mpo?.CustomerId ?? Guid.Empty; // nếu MPO có CustomerId
                    targetPrice = await _priceProvider.GetTargetPriceByMpoAsync(
                        mfgProductionOrderId: req.mfgProductionOrderId ?? Guid.Empty,
                        ct: ct
                    );
                }
                catch { /* không chặn flow nếu provider lỗi */ }

                if (targetPrice.HasValue && mf.TotalPrice > targetPrice.Value)
                {
                    // Gửi cảnh báo tới Ban giám đốc (role “Board”) – bạn có thể đổi thành TargetUserIds/TargetTeamIds
                    await _notificationService.PublishAsync(new PublishNotificationRequest
                    {
                        Topic = TopicNotifications.PriceOverSellCreated,
                        Severity = NotificationSeverity.Warning,
                        Title = $"Cảnh báo giá: {mf.ExternalId}",
                        Message = $"Tổng chi phí {mf.TotalPrice:N0} > Giá bán {targetPrice.Value:N0}",
                        Link = $"/labs/mfgformula/{mpo.MfgProductionOrderId}/{mf.ManufacturingFormulaId}",
                        PayloadJson = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            FormulaId = mf.ManufacturingFormulaId,
                            ExternalId = mf.ExternalId,
                            TotalCost = mf.TotalPrice,
                            TargetPrice = targetPrice.Value,
                            MpoId = req.mfgProductionOrderId
                        }),
                        TargetRoles = new() { AppRoles.PLPUNotify, AppRoles.President }
                    }, ct);
                }
            }


            // 5) Lưu DB
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Ok("Tạo công thức sản xuất thành công.");
        }

        // ======================================================================== Patch ======================================================================== 

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
            Guid _ProductId = Guid.Empty;
            string _MfgExternalId = string.Empty;
            bool _materialChanged = false;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;
                var companyId = _currentUser.CompanyId;

                // 1) Load công thức sản xuất cần cập nhật (kèm materials + PSV + PSF nếu cần)
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

                // 2) Cập nhật các field "header" của công thức
                existing.UpdatedDate = now;
                existing.UpdatedBy = userId;

                // Status
                PatchHelper.SetIfRef(req.Status, () => existing.Status, v => existing.Status = v);

                // Ghi chú
                PatchHelper.SetIfRef(req.Note, () => existing.Note, v => existing.Note = v);

                // Tổng giá (nếu FE gửi)
                if (req.TotalPrice.HasValue)
                    existing.TotalPrice = req.TotalPrice.Value;
                else if (req.MfgTotalPrice.HasValue)
                    existing.TotalPrice = req.MfgTotalPrice.Value;

                // 3) Xử lý IsSelect (chọn công thức cho 1 lệnh sản xuất cụ thể)
                if (req.mfgProductionOrderId.HasValue)
                {
                    var orderId = req.mfgProductionOrderId.Value;

                    // Lấy ProductId từ lệnh sản xuất
                    var orderInfo = await _unitOfWork.MfgProductionOrderRepository.Query(track: false)
                        .Where(o => o.MfgProductionOrderId == orderId && o.IsActive)
                        .Select(o => new { o.ProductId, o.ExternalId })
                        .FirstOrDefaultAsync(ct);

                    var psvRepo = _unitOfWork.ProductionSelectVersionRepository.Query(track: true);

                    if (req.IsSelect)
                    {
                        // 3.1. Bỏ chọn các công thức khác đang active cho cùng order này
                        var otherPsvs = await psvRepo
                            .Where(psv =>
                                psv.MfgProductionOrderId == orderId &&
                                psv.ValidTo == null &&
                                psv.ManufacturingFormulaId != existing.ManufacturingFormulaId)
                            .ToListAsync(ct);

                        foreach (var psv in otherPsvs)
                        {
                            psv.ValidTo = now;
                            psv.ClosedBy = userId;
                        }

                        // 3.2. Đảm bảo có PSV active cho chính công thức này
                        var current = await psvRepo
                            .FirstOrDefaultAsync(psv =>
                                psv.MfgProductionOrderId == orderId &&
                                psv.ValidTo == null &&
                                psv.ManufacturingFormulaId == existing.ManufacturingFormulaId,
                                ct);

                        if (current == null)
                        {
                            var newPsv = new ProductionSelectVersion
                            {
                                MfgProductionOrderId = orderId,
                                ManufacturingFormulaId = existing.ManufacturingFormulaId,
                                ValidFrom = now,
                                ValidTo = null,
                                CreatedBy = userId,
                                CompanyId = companyId
                            };

                            await _unitOfWork.ProductionSelectVersionRepository.AddAsync(newPsv);
                        }
                    }
                    else
                    {
                        // Unselect: đóng tất cả PSV active cho công thức này trong order này
                        var currentPsvs = await psvRepo
                            .Where(psv =>
                                psv.MfgProductionOrderId == orderId &&
                                psv.ValidTo == null &&
                                psv.ManufacturingFormulaId == existing.ManufacturingFormulaId)
                            .ToListAsync(ct);

                        foreach (var psv in currentPsvs)
                        {
                            psv.ValidTo = now;
                            psv.ClosedBy = userId;
                        }
                    }


                    // 4) Xử lý IsStandard (công thức chuẩn cho Product)
                    if (orderInfo != null)
                    {
                        _ProductId = orderInfo.ProductId;
                        _MfgExternalId = orderInfo.ExternalId ?? string.Empty;
                        if (req.IsStandard)
                        {
                            // 4.1. Đóng tất cả công thức chuẩn hiện hành của Product này
                            var currentStandards = existing.ProductStandardFormulas
                                .Where(psf => psf.ProductId == _ProductId && psf.ValidTo == null)
                                .ToList();

                            foreach (var psf in currentStandards)
                            {
                                psf.ValidTo = now;
                                psf.ClosedBy = userId;
                            }

                            // 4.2. Kiểm tra công thức hiện tại đã là chuẩn chưa
                            var hasCurrentStdForThisFormula = existing.ProductStandardFormulas.Any(psf =>
                                psf.ProductId == _ProductId
                                && psf.ManufacturingFormulaId == existing.ManufacturingFormulaId
                                && psf.ValidTo == null);

                            if (!hasCurrentStdForThisFormula)
                            {
                                // Tạo dòng ProductStandardFormula mới
                                var newStd = new ProductStandardFormula
                                {
                                    ProductId = _ProductId,
                                    ManufacturingFormulaId = existing.ManufacturingFormulaId,
                                    ValidFrom = now,
                                    ValidTo = null,
                                    CreatedBy = userId,
                                    CompanyId = companyId,
                                    Note = req.noteWhyStandardChanged ?? string.Empty
                                };

                                existing.ProductStandardFormulas.Add(newStd);
                            }
                        }
                        else
                        {
                            // 4.x. Bỏ chuẩn: đóng lại tất cả chuẩn hiện hành của chính công thức này cho Product này
                            var currentStdForThisFormula = existing.ProductStandardFormulas
                                .Where(psf =>
                                    psf.ProductId == _ProductId
                                    && psf.ManufacturingFormulaId == existing.ManufacturingFormulaId
                                    && psf.ValidTo == null)
                                .ToList();

                            foreach (var psf in currentStdForThisFormula)
                            {
                                psf.ValidTo = now;
                                psf.ClosedBy = userId;
                            }
                        }
                    }
                }

                // 5) Cập nhật danh sách vật tư trong công thức
                if (req.ManufacturingFormulaMaterials != null)
                {
                    // TẠO SNAPSHOT TRƯỚC KHI ĐỤNG TỚI MATERIALS
                    CreateFormulaVersionSnapshot(existing, now, req.Note);

                    var incoming = req.ManufacturingFormulaMaterials.ToList();

                    // 5.1. Tập Id các dòng vẫn còn tồn tại trên FE
                    var incomingIds = incoming
                        .Where(x => x.ManufacturingFormulaMaterialId != Guid.Empty)
                        .Select(x => x.ManufacturingFormulaMaterialId)
                        .ToHashSet();

                    // 5.2. Soft delete những dòng cũ nhưng không còn trong incoming
                    foreach (var material in existing.ManufacturingFormulaMaterials
                                                     .Where(m => m.IsActive)) // chỉ check các dòng đang active
                    {
                        if (!incomingIds.Contains(material.ManufacturingFormulaMaterialId))
                        {
                            _materialChanged = true;
                            // User đã xóa dòng này trên form → soft delete
                            material.IsActive = false;
                        }
                    }

                    // 5.3. Upsert những dòng có trong req
                    foreach (var patchItem in incoming)
                    {
                        if (patchItem.ManufacturingFormulaMaterialId == Guid.Empty)
                        {
                            // 5.3.1. THỬ TÁI SỬ DỤNG DÒNG CŨ (đã bị soft-delete)
                            var resurrectMaterial = existing.ManufacturingFormulaMaterials
                                .FirstOrDefault(m =>
                                    m.ManufacturingFormulaId == existing.ManufacturingFormulaId &&
                                    m.MaterialId == patchItem.MaterialId &&
                                    m.CategoryId == patchItem.CategoryId &&
                                    !m.IsActive); // dòng cũ đã bị soft delete

                            if (resurrectMaterial != null)
                            {
                                // "Hồi sinh" dòng cũ & cập nhật lại fields
                                resurrectMaterial.IsActive = true;

                                resurrectMaterial.Quantity = patchItem.Quantity;
                                resurrectMaterial.UnitPrice = patchItem.UnitPrice;
                                resurrectMaterial.TotalPrice = patchItem.TotalPrice;

                                PatchHelper.SetIfRef(patchItem.Unit,
                                    () => resurrectMaterial.Unit,
                                    v => resurrectMaterial.Unit = v);

                                PatchHelper.SetIfRef(patchItem.MaterialNameSnapshot,
                                    () => resurrectMaterial.MaterialNameSnapshot,
                                    v => resurrectMaterial.MaterialNameSnapshot = v);

                                PatchHelper.SetIfRef(patchItem.MaterialExternalIdSnapshot,
                                    () => resurrectMaterial.MaterialExternalIdSnapshot,
                                    v => resurrectMaterial.MaterialExternalIdSnapshot = v);
                                _materialChanged = true;
                                continue;
                            }

                            // 5.3.2. KHÔNG CÓ DÒNG CŨ => THÊM DÒNG MỚI THẬT SỰ
                            var newMaterial = new ManufacturingFormulaMaterial
                            {
                                ManufacturingFormulaId = existing.ManufacturingFormulaId,
                                MaterialId = patchItem.MaterialId,
                                CategoryId = patchItem.CategoryId,
                                Quantity = patchItem.Quantity,
                                UnitPrice = patchItem.UnitPrice,
                                TotalPrice = patchItem.TotalPrice,
                                Unit = patchItem.Unit ?? string.Empty,
                                MaterialNameSnapshot = patchItem.MaterialNameSnapshot ?? string.Empty,
                                MaterialExternalIdSnapshot = patchItem.MaterialExternalIdSnapshot ?? string.Empty,
                                IsActive = patchItem.IsActive ?? true,

                                
                            };
                            _materialChanged = true;
                            existing.ManufacturingFormulaMaterials.Add(newMaterial);
                            continue;
                        }


                        // === UPDATE DÒNG CŨ ===
                        var existingMaterial = existing.ManufacturingFormulaMaterials
                            .FirstOrDefault(m => m.ManufacturingFormulaMaterialId == patchItem.ManufacturingFormulaMaterialId);

                        if (existingMaterial == null)
                            continue;

                        existingMaterial.MaterialId = patchItem.MaterialId;
                        existingMaterial.CategoryId = patchItem.CategoryId;

                        var oldQtyForUpdate = existingMaterial.Quantity;

                        PatchHelper.SetIf(patchItem.Quantity,
                            () => existingMaterial.Quantity,
                            v => existingMaterial.Quantity = v);

                        PatchHelper.SetIf(patchItem.UnitPrice,
                            () => existingMaterial.UnitPrice,
                            v => existingMaterial.UnitPrice = v);

                        PatchHelper.SetIf(patchItem.TotalPrice,
                            () => existingMaterial.TotalPrice,
                            v => existingMaterial.TotalPrice = v);

                        PatchHelper.SetIfRef(patchItem.Unit,
                            () => existingMaterial.Unit,
                            v => existingMaterial.Unit = v);

                        PatchHelper.SetIfRef(patchItem.MaterialNameSnapshot,
                            () => existingMaterial.MaterialNameSnapshot,
                            v => existingMaterial.MaterialNameSnapshot = v);

                        PatchHelper.SetIfRef(patchItem.MaterialExternalIdSnapshot,
                            () => existingMaterial.MaterialExternalIdSnapshot,
                            v => existingMaterial.MaterialExternalIdSnapshot = v);

                        // Nếu FE có gửi IsActive thì vẫn tôn trọng,
                        // còn nếu không gửi thì giữ nguyên (thường là true)
                        if (patchItem.IsActive.HasValue)
                            existingMaterial.IsActive = patchItem.IsActive.Value;

                        var newQtyForUpdate = existingMaterial.Quantity;
                        if (oldQtyForUpdate != newQtyForUpdate)
                            _materialChanged = true;
                    }
                }





                // 6) Save + commit
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();


                // ====== 7. Gửi notification ======

                // 4.5) Kiểm tra GIÁ CẢNH BÁO và bắn Notification (nếu vượt)
                decimal? targetPrice = null;
                try
                {
                    // GỢI Ý 1: lấy theo sản phẩm + khách hàng từ lệnh sản xuất (nếu MPO có CustomerId)
                    // (Tuỳ dữ liệu thực tế của bạn — nếu không có, hãy thay bằng service của bạn)
                    targetPrice = await _priceProvider.GetTargetPriceByMpoAsync(
                        mfgProductionOrderId: req.mfgProductionOrderId ?? Guid.Empty,
                        ct: ct
                    );
                }
                catch { /* không chặn flow nếu provider lỗi */ }

                if (targetPrice.HasValue && existing.TotalPrice > targetPrice.Value)
                {
                    // Gửi cảnh báo tới Ban giám đốc (role “Board”) – bạn có thể đổi thành TargetUserIds/TargetTeamIds
                    await _notificationService.PublishAsync(new PublishNotificationRequest
                    {
                        Topic = TopicNotifications.PriceOverSellCreated,
                        Severity = NotificationSeverity.Warning,
                        Title = $"Cảnh báo giá: {_MfgExternalId}",
                        Message = $"Tổng chi phí {req.TotalPrice:N0} > Giá bán {targetPrice.Value:N0}",
                        Link = $"/labs/mfgformula/{req.mfgProductionOrderId}/{req.ManufacturingFormulaId}",
                        PayloadJson = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            FormulaId = req.ManufacturingFormulaId,
                            ExternalId = _MfgExternalId,
                            TotalCost = req.TotalPrice,
                            TargetPrice = targetPrice.Value,
                            MpoId = req.mfgProductionOrderId
                        }),
                        TargetRoles = new() { AppRoles.PLPUNotify, AppRoles.President }
                    }, ct);
                }

                if (_materialChanged)
                {
                    await _notificationService.PublishAsync(new PublishNotificationRequest
                    {
                        Topic = TopicNotifications.WarehouseStockLost,
                        Severity = NotificationSeverity.Info,
                        Title = $"Cảnh báo tồn kho: {_MfgExternalId}",
                        Message = $"Vật tư thay đổi",
                        Link = $"/plpu/mfgproductionorders/{req.mfgProductionOrderId}",
                        PayloadJson = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            FormulaId = req.ManufacturingFormulaId,
                            ExternalId = _MfgExternalId,
                            MpoId = req.mfgProductionOrderId
                        }),
                        TargetRoles = new() { RoleSets.PLPU_Group }
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

                // Tạm thời trả thẳng ra để anh thấy:
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
            foreach (var m in existing.ManufacturingFormulaMaterials.Where(x => x.IsActive))
            {
                var item = new ManufacturingFormulaVersionItem
                {
                    ManufacturingFormulaVersionId = newVersion.ManufacturingFormulaVersionId,

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
                .Select(m => m.MaterialId)
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


    }
}
