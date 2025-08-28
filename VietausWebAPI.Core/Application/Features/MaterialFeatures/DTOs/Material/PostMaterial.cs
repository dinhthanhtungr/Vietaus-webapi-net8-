using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material
{
    public class PostMaterial
    {
        public string? ExternalId { get; set; }

        public string? CustomCode { get; set; }

        public string? Name { get; set; }

        public Guid CategoryId { get; set; }

        public double? Weight { get; set; }

        public string? Unit { get; set; }

        public string? Package { get; set; }

        public string? Comment { get; set; }

        public double? MinQuantity { get; set; }

        public Guid CompanyId { get; set; }

        public bool? IsActive { get; set; }

        public string? Barcode { get; set; }

        public string? ImagePath { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        //public Guid MaterialsSuppliers { get; set; } 

        public PostPriceHistory InitialPrice { get; set; } = new PostPriceHistory();
        public List<PostMaterialSupplier>? Suppliers { get; set; } // nhiều NCC

    }
}
