using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos
{
    public class GetColorChipRecordDevelopmentFormulaDto
    {
        public Guid ColorChipRecordDevelopmentFormulaId { get; set; }
        public Guid? DevelopmentFormulaId { get; set; }
        public bool IsActive { get; set; } = true;

        public string? DevelopmentFormulaExternalId { get; set; }
        public string? DevelopmentFormulaName { get; set; }
    }
}
