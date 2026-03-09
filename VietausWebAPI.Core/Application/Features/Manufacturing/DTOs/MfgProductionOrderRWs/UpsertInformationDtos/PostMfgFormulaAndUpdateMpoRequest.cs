using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos
{
    public class PostMfgFormulaAndUpdateMpoRequest
    {
        // MPO
        public Guid MfgProductionOrderId { get; set; }

        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? RequiredDate { get; set; }

        public int? TotalQuantity { get; set; }
        public int? NumOfBatches { get; set; }

        public string? LabNote { get; set; }
        public string? Requirement { get; set; }
        public string? PlpuNote { get; set; }
        public StepOfProduct? StepOfProduct { get; set; }
        public string? QcCheck { get; set; }

        // Formula
        public Guid ProductId { get; set; }
        public FormulaSource? SourceType { get; set; }
        public Guid? FormulaSourceId { get; set; }
        public string? FormulaSourceNameSnapshot { get; set; }
        public Guid? VUFormulaId { get; set; }
        public string? FormulaExternalIdSnapshot { get; set; }

        public FormulaType FormulaType { get; set; }

        public bool IsStandard { get; set; }
        public string? Note { get; set; }
        public string? FormulaStatus { get; set; }

        public List<PostManufacturingFormulaMaterial> ManufacturingFormulaMaterials { get; set; } = new();
    }
}
