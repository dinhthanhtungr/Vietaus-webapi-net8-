using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.Suppliers
{
    public class SupplierInformationsDTO
    {
        public Guid supplierId { get; set; }
        public string? externalId { get; set; }
        public string? Name { get; set; }
        public string? VendorPhone { get; set; } // Tên vendor
    }
}
