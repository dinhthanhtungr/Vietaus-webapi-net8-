using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class GetDeliverer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? DelivererType { get; set; }
    }
}
