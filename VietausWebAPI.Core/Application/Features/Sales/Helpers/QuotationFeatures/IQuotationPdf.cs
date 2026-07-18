using VietausWebAPI.Core.Application.Features.Sales.DTOs.QuotationDTOs;

namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.QuotationFeatures
{
    public interface IQuotationPdf
    {
        byte[] Render(QuotationDetailDto quotation);
    }
}
