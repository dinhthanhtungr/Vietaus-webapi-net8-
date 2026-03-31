using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PostDtos
{
    public class CreateColorChipRecordRequest
    {
        public RecordType RecordType { get; set; }
        public ChipPurpose ChipPurpose { get; set; }
        public ResinType ResinType { get; set; }

        public Guid? ProductId { get; set; }
        public string? ProductCodeSnapshot { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public string? ColorNameSnapshot { get; set; }

        public Guid? ManufacturingFormulaId { get; set; }
        public string? ManufacturingFormulaExternalIdSnapshot { get; set; }

        public Guid? DevelopmentFormulaId { get; set; }
        public string? DevelopmentFormulaExternalIdSnapshot { get; set; }

        public Guid AttachmentCollectionId { get; set; }
        public DateTime? RecordDate { get; set; }

        public Guid? CustomerId { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }
        public string? CustomerNameSnapshot { get; set; }

        public decimal? AddRate { get; set; }
        public string? Resin { get; set; }

        public decimal? TemperatureMin { get; set; }
        public decimal? TemperatureMax { get; set; }

        public string? SizeText { get; set; }
        public decimal? PelletWeightGram { get; set; }
        public string? AntiStaticInfo { get; set; }

        public string? Note { get; set; }
        public string? PrintNote { get; set; }
    }
}
