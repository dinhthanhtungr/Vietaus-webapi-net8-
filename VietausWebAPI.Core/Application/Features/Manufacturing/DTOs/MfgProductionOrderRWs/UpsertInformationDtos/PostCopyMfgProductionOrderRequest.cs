using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos
{
    public class PostCopyMfgProductionOrderRequest
    {
        public Guid SourceMfgProductionOrderId { get; set; }
        public string? CopyReason { get; set; }

        public decimal? TotalQuantity { get; set; }
        public int? NumOfBatches { get; set; }
        public DateTime? ExpectedDate { get; set; }

        public StepOfProduct? StepOfProduct { get; set; }
        public string? LabNote { get; set; }
        public string? Requirement { get; set; }
        public string? PlpuNote { get; set; }
        public string? QcCheck { get; set; }
    }
}
