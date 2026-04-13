using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PDFDtos;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers.SampleRequests.ColourChipRecords
{
    public interface IColorChipRecordTanPhuPdf
    {
        byte[] Render(ColorChipRecordPdfModel model, bool templateOnly = false);
        byte[] RenderTemplate();
    }
}
