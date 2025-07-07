using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.Queries.ProductStandardFeature
{
    public class ProductStandardQuery : PaginationQuery
    {
        public string? keyword { get; set; } = null;
    }
}
