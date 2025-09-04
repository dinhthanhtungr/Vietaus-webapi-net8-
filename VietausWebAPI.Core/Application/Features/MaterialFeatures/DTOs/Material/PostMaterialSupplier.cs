using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material
{
    public class PostMaterialSupplier
    {
        public Guid SupplierId { get; set; }
        public int? MinDeliveryDays { get; set; } = 1;
        public decimal? CurrentPrice { get; set; }
        public string? Currency { get; set; } = "VND";
        public bool? IsPreferred { get; set; }
        public bool? isActive { get; set; } = true;
    }
}
