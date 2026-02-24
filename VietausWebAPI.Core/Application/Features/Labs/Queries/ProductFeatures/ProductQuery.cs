using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.Queries.ProductFeatures
{
    public class ProductQuery : PaginationQuery
    {
        public Guid? CompanyId { get; set; }
        public string? Keyword { get; set; }
        public string? status { get; set; }
        public Guid? FormulaId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? ProductId { get; set; }
    }
}
