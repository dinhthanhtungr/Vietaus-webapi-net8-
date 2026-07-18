using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Attachments.DTOs;
using VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.PurchaseOrderDocuments;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.Services
{
    public class PurchaseOrderDocumentService : IPurchaseOrderDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentSchemaService _attachmentSchemaService;
        private readonly ICurrentUser _currentUser;

        public PurchaseOrderDocumentService(
            IUnitOfWork unitOfWork,
            IAttachmentSchemaService attachmentSchemaService,
            ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _attachmentSchemaService = attachmentSchemaService;
            _currentUser = currentUser;
        }

        public async Task<OperationResult<IReadOnlyList<PurchaseOrderDocumentDto>>> GetListAsync(
            Guid purchaseOrderId,
            AttachmentSlot? documentType,
            bool includeInactive = false,
            CancellationToken ct = default)
        {
            var exists = await PurchaseOrderExistsAsync(purchaseOrderId, ct);
            if (!exists)
                return OperationResult<IReadOnlyList<PurchaseOrderDocumentDto>>.Fail("Không tìm thấy đơn mua hàng.");

            var query = _unitOfWork.PurchaseOrderDocumentRepository
                .Query(track: false)
                .Where(x => x.PurchaseOrderId == purchaseOrderId);

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            if (documentType.HasValue)
                query = query.Where(x => x.DocumentType == documentType.Value);

            var rows = await query
                .OrderBy(x => x.DocumentType)
                .ThenByDescending(x => x.CreatedDate)
                .ToListAsync(ct);

            var result = new List<PurchaseOrderDocumentDto>(rows.Count);
            foreach (var row in rows)
            {
                result.Add(await MapAsync(row, includeAttachments: true, ct));
            }

            return OperationResult<IReadOnlyList<PurchaseOrderDocumentDto>>.Ok(result);
        }

        public async Task<OperationResult<PurchaseOrderDocumentDto>> GetByIdAsync(
            Guid purchaseOrderId,
            Guid documentId,
            CancellationToken ct = default)
        {
            var document = await FindDocumentAsync(purchaseOrderId, documentId, track: false, ct);
            if (document is null)
                return OperationResult<PurchaseOrderDocumentDto>.Fail("Không tìm thấy chứng từ đơn mua hàng.");

            return OperationResult<PurchaseOrderDocumentDto>.Ok(await MapAsync(document, includeAttachments: true, ct));
        }

        public async Task<OperationResult<PurchaseOrderDocumentDto>> CreateAsync(
            Guid purchaseOrderId,
            CreatePurchaseOrderDocumentRequest request,
            CancellationToken ct = default)
        {
            var exists = await PurchaseOrderExistsAsync(purchaseOrderId, ct);
            if (!exists)
                return OperationResult<PurchaseOrderDocumentDto>.Fail("Không tìm thấy đơn mua hàng.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;

            var attachmentCollectionId = request.AttachmentCollectionId;
            if (attachmentCollectionId.HasValue)
                await EnsureAttachmentCollectionAsync(attachmentCollectionId.Value, ct);

            var document = new PurchaseOrderDocument
            {
                PurchaseOrderDocumentId = Guid.CreateVersion7(),
                PurchaseOrderId = purchaseOrderId,
                DocumentType = request.DocumentType,
                AttachmentCollectionId = attachmentCollectionId,
                DocumentCode = Normalize(request.DocumentCode),
                DocumentName = Normalize(request.DocumentName),
                Note = Normalize(request.Note),
                IsReceived = request.IsReceived,
                ReceivedDate = request.IsReceived ? request.ReceivedDate ?? now : request.ReceivedDate,
                CreatedDate = now,
                CreatedBy = userId,
                IsActive = true
            };

            await _unitOfWork.PurchaseOrderDocumentRepository.AddAsync(document, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return OperationResult<PurchaseOrderDocumentDto>.Ok(
                await MapAsync(document, includeAttachments: true, ct),
                "Đã tạo chứng từ đơn mua hàng.");
        }

        public async Task<OperationResult<PurchaseOrderDocumentDto>> UpdateAsync(
            Guid purchaseOrderId,
            Guid documentId,
            UpdatePurchaseOrderDocumentRequest request,
            CancellationToken ct = default)
        {
            var document = await FindDocumentAsync(purchaseOrderId, documentId, track: true, ct);
            if (document is null)
                return OperationResult<PurchaseOrderDocumentDto>.Fail("Không tìm thấy chứng từ đơn mua hàng.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;

            if (request.DocumentType.HasValue)
                document.DocumentType = request.DocumentType.Value;

            if (request.AttachmentCollectionId.HasValue)
            {
                await EnsureAttachmentCollectionAsync(request.AttachmentCollectionId.Value, ct);
                document.AttachmentCollectionId = request.AttachmentCollectionId.Value;
            }

            if (request.DocumentCode is not null)
                document.DocumentCode = Normalize(request.DocumentCode);

            if (request.DocumentName is not null)
                document.DocumentName = Normalize(request.DocumentName);

            if (request.Note is not null)
                document.Note = Normalize(request.Note);

            if (request.IsReceived.HasValue)
            {
                document.IsReceived = request.IsReceived.Value;
                if (document.IsReceived && document.ReceivedDate is null)
                    document.ReceivedDate = request.ReceivedDate ?? now;
                else if (!document.IsReceived && request.ReceivedDate.HasValue)
                    document.ReceivedDate = request.ReceivedDate;
            }
            else if (request.ReceivedDate.HasValue)
            {
                document.ReceivedDate = request.ReceivedDate;
            }

            if (request.Verify.HasValue)
            {
                document.VerifiedBy = request.Verify.Value ? userId : null;
                document.VerifiedDate = request.Verify.Value ? now : null;
            }

            if (request.IsActive.HasValue)
                document.IsActive = request.IsActive.Value;

            document.UpdatedDate = now;
            document.UpdatedBy = userId;

            await _unitOfWork.SaveChangesAsync(ct);

            return OperationResult<PurchaseOrderDocumentDto>.Ok(
                await MapAsync(document, includeAttachments: true, ct),
                "Đã cập nhật chứng từ đơn mua hàng.");
        }

        public async Task<OperationResult> DeleteAsync(
            Guid purchaseOrderId,
            Guid documentId,
            CancellationToken ct = default)
        {
            var document = await FindDocumentAsync(purchaseOrderId, documentId, track: true, ct);
            if (document is null)
                return OperationResult.Fail("Không tìm thấy chứng từ đơn mua hàng.");

            document.IsActive = false;
            document.UpdatedDate = DateTime.Now;
            document.UpdatedBy = _currentUser.EmployeeId;

            await _unitOfWork.SaveChangesAsync(ct);
            return OperationResult.Ok("Đã xóa chứng từ đơn mua hàng.");
        }

        public async Task<OperationResult<IReadOnlyList<AttachmentDTO>>> GetAttachmentsAsync(
            Guid purchaseOrderId,
            Guid documentId,
            AttachmentSlot? slot,
            CancellationToken ct = default)
        {
            var document = await FindDocumentAsync(purchaseOrderId, documentId, track: false, ct);
            if (document is null)
                return OperationResult<IReadOnlyList<AttachmentDTO>>.Fail("Không tìm thấy chứng từ đơn mua hàng.");

            if (!document.AttachmentCollectionId.HasValue)
                return OperationResult<IReadOnlyList<AttachmentDTO>>.Ok(new List<AttachmentDTO>());

            var attachments = await _attachmentSchemaService.ListAsync(
                document.AttachmentCollectionId.Value,
                slot ?? document.DocumentType,
                ct);

            return OperationResult<IReadOnlyList<AttachmentDTO>>.Ok(attachments);
        }

        public async Task<OperationResult<IReadOnlyList<AttachmentDTO>>> UploadAttachmentsAsync(
            Guid purchaseOrderId,
            Guid documentId,
            List<IFormFile> files,
            CancellationToken ct = default)
        {
            if (files is null || files.Count == 0)
                return OperationResult<IReadOnlyList<AttachmentDTO>>.Fail("Chưa chọn file cần tải lên.");

            var document = await FindDocumentAsync(purchaseOrderId, documentId, track: true, ct);
            if (document is null)
                return OperationResult<IReadOnlyList<AttachmentDTO>>.Fail("Không tìm thấy chứng từ đơn mua hàng.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;

            if (!document.AttachmentCollectionId.HasValue || document.AttachmentCollectionId.Value == Guid.Empty)
            {
                document.AttachmentCollectionId = await CreateAttachmentCollectionAsync(ct);
                document.UpdatedDate = now;
                document.UpdatedBy = userId;

                await _unitOfWork.SaveChangesAsync(ct);
            }

            var attachments = await _attachmentSchemaService.UploadListAsync(
                document.AttachmentCollectionId.Value,
                document.DocumentType,
                files,
                userId,
                ct);

            document.IsReceived = true;
            document.ReceivedDate ??= now;
            document.UpdatedDate = now;
            document.UpdatedBy = userId;

            await _unitOfWork.SaveChangesAsync(ct);

            return OperationResult<IReadOnlyList<AttachmentDTO>>.Ok(attachments, "Đã tải chứng từ lên đơn mua hàng.");
        }

        public async Task<OperationResult<PurchaseOrderDocumentDto>> UploadAttachmentsByTypeAsync(
            Guid purchaseOrderId,
            AttachmentSlot documentType,
            List<IFormFile> files,
            CancellationToken ct = default)
        {
            if (files is null || files.Count == 0)
                return OperationResult<PurchaseOrderDocumentDto>.Fail("Chưa chọn file cần tải lên.");

            var exists = await PurchaseOrderExistsAsync(purchaseOrderId, ct);
            if (!exists)
                return OperationResult<PurchaseOrderDocumentDto>.Fail("Không tìm thấy đơn mua hàng.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;

            var document = await _unitOfWork.PurchaseOrderDocumentRepository
                .Query(track: true)
                .Where(x =>
                    x.PurchaseOrderId == purchaseOrderId &&
                    x.DocumentType == documentType &&
                    x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync(ct);

            if (document is null)
            {
                document = new PurchaseOrderDocument
                {
                    PurchaseOrderDocumentId = Guid.CreateVersion7(),
                    PurchaseOrderId = purchaseOrderId,
                    DocumentType = documentType,
                    AttachmentCollectionId = await CreateAttachmentCollectionAsync(ct),
                    DocumentName = documentType.ToString(),
                    IsReceived = false,
                    CreatedDate = now,
                    CreatedBy = userId,
                    IsActive = true
                };

                await _unitOfWork.PurchaseOrderDocumentRepository.AddAsync(document, ct);
                await _unitOfWork.SaveChangesAsync(ct);
            }
            else if (!document.AttachmentCollectionId.HasValue || document.AttachmentCollectionId.Value == Guid.Empty)
            {
                document.AttachmentCollectionId = await CreateAttachmentCollectionAsync(ct);
                document.UpdatedDate = now;
                document.UpdatedBy = userId;

                await _unitOfWork.SaveChangesAsync(ct);
            }

            await _attachmentSchemaService.UploadListAsync(
                document.AttachmentCollectionId!.Value,
                document.DocumentType,
                files,
                userId,
                ct);

            document.IsReceived = true;
            document.ReceivedDate ??= now;
            document.UpdatedDate = now;
            document.UpdatedBy = userId;

            await _unitOfWork.SaveChangesAsync(ct);

            return OperationResult<PurchaseOrderDocumentDto>.Ok(
                await MapAsync(document, includeAttachments: true, ct),
                "Đã tải chứng từ lên đơn mua hàng.");
        }

        public async Task<OperationResult<StreamResult>> GetAttachmentContentAsync(
            Guid purchaseOrderId,
            Guid documentId,
            Guid attachmentId,
            CancellationToken ct = default)
        {
            var belongs = await AttachmentBelongsToDocumentAsync(purchaseOrderId, documentId, attachmentId, ct);
            if (!belongs)
                return OperationResult<StreamResult>.Fail("File không thuộc chứng từ đơn mua hàng này.");

            return OperationResult<StreamResult>.Ok(await _attachmentSchemaService.GetContentAsync(attachmentId, ct));
        }

        public async Task<OperationResult> DeleteAttachmentAsync(
            Guid purchaseOrderId,
            Guid documentId,
            Guid attachmentId,
            CancellationToken ct = default)
        {
            var belongs = await AttachmentBelongsToDocumentAsync(purchaseOrderId, documentId, attachmentId, ct);
            if (!belongs)
                return OperationResult.Fail("File không thuộc chứng từ đơn mua hàng này.");

            await _attachmentSchemaService.DeleteAsync(attachmentId, ct);
            return OperationResult.Ok("Đã xóa file chứng từ.");
        }

        public async Task<OperationResult> HardDeleteAttachmentAsync(
            Guid purchaseOrderId,
            Guid documentId,
            Guid attachmentId,
            CancellationToken ct = default)
        {
            var belongs = await AttachmentBelongsToDocumentAsync(purchaseOrderId, documentId, attachmentId, ct);
            if (!belongs)
                return OperationResult.Fail("File không thuộc chứng từ đơn mua hàng này.");

            await _attachmentSchemaService.HardDeleteAsync(attachmentId, ct);
            return OperationResult.Ok("Đã xóa vĩnh viễn file chứng từ.");
        }

        private async Task<bool> PurchaseOrderExistsAsync(Guid purchaseOrderId, CancellationToken ct)
        {
            return await _unitOfWork.PurchaseOrderRepository
                .Query(track: false)
                .AnyAsync(x => x.PurchaseOrderId == purchaseOrderId && (x.IsActive ?? true), ct);
        }

        private async Task<PurchaseOrderDocument?> FindDocumentAsync(
            Guid purchaseOrderId,
            Guid documentId,
            bool track,
            CancellationToken ct)
        {
            return await _unitOfWork.PurchaseOrderDocumentRepository
                .Query(track)
                .FirstOrDefaultAsync(x =>
                    x.PurchaseOrderDocumentId == documentId &&
                    x.PurchaseOrderId == purchaseOrderId,
                    ct);
        }

        private async Task<PurchaseOrderDocumentDto> MapAsync(
            PurchaseOrderDocument document,
            bool includeAttachments,
            CancellationToken ct)
        {
            var attachments = new List<AttachmentDTO>();
            if (includeAttachments && document.AttachmentCollectionId.HasValue)
            {
                attachments = await _attachmentSchemaService.ListAsync(
                    document.AttachmentCollectionId.Value,
                    document.DocumentType,
                    ct);
            }

            return new PurchaseOrderDocumentDto
            {
                PurchaseOrderDocumentId = document.PurchaseOrderDocumentId,
                PurchaseOrderId = document.PurchaseOrderId,
                DocumentType = document.DocumentType,
                DocumentTypeName = document.DocumentType.ToString(),
                AttachmentCollectionId = document.AttachmentCollectionId,
                DocumentCode = document.DocumentCode,
                DocumentName = document.DocumentName,
                Note = document.Note,
                IsReceived = document.IsReceived,
                ReceivedDate = document.ReceivedDate,
                VerifiedBy = document.VerifiedBy,
                VerifiedDate = document.VerifiedDate,
                CreatedDate = document.CreatedDate,
                CreatedBy = document.CreatedBy,
                UpdatedDate = document.UpdatedDate,
                UpdatedBy = document.UpdatedBy,
                IsActive = document.IsActive,
                Attachments = attachments
            };
        }

        private async Task<Guid> CreateAttachmentCollectionAsync(CancellationToken ct)
        {
            var id = Guid.CreateVersion7();
            await _unitOfWork.AttachmentCollectionRepository.AddAsync(new AttachmentCollection
            {
                AttachmentCollectionId = id
            }, ct);

            return id;
        }

        private async Task EnsureAttachmentCollectionAsync(Guid attachmentCollectionId, CancellationToken ct)
        {
            var exists = await _unitOfWork.AttachmentCollectionRepository
                .Query(track: false)
                .AnyAsync(x => x.AttachmentCollectionId == attachmentCollectionId, ct);

            if (!exists)
            {
                await _unitOfWork.AttachmentCollectionRepository.AddAsync(new AttachmentCollection
                {
                    AttachmentCollectionId = attachmentCollectionId
                }, ct);
            }
        }

        private async Task<bool> AttachmentBelongsToDocumentAsync(
            Guid purchaseOrderId,
            Guid documentId,
            Guid attachmentId,
            CancellationToken ct)
        {
            var document = await FindDocumentAsync(purchaseOrderId, documentId, track: false, ct);
            if (document?.AttachmentCollectionId is null)
                return false;

            return await _unitOfWork.AttachmentModelRepository
                .Query(track: false)
                .AnyAsync(x =>
                    x.AttachmentId == attachmentId &&
                    x.AttachmentCollectionId == document.AttachmentCollectionId.Value,
                    ct);
        }

        private static string? Normalize(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
