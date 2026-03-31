using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Domain.Entities.SampleRequestSchema
{
    public class ColorChipRecord
    {
        public Guid ColorChipRecordId { get; set; }

        public RecordType RecordType { get; set; }
        public ChipPurpose ChipPurpose { get; set; }
        public ResinType ResinType { get; set; }

        public Guid? ProductId { get; set; }
        public string? ProductCodeSnapshot { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public string? ColorNameSnapshot { get; set; }

        public Guid? ManufacturingFormulaId { get; set; } // Công thức VA lúc tạo mẫu
        public string? ManufacturingFormulaExternalIdSnapshot { get; set; } // Snapshot của ExternalId công thức VA lúc tạo mẫu

        public Guid? DevelopmentFormulaId { get; set; } // Công thức VU lúc phát triển mẫu
        public string? DevelopmentFormulaExternalIdSnapshot { get; set; } // Snapshot của ExternalId công thức VU lúc phát triển mẫu

        public Guid AttachmentCollectionId { get; set; }
        public DateTime? RecordDate { get; set; }

        public Guid? CustomerId { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }
        public string? CustomerNameSnapshot { get; set; }

        public decimal? AddRate { get; set; }
        public string? Resin { get; set; }

        public decimal? TemperatureMin { get; set; }
        public decimal? TemperatureMax { get; set; }

        public string? SizeText { get; set; }          // ví dụ: 2.4 x 3.2
        public decimal? PelletWeightGram { get; set; } // trọng lượng hạt
        public string? AntiStaticInfo { get; set; }    // "Không có", "Có", ...

        public string? Note { get; set; }
        public string? PrintNote { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; }


        public virtual AttachmentCollection AttachmentCollection { get; set; } = null!;
        public virtual Customer? Customer { get; set; }
        public virtual Formula? DevelopmentFormula { get; set; }
        public virtual ManufacturingFormula? ManufacturingFormula { get; set; }
        public virtual Product? Product { get; set; }
    }
}
