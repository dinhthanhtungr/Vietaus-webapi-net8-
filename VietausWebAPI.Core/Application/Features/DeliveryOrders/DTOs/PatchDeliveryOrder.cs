using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class PatchDeliveryOrder
    {
        public Guid Id { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Status { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UpdateBy { get; set; }
        public List<Guid> Deliverers { get; set; } = new List<Guid>();

        public List<PatchDeliveryOrderDetailBag> Details { get; set; } = new();
    }
}
