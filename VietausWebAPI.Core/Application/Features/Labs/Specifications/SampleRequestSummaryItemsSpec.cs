using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Queries.CreateSampleRequest;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.Specifications
{
    public sealed class SampleRequestSummaryItemsSpec : Specification<SampleRequest, SampleRequestSummaryItemsSpec>
    {
        public SampleRequestSummaryItemsSpec(SampleRequestQuery q)
        {
            if (!string.IsNullOrWhiteSpace(q.Keyword))
            {
                var kw = q.Keyword.Trim();
                Query.Where(x =>
                    (x.ExternalId ?? "").Contains(kw) ||
                    (x.CreatedByNavigation.ExternalId ?? "").Contains(kw) ||
                    (x.Product.ColourCode ?? "").Contains(kw) ||
                    (x.Customer.ExternalId ?? "").Contains(kw) ||
                    (x.Customer.CustomerName ?? "").Contains(kw)
                );
            }

            if (q.CompanyId.HasValue && q.CompanyId.Value != Guid.Empty)
            {
                Query.Where(x => x.CompanyId == q.CompanyId.Value);
            }

            Query.OrderBy(x => x.ExternalId)
                 .Skip((q.PageNumber <= 0 ? 0 : (q.PageNumber - 1) * (q.PageSize <= 0 ? 15 : q.PageSize)))
                 .Take(q.PageSize <= 0 ? 15 : q.PageSize);

            // Projection sớm ra DTO (EF sẽ dịch thành SELECT đúng cột)
            Query.Select(c => new SampleRequestSummaryDTO
            {
                SampleRequestId = c.SampleRequestId,
                ExternalId = c.ExternalId,
                ProductId = c.ProductId,
                ColourCode = c.Product.ColourCode,
                Status = c.Status,
                CustomerName = c.Customer.CustomerName,
                LabName = c.Product.CreatedByNavigation != null ? c.Product.CreatedByNavigation.FullName : null,
                CreatedBy = c.CreatedByNavigation != null ? c.CreatedByNavigation.FullName : null,
                CreatedDate = c.CreatedDate,
                ExpectedDeliveryDate = c.ExpectedDeliveryDate,
                RequestDeliveryDate = c.RequestDeliveryDate,
                RealDeliveryDate = c.RealDeliveryDate,
                RealPriceQuoteDate = c.RealPriceQuoteDate,
                ExpectedPriceQuoteDate = c.ExpectedPriceQuoteDate
            });
        }
    }
}
