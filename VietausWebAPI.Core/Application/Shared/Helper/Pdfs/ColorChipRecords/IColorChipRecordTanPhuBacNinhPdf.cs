using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PDFDtos;

namespace VietausWebAPI.Core.Application.Shared.Helper.Pdfs.ColorChipRecords
{
    public interface IColorChipRecordTanPhuBacNinhPdf
    {
        byte[] Render(ColorChipRecordPdfModel model, bool templateOnly = false);
        byte[] RenderTemplate();
    }
}
