using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.PurchaseOrders
{
    public class PurchaseOrderDetailsDTO
    {
        //public Guid? Poid { get; set; }

        public Guid? MaterialId { get; set; }

        public decimal? UnitPrice { get; set; }

        //public DateTime? DeliveryDate { get; set; }
        public int Quantity { get; set; }

        public string? Note { get; set; }

    }
}
