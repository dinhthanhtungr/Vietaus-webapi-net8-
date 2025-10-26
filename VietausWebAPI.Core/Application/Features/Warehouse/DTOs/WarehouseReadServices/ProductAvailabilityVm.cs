using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public class ProductAvailabilityVm
    {
        public string ProductExternalId { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;

        public decimal AvailableQuantity { get; set; } // DECIMAL
    }
}
