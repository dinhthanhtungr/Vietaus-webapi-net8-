using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PDFDtos;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos
{
    public class ColorChipRecordPdfQueryResult
    {
        public FormStyle FormStyle { get; set; }
        public ColorChipRecordPdfModel PdfModel { get; set; } = new();
    }
}
