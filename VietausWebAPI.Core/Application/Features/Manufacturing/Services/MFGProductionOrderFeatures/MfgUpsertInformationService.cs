using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.MFGProductionOrderFeatures;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
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
using VietausWebAPI.Core.Domain.Enums.Category;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;
using VietausWebAPI.Core.Domain.Enums.Merchadises;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services.MFGProductionOrderFeatures
{
    public class MfgUpsertInformationService : IMfgUpsertInformationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IExternalIdService _externalId;
        private readonly ITimelineService _timeLineService;
        private readonly INotificationService _notificationService;
        private readonly IPriceProvider _priceProvider;
        private readonly IWarehouseReservationService _warehouseReservationService;

        public MfgUpsertInformationService(
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IExternalIdService externalId,
            ITimelineService timeLineService,
            INotificationService notificationService,
            IPriceProvider priceProvider,
            IWarehouseReservationService warehouseReservationService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _externalId = externalId;
            _timeLineService = timeLineService;
            _notificationService = notificationService;
            _priceProvider = priceProvider;
            _warehouseReservationService = warehouseReservationService;
        }

        //============================================================ Update New ============================================================

        /// <summary>
        /// Xuât tồn kho cho lệnh sản xuất (MPO) - cập nhật thông tin MPO và chuyển trạng thái sang "Stocked".
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> ExportFromStockAsync(PatchStockMfgProductionOrderRequest req, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;

                var existingMfgOrderPO = await _unitOfWork.MfgOrderPORepository.Query(track: true)
                    .Where(x => x.MfgProductionOrderId == req.MfgProductionOrderId && x.IsActive)
                    .Include(x => x.ProductionOrder)
                    .Include(x => x.Detail)
                    .FirstOrDefaultAsync(ct);

                if (existingMfgOrderPO == null)
                    return OperationResult.Fail($"Không tìm thấy lệnh sản xuất với ID {req.MfgProductionOrderId}");

                var existing = existingMfgOrderPO.ProductionOrder;
                if (existing == null)
                    return OperationResult.Fail("Không tìm thấy dữ liệu Production Order.");

                // Validate nghiệp vụ
                if (!req.TotalQuantity.HasValue || req.TotalQuantity.Value <= 0)
                    return OperationResult.Fail("Khối lượng sản xuất phải lớn hơn 0.");

                existing.UpdatedDate = now;
                existing.UpdatedBy = userId;

                existing.StepOfProduct = req.StepOfProduct;

                PatchHelper.SetIfRef(req.PlpuNote, () => existing.PlpuNote, v => existing.PlpuNote = v);
                PatchHelper.SetIfRef(req.LabNote, () => existing.LabNote, v => existing.LabNote = v);
                PatchHelper.SetIfRef(req.Requirement, () => existing.Requirement, v => existing.Requirement = v);
                PatchHelper.SetIfRef(req.QcCheck, () => existing.QcCheck, v => existing.QcCheck = v);

                PatchHelper.SetIfNullable(req.ExpectedDate, () => existing.ExpectedDate, v => existing.ExpectedDate = v);
                PatchHelper.SetIfNullable(req.TotalQuantity, () => existing.TotalQuantity, v => existing.TotalQuantity = v);
                PatchHelper.SetIfNullable(req.NumOfBatches, () => existing.NumOfBatches, v => existing.NumOfBatches = v);

                // Action nghiệp vụ: Xuất tồn kho => BE tự set status
                var oldStatus = existing.Status;
                existing.Status = ManufacturingProductOrder.Stocked.ToString();

                if (!string.Equals(oldStatus, existing.Status, StringComparison.OrdinalIgnoreCase))
                {
                    await _timeLineService.AddEventLogAsync(new EventLogModels
                    {
                        employeeId = userId,
                        eventType = EventType.ManufacturingProductOrder,
                        sourceCode = existing.ExternalId ?? string.Empty,
                        sourceId = existing.MfgProductionOrderId,
                        status = existing.Status,
                        note = $"Xuất tồn kho bởi {_currentUser.personName} vào {now}"
                    }, ct);
                }

                // Đồng bộ schedule nếu có
                var schedule = await _unitOfWork.SchedualMfgRepository.Query(track: true)
                    .FirstOrDefaultAsync(s => s.MfgProductionOrderId == existing.MfgProductionOrderId, ct);

                if (schedule != null)
                {
                    schedule.DeliveryPlanDate = existing.ExpectedDate;
                }

                // Đồng bộ MerchandiseOrder nếu cần
                var detail = existingMfgOrderPO.Detail;
                if (detail != null)
                {
                    var relatedOrder = await _unitOfWork.MerchandiseOrderRepository
                        .Query(track: true)
                        .FirstOrDefaultAsync(o => o.MerchandiseOrderId == detail.MerchandiseOrderId, ct);

                    if (relatedOrder != null)
                    {
                        if (!Enum.TryParse<MerchadiseStatus>(relatedOrder.Status, true, out var roStatus))
                            roStatus = MerchadiseStatus.New;

                        if (roStatus == MerchadiseStatus.Approved || roStatus == MerchadiseStatus.New)
                        {
                            relatedOrder.Status = MerchadiseStatus.Processing.ToString();
                            relatedOrder.UpdatedDate = now;
                            relatedOrder.UpdatedBy = userId;

                            await _timeLineService.AddEventLogAsync(new EventLogModels
                            {
                                employeeId = userId,
                                eventType = EventType.MerchadiseStatus,
                                sourceCode = relatedOrder.ExternalId ?? string.Empty,
                                sourceId = relatedOrder.MerchandiseOrderId,
                                status = relatedOrder.Status,
                                note = $"Cập nhật bởi hệ thống vào {now} bởi {_currentUser.personName}"
                            }, ct);
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return OperationResult.Ok("Xuất tồn kho thành công.");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail("Có lỗi xảy ra trong quá trình xuất tồn kho.");
            }
        }


        /// <summary>
        /// Lưu thông tin cập nhật của lệnh sản xuất (MPO) đồng thời tạo mới một công thức sản xuất (Manufacturing Formula)
        /// và các dòng nguyên vật liệu của công thức đó.
        ///
        /// Luồng xử lý chính:
        /// 1. Kiểm tra dữ liệu đầu vào.
        /// 2. Mở transaction.
        /// 3. Nạp MPO hiện tại theo <see cref="PostMfgFormulaAndUpdateMpoRequest.MfgProductionOrderId"/>.
        /// 4. Cập nhật các thông tin cơ bản của MPO như:
        ///    - StepOfProduct
        ///    - LabNote
        ///    - Requirement
        ///    - PlpuNote
        ///    - QcCheck
        ///    - TotalQuantity
        ///    - NumOfBatches
        ///    - ExpectedDate
        ///    - ManufacturingDate
        /// 5. Tạo mới <see cref="ManufacturingFormula"/>.
        /// 6. Tạo các dòng <see cref="ManufacturingFormulaMaterial"/> và tự đánh <c>LineNo</c>
        ///    theo đúng thứ tự FE gửi lên.
        /// 7. Tạo liên kết <see cref="ProductionSelectVersion"/> giữa MPO và Formula:
        ///    - Nếu FormulaType là FromVu / Standard / Improvement:
        ///      tạo select ở trạng thái active (<c>ValidFrom = now</c>) và chuyển MPO sang <c>Scheduling</c>.
        ///    - Nếu FormulaType là ProductionOld / Production:
        ///      vẫn tạo select nhưng ở trạng thái chờ QC (<c>ValidFrom = null</c>, <c>ValidTo = null</c>)
        ///      và chuyển MPO sang <c>Waiting</c>.
        /// 8. Nếu MPO đã có schedule thì cập nhật lại thông tin schedule.
        /// 9. Ghi timeline nếu trạng thái MPO thay đổi.
        /// 10. Đồng bộ trạng thái sang MerchandiseOrder nếu cần.
        /// 11. Gửi cảnh báo giá nếu tổng giá công thức vượt giá mục tiêu.
        /// 12. Commit transaction.
        ///
        /// Lưu ý:
        /// - Hàm này vừa làm nhiệm vụ "save MPO" vừa "create formula".
        /// - Formula luôn được tạo mới.
        /// - Trạng thái "được phép sản xuất hay chưa" được phân biệt bằng record
        ///   <see cref="ProductionSelectVersion"/> thông qua cặp <c>ValidFrom</c>/<c>ValidTo</c>.
        /// </summary>
        /// <param name="req">
        /// Dữ liệu đầu vào bao gồm:
        /// - Thông tin cần cập nhật cho MPO
        /// - Thông tin nguồn gốc công thức
        /// - Danh sách nguyên vật liệu để tạo công thức mới
        /// - Loại công thức để quyết định nhánh nghiệp vụ tiếp theo
        /// </param>
        /// <param name="ct">CancellationToken dùng để hủy tác vụ bất đồng bộ nếu cần.</param>
        /// <returns>
        /// Trả về <see cref="OperationResult"/>:
        /// - Success nếu lưu MPO và tạo formula thành công
        /// - Fail nếu dữ liệu không hợp lệ hoặc có lỗi trong quá trình xử lý
        /// </returns>
        public async Task<OperationResult> SaveAndCreateFormulaAsync(PostMfgFormulaAndUpdateMpoRequest req, CancellationToken ct = default)
        {
            if (req.MfgProductionOrderId == Guid.Empty)
                return OperationResult.Fail("MfgProductionOrderId không hợp lệ.");

            if (req.ProductId == Guid.Empty)
                return OperationResult.Fail("ProductId không hợp lệ.");

            if (req.SourceType == null)
                return OperationResult.Fail("SourceType là bắt buộc.");

            if (req.ManufacturingFormulaMaterials == null || !req.ManufacturingFormulaMaterials.Any())
                return OperationResult.Fail("Công thức phải có ít nhất 1 nguyên vật liệu.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;
                var companyId = _currentUser.CompanyId;

                var existingMfgOrderPO = await _unitOfWork.MfgOrderPORepository.Query(track: true)
                    .Where(x => x.MfgProductionOrderId == req.MfgProductionOrderId && x.IsActive)
                    .Include(x => x.ProductionOrder)
                    .Include(x => x.Detail)
                    .FirstOrDefaultAsync(ct);

                if (existingMfgOrderPO == null)
                    return OperationResult.Fail($"Không tìm thấy lệnh sản xuất với ID {req.MfgProductionOrderId}");

                var mpo = existingMfgOrderPO.ProductionOrder;
                if (mpo == null)
                    return OperationResult.Fail("Không tìm thấy dữ liệu ProductionOrder.");

                var oldStatus = mpo.Status;

                // =====================================================
                // 1. UPDATE MPO
                // =====================================================
                mpo.UpdatedDate = now;
                mpo.UpdatedBy = userId;
                mpo.StepOfProduct = req.StepOfProduct;

                PatchHelper.SetIfRef(req.PlpuNote, () => mpo.PlpuNote, v => mpo.PlpuNote = v);
                PatchHelper.SetIfRef(req.LabNote, () => mpo.LabNote, v => mpo.LabNote = v);
                PatchHelper.SetIfRef(req.Requirement, () => mpo.Requirement, v => mpo.Requirement = v);
                PatchHelper.SetIfRef(req.QcCheck, () => mpo.QcCheck, v => mpo.QcCheck = v);

                PatchHelper.SetIfNullable(req.TotalQuantity, () => mpo.TotalQuantity, v => mpo.TotalQuantity = v);
                PatchHelper.SetIfNullable(req.NumOfBatches, () => mpo.NumOfBatches, v => mpo.NumOfBatches = v);
                PatchHelper.SetIfNullable(req.ExpectedDate, () => mpo.ExpectedDate, v => mpo.ExpectedDate = v);
                PatchHelper.SetIfNullable(req.ManufacturingDate, () => mpo.ManufacturingDate, v => mpo.ManufacturingDate = v);

                // =====================================================
                // 2. CREATE MANUFACTURING FORMULA
                // =====================================================
                var autoSelectAndScheduling =
                    req.FormulaType == FormulaType.FromVu ||
                    req.FormulaType == FormulaType.Standard ||
                    req.FormulaType == FormulaType.Improvement;

                var waitForQc =
                    req.FormulaType == FormulaType.ProductionOld ||
                    req.FormulaType == FormulaType.Production;

                var shouldCreateStandard =
                    req.FormulaType == FormulaType.FromVu ||
                    req.FormulaType == FormulaType.Improvement;

                var formulaStatus = autoSelectAndScheduling
                    ? ManufacturingProductOrderFormula.Checking.ToString()
                    : ManufacturingProductOrderFormula.New.ToString();

                var mf = new ManufacturingFormula
                {
                    ManufacturingFormulaId = Guid.CreateVersion7(),
                    ExternalId = await _externalId.NextAsync(DocumentPrefix.VA.ToString(), ct: ct),
                    Name = "F001",

                    Status = formulaStatus,
                    TotalPrice = req.ManufacturingFormulaMaterials.Sum(x => x.UnitPrice * x.Quantity),

                    SourceType = req.SourceType.Value,
                    IsActive = true,
                    Note = req.Note,

                    CreatedDate = now,
                    UpdatedDate = now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    CompanyId = companyId
                };

                switch (req.SourceType)
                {
                    case FormulaSource.FromVA:
                        mf.SourceManufacturingFormulaId = req.FormulaSourceId;
                        mf.SourceManufacturingExternalIdSnapshot = req.SourceManufacturingExternalIdSnapshot;
                        mf.SourceVUFormulaId = req.VUFormulaId;
                        mf.SourceVUExternalIdSnapshot = req.FormulaExternalIdSnapshot;
                        break;

                    case FormulaSource.FromVU:
                        mf.SourceVUFormulaId = req.VUFormulaId;
                        mf.SourceVUExternalIdSnapshot = req.FormulaExternalIdSnapshot;
                        break;
                }

                await _unitOfWork.ManufacturingFormulaRepository.AddAsync(mf, ct);

                // =====================================================
                // 3. CREATE FORMULA MATERIALS
                // =====================================================
                var materialEntities = req.ManufacturingFormulaMaterials
                    .Select((m, index) => new ManufacturingFormulaMaterial
                    {
                        ManufacturingFormulaMaterialId = Guid.CreateVersion7(),
                        ManufacturingFormulaId = mf.ManufacturingFormulaId,

                        itemType = m.ItemType,
                        MaterialId = m.ItemType == ItemType.Material ? m.ItemId : (Guid?)null,
                        ProductId = m.ItemType == ItemType.Product ? m.ItemId : (Guid?)null,

                        CategoryId = m.CategoryId,
                        LineNo = index + 1,

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

                // =====================================================
                // 4. SET STANDARD IF NEEDED
                // =====================================================
                //if (shouldCreateStandard)
                //{
                //    var currentStd = await _unitOfWork.ProductStandardFormulaRepository.Query(track: true)
                //        .Where(x => x.CompanyId == companyId
                //                    && x.ProductId == req.ProductId
                //                    && x.ValidTo == null)
                //        .FirstOrDefaultAsync(ct);

                //    if (currentStd != null)
                //    {
                //        currentStd.ValidTo = now;
                //        currentStd.ClosedBy = userId;
                //        _unitOfWork.ProductStandardFormulaRepository.UpdateAsync(currentStd, ct);
                //    }

                //    var newStd = new ProductStandardFormula
                //    {
                //        ProductStandardFormulaId = Guid.CreateVersion7(),
                //        ProductId = req.ProductId,
                //        ManufacturingFormulaId = mf.ManufacturingFormulaId,
                //        ValidFrom = now,
                //        ValidTo = null,
                //        CreatedBy = userId,
                //        ClosedBy = null,
                //        CompanyId = companyId,
                //        Note = req.Note
                //    };

                //    await _unitOfWork.ProductStandardFormulaRepository.AddAsync(newStd, ct);
                //}

                // =====================================================
                // 5. APPLY FLOW BY FORMULA TYPE
                // =====================================================

                if (autoSelectAndScheduling)
                {
                    await CreateNewSelectedFormulaVersionAsync(
                        mpo.MfgProductionOrderId,
                        mf.ManufacturingFormulaId,
                        companyId,
                        now,
                        userId,
                        isActiveSelect: true,
                        ct);

                    mpo.Status = ManufacturingProductOrder.Scheduling.ToString();
                    mpo.UpdatedDate = now;
                    mpo.UpdatedBy = userId;

                    await EnsureScheduleAsync(mpo, now, ct);

                    // =====================================================
                    // 5.1 RESERVE VIRTUAL STOCK
                    // chỉ reserve khi formula mới được chọn active để sản xuất
                    // =====================================================
                    var totalQty = mpo.TotalQuantity ?? 0m;
                    if (totalQty > 0)
                    {
                        var reserveResult = await _warehouseReservationService.ReserveByFormulaMaterialsAsync(
                            mpo.MfgProductionOrderId,
                            totalQty,
                            req.ManufacturingFormulaMaterials,
                            ct);

                        if (!reserveResult.Success)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return OperationResult.Fail(reserveResult.Message ?? "Không thể cập nhật tồn kho ảo.");
                        }
                    }
                }
                else if (waitForQc)
                {
                    await CreateNewSelectedFormulaVersionAsync(
                        mpo.MfgProductionOrderId,
                        mf.ManufacturingFormulaId,
                        companyId,
                        now,
                        userId,
                        isActiveSelect: false,
                        ct);

                    mpo.Status = ManufacturingProductOrder.FormulaRequested.ToString();
                    mpo.UpdatedDate = now;
                    mpo.UpdatedBy = userId;
                }

                // =====================================================
                // 6. UPDATE SCHEDULE IF EXISTS
                // =====================================================
                var schedule = await _unitOfWork.SchedualMfgRepository.Query(track: true)
                    .Where(s => s.MfgProductionOrderId == mpo.MfgProductionOrderId)
                    .FirstOrDefaultAsync(ct);

                if (schedule != null)
                {
                    schedule.DeliveryPlanDate = mpo.ExpectedDate;
                    schedule.Note = mpo.LabNote ?? mpo.PlpuNote;
                    schedule.requirement = mpo.Requirement;
                }

                // =====================================================
                // 7. TIMELINE MPO WHEN STATUS CHANGED
                // =====================================================
                if (!string.Equals(oldStatus, mpo.Status, StringComparison.OrdinalIgnoreCase))
                {
                    await _timeLineService.AddEventLogAsync(new EventLogModels
                    {
                        employeeId = userId,
                        eventType = EventType.ManufacturingProductOrder,
                        sourceCode = mpo.ExternalId ?? string.Empty,
                        sourceId = mpo.MfgProductionOrderId,
                        status = mpo.Status,
                        note = $"Cập nhật bởi hệ thống vào {now} bởi {_currentUser.personName}"
                    }, ct);
                }

                // =====================================================
                // 8. SYNC MERCHANDISE ORDER STATUS
                // =====================================================
                await SyncMerchandiseOrderAsync(existingMfgOrderPO, now, userId, ct);

                // =====================================================
                // 9. PRICE WARNING
                // =====================================================
                await TrySendPriceWarningAsync(req, mf, mpo, ct);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return OperationResult.Ok("Lưu và tạo công thức thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Có lỗi xảy ra trong quá trình lưu và tạo công thức: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo mới một bản ghi <see cref="ProductionSelectVersion"/> để liên kết giữa MPO và Manufacturing Formula.
        ///
        /// Ý nghĩa của tham số <paramref name="isActiveSelect"/>:
        /// - true:
        ///   công thức được chọn chính thức để sản xuất ngay,
        ///   do đó <c>ValidFrom = now</c>, <c>ValidTo = null</c>.
        /// - false:
        ///   công thức đã được gắn vào MPO nhưng chưa được phép sản xuất,
        ///   thường dùng cho luồng chờ QC,
        ///   do đó <c>ValidFrom = null</c>, <c>ValidTo = null</c>.
        ///
        /// Lưu ý:
        /// Hàm này chỉ tạo liên kết version mới, không tự đóng version cũ.
        /// Nếu nghiệp vụ yêu cầu chỉ có 1 version mở tại một thời điểm,
        /// cần xử lý đóng version cũ ở hàm gọi trước khi tạo version mới.
        /// </summary>
        /// <param name="mpoId">Id của lệnh sản xuất (MPO) cần gắn công thức.</param>
        /// <param name="formulaId">Id của công thức sản xuất vừa tạo hoặc cần liên kết.</param>
        /// <param name="companyId">Id công ty hiện hành.</param>
        /// <param name="now">Thời điểm hiện tại dùng làm mốc hiệu lực nếu version được active ngay.</param>
        /// <param name="userId">Id người thực hiện thao tác tạo version.</param>
        /// <param name="isActiveSelect">
        /// Xác định version vừa tạo có hiệu lực ngay hay không:
        /// true = chọn chính thức để sản xuất,
        /// false = chỉ gắn tạm/chờ QC.
        /// </param>
        /// <param name="ct">CancellationToken dùng để hủy tác vụ bất đồng bộ nếu cần.</param>
        /// <returns>Task bất đồng bộ hoàn tất khi bản ghi version đã được thêm vào repository.</returns>
        private async Task CreateNewSelectedFormulaVersionAsync(Guid mpoId, Guid formulaId, Guid companyId, DateTime now, Guid userId, bool isActiveSelect, CancellationToken ct)
        {
            var newPsv = new ProductionSelectVersion
            {
                ProductionSelectVersionId = Guid.CreateVersion7(),
                MfgProductionOrderId = mpoId,
                ManufacturingFormulaId = formulaId,
                ValidFrom = isActiveSelect ? now : null,
                ValidTo = null,
                CreatedBy = userId,
                ClosedBy = null,
                CompanyId = companyId
            };

            await _unitOfWork.ProductionSelectVersionRepository.AddAsync(newPsv, ct);
        }


        /// <summary>
        /// Đảm bảo lệnh sản xuất đã có bản ghi schedule trong bảng <see cref="SchedualMfg"/>.
        ///
        /// Nếu MPO đã có schedule thì hàm không tạo thêm.
        /// Nếu chưa có, hàm sẽ tạo mới một schedule với các thông tin cơ bản lấy từ MPO:
        /// - ProductId
        /// - Requirement
        /// - Note
        /// - ExpectedDate
        ///
        /// Hàm này thường được gọi khi công thức đã được chọn chính thức
        /// và MPO được chuyển sang trạng thái <c>Scheduling</c>.
        /// </summary>
        /// <param name="mpo">Thực thể lệnh sản xuất đang xử lý.</param>
        /// <param name="now">Thời điểm hiện tại dùng làm CreatedDate cho schedule mới.</param>
        /// <param name="ct">CancellationToken dùng để hủy tác vụ bất đồng bộ nếu cần.</param>
        /// <returns>Task bất đồng bộ hoàn tất khi kiểm tra/tạo schedule xong.</returns>
        private async Task EnsureScheduleAsync(MfgProductionOrder mpo, DateTime now, CancellationToken ct)
        {
            var scheduleExists = await _unitOfWork.SchedualMfgRepository
                .Query(track: true)
                .AnyAsync(s => s.MfgProductionOrderId == mpo.MfgProductionOrderId, ct);

            if (scheduleExists) return;

            var schedual = new SchedualMfg
            {
                MfgProductionOrderId = mpo.MfgProductionOrderId,
                ProductId = mpo.ProductId,
                requirement = mpo.Requirement,
                Note = mpo.LabNote ?? mpo.PlpuNote,
                DeliveryPlanDate = mpo.ExpectedDate,
                CreatedDate = now,
                Status = ManufacturingProductOrder.Scheduling.ToString(),
                Qcstatus = null,
                Area = null,
                BTPStatus = null,
                StepOfProduct = null
            };

            await _unitOfWork.SchedualMfgRepository.AddAsync(schedual, ct);
        }


        /// <summary>
        /// Đồng bộ trạng thái của <see cref="MerchandiseOrder"/> liên quan khi MPO thay đổi theo luồng nghiệp vụ.
        ///
        /// Rule hiện tại:
        /// - Nếu MPO có liên kết tới chi tiết đơn hàng bán (<see cref="MfgOrderPO.Detail"/>),
        /// - và đơn hàng bán hiện đang ở trạng thái <c>Approved</c> hoặc <c>New</c>,
        ///   thì chuyển sang <c>Processing</c>.
        ///
        /// Đồng thời ghi timeline cho MerchandiseOrder sau khi cập nhật.
        ///
        /// Hàm này không ném lỗi nếu không tìm thấy detail hoặc không tìm thấy order liên quan;
        /// trong các trường hợp đó hàm sẽ thoát im lặng.
        /// </summary>
        /// <param name="existingMfgOrderPO">
        /// Bản ghi liên kết giữa MPO và đơn hàng bán, đã include sẵn ProductionOrder/Detail.
        /// </param>
        /// <param name="now">Thời điểm hiện tại dùng để cập nhật UpdatedDate và ghi timeline.</param>
        /// <param name="userId">Id người thực hiện thao tác cập nhật.</param>
        /// <param name="ct">CancellationToken dùng để hủy tác vụ bất đồng bộ nếu cần.</param>
        /// <returns>Task bất đồng bộ hoàn tất khi xử lý đồng bộ trạng thái xong.</returns>
        private async Task SyncMerchandiseOrderAsync(
            MfgOrderPO existingMfgOrderPO,
            DateTime now,
            Guid userId,
            CancellationToken ct)
        {
            var detail = existingMfgOrderPO.Detail;
            if (detail == null) return;

            var relatedOrder = await _unitOfWork.MerchandiseOrderRepository
                .Query(track: true)
                .FirstOrDefaultAsync(o => o.MerchandiseOrderId == detail.MerchandiseOrderId, ct);

            if (relatedOrder == null) return;

            if (!Enum.TryParse<MerchadiseStatus>(relatedOrder.Status, true, out var roStatus))
                roStatus = MerchadiseStatus.New;

            if (roStatus != MerchadiseStatus.Approved && roStatus != MerchadiseStatus.New)
                return;

            relatedOrder.Status = MerchadiseStatus.Processing.ToString();
            relatedOrder.UpdatedDate = now;
            relatedOrder.UpdatedBy = userId;

            await _timeLineService.AddEventLogAsync(new EventLogModels
            {
                employeeId = userId,
                eventType = EventType.MerchadiseStatus,
                sourceCode = relatedOrder.ExternalId ?? string.Empty,
                sourceId = relatedOrder.MerchandiseOrderId,
                status = relatedOrder.Status,
                note = $"Cập nhật bởi hệ thống vào {now} bởi {_currentUser.personName}"
            }, ct);
        }

        /// <summary>
        /// Kiểm tra chênh lệch giữa tổng giá công thức vừa tạo và giá mục tiêu của MPO.
        /// Nếu tổng giá công thức vượt giá mục tiêu thì gửi notification cảnh báo.
        ///
        /// Hàm này chỉ mang tính hỗ trợ cảnh báo, không được phép làm fail luồng chính.
        /// Vì vậy toàn bộ logic bên trong được bọc try/catch và sẽ bỏ qua im lặng nếu xảy ra lỗi.
        ///
        /// Notification được gửi tới các role đã cấu hình, ví dụ:
        /// - PLPUNotify
        /// - President
        /// </summary>
        /// <param name="req">Request gốc chứa thông tin MfgProductionOrderId để truy xuất giá mục tiêu.</param>
        /// <param name="mf">Công thức vừa tạo, dùng để lấy ExternalId, FormulaId và TotalPrice.</param>
        /// <param name="mpo">Lệnh sản xuất hiện tại, dùng để tạo link điều hướng và payload cảnh báo.</param>
        /// <param name="ct">CancellationToken dùng để hủy tác vụ bất đồng bộ nếu cần.</param>
        /// <returns>Task bất đồng bộ hoàn tất khi xử lý kiểm tra/gửi cảnh báo xong.</returns>
        private async Task TrySendPriceWarningAsync(
            PostMfgFormulaAndUpdateMpoRequest req,
            ManufacturingFormula mf,
            MfgProductionOrder mpo,
            CancellationToken ct)
        {
            try
            {
                decimal? targetPrice = await _priceProvider.GetTargetPriceByMpoAsync(
                    mfgProductionOrderId: req.MfgProductionOrderId,
                    ct: ct
                );

                if (!targetPrice.HasValue || mf.TotalPrice <= targetPrice.Value)
                    return;

                await _notificationService.PublishAsync(new PublishNotificationRequest
                {
                    Topic = TopicNotifications.PriceOverSellCreated,
                    Severity = NotificationSeverity.Warning,
                    Title = $"Cảnh báo giá: {mf.ExternalId}",
                    Message = $"Tổng chi phí {mf.TotalPrice:N0} > Giá bán {targetPrice.Value:N0}",
                    Link = $"/labs/mfgformula/{mpo.MfgProductionOrderId}/{mf.ManufacturingFormulaId}",
                    PayloadJson = JsonSerializer.Serialize(new
                    {
                        FormulaId = mf.ManufacturingFormulaId,
                        ExternalId = mf.ExternalId,
                        TotalCost = mf.TotalPrice,
                        TargetPrice = targetPrice.Value,
                        MpoId = req.MfgProductionOrderId
                    }),
                    TargetRoles = new() { AppRoles.PLPUNotify, AppRoles.President }
                }, ct);
            }
            catch
            {
            }
        }


        //============================================================ Update Information ============================================================

        public async Task<OperationResult> PatchInformAsync(PatchMfgProductionOrderInformRequest req, CancellationToken ct = default)
        {
            if (req == null || req.MfgProductionOrderId == Guid.Empty)
                return OperationResult.Fail("Dữ liệu không hợp lệ.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;

                var mpo = await _unitOfWork.MfgProductionOrderRepository
                    .Query(track: true)
                    .FirstOrDefaultAsync(x => x.MfgProductionOrderId == req.MfgProductionOrderId && x.IsActive, ct);

                if (mpo == null)
                    return OperationResult.Fail("Không tìm thấy lệnh sản xuất.");

                var oldStatus = mpo.Status;



                // =====================================================
                // TIMELINE MPO WHEN STATUS CHANGED
                // =====================================================
                if (!string.Equals(oldStatus, mpo.Status, StringComparison.OrdinalIgnoreCase))
                {
                    await _timeLineService.AddEventLogAsync(new EventLogModels
                    {
                        employeeId = userId,
                        eventType = EventType.ManufacturingProductOrder,
                        sourceCode = mpo.ExternalId ?? string.Empty,
                        sourceId = mpo.MfgProductionOrderId,
                        status = mpo.Status,
                        note = $"Cập nhật bởi hệ thống vào {now} bởi {_currentUser.personName}"
                    }, ct);
                }

                mpo.UpdatedDate = now;
                mpo.UpdatedBy = userId;

                var oldExpectedDate = mpo.ExpectedDate;

                PatchHelper.SetIfNullable(req.ManufacturingDate, () => mpo.ManufacturingDate, v => mpo.ManufacturingDate = v);
                PatchHelper.SetIfNullable(req.ExpectedDate, () => mpo.ExpectedDate, v => mpo.ExpectedDate = v);
                PatchHelper.SetIfNullable(req.TotalQuantity, () => mpo.TotalQuantity, v => mpo.TotalQuantity = v);
                PatchHelper.SetIfNullable(req.NumOfBatches, () => mpo.NumOfBatches, v => mpo.NumOfBatches = v);

                PatchHelper.SetIfRef(req.LabNote, () => mpo.LabNote, v => mpo.LabNote = v);
                PatchHelper.SetIfRef(req.Requirement, () => mpo.Requirement, v => mpo.Requirement = v);
                PatchHelper.SetIfRef(req.PlpuNote, () => mpo.PlpuNote, v => mpo.PlpuNote = v);
                PatchHelper.SetIfRef(req.QcCheck, () => mpo.QcCheck, v => mpo.QcCheck = v);
                PatchHelper.SetIfRef(req.BagType, () => mpo.BagType, v => mpo.BagType = v);

                mpo.StepOfProduct = req.StepOfProduct;

                ManufacturingFormula? formula = null;
                bool formulaItemsChanged = false;
                bool formulaSelectionChanged = false;

                var expectedDateChanged =
                    req.ExpectedDate.HasValue &&
                    oldExpectedDate?.Date != mpo.ExpectedDate?.Date;

                if (expectedDateChanged
    && mpo.ExpectedDate.HasValue
    && mpo.ExpectedDate.Value.Date > req.RequiredDate.Date)
                {
                    await TryNotifySaleWhenExpectedDateExceededAsync(req, mpo, oldExpectedDate, now, ct);
                }

                // dùng để sync reserve
                List<PatchMfgProductionOrderFormulaItemRequest>? formulaItemsForReserve = null;

                if (req.ManufacturingFormulaIdIsSelect.HasValue && req.ManufacturingFormulaIdIsSelect.Value != Guid.Empty)
                {
                    if (req.FormulaItems == null)
                        return OperationResult.Fail("Thiếu danh sách nguyên vật liệu công thức.");

                    var formulaId = req.ManufacturingFormulaIdIsSelect.Value;

                    var currentSelectedFormulaId = await _unitOfWork.ProductionSelectVersionRepository
                        .Query(track: false)
                        .Where(x => x.MfgProductionOrderId == mpo.MfgProductionOrderId && x.ValidTo == null)
                        .OrderByDescending(x => x.ValidFrom ?? DateTime.MinValue)
                        .Select(x => (Guid?)x.ManufacturingFormulaId)
                        .FirstOrDefaultAsync(ct);

                    if (currentSelectedFormulaId == null)
                        return OperationResult.Fail("Lệnh sản xuất hiện chưa có công thức VA đang chọn.");

                    if (currentSelectedFormulaId.Value != formulaId)
                        return OperationResult.Fail("Công thức gửi lên không phải công thức đang được select của lệnh sản xuất.");

                    formula = await _unitOfWork.ManufacturingFormulaRepository
                        .Query(track: true)
                        .FirstOrDefaultAsync(x => x.ManufacturingFormulaId == formulaId && x.IsActive, ct);

                    if (formula == null)
                        return OperationResult.Fail("Không tìm thấy công thức VA cần cập nhật.");

                    formula.UpdatedDate = now;
                    formula.UpdatedBy = userId;
                    formula.TotalPrice = req.FormulaItems
                        .Where(x => x.IsActive && x.Quantity > 0)
                        .Sum(x => x.Quantity * x.UnitPrice);

                    var existingItems = await _unitOfWork.ManufacturingFormulaMaterialRepository
                        .Query(track: true)
                        .Where(x => x.ManufacturingFormulaId == formula.ManufacturingFormulaId)
                        .ToListAsync(ct);

                    var requestIds = req.FormulaItems
                        .Where(x => x.ManufacturingFormulaMaterialId.HasValue && x.ManufacturingFormulaMaterialId.Value != Guid.Empty)
                        .Select(x => x.ManufacturingFormulaMaterialId!.Value)
                        .ToHashSet();

                    foreach (var dbItem in existingItems)
                    {
                        if (!requestIds.Contains(dbItem.ManufacturingFormulaMaterialId))
                        {
                            // chỉ đánh dấu inactive, chưa reserve ngay ở đây
                            if (dbItem.IsActive)
                            {
                                dbItem.IsActive = false;
                                formulaItemsChanged = true;
                            }
                        }
                    }

                    foreach (var item in req.FormulaItems.OrderBy(x => x.LineNo))
                    {
                        if (item.ItemId == Guid.Empty)
                            return OperationResult.Fail("Có nguyên vật liệu / bán thành phẩm không hợp lệ.");

                        if (item.Quantity <= 0)
                            return OperationResult.Fail("Số lượng công thức phải lớn hơn 0.");

                        ManufacturingFormulaMaterial? entity = null;

                        if (item.ManufacturingFormulaMaterialId.HasValue && item.ManufacturingFormulaMaterialId.Value != Guid.Empty)
                        {
                            entity = existingItems.FirstOrDefault(x =>
                                x.ManufacturingFormulaMaterialId == item.ManufacturingFormulaMaterialId.Value);
                        }

                        if (entity == null)
                        {
                            entity = new ManufacturingFormulaMaterial
                            {
                                ManufacturingFormulaMaterialId = Guid.CreateVersion7(),
                                ManufacturingFormulaId = formula.ManufacturingFormulaId
                            };

                            await _unitOfWork.ManufacturingFormulaMaterialRepository.AddAsync(entity, ct);
                            formulaItemsChanged = true;
                        }
                        else
                        {
                            // so sánh trước/sau để chỉ set changed khi thực sự đổi
                            if (entity.itemType != item.ItemType
                                || entity.MaterialId != (item.ItemType == ItemType.Material ? item.ItemId : null)
                                || entity.ProductId != (item.ItemType == ItemType.Product ? item.ItemId : null)
                                || entity.CategoryId != item.CategoryId
                                || entity.LineNo != item.LineNo
                                || entity.Quantity != item.Quantity
                                || entity.UnitPrice != item.UnitPrice
                                || entity.MaterialNameSnapshot != item.MaterialNameSnapshot
                                || entity.MaterialExternalIdSnapshot != item.MaterialExternalIdSnapshot
                                || entity.Unit != item.Unit
                                || entity.IsActive != item.IsActive)
                            {
                                formulaItemsChanged = true;
                            }
                        }

                        entity.itemType = item.ItemType;
                        entity.MaterialId = item.ItemType == ItemType.Material ? item.ItemId : null;
                        entity.ProductId = item.ItemType == ItemType.Product ? item.ItemId : null;
                        entity.CategoryId = item.CategoryId;
                        entity.LineNo = item.LineNo;
                        entity.Quantity = item.Quantity;
                        entity.UnitPrice = item.UnitPrice;
                        entity.TotalPrice = item.Quantity * item.UnitPrice;
                        entity.MaterialNameSnapshot = item.MaterialNameSnapshot;
                        entity.MaterialExternalIdSnapshot = item.MaterialExternalIdSnapshot;
                        entity.Unit = item.Unit;
                        entity.IsActive = item.IsActive;
                    }

                    formulaItemsForReserve = req.FormulaItems
                        .Where(x => x.IsActive && x.Quantity > 0)
                        .OrderBy(x => x.LineNo)
                        .ToList();
                }
                else
                {
                    // không patch formula items thì không reserve lại
                    formulaItemsForReserve = null;
                }

                // Nếu công thức đã thành công thì chuyển sang Scheduling
                if (mpo.Status == ManufacturingProductOrder.FormulaSuccess.ToString())
                {
                    mpo.Status = ManufacturingProductOrder.Scheduling.ToString();
                }

                // đảm bảo có schedule nếu chưa Stocked và formula đang ở trạng thái chờ công thức
                if (
                    !string.Equals(mpo.Status, ManufacturingProductOrder.FormulaRequested.ToString(), StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(mpo.Status, ManufacturingProductOrder.Stocked.ToString(), StringComparison.OrdinalIgnoreCase)
                   )
                {
                    await EnsureScheduleAsync(mpo, now, ct);
                }

                var schedule = await _unitOfWork.SchedualMfgRepository
                    .Query(track: true)
                    .FirstOrDefaultAsync(x => x.MfgProductionOrderId == mpo.MfgProductionOrderId, ct);

                if (schedule != null)
                {
                    schedule.DeliveryPlanDate = mpo.ExpectedDate;
                    schedule.Note = mpo.LabNote ?? mpo.PlpuNote;
                    schedule.requirement = mpo.Requirement;
                    schedule.StepOfProduct = mpo.StepOfProduct;
                }

                // =====================================================
                // SYNC RESERVE VIRTUAL STOCK
                // Chỉ sync khi:
                // - có FormulaItems request
                // - và TotalQuantity đổi, hoặc item đổi, hoặc vừa chuyển sang Scheduling
                // =====================================================
                var enteredSchedulingNow =
                    !string.Equals(oldStatus, ManufacturingProductOrder.Scheduling.ToString(), StringComparison.OrdinalIgnoreCase)
                    && string.Equals(mpo.Status, ManufacturingProductOrder.Scheduling.ToString(), StringComparison.OrdinalIgnoreCase);

                var shouldSyncReservation =
                    formulaItemsForReserve != null
                    && formulaItemsForReserve.Count > 0
                    && (mpo.TotalQuantity ?? 0m) > 0
                    && (
                        req.TotalQuantity.HasValue
                        || formulaItemsChanged
                        || formulaSelectionChanged
                        || enteredSchedulingNow
                    );

                if (shouldSyncReservation)
                {
                    var reserveResult = await _warehouseReservationService.SyncReservationsByFormulaItemsAsync(
                        mpo.MfgProductionOrderId,
                        mpo.TotalQuantity ?? 0m,
                        formulaItemsForReserve,
                        ct);

                    if (!reserveResult.Success)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return OperationResult.Fail(reserveResult.Message ?? "Không thể cập nhật tồn kho ảo.");
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return OperationResult.Ok("Cập nhật thông tin và công thức VA thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Có lỗi xảy ra: {ex.Message}");
            }
        }


        //============================================================= Copy MFGProductionOrder =============================================================

    //    public async Task<OperationResult<CopyMfgProductionOrderResult>> CopyAsync(
    //PostCopyMfgProductionOrderRequest req,
    //CancellationToken ct = default)
    //    {
    //        await using var tx = await _unitOfWork.BeginTransactionAsync();

    //        try
    //        {
    //            var now = DateTime.Now;
    //            var userId = _currentUser.EmployeeId;

    //            var source = await _unitOfWork.MfgProductionOrderRepository.Query(track: true)
    //                .FirstOrDefaultAsync(x => x.MfgProductionOrderId == req.SourceMfgProductionOrderId && x.IsActive, ct);

    //            if (source == null)
    //                return OperationResult<CopyMfgProductionOrderResult>.Fail("Không tìm thấy lệnh sản xuất nguồn.");

    //            var sourceLink = await _unitOfWork.MfgOrderPORepository.Query(track: true)
    //                .FirstOrDefaultAsync(x => x.MfgProductionOrderId == source.MfgProductionOrderId && x.IsActive, ct);

    //            if (sourceLink == null)
    //                return OperationResult<CopyMfgProductionOrderResult>.Fail("Không tìm thấy liên kết đơn hàng của lệnh nguồn.");

    //            var newId = Guid.CreateVersion7();
    //            var newExternalId = await _externalId.NextAsync(DocumentPrefix.MFG.ToString(), ct: ct);

    //            var clone = new MfgProductionOrder
    //            {
    //                MfgProductionOrderId = newId,
    //                ExternalId = newExternalId,

    //                ProductId = source.ProductId,
    //                ProductExternalIdSnapshot = source.ProductExternalIdSnapshot,
    //                ProductNameSnapshot = source.ProductNameSnapshot,
    //                ColorName = source.ColorName,

    //                CustomerId = source.CustomerId,
    //                CustomerExternalIdSnapshot = source.CustomerExternalIdSnapshot,
    //                CustomerNameSnapshot = source.CustomerNameSnapshot,

    //                FormulaId = source.FormulaId,
    //                FormulaExternalIdSnapshot = source.FormulaExternalIdSnapshot,

    //                ManufacturingDate = null,
    //                RequiredDate = source.RequiredDate,
    //                ExpectedDate = req.ExpectedDate ?? source.ExpectedDate,

    //                TotalQuantityRequest = source.TotalQuantityRequest,
    //                TotalQuantity = req.TotalQuantity ?? source.TotalQuantity,
    //                NumOfBatches = req.NumOfBatches ?? source.NumOfBatches,

    //                UnitPriceAgreed = source.UnitPriceAgreed,
    //                Status = ManufacturingProductOrder.New.ToString(),

    //                StepOfProduct = req.StepOfProduct ?? source.StepOfProduct,
    //                LabNote = req.LabNote ?? source.LabNote,
    //                Requirement = req.Requirement ?? source.Requirement,
    //                PlpuNote = req.PlpuNote ?? source.PlpuNote,
    //                QcCheck = req.QcCheck ?? source.QcCheck,

    //                BagType = source.BagType,
    //                IsActive = true,
    //                CompanyId = source.CompanyId,
    //                CreatedDate = now,
    //                CreatedBy = userId,
    //                UpdatedDate = now,
    //                UpdatedBy = userId
    //            };

    //            var newLink = new MfgOrderPO
    //            {
    //                MfgProductionOrderId = newId,
    //                MerchandiseOrderDetailId = sourceLink.MerchandiseOrderDetailId,
    //                IsActive = true
    //            };

    //            await _unitOfWork.MfgProductionOrderRepository.AddAsync(clone, ct);
    //            await _unitOfWork.MfgOrderPORepository.AddAsync(newLink, ct);

    //            await _TimelineService.AddEventLogAsync(new EventLogModels
    //            {
    //                employeeId = userId,
    //                eventType = EventType.ManufacturingProductOrder,
    //                sourceCode = clone.ExternalId,
    //                sourceId = clone.MfgProductionOrderId,
    //                status = clone.Status,
    //                note = $"Copy từ lệnh sản xuất {source.ExternalId}"
    //            }, ct);

    //            await _unitOfWork.SaveChangesAsync();
    //            await tx.CommitAsync(ct);

    //            return OperationResult<CopyMfgProductionOrderResult>.Ok(
    //                new CopyMfgProductionOrderResult
    //                {
    //                    MfgProductionOrderId = clone.MfgProductionOrderId,
    //                    ExternalId = clone.ExternalId
    //                },
    //                "Copy lệnh sản xuất thành công.");
    //        }
    //        catch (Exception ex)
    //        {
    //            await tx.RollbackAsync(ct);
    //            return OperationResult<CopyMfgProductionOrderResult>.Fail($"Copy lệnh sản xuất thất bại: {ex.Message}");
    //        }
    //    }










        //============================================================== TEST NOTIFICATION =============================================================
        private async Task TryNotifySaleWhenExpectedDateExceededAsync(PatchMfgProductionOrderInformRequest req, MfgProductionOrder mpo, DateTime? oldExpectedDate, DateTime now, CancellationToken ct)
        {
            // 1. Chỉ xử lý khi có truyền ngày mới và ngày request
            if (!req.ExpectedDate.HasValue)
                return;

            var newExpectedDate = req.ExpectedDate.Value.Date;
            var requestDate = req.RequiredDate.Date;

            // 2. Chỉ notify khi ngày hứa giao mới lớn hơn ngày request
            if (newExpectedDate <= requestDate)
                return;

            // 3. Tránh spam: chỉ notify khi ngày thật sự đổi
            if (oldExpectedDate.HasValue && oldExpectedDate.Value.Date == newExpectedDate)
                return;

            // 4. Resolve sale phụ trách
            var saleEmployeeId = await ResolveSaleEmployeeIdAsync(mpo.MfgProductionOrderId, ct);
            if (!saleEmployeeId.HasValue || saleEmployeeId.Value == Guid.Empty)
                return;

            // 5. Gửi notification
            await _notificationService.PublishAsync(new PublishNotificationRequest
            {
                Topic = TopicNotifications.MfgProductionOrderChangeExpectiveDate,
                Severity = NotificationSeverity.Warning,
                Title = $"Ngày hứa giao thay đổi - {mpo.ExternalId}",
                Message = $"Ngày hứa giao mới ({newExpectedDate:dd/MM/yyyy}) lớn hơn ngày request ({requestDate:dd/MM/yyyy}).",
                Link = $"/sales/merchandise-orders?EventType=MerchadiseStatus&q={mpo.ProductExternalIdSnapshot}&CreatedScope=Merchandise&page=1&pageSize=15",
                PayloadJson = JsonSerializer.Serialize(new
                {
                    MfgProductionOrderId = mpo.MfgProductionOrderId,
                    MfgExternalId = mpo.ExternalId,
                    OldExpectedDate = oldExpectedDate,
                    NewExpectedDate = req.ExpectedDate,
                    RequestDate = req.RequiredDate
                }),
                TargetUserIds = new List<Guid> { saleEmployeeId.Value }
            }, ct);
        }

        private async Task<Guid?> ResolveSaleEmployeeIdAsync(Guid mfgProductionOrderId, CancellationToken ct)
        {
            var result = await _unitOfWork.MfgOrderPORepository
                .Query(track: false)
                .Where(x => x.MfgProductionOrderId == mfgProductionOrderId && x.IsActive)
                .Select(x => x.Detail!.MerchandiseOrderId)
                .FirstOrDefaultAsync(ct);

            if (result == Guid.Empty)
                return null;

            var saleEmployeeId = await _unitOfWork.MerchandiseOrderRepository
                .Query(track: false)
                .Where(x => x.MerchandiseOrderId == result)
                .Select(x => x.CreatedBy) // đổi thành field thật của bạn
                .FirstOrDefaultAsync(ct);

            return saleEmployeeId;
        }
    }
}
