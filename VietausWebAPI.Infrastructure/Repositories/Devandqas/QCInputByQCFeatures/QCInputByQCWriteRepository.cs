using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;
using VietausWebAPI.Core.Domain.Enums.Attachment;
using VietausWebAPI.Core.Domain.Enums.Devandqa;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Devandqas.QCInputByQCFeatures
{
    public class QCInputByQCWriteRepository : IQCInputByQCWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public QCInputByQCWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================================================================== Post ======================================================================== 
        public async Task<QCInputByQC> AddAsync(QCInputByQC entity, CancellationToken ct)
        {
            if (entity.VoucherDetailId <= 0)
                throw new ArgumentException("VoucherDetailId is required.", nameof(entity.VoucherDetailId));
            if (entity.CreatedBy == Guid.Empty)
                throw new ArgumentException("CreatedBy is required.", nameof(entity.CreatedBy));

            if (!entity.AttachmentCollectionId.HasValue || entity.AttachmentCollectionId.Value == Guid.Empty)
            {
                var ac = new AttachmentCollection { AttachmentCollectionId = Guid.CreateVersion7() };
                _context.AttachmentCollections.Add(ac);
                entity.AttachmentCollectionId = ac.AttachmentCollectionId;
            }

            var voucherDetail = await _context.WarehouseVoucherDetails
                .FirstOrDefaultAsync(x => x.VoucherDetailId == entity.VoucherDetailId, ct)
                ?? throw new InvalidOperationException($"WarehouseVoucherDetail not found: {entity.VoucherDetailId}");

            var existedQc = await _context.QCInputByQCs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.VoucherDetailId == entity.VoucherDetailId, ct);

            if (existedQc != null)
                throw new InvalidOperationException("QC data already exists for this voucher detail.");

            if (entity.QCInputByQCId == Guid.Empty)
                entity.QCInputByQCId = Guid.CreateVersion7();

            if (entity.CreatedDate == default)
                entity.CreatedDate = DateTime.Now;

            // Status default: None (FE có file thì sẽ set Pending)
            entity.AttachmentLastError = null;

            _context.QCInputByQCs.Add(entity);

            if (entity.ImportWarehouseType.HasValue)
            {
                var v = (int)entity.ImportWarehouseType.Value;
                if (!Enum.IsDefined(typeof(VoucherDetailType), v))
                    throw new InvalidOperationException($"Cannot map QcDecision({v}) to VoucherDetailType.");

                voucherDetail.VoucherType = (VoucherDetailType)v;
            }

            return entity;
        }


        public async Task<QCInputByQC> PatchByVoucherDetailIdAsync(
            long voucherDetailId,
            PatchQCInputByQC patch,
            Guid userId,
            CancellationToken ct)
        {
            var entity = await _context.QCInputByQCs
                .FirstOrDefaultAsync(x => x.VoucherDetailId == voucherDetailId, ct)
                ?? throw new InvalidOperationException($"QCInputByQC not found for VoucherDetailId: {voucherDetailId}");

            // Rule: chỉ cho sửa khi đang Waiter (trạng thái HIỆN TẠI)
            if (entity.ImportWarehouseType != QcDecision.Waiter)
                throw new InvalidOperationException("QC đã kết luận (không phải Tạm giữ), không được chỉnh sửa.");

            var changed = false;

            changed |= PatchHelper.SetIfRef(patch.InspectionMethod, () => entity.InspectionMethod, v => entity.InspectionMethod = v);
            changed |= PatchHelper.SetIfNullable(patch.IsCOAProvided, () => entity.IsCOAProvided, v => entity.IsCOAProvided = v);
            changed |= PatchHelper.SetIfNullable(patch.IsMSDSTDSProvided, () => entity.IsMSDSTDSProvided, v => entity.IsMSDSTDSProvided = v);
            changed |= PatchHelper.SetIfNullable(patch.IsMetalDetectionRequired, () => entity.IsMetalDetectionRequired, v => entity.IsMetalDetectionRequired = v);
            changed |= PatchHelper.SetIfNullable(patch.IsSuccessQuality, () => entity.IsSuccessQuality, v => entity.IsSuccessQuality = v);
            changed |= PatchHelper.SetIfRef(patch.Note, () => entity.Note, v => entity.Note = v);

            // ImportWarehouseType changed?
            var importChanged = PatchHelper.SetIfNullable(
                patch.ImportWarehouseType,
                () => entity.ImportWarehouseType,
                v => entity.ImportWarehouseType = v
            );
            changed |= importChanged;

            // AttachmentCollectionId (nếu bạn cho patch)
            if (patch.AttachmentCollectionId.HasValue)
            {
                changed |= PatchHelper.SetIfGuid(
                    patch.AttachmentCollectionId,
                    () => entity.AttachmentCollectionId ?? Guid.Empty,
                    v => entity.AttachmentCollectionId = v,
                    ignoreEmpty: true
                );
            }

            // Nếu FE báo có upload file mới -> set Pending để tracking
            if (patch.HasNewAttachments == true)
            {
                entity.AttachmentStatus = AttachmentUploadStatus.Pending;
                entity.AttachmentLastError = null;
                changed = true;

                // đảm bảo có AttachmentCollectionId để upload
                if (!entity.AttachmentCollectionId.HasValue || entity.AttachmentCollectionId.Value == Guid.Empty)
                {
                    var ac = new AttachmentCollection { AttachmentCollectionId = Guid.CreateVersion7() };
                    _context.AttachmentCollections.Add(ac);
                    entity.AttachmentCollectionId = ac.AttachmentCollectionId;
                }
            }

            // Sync voucher type nếu ImportWarehouseType đổi
            if (importChanged && entity.ImportWarehouseType.HasValue)
            {
                var voucherDetail = await _context.WarehouseVoucherDetails
                    .FirstOrDefaultAsync(x => x.VoucherDetailId == entity.VoucherDetailId, ct)
                    ?? throw new InvalidOperationException($"WarehouseVoucherDetail not found: {entity.VoucherDetailId}");

                var v = (int)entity.ImportWarehouseType.Value;
                if (!Enum.IsDefined(typeof(VoucherDetailType), v))
                    throw new InvalidOperationException($"Cannot map QcDecision({v}) to VoucherDetailType.");

                voucherDetail.VoucherType = (VoucherDetailType)v;
                changed = true;
            }

            if (!changed)
                return entity;

            await _context.SaveChangesAsync(ct);
            return entity;
        }



        // ======================================================================== Helper ======================================================================== 
        public async Task<QCInputByQC> UpdateAttachmentStatusByVoucherDetailIdAsync(
            long voucherDetailId,
            AttachmentUploadStatus status,
            string? lastError,
            CancellationToken ct)
        {
            var entity = await _context.QCInputByQCs
                .FirstOrDefaultAsync(x => x.VoucherDetailId == voucherDetailId, ct);

            if (entity == null)
                throw new InvalidOperationException($"QCInputByQC not found for VoucherDetailId: {voucherDetailId}");

            entity.AttachmentStatus = status;
            entity.AttachmentLastError = string.IsNullOrWhiteSpace(lastError) ? null : lastError.Trim();

            await _context.SaveChangesAsync(ct);
            return entity;
        }


    }
}
