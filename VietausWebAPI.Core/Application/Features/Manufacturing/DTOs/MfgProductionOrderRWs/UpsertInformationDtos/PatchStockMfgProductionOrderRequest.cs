using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos
{
    public class PatchStockMfgProductionOrderRequest
    {
        public Guid MfgProductionOrderId { get; set; }

        public DateTime? ExpectedDate { get; set; }
        public int? TotalQuantity { get; set; }
        public int? NumOfBatches { get; set; }

        public StepOfProduct? StepOfProduct { get; set; }

        public string? LabNote { get; set; }
        public string? Requirement { get; set; }
        public string? PlpuNote { get; set; }
        public string? QcCheck { get; set; }
    }
}
