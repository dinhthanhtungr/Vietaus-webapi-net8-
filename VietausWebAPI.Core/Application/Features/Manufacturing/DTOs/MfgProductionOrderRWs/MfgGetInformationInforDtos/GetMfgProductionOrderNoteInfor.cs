using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.MfgGetInformationDtos
{
    public class GetMfgProductionOrderNoteInfor
    {
        public Guid? MfgProductionOrderId { get; set; }

        public string? LabNote { get; set; }
        public string? Requirement { get; set; }
        public string? PlpuNote { get; set; }

        public string? QcCheck { get; set; }
        public StepOfProduct? StepOfProduct { get; set; }

        public GetMfgProductionOrderFormulaInfor? FormulaInfor { get; set; }
    }
}
