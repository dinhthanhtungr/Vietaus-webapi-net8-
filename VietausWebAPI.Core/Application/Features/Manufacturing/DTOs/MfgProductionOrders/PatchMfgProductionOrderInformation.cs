using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class PatchMfgProductionOrderInformation
    {
        public Guid MfgProductionOrderId { get; set; }
        public Guid ManufacturingFormulaId { get; set; }

        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? requiredDate { get; set; }

        public int? TotalQuantity { get; set; }
        public int? NumOfBatches { get; set; }

        public string? Status { get; set; }

        public string? LabNote { get; set; }
        public string? Requirement { get; set; }
        public string? PlpuNote { get; set; }

        public bool? IsActive { get; set; }


        public string? QcCheck { get; set; }
        //public virtual ICollection<PatchMfgFormulaMaterial> ManufacturingFormulaMaterials { get; set; } = new List<PatchMfgFormulaMaterial>();
    }
}
