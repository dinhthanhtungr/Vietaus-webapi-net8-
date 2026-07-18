using Microsoft.AspNetCore.Http;
using VietausWebAPI.Core.Application.Features.Attachments.DTOs;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.PurchaseOrderDocuments;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts
{
    public interface IPurchaseOrderDocumentService
    {
        Task<OperationResult<IReadOnlyList<PurchaseOrderDocumentDto>>> GetListAsync(
            Guid purchaseOrderId,
            AttachmentSlot? documentType,
            bool includeInactive = false,
            CancellationToken ct = default);

        Task<OperationResult<PurchaseOrderDocumentDto>> GetByIdAsync(
            Guid purchaseOrderId,
            Guid documentId,
            CancellationToken ct = default);

        Task<OperationResult<PurchaseOrderDocumentDto>> CreateAsync(
            Guid purchaseOrderId,
            CreatePurchaseOrderDocumentRequest request,
            CancellationToken ct = default);

        Task<OperationResult<PurchaseOrderDocumentDto>> UpdateAsync(
            Guid purchaseOrderId,
            Guid documentId,
            UpdatePurchaseOrderDocumentRequest request,
            CancellationToken ct = default);

        Task<OperationResult> DeleteAsync(
            Guid purchaseOrderId,
            Guid documentId,
            CancellationToken ct = default);

        Task<OperationResult<IReadOnlyList<AttachmentDTO>>> GetAttachmentsAsync(
            Guid purchaseOrderId,
            Guid documentId,
            AttachmentSlot? slot,
            CancellationToken ct = default);

        Task<OperationResult<IReadOnlyList<AttachmentDTO>>> UploadAttachmentsAsync(
            Guid purchaseOrderId,
            Guid documentId,
            List<IFormFile> files,
            CancellationToken ct = default);

        Task<OperationResult<PurchaseOrderDocumentDto>> UploadAttachmentsByTypeAsync(
            Guid purchaseOrderId,
            AttachmentSlot documentType,
            List<IFormFile> files,
            CancellationToken ct = default);

        Task<OperationResult<StreamResult>> GetAttachmentContentAsync(
            Guid purchaseOrderId,
            Guid documentId,
            Guid attachmentId,
            CancellationToken ct = default);

        Task<OperationResult> DeleteAttachmentAsync(
            Guid purchaseOrderId,
            Guid documentId,
            Guid attachmentId,
            CancellationToken ct = default);

        Task<OperationResult> HardDeleteAttachmentAsync(
            Guid purchaseOrderId,
            Guid documentId,
            Guid attachmentId,
            CancellationToken ct = default);
    }
}
