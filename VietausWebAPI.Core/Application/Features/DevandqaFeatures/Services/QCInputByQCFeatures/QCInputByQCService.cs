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
using VietausWebAPI.Core.Domain.Enums.Attachment;
using VietausWebAPI.Core.Domain.Enums.Devandqa;
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
            var entity = await _uow.QCInputByQCWriteRepository.PatchByVoucherDetailIdAsync(voucherDetailId, input, _currentUser.EmployeeId,ct);

            // Map entity -> dto (nhớ trả AttachmentCollectionId)
            var dto = new GetQCInputByQC
            {
                QCInputByQCId = entity.QCInputByQCId,
                VoucherDetailId = entity.VoucherDetailId,
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
    }
}
