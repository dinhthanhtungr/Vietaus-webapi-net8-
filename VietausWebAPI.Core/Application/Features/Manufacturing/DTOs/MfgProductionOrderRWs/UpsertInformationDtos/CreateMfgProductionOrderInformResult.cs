using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos
{
    public class CreateMfgProductionOrderInformResult
    {
        public Guid MfgProductionOrderId { get; set; }
        public string ExternalId { get; set; } = string.Empty;

        public Guid? ManufacturingFormulaId { get; set; }
        public string? ManufacturingFormulaExternalId { get; set; }
    }
}
