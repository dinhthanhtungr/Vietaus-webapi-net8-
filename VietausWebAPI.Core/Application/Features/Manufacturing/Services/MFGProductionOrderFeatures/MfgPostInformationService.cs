using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.MFGProductionOrderFeatures;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseWriteServices;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
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
    public class MfgPostInformationService : IMfgPostInformationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IExternalIdService _externalId;
        private readonly ITimelineService _timeLineService;
        private readonly INotificationService _notificationService;
        private readonly IPriceProvider _priceProvider;
        private readonly IWarehouseReservationService _warehouseReservationService;

        public MfgPostInformationService(
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

        public async Task<OperationResult<CreateMfgProductionOrderInformResult>> CreateInformAsync(CreateMfgProductionOrderInformRequest req, CancellationToken ct = default)
        {
            if (req == null)
                return OperationResult<CreateMfgProductionOrderInformResult>.Fail("Dữ liệu không hợp lệ.");

            if (req.MerchandiseOrderId == Guid.Empty)
                return OperationResult<CreateMfgProductionOrderInformResult>.Fail("MerchandiseOrderId không hợp lệ.");

            if (req.MerchandiseOrderDetailId == Guid.Empty)
                return OperationResult<CreateMfgProductionOrderInformResult>.Fail("MerchandiseOrderDetailId không hợp lệ.");

            if (!req.ProductId.HasValue || req.ProductId.Value == Guid.Empty)
                return OperationResult<CreateMfgProductionOrderInformResult>.Fail("ProductId không hợp lệ.");

            if (req.RequiredDate == default)
                return OperationResult<CreateMfgProductionOrderInformResult>.Fail("RequiredDate không hợp lệ.");

            var hasFormula = req.FormulaItems != null && req.FormulaItems.Any(x => x.IsActive && x.Quantity > 0);

            if (hasFormula)
            {
                var totalStd = req.FormulaItems
                    .Where(x => x.IsActive && x.Quantity > 0)
                    .Sum(x => x.Quantity);

                if (Math.Abs(totalStd - 1m) > 0.0001m)
                    return OperationResult<CreateMfgProductionOrderInformResult>.Fail("Tổng tỷ lệ công thức phải bằng 1.");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;
                var companyId = _currentUser.CompanyId;

                // =====================================================
                // 1. LOAD ORDER DETAIL
                // =====================================================
                var detail = await _unitOfWork.MerchandiseOrderRepository
                    .QueryDetail(track: false)
                    .FirstOrDefaultAsync(x =>
                        x.MerchandiseOrderDetailId == req.MerchandiseOrderDetailId
                        && x.MerchandiseOrderId == req.MerchandiseOrderId
                        && x.IsActive,
                        ct);

                if (detail == null)
                    return OperationResult<CreateMfgProductionOrderInformResult>.Fail("Không tìm thấy chi tiết đơn hàng.");

                // Optional: validate product khớp detail
                if (detail.ProductId != req.ProductId.Value)
                    return OperationResult<CreateMfgProductionOrderInformResult>.Fail("ProductId không khớp với chi tiết đơn hàng.");

                // =====================================================
                // 2. CREATE MPO
                // =====================================================
                var mpoId = Guid.CreateVersion7();
                var mpoExternalId = await _externalId.NextAsync(DocumentPrefix.MFG.ToString(), ct: ct);

                var mpo = new MfgProductionOrder
                {
                    MfgProductionOrderId = mpoId,
                    ExternalId = mpoExternalId,

                    ProductId = req.ProductId.Value,
                    ProductExternalIdSnapshot = req.ProductExternalIdSnapshot,
                    ProductNameSnapshot = req.ProductNameSnapshot,

                    CustomerId = req.CustomerId,
                    CustomerExternalIdSnapshot = req.CustomerExternalIdSnapshot,
                    CustomerNameSnapshot = req.CustomerNameSnapshot,

                    FormulaId = req.FormulaCustomerSelect == Guid.Empty ? null : req.FormulaCustomerSelect,
                    FormulaExternalIdSnapshot = req.FormulaCustomerExternalIdSelect,

                    ManufacturingDate = req.ManufacturingDate,
                    ExpectedDate = req.ExpectedDate,
                    RequiredDate = req.RequiredDate,

                    TotalQuantityRequest = req.TotalQuantityRequest,
                    TotalQuantity = req.TotalQuantity,

                    NumOfBatches = req.NumOfBatches,
                    UnitPriceAgreed = req.UnitPriceAgreed,

                    Status = hasFormula
                        ? ManufacturingProductOrder.Scheduling.ToString()
                        : ManufacturingProductOrder.New.ToString(),

                    LabNote = req.LabNote,
                    Requirement = req.Requirement,
                    PlpuNote = req.PlpuNote,
                    QcCheck = req.QcCheck,

                    BagType = req.BagType ?? string.Empty,
                    StepOfProduct = req.StepOfProduct,

                    IsActive = true,
                    CompanyId = companyId,
                    CreatedDate = now,
                    CreatedBy = userId,
                    UpdatedDate = now,
                    UpdatedBy = userId
                };

                await _unitOfWork.MfgProductionOrderRepository.AddAsync(mpo, ct);

                // =====================================================
                // 3. CREATE LINK MPO <-> ORDER DETAIL
                // =====================================================
                var link = new MfgOrderPO
                {
                    MfgProductionOrderId = mpo.MfgProductionOrderId,
                    MerchandiseOrderDetailId = req.MerchandiseOrderDetailId,
                    IsActive = true
                };

                await _unitOfWork.MfgOrderPORepository.AddAsync(link, ct);

                // =====================================================
                // 4. CREATE FORMULA + FORMULA ITEMS (IF ANY)
                // =====================================================
                ManufacturingFormula? mf = null;

                if (hasFormula)
                {
                    mf = new ManufacturingFormula
                    {
                        ManufacturingFormulaId = Guid.CreateVersion7(),
                        ExternalId = await _externalId.NextAsync(DocumentPrefix.VA.ToString(), ct: ct),
                        Name = "COPY",
                        Status = ManufacturingProductOrderFormula.Checking.ToString(),
                        TotalPrice = req.FormulaItems
                            .Where(x => x.IsActive && x.Quantity > 0)
                            .Sum(x => x.UnitPrice * x.Quantity),

                        // Copy từ VA cũ nếu có
                        SourceType = FormulaSource.FromVA,
                        SourceManufacturingFormulaId = req.ManufacturingFormulaIdIsSelect,
                        SourceManufacturingExternalIdSnapshot = req.ManufacturingFormulaExternalIdIsSelect,

                        // Giữ trace VU khách chọn
                        SourceVUFormulaId = req.FormulaCustomerSelect == Guid.Empty ? null : req.FormulaCustomerSelect,
                        SourceVUExternalIdSnapshot = req.FormulaCustomerExternalIdSelect,

                        IsActive = true,
                        Note = req.LabNote,

                        CreatedDate = now,
                        UpdatedDate = now,
                        CreatedBy = userId,
                        UpdatedBy = userId,
                        CompanyId = companyId
                    };

                    await _unitOfWork.ManufacturingFormulaRepository.AddAsync(mf, ct);

                    var materialEntities = req.FormulaItems
                        .Where(x => x.IsActive && x.Quantity > 0)
                        .OrderBy(x => x.LineNo)
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
                    // 5. CREATE SELECTED VERSION
                    // =====================================================
                    await CreateNewSelectedFormulaVersionAsync(
                        mpo.MfgProductionOrderId,
                        mf.ManufacturingFormulaId,
                        companyId,
                        now,
                        userId,
                        isActiveSelect: true,
                        ct);

                    // =====================================================
                    // 6. ENSURE SCHEDULE
                    // =====================================================
                    await EnsureScheduleAsync(mpo, now, ct);

                    // =====================================================
                    // 7. RESERVE VIRTUAL STOCK
                    // =====================================================
                    if ((mpo.TotalQuantity ?? 0m) > 0)
                    {
                        var reserveInput = req.FormulaItems
                            .Where(x => x.IsActive && x.Quantity > 0)
                            .OrderBy(x => x.LineNo)
                            .Select(x => new PatchMfgProductionOrderFormulaItemRequest
                            {
                                ManufacturingFormulaMaterialId = null,
                                ItemId = x.ItemId,
                                ItemType = x.ItemType,
                                CategoryId = x.CategoryId,
                                Quantity = x.Quantity,
                                UnitPrice = x.UnitPrice,
                                MaterialNameSnapshot = x.MaterialNameSnapshot,
                                MaterialExternalIdSnapshot = x.MaterialExternalIdSnapshot,
                                Unit = x.Unit,
                                IsActive = x.IsActive,
                                LineNo = x.LineNo
                            })
                            .ToList();

                        var reserveContext = new ReservationSyncContext
                        {
                            MfgProductionOrderId = mpo.MfgProductionOrderId,
                            VaCode = mpo.ExternalId ?? string.Empty,
                            CompanyId = mpo.CompanyId
                        };

                        var reserveResult = await _warehouseReservationService.SyncReservationsByFormulaItemsAsync(
                            reserveContext,
                            mpo.TotalQuantity ?? 0m,
                            reserveInput,
                            ct);

                        if (!reserveResult.Success)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return OperationResult<CreateMfgProductionOrderInformResult>.Fail(
                                reserveResult.Message ?? "Không thể cập nhật tồn kho ảo.");
                        }
                    }
                }

                // =====================================================
                // 8. TIMELINE MPO
                // =====================================================
                await _timeLineService.AddEventLogAsync(new EventLogModels
                {
                    employeeId = userId,
                    eventType = EventType.ManufacturingProductOrder,
                    sourceCode = mpo.ExternalId ?? string.Empty,
                    sourceId = mpo.MfgProductionOrderId,
                    status = mpo.Status,
                    note = hasFormula
                        ? $"Tạo lệnh sản xuất kèm công thức bởi {_currentUser.personName} vào {now}"
                        : $"Tạo lệnh sản xuất bởi {_currentUser.personName} vào {now}"
                }, ct);

                // =====================================================
                // 9. SYNC MERCHANDISE ORDER
                // =====================================================
                var createdMfgOrderPo = await _unitOfWork.MfgOrderPORepository.Query(track: true)
                    .Where(x => x.MfgProductionOrderId == mpo.MfgProductionOrderId && x.IsActive)
                    .Include(x => x.Detail)
                    .Include(x => x.ProductionOrder)
                    .FirstOrDefaultAsync(ct);

                if (createdMfgOrderPo != null)
                {
                    await SyncMerchandiseOrderAsync(createdMfgOrderPo, now, userId, ct);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return OperationResult<CreateMfgProductionOrderInformResult>.Ok(
                    new CreateMfgProductionOrderInformResult
                    {
                        MfgProductionOrderId = mpo.MfgProductionOrderId,
                        ExternalId = mpo.ExternalId ?? string.Empty,
                        ManufacturingFormulaId = mf?.ManufacturingFormulaId,
                        ManufacturingFormulaExternalId = mf?.ExternalId
                    },
                    hasFormula
                        ? "Tạo lệnh sản xuất và công thức thành công."
                        : "Tạo lệnh sản xuất thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult<CreateMfgProductionOrderInformResult>.Fail(
                    $"Có lỗi xảy ra trong quá trình tạo lệnh sản xuất: {ex.Message}");
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

    }
}
