using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier
{
    public class SupplierQuery : PaginationQuery
    {
        public Guid? SupplierId { get; set; }
        public string? Keyword { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
