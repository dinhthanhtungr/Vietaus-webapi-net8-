using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.PurchaseOrderDocuments
{
    public class CreatePurchaseOrderDocumentRequest
    {
        public AttachmentSlot DocumentType { get; set; } = AttachmentSlot.Other;
        public Guid? AttachmentCollectionId { get; set; }
        public string? DocumentCode { get; set; }
        public string? DocumentName { get; set; }
        public string? Note { get; set; }
        public bool IsReceived { get; set; }
        public DateTime? ReceivedDate { get; set; }
    }
}
