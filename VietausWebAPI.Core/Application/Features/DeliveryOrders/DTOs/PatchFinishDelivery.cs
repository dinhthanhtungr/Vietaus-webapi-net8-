
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Merchadises;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class PatchFinishDelivery
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
