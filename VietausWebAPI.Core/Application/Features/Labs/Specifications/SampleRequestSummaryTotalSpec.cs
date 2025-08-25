using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.Queries.CreateSampleRequest;
using VietausWebAPI.Core.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VietausWebAPI.Core.Application.Features.Labs.Specifications
{
    /// <summary>
    /// Thử nghiệp sử dụng Specification Pattern nhưng không dùng để lấy tổng số lượng
    /// </summary>
    public sealed class SampleRequestSummaryTotalSpec : Specification<SampleRequest>
    {
        public SampleRequestSummaryTotalSpec(SampleRequestQuery q)
        {
            if (!string.IsNullOrWhiteSpace(q.Keyword))
            {
                var kw = q.Keyword.Trim();
                Query.Where(x =>
                    (x.ExternalId ?? "").Contains(kw) ||
                    (x.CreatedByNavigation.ExternalId ?? "").Contains(kw) ||
                    (x.Product.ColourCode ?? "").Contains(kw) ||
                    (x.Customer.ExternalId ?? "").Contains(kw) ||
                    (x.Customer.CustomerName ?? "").Contains(kw));
            }
            if (q.CompanyId is Guid cid && cid != Guid.Empty)
                Query.Where(x => x.CompanyId == cid);
        }
    }
}
