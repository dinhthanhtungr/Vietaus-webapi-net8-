using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public partial class MfgProductionOrder
    {
        public Guid MfgProductionOrderId { get; set; }
        public string? ExternalId { get; set; }

        public Guid MerchandiseOrderId { get; set; }
        public string? MerchandiseOrderExternalId { get; set; }
       
        public string? ProductionType { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductExternalIdSnapshot { get; set; }
        public string? ProductNameSnapshot { get; set; }

        public Guid? CustomerId { get; set; }
        public string? CustomerNameSnapshot { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }

        public Guid? FormulaId { get; set; }
        public string? FormulaExternalIdSnapshot { get; set; }

        //public Guid? ManufacturingFormulaId { get; set; }
        //public string? ManufacturingFormulaExternalId { get; set; }

        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? requiredDate { get; set; }

        public int? TotalQuantity { get; set; }
        public int? TotalQuantityRequest { get; set; }
        public int? NumOfBatches { get; set; }
        public decimal UnitPriceAgreed { get; set; }

        public string? Status { get; set; }

        public string? LabNote { get; set; }
        public string? Requirement { get; set; }
        public string? PlpuNote { get; set; }
        public string? BagType { get; set; }

        public Guid? CompanyId { get; set; }
        public bool? IsActive { get; set; }

        public string? QcCheck { get; set; }              
        public decimal? QualifiedQuantity { get; set; } 
        public decimal? RejectedQuantity { get; set; } 
        public decimal? WasteQuantity { get; set; } 

        public DateTime? CreateDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual MerchandiseOrder? MerchandiseOrder { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Company? Company { get; set; }
        public virtual Employee? CreatedByNavigation { get; set; }
        public virtual Employee? UpdatedByNavigation { get; set; }


        public virtual ICollection<ManufacturingFormula> ManufacturingFormulas { get; set; } = new List<ManufacturingFormula>();
        public virtual ICollection<MfgProductionOrderLog> ManufacturingOrderLogs { get; set; } = new List<MfgProductionOrderLog>();

    }
}
