using VietausWebAPI.Core.Application.Features.Attachments.DTOs;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.PurchaseOrderDocuments
{
    public class PurchaseOrderDocumentDto
    {
        public Guid PurchaseOrderDocumentId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public AttachmentSlot DocumentType { get; set; }
        public string DocumentTypeName { get; set; } = string.Empty;
        public Guid? AttachmentCollectionId { get; set; }
        public string? DocumentCode { get; set; }
        public string? DocumentName { get; set; }
        public string? Note { get; set; }
        public bool IsReceived { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public Guid? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public IReadOnlyList<AttachmentDTO> Attachments { get; set; } = new List<AttachmentDTO>();
    }
}
