using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Queries
{
    public class DeliveryOrderQuery : PaginationQuery
    {
        public Guid? CompanyId { get; set; }
        public string? Keyword { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? MerchandiseOrderId { get; set; }
        public Guid? DeliveryOrderId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
