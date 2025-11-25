using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material
{
    public class GetMaterialSupplier
    {
        public Guid? MaterialSupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? ExternalId { get; set; }
        public decimal? CurrentPrice { get; set; }
        public string? Currency { get; set; } // "VND"
        public int? MinDeliveryDays { get; set; }
        public bool? IsPreferred { get; set; }
        public bool? isActive { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}