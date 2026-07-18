using System;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulaAdjustments
{
    public class ManufacturingFormulaAdjustmentQuery
    {
        public Guid? MfgProductionOrderId { get; set; }
        public Guid? ManufacturingFormulaId { get; set; }
        public int? BatchNo { get; set; }
        public int? TrialNo { get; set; }
        public string? Status { get; set; }
        public string? BatchStatus { get; set; }
        public bool? IsAdditionalBatch { get; set; }
        public bool IncludeInactive { get; set; }
    }
}
