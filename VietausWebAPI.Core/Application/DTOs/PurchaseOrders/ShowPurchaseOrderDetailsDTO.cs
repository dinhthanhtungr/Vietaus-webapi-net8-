using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.PurchaseOrders
{
    public class ShowPurchaseOrderDetailsDTO
    {
        public string name { get; set; }
        public string MaterialGroupName { get; set; }
        public string externalId { get; set; }
        public Guid materialId { get; set; }
        public string unit { get;set; }

        public decimal? UnitPrice { get; set; }

        //public DateTime? DeliveryDate { get; set; }
        public int RequestedQuantity { get; set; }

        public string? Note { get; set; }
    }
}
