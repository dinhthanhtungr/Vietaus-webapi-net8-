using System;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Domain.Entities.SampleRequestSchema
{
    public class ColorChipRecord
    {
        // =========================================================
        // 1. Identity
        // =========================================================
        public Guid ColorChipRecordId { get; set; }

        // =========================================================
        // 2. Classification / Business Type
        // =========================================================
        public RecordType RecordType { get; set; }
        public ResinType ResinType { get; set; }
        public LogoType LogoType { get; set; }
        public FormStyle FormStyle { get; set; }

        // =========================================================
        // 3. Product Snapshot
        // =========================================================
        public Guid? ProductId { get; set; }
        public string? ProductCodeSnapshot { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public string? ColorNameSnapshot { get; set; }

        // =========================================================
        // 4. Formula Snapshot
        // =========================================================
        public Guid? ManufacturingFormulaId { get; set; } // Công thức VA lúc tạo mẫu
        public string? ManufacturingFormulaExternalIdSnapshot { get; set; }

        public Guid? DevelopmentFormulaId { get; set; } // Công thức VU lúc phát triển mẫu
        public string? DevelopmentFormulaExternalIdSnapshot { get; set; }

        // =========================================================
        // 5. Customer Snapshot
        // =========================================================
        public Guid? CustomerId { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }
        public string? CustomerNameSnapshot { get; set; }

        // =========================================================
        // 6. Technical Information
        // =========================================================
        public decimal? AddRate { get; set; }
        public string? Resin { get; set; }

        public decimal? TemperatureMin { get; set; }
        public decimal? TemperatureMax { get; set; }

        // =========================================================
        // 7. Physical / Form Information
        // =========================================================
        public string? SizeText { get; set; }          // ví dụ: 2.4 x 3.2
        public decimal? PelletWeightGram { get; set; } // trọng lượng hạt
        public string? AntiStaticInfo { get; set; }    // "Không có", "Có", ...

        // =========================================================
        // 8. Document / Record Info
        // =========================================================
        public Guid AttachmentCollectionId { get; set; }
        public DateTime? RecordDate { get; set; }
        public string? Note { get; set; }
        public string? PrintNote { get; set; }

        // =========================================================
        // 9. Audit
        // =========================================================
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; }

        // =========================================================
        // 10. Navigation Properties
        // =========================================================
        public virtual AttachmentCollection AttachmentCollection { get; set; } = null!;
        public virtual Customer? Customer { get; set; }
        public virtual Formula? DevelopmentFormula { get; set; }
        public virtual ManufacturingFormula? ManufacturingFormula { get; set; }
        public virtual Product? Product { get; set; }
    }
}
