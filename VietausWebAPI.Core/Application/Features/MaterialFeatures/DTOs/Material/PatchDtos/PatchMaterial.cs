using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.PatchDtos
{
    public class PatchMaterial
    {
        public Guid MaterialId { get; set; }
        public string? CustomCode { get; set; }

        public string? Name { get; set; }

        public Guid CategoryId { get; set; }

        public double? Weight { get; set; }

        public string? Unit { get; set; }

        public string? Package { get; set; }

        public string? Comment { get; set; }

        public double? MinQuantity { get; set; }

        public string? Barcode { get; set; }

        public List<PatchMaterialSupplier>? Suppliers { get; set; } // nhiều NCC

    }
}
