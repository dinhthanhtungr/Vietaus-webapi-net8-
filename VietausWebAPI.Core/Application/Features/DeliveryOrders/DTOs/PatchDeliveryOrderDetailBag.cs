using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class PatchDeliveryOrderDetailBag
    {
        public Guid Id { get; set; }
        public int NumOfBags { get; set; }
    }
}
