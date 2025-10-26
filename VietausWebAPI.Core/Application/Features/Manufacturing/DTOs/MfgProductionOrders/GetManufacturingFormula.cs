using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class GetManufacturingFormula
    {
        public Guid ManufacturingFormulaId { get; set; }
        public string? ExternalId { get; set; }
        public string? MfgProductionOrderExternalId { get; set; }
        public string? Name { get; set; }

        // các field từ MfgProductionOrder bạn muốn hiển thị
        public string? MfgExternalId { get; set; }
        public string? MerchandiseOrderExternalId { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public string? ProductExternalIdSnapshot { get; set; }
        public bool IsRecycle { get; set; } // Lấy từ Product.IsRecycle
        public string? CustomerNameSnapshot { get; set; }

        public decimal? MfgTotalPrice { get; set; } // Giá sale lên

        public Guid? mfgProductionOrderId { get; set; }
        public string? Status { get; set; }

        public int TotalQuantityRequest { get; set; }

        public Guid? VUFormulaId { get; set; }
        public string? FormulaExternalIdSnapshot { get; set; }

        public string? SourceType { get; set; }                     // "FromVA" hoặc "FromVU"

        public Guid? SourceManufacturingFormulaId { get; set; }
        public string? SourceManufacturingExternalIdSnapshot { get; set; } // ví dụ: mã của nguồn lúc copy

        public Guid? SourceVUFormulaId { get; set; }
        public string? SourceVUExternalIdSnapshot { get; set; }

        public bool IsSelect { get; set; }
        public bool IsActive { get; set; }
        public bool IsStandard { get; set; }
        public string? Note { get; set; }

        public DateTime? createdDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? companyId { get; set; }

        public virtual ICollection<GetManufacturingFormulaMaterial> ManufacturingFormulaMaterials { get; set; } = new List<GetManufacturingFormulaMaterial>();
    }
}
