using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class GetMfgProductionOrder
    {
        public Guid MfgProductionOrderId { get; set; }
        public string? ExternalId { get; set; }

        public Guid MerchandiseOrderId { get; set; }
        public string? MerchandiseOrderExternalId { get; set; }
        public string? ProductExternalIdSnapshot { get; set; }
        public string? ProductNameSnapshot { get; set; }



        public string? CustomerNameSnapshot { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }

        public Guid? FormulaId { get; set; }
        public string? FormulaExternalIdSnapshot { get; set; }

        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? requiredDate { get; set; }

        public int? TotalQuantity { get; set; }
        public int? TotalQuantityRequest { get; set; }
        public int? NumOfBatches { get; set; }

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


        public double? UsageRate { get; set; }


        public DateTime? CreateDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public GetManufacturingFormula selectManufacturing { get; set; } = new GetManufacturingFormula(); // Công thức được lab cho phép sản xuất

    }
}
