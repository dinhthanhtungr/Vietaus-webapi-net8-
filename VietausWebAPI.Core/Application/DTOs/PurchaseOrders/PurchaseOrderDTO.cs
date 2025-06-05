using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.PurchaseOrders
{
    public class PurchaseOrderDTO
    {
        public string? POCode { get; set; }
        //public Guid supplierId { get; set; }
        public Guid POID { get; set; }
        public string? SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        //public string? EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
    }
}
