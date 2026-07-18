using VietausWebAPI.Core.Application.Features.Sales.DTOs.QuotationDTOs;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.QuotationFeatures
{
    public interface IQuotationService
    {
        Task<OperationResult<Guid>> CreateAsync(CreateQuotationRequest request, CancellationToken ct = default);
        Task<OperationResult<PagedResult<QuotationSummaryDto>>> GetPagedAsync(QuotationQuery query, CancellationToken ct = default);
        Task<OperationResult<QuotationDetailDto>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<OperationResult<byte[]>> PrintPdfAsync(Guid id, CancellationToken ct = default);
    }
}
