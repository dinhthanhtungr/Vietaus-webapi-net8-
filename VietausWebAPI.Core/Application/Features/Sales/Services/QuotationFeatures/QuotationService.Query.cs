using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.QuotationDTOs;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.QuotationFeatures
{
    public sealed partial class QuotationService
    {
        public async Task<OperationResult<PagedResult<QuotationSummaryDto>>> GetPagedAsync(QuotationQuery query, CancellationToken ct = default)
        {
            query ??= new QuotationQuery();
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 15;

            var viewer = await _visibilityHelper.BuildViewerScopeAsync(ct);
            var source = ApplyQuotationVisibility(_quotationRepository.Query(), viewer);

            if (!query.IncludeInactive)
                source = source.Where(x => x.IsActive);

            if (query.CustomerId.HasValue)
                source = source.Where(x => x.CustomerId == query.CustomerId.Value);

            if (query.SaleEmployeeId.HasValue)
                source = source.Where(x => x.SaleEmployeeId == query.SaleEmployeeId.Value);

            if (query.Status.HasValue)
                source = source.Where(x => x.Status == query.Status.Value);

            if (query.From.HasValue)
                source = source.Where(x => x.QuotationDate >= query.From.Value.Date);

            if (query.To.HasValue)
            {
                var toExclusive = query.To.Value.Date.AddDays(1);
                source = source.Where(x => x.QuotationDate < toExclusive);
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                source = source.Where(x =>
                    x.ExternalId.Contains(keyword) ||
                    x.Customer.ExternalId.Contains(keyword) ||
                    x.Customer.CustomerName.Contains(keyword) ||
                    x.Lines.Any(l =>
                        l.ProductExternalIdSnapshot.Contains(keyword) ||
                        l.ProductNameSnapshot.Contains(keyword)));
            }

            var total = await source.CountAsync(ct);
            var items = await source
                .OrderByDescending(x => x.QuotationDate)
                .ThenByDescending(x => x.CreatedDate)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new QuotationSummaryDto
                {
                    QuotationId = x.QuotationId,
                    ExternalId = x.ExternalId,
                    CustomerId = x.CustomerId,
                    CustomerCode = x.Customer.ExternalId,
                    CustomerName = x.Customer.CustomerName,
                    SaleEmployeeId = x.SaleEmployeeId,
                    SaleEmployeeCode = x.SaleEmployee.ExternalId,
                    SaleEmployeeName = x.SaleEmployee.FullName,
                    Status = x.Status,
                    QuotationDate = x.QuotationDate,
                    ValidUntil = x.ValidUntil,
                    TotalAmount = x.TotalAmount,
                    LineCount = x.Lines.Count,
                    IsActive = x.IsActive
                })
                .ToListAsync(ct);

            return OperationResult<PagedResult<QuotationSummaryDto>>.Ok(
                new PagedResult<QuotationSummaryDto>(items, total, query.PageNumber, query.PageSize));
        }

        private IQueryable<Quotation> ApplyQuotationVisibility(IQueryable<Quotation> query, ViewerScope viewer)
        {
            var visibleCustomerIds = _visibilityHelper
                .ApplyCustomer(_unitOfWork.CustomerRepository.Query(), viewer)
                .Select(x => x.CustomerId);

            return query.Where(x => x.CompanyId == viewer.CompanyId && visibleCustomerIds.Contains(x.CustomerId));
        }
    }
}
