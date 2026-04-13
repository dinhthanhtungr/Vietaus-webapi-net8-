using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos
{
    public class SaveColorChipRecordResultDto
    {
        public Guid ColorChipRecordId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? AttachmentCollectionId { get; set; }
    }
}
