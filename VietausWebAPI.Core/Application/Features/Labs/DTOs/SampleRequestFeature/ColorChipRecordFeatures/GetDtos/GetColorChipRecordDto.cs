using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos
{
    public class GetColorChipRecordDto
    {
        public Guid ColorChipRecordId { get; set; }

        public RecordType RecordType { get; set; }
        public ResinType ResinType { get; set; }
        public LogoType LogoType { get; set; }
        public FormStyle FormStyle { get; set; }

        public Guid? ProductId { get; set; }

        public string? Machine { get; set; }
        public string? Resin { get; set; }
        public string? TemperatureLimit { get; set; }

        public string? SizeText { get; set; }
        public decimal? PelletWeightGram { get; set; }
        public string? NetWeightGram { get; set; }
        public bool? Electrostatic { get; set; }

        public decimal Lightness { get; set; }   // L*
        public decimal AValue { get; set; }      // a*
        public decimal BValue { get; set; }      // b*


        public Guid? AttachmentCollectionId { get; set; }
        public DateTime? RecordDate { get; set; }
        public string? Note { get; set; }
        public string? PrintNote { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; }

        public string? ProductName { get; set; }
        public string? ProductExternalId { get; set; }

        public List<GetColorChipRecordDevelopmentFormulaDto> DevelopmentFormulas { get; set; } = new();
    }
}
