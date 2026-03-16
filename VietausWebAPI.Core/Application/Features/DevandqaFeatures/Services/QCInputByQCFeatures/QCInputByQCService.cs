using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Core.Domain.Enums.Attachment;
using VietausWebAPI.Core.Domain.Enums.Devandqa;
using VietausWebAPI.Core.Domain.Enums.Orders;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Services.QCInputByQCFeatures
{
    public class QCInputByQCService : IQCInputByQCService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;

        public QCInputByQCService(IUnitOfWork uow, ICurrentUser currentUser)
        {
            _uow = uow;
            _currentUser = currentUser;
        }
       
        public async Task<OperationResult<PagedResult<GetSummaryQCInput>>> GetPagedSummaryAsync(QCInputQuery query, CancellationToken ct)
        {
            try
            {
                var page = await _uow.QCInputByQCReadRepository.GetPagedSummaryAsync(query, ct);

                return OperationResult<PagedResult<GetSummaryQCInput>>.Ok(page);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<GetSummaryQCInput>>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<GetQCInputByQC?>> GetByVoucherDetailIdAsync(long voucherDetailId, CancellationToken ct)
        {
            try
            {
                if (voucherDetailId <= 0)
                    return OperationResult<GetQCInputByQC?>.Fail("VoucherDetailId không hợp lệ.");

                var qcEntity = await _uow.QCInputByQCReadRepository
                    .GetDetailByVoucherDetailIdAsync(voucherDetailId, ct);

                // Chưa có QC -> Ok(null) để UI hiểu là create mode
                if (qcEntity == null)
                    return OperationResult<GetQCInputByQC?>.Ok(null);

                return OperationResult<GetQCInputByQC?>.Ok(qcEntity);
            }
            catch (Exception ex)
            {
                return OperationResult<GetQCInputByQC?>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<GetQCInputByQC>> CreateAsync(PostQCInputByQC input, CancellationToken ct)
        {
            try
            {
                if (input == null)
                    return OperationResult<GetQCInputByQC>.Fail("Input is null.");

                if (input.VoucherDetailId <= 0)
                    return OperationResult<GetQCInputByQC>.Fail("VoucherDetailId không hợp lệ.");

                var userId = _currentUser.EmployeeId;
                if (userId == Guid.Empty)
                    return OperationResult<GetQCInputByQC>.Fail("Không lấy được thông tin người dùng hiện tại.");

                // chặn tạo trùng
                var existed = await _uow.QCInputByQCReadRepository
                    .GetLatestByVoucherDetailIdAsync(input.VoucherDetailId, ct);

                if (existed != null)
                {
                    var existedDto = new GetQCInputByQC
                    {
                        QCInputByQCId = existed.QCInputByQCId,
                        VoucherDetailId = existed.VoucherDetailId,
                        MaterialName = existed.MaterialName,
                        MaterialExternalId = existed.MaterialExternalId,
                        InspectionMethod = existed.InspectionMethod,
                        IsCOAProvided = existed.IsCOAProvided,
                        IsMSDSTDSProvided = existed.IsMSDSTDSProvided,
                        IsMetalDetectionRequired = existed.IsMetalDetectionRequired,
                        IsSuccessQuality = existed.IsSuccessQuality,
                        ImportWarehouseType = existed.ImportWarehouseType,
                        Note = existed.Note,
                        AttachmentCollectionId = existed.AttachmentCollectionId
                    };

                    return OperationResult<GetQCInputByQC>.Ok(existedDto);
                }

                // lấy voucher detail để đọc mã NVL + số lot
                var voucherDetail = await _uow.WarehouseVoucherDetailReadRepository
                    .GetByIdAsync(input.VoucherDetailId, ct);

                if (voucherDetail == null)
                    return OperationResult<GetQCInputByQC>.Fail("Không tìm thấy WarehouseVoucherDetail.");

                var entity = new QCInputByQC
                {
                    QCInputByQCId = Guid.CreateVersion7(),
                    VoucherDetailId = input.VoucherDetailId,

                    CSName = input.CSName,
                    CSExternalId = input.CSExternalId,
                    MaterialName = voucherDetail.ProductName,
                    MaterialExternalId = voucherDetail.ProductCode,

                    InspectionMethod = input.InspectionMethod,
                    IsCOAProvided = input.IsCOAProvided,
                    IsMSDSTDSProvided = input.IsMSDSTDSProvided,
                    IsMetalDetectionRequired = input.IsMetalDetectionRequired,
                    IsSuccessQuality = input.IsSuccessQuality,

                    ImportWarehouseType = input.ImportWarehouseType,
                    Note = input.Note,

                    AttachmentCollectionId = input.AttachmentCollectionId,
                    AttachmentStatus = (input.HasNewAttachments == true)
                        ? AttachmentUploadStatus.Pending
                        : AttachmentUploadStatus.None,
                    AttachmentLastError = null,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now
                };

                await _uow.QCInputByQCWriteRepository.AddAsync(entity, ct);

                // ===== cập nhật trạng thái kho =====
                var targetStockType = MapQcDecisionToStockType(input.ImportWarehouseType);
                if (targetStockType.HasValue)
                {
                    var materialCode = voucherDetail.ProductCode?.Trim();
                    var lotNumber = voucherDetail.LotNumber?.Trim();

                    if (!string.IsNullOrWhiteSpace(materialCode) && !string.IsNullOrWhiteSpace(lotNumber))
                    {
                        var now = DateTime.Now;

                        var affected = await _uow.WarehouseShelfStockRepository.Query()
                            .Where(x => x.Code == materialCode
                                     && x.LotNo == lotNumber
                                     && x.StockType == (StockType)6)
                            .ExecuteUpdateAsync(setters => setters
                                .SetProperty(x => x.StockType, targetStockType.Value)
                                .SetProperty(x => x.UpdatedBy, userId)
                                .SetProperty(x => x.UpdatedDate, now), ct);
                    }
                }

                await _uow.SaveChangesAsync(ct);

                var result = new GetQCInputByQC
                {
                    QCInputByQCId = entity.QCInputByQCId,
                    VoucherDetailId = entity.VoucherDetailId,
                    MaterialName = entity.MaterialName,
                    MaterialExternalId = entity.MaterialExternalId,
                    InspectionMethod = entity.InspectionMethod,
                    IsCOAProvided = entity.IsCOAProvided,
                    IsMSDSTDSProvided = entity.IsMSDSTDSProvided,
                    IsMetalDetectionRequired = entity.IsMetalDetectionRequired,
                    IsSuccessQuality = entity.IsSuccessQuality,
                    ImportWarehouseType = entity.ImportWarehouseType,
                    Note = entity.Note,
                    AttachmentCollectionId = entity.AttachmentCollectionId,
                    AttachmentStatus = entity.AttachmentStatus,
                    AttachmentLastError = entity.AttachmentLastError,
                };

                return OperationResult<GetQCInputByQC>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<GetQCInputByQC>.Fail(ex.Message);
            }
        }
        public async Task<OperationResult<GetQCInputByQC>> PatchByVoucherDetailIdAsync(PatchQCInputByQC input, long voucherDetailId, CancellationToken ct)
        {
            try
            {
                if (input == null)
                    return OperationResult<GetQCInputByQC>.Fail("Input is null.");

                if (voucherDetailId <= 0)
                    return OperationResult<GetQCInputByQC>.Fail("VoucherDetailId không hợp lệ.");

                var userId = _currentUser.EmployeeId;
                if (userId == Guid.Empty)
                    return OperationResult<GetQCInputByQC>.Fail("Không lấy được thông tin người dùng hiện tại.");

                // update QCInputByQC
                var entity = await _uow.QCInputByQCWriteRepository
                    .PatchByVoucherDetailIdAsync(voucherDetailId, input, userId, ct);

                if (entity == null)
                    return OperationResult<GetQCInputByQC>.Fail("Không tìm thấy dữ liệu QC để cập nhật.");

                // lấy voucher detail để update stock giống CreateAsync
                var voucherDetail = await _uow.WarehouseVoucherDetailReadRepository
                    .GetByIdAsync(voucherDetailId, ct);

                if (voucherDetail == null)
                    return OperationResult<GetQCInputByQC>.Fail("Không tìm thấy WarehouseVoucherDetail.");

                // ===== cập nhật trạng thái kho =====
                var targetStockType = MapQcDecisionToStockType(input.ImportWarehouseType);
                if (targetStockType.HasValue)
                {
                    var materialCode = voucherDetail.ProductCode?.Trim();
                    var lotNumber = voucherDetail.LotNumber?.Trim();

                    if (!string.IsNullOrWhiteSpace(materialCode) &&
                        !string.IsNullOrWhiteSpace(lotNumber))
                    {
                        var now = DateTime.Now;

                        await _uow.WarehouseShelfStockRepository.Query()
                            .Where(x => x.Code == materialCode
                                     && x.LotNo == lotNumber
                                     && x.StockType == (StockType)6)
                            .ExecuteUpdateAsync(setters => setters
                                .SetProperty(x => x.StockType, targetStockType.Value)
                                .SetProperty(x => x.UpdatedBy, userId)
                                .SetProperty(x => x.UpdatedDate, now), ct);
                    }
                }

                await _uow.SaveChangesAsync(ct);

                var dto = new GetQCInputByQC
                {
                    QCInputByQCId = entity.QCInputByQCId,
                    VoucherDetailId = entity.VoucherDetailId,
                    MaterialName = entity.MaterialName,
                    MaterialExternalId = entity.MaterialExternalId,
                    InspectionMethod = entity.InspectionMethod,
                    IsCOAProvided = entity.IsCOAProvided,
                    IsMSDSTDSProvided = entity.IsMSDSTDSProvided,
                    IsMetalDetectionRequired = entity.IsMetalDetectionRequired,
                    IsSuccessQuality = entity.IsSuccessQuality,
                    ImportWarehouseType = entity.ImportWarehouseType,
                    Note = entity.Note,
                    AttachmentCollectionId = entity.AttachmentCollectionId,
                    QCCreatedDate = entity.CreatedDate,
                    AttachmentStatus = entity.AttachmentStatus,
                    AttachmentLastError = entity.AttachmentLastError,
                };

                return OperationResult<GetQCInputByQC>.Ok(dto);
            }
            catch (Exception ex)
            {
                return OperationResult<GetQCInputByQC>.Fail(ex.Message);
            }
        }

        // =================================================== Helper ===================================================
        private static StockType? MapQcDecisionToStockType(QcDecision? decision)
        {
            return decision switch
            {
                QcDecision.QCPass => (StockType)3,
                QcDecision.Special => (StockType)3,
                QcDecision.QCFail => (StockType)4,
                _ => null
            };
        }

        public async Task<List<QCInputByQCExportRow>> BuildExportRowsAsync(QCInputQuery query, CancellationToken ct)
        {
            return await _uow.QCInputByQCReadRepository.GetExportRowsAsync(query, ct);
        }


        // =================================================== Helper UpdatePunchart ===================================================

        /// <summary>
        /// Lấy chi tiết phiếu kho theo <paramref name="voucherDetailId"/>.
        /// </summary>
        /// <param name="voucherDetailId">Id của dòng chi tiết phiếu kho.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Đối tượng <see cref="WarehouseVoucherDetail"/> nếu tìm thấy; ngược lại trả về <c>null</c>.
        /// </returns>
        private async Task<WarehouseVoucherDetail?> GetVoucherDetailAsync(long voucherDetailId, CancellationToken ct)
        {
            return await _uow.WarehouseVoucherDetailReadRepository.GetByIdAsync(voucherDetailId, ct);
        }

        /// <summary>
        /// Đồng bộ lại loại tồn kho của lô hàng theo kết quả QC.
        /// </summary>
        /// <param name="voucherDetail">Dòng chi tiết phiếu kho chứa mã vật tư và số lô cần cập nhật.</param>
        /// <param name="importWarehouseType">Kết luận QC dùng để ánh xạ sang loại tồn kho đích.</param>
        /// <param name="userId">Người thực hiện cập nhật.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <remarks>
        /// Chỉ cập nhật khi:
        /// - ánh xạ được <see cref="StockType"/> từ <paramref name="importWarehouseType"/>
        /// - có đủ <c>ProductCode</c> và <c>LotNumber</c>
        /// - bản ghi tồn kho hiện tại đang ở loại tồn kho chờ QC (StockType = 6).
        /// </remarks>
        private async Task SyncWarehouseStockByQcAsync(WarehouseVoucherDetail voucherDetail, QcDecision? importWarehouseType, Guid userId, CancellationToken ct)
        {
            var targetStockType = MapQcDecisionToStockType(importWarehouseType);
            if (!targetStockType.HasValue)
                return;

            var materialCode = voucherDetail.ProductCode?.Trim();
            var lotNumber = voucherDetail.LotNumber?.Trim();

            if (string.IsNullOrWhiteSpace(materialCode) || string.IsNullOrWhiteSpace(lotNumber))
                return;

            var now = DateTime.Now;

            await _uow.WarehouseShelfStockRepository.Query()
                .Where(x => x.Code == materialCode
                         && x.LotNo == lotNumber
                         && x.StockType == (StockType)6)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.StockType, targetStockType.Value)
                    .SetProperty(x => x.UpdatedBy, userId)
                    .SetProperty(x => x.UpdatedDate, now), ct);
        }

        /// <summary>
        /// Từ một dòng chi tiết phiếu kho, truy vết về phiếu yêu cầu nhập kho và đơn mua liên quan.
        /// </summary>
        /// <param name="voucherDetail">Dòng chi tiết phiếu kho nguồn.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Bộ 3 gồm:
        /// - <see cref="PurchaseOrder"/> nếu tìm thấy đơn mua,
        /// - <see cref="PurchaseOrderDetail"/> khớp với mã vật tư,
        /// - <see cref="WarehouseRequest"/> được gắn với phiếu kho.
        /// </returns>
        /// <remarks>
        /// Luồng resolve:
        /// 1. Lấy voucher từ <c>VoucherId</c>
        /// 2. Từ voucher lấy <c>RequestId</c>
        /// 3. Từ request lấy <c>codeFromRequest</c> để đối chiếu với <c>PurchaseOrder.ExternalId</c>
        /// 4. Tìm detail của PO theo <c>MaterialExternalIDSnapshot</c>, ưu tiên dòng có cùng <c>LineNo</c>.
        /// </remarks>
        private async Task<(PurchaseOrder? po, PurchaseOrderDetail? pod, WarehouseRequest? request)>
            ResolvePurchaseOrderAsync(WarehouseVoucherDetail voucherDetail, CancellationToken ct)
        {
            var voucher = await _uow.WarehouseVoucherReadRepository.GetByIdAsync(voucherDetail.VoucherId, ct);

            if (voucher == null || !voucher.RequestId.HasValue)
                return (null, null, null);

            var request = await _uow.WarehouseRequestRepository.Query(track: false)
                .FirstOrDefaultAsync(x => x.RequestId == voucher.RequestId.Value && x.IsActive, ct);

            if (request == null || string.IsNullOrWhiteSpace(request.codeFromRequest))
                return (null, null, request);

            var poCode = request.codeFromRequest.Trim();

            var po = await _uow.PurchaseOrderRepository.Query(track: true)
                .Include(x => x.PurchaseOrderDetails)
                .FirstOrDefaultAsync(x =>
                    (x.IsActive ?? true) &&
                    x.ExternalId != null &&
                    x.ExternalId.Trim() == poCode,
                    ct);

            if (po == null)
                return (null, null, request);

            var materialCode = voucherDetail.ProductCode?.Trim();

            var pod = po.PurchaseOrderDetails
                .Where(x => x.IsActive)
                .Where(x => (x.MaterialExternalIDSnapshot ?? "").Trim() == materialCode)
                .OrderBy(x => x.LineNo == voucherDetail.LineNo ? 0 : 1)
                .FirstOrDefault();

            return (po, pod, request);
        }

        /// <summary>
        /// Tính lại số lượng thực nhận của một dòng đơn mua dựa trên tổng khối lượng đã được QC chấp nhận.
        /// </summary>
        /// <param name="po">Đơn mua chứa dòng cần cập nhật.</param>
        /// <param name="pod">Dòng chi tiết đơn mua cần cập nhật <c>RealQuantity</c>.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <remarks>
        /// Chỉ cộng các lượng nhập có kết quả QC thuộc nhóm được chấp nhận, hiện tại gồm:
        /// - <see cref="QcDecision.QCPass"/>
        /// - <see cref="QcDecision.Special"/>
        /// </remarks>
        private async Task RecalculatePurchaseOrderDetailRealQuantityAsync( PurchaseOrder po, PurchaseOrderDetail pod, CancellationToken ct)
        {
            var poCode = po.ExternalId?.Trim();
            var materialCode = pod.MaterialExternalIDSnapshot?.Trim();

            if (string.IsNullOrWhiteSpace(poCode) || string.IsNullOrWhiteSpace(materialCode))
                return;

            var acceptedTypes = new[]
            {
        QcDecision.QCPass,
        QcDecision.Special
    };

            var totalAcceptedQty = await _uow.WarehouseVoucherDetailReadRepository
                .SumAcceptedQtyByPoCodeAndMaterialCodeAsync(
                    poCode,
                    materialCode,
                    acceptedTypes,
                    ct);

            pod.RealQuantity = totalAcceptedQty;
        }

        /// <summary>
        /// Tính lại trạng thái của đơn mua dựa trên tiến độ nhận hàng thực tế của các dòng chi tiết.
        /// </summary>
        /// <param name="purchaseOrderId">Id của đơn mua cần cập nhật trạng thái.</param>
        /// <param name="userId">Người thực hiện cập nhật.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <remarks>
        /// Quy tắc:
        /// - <c>Completed</c>: tất cả dòng active đã nhận đủ hoặc vượt số lượng yêu cầu
        /// - <c>InProgress</c>: có ít nhất một dòng đã nhận
        /// - <c>Pending</c>: chưa có dòng nào nhận hàng
        /// </remarks>
        private async Task RecalculatePurchaseOrderStatusAsync(Guid purchaseOrderId, Guid userId, CancellationToken ct)
        {
            var po = await _uow.PurchaseOrderRepository.Query(track: true)
                .Include(x => x.PurchaseOrderDetails)
                .FirstOrDefaultAsync(x => x.PurchaseOrderId == purchaseOrderId, ct);

            if (po == null)
                return;

            var details = po.PurchaseOrderDetails
                .Where(x => x.IsActive)
                .ToList();

            if (!details.Any())
                return;

            var anyReceived = details.Any(x => (x.RealQuantity ?? 0m) > 0m);
            var allReceived = details.All(x => (x.RealQuantity ?? 0m) >= (x.RequestQuantity ?? 0m));

            if (allReceived)
                po.Status = PurchaseOrderStatus.Completed.ToString();
            else if (anyReceived)
                po.Status = PurchaseOrderStatus.InProgress.ToString();
            else
                po.Status = PurchaseOrderStatus.Pending.ToString();

            po.UpdatedBy = userId;
            po.UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Đồng bộ dữ liệu đơn mua sau khi kết quả QC của một dòng nhập kho thay đổi.
        /// </summary>
        /// <param name="voucherDetail">Dòng chi tiết phiếu kho phát sinh từ nhập hàng.</param>
        /// <param name="importWarehouseType">Kết luận QC mới.</param>
        /// <param name="userId">Người thực hiện cập nhật.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <remarks>
        /// Hàm này sẽ:
        /// 1. Resolve ra đơn mua và dòng đơn mua tương ứng
        /// 2. Tính lại <c>RealQuantity</c> của dòng chi tiết
        /// 3. Tính lại trạng thái tổng thể của đơn mua
        ///
        /// Luôn recalculate kể cả khi QC đổi giữa các trạng thái như:
        /// Pass -> Reject, Reject -> Pass, Pass -> Special...
        /// </remarks>
        private async Task SyncPurchaseOrderByQcAsync( WarehouseVoucherDetail voucherDetail, QcDecision? importWarehouseType, Guid userId, CancellationToken ct)
        {
            var (po, pod, _) = await ResolvePurchaseOrderAsync(voucherDetail, ct);

            if (po == null || pod == null)
                return;

            // luôn recalculate, kể cả patch từ Pass -> Reject hay Reject -> Pass
            await RecalculatePurchaseOrderDetailRealQuantityAsync(po, pod, ct);
            await RecalculatePurchaseOrderStatusAsync(po.PurchaseOrderId, userId, ct);
        }

        /// <summary>
        /// Thực hiện toàn bộ luồng đồng bộ dữ liệu phụ thuộc sau khi QC thay đổi.
        /// </summary>
        /// <param name="voucherDetail">Dòng chi tiết phiếu kho bị ảnh hưởng bởi thay đổi QC.</param>
        /// <param name="importWarehouseType">Kết luận QC mới.</param>
        /// <param name="userId">Người thực hiện cập nhật.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <remarks>
        /// Hiện tại hàm sẽ đồng bộ 2 phần:
        /// - tồn kho theo loại stock sau QC
        /// - đơn mua liên quan và trạng thái nhận hàng
        /// </remarks>
        private async Task SyncAfterQcChangedAsync( WarehouseVoucherDetail voucherDetail, QcDecision? importWarehouseType, Guid userId, CancellationToken ct)
        {
            await SyncWarehouseStockByQcAsync(voucherDetail, importWarehouseType, userId, ct);
            await SyncPurchaseOrderByQcAsync(voucherDetail, importWarehouseType, userId, ct);
        }
    }
}
