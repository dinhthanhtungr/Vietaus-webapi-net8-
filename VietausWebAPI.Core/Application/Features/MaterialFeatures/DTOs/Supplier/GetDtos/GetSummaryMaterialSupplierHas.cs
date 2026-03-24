using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.GetDtos
{
    public class GetSummaryMaterialSupplierHas
    {
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public string? MaterialCode { get; set; } = null;
        public string? CategoryName { get; set; } = null;
        public string? Notes { get; set; }
    }
}
