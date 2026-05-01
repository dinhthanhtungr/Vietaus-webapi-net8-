using VietausWebAPI.Core.Application.Features.Labs.Helpers.SampleRequests;

namespace VietausWebAPI.Core.Application.Shared.Helper.Pdfs.ColorChipRecords
{
    public class ColorChipRecordTanPhuBacNinhPdf : ColorChipRecordTanPhuPdf, IColorChipRecordTanPhuBacNinhPdf
    {
        protected override string GetSizeStandardText(ResinStandardSpec standard)
        {
            return "1.8 * 3.4";
        }

        protected override string GetWeightStandardText(ResinStandardSpec standard)
        {
            return "80-120";
        }
    }
}
