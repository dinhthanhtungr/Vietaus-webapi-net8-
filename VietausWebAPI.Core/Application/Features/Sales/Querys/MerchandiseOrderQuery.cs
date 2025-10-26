using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Sales.Querys
{
    public class MerchandiseOrderQuery : PaginationQuery
    {
        public Guid? CompanyId { get; set; }
        public string? Keyword { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? MerchandiseOrderId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
