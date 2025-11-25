using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Models.SaleAndMfgs
{
    public sealed class OrderSlim
    {
        public Guid MerchandiseOrderId { get; set; }
        public string ExternalId { get; set; } = "";
        public Guid CompanyId { get; set; }

        public Guid CustomerId { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }
        public string? CustomerNameSnapshot { get; set; }

        public List<OrderDetailSlim> Details { get; set; } = new();
    }
}
