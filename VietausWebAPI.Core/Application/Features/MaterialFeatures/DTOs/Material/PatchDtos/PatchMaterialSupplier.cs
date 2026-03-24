using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.PatchDtos
{
    public class PatchMaterialSupplier
    {
        public Guid? MaterialSupplierId { get; set; }
        public Guid? SupplierId { get; set; }
        public int? MinDeliveryDays { get; set; } = 1;
        public decimal? CurrentPrice { get; set; }
        public string? Currency { get; set; } = "VND";
        public bool? IsPreferred { get; set; }

        public bool? IsActive { get; set; }
    }
}
