using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Attachment;
using VietausWebAPI.Core.Domain.Enums.Devandqa;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures
{
    public class GetQCInputByQC
    {
        // ----- Context: voucher detail đang được QC -----
        public long VoucherDetailId { get; set; }
        public string? MaterialExternalId { get; set; } // ProductCode
        public string? MaterialName { get; set; }       // ProductName
        public string? LotNumber { get; set; }
        public decimal? QtyKg { get; set; }
        public string? Unit { get; set; } = "kg";


        // ----- Supplier (nguồn “đúng”) -----
        public Guid? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierExternalId { get; set; }

        // ----- QC record (nullable: chưa QC) -----
        public Guid? QCInputByQCId { get; set; }
        public string? InspectionMethod { get; set; }
        public bool? IsCOAProvided { get; set; }
        public bool? IsMSDSTDSProvided { get; set; }
        public bool? IsMetalDetectionRequired { get; set; }
        public bool? IsSuccessQuality { get; set; } // Kiểm tra chất lượng đạt
        public Guid? AttachmentCollectionId { get; set; }
        public AttachmentUploadStatus AttachmentStatus { get; set; }
        public string? AttachmentLastError { get; set; }

        public QcDecision? ImportWarehouseType { get; set; }
        public string? Note { get; set; }

        // ----- QC audit -----
        public DateTime? QCCreatedDate { get; set; }
        public Guid? QCCreatedBy { get; set; }
        public string? QCEmployeeName { get; set; }
    }

}
