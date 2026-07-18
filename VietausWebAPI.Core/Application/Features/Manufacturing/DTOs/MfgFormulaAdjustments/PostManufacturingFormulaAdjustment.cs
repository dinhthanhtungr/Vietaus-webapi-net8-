using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulaAdjustments
{
    public class PostManufacturingFormulaAdjustment
    {
        public Guid MfgProductionOrderId { get; set; }
        public Guid? ManufacturingFormulaId { get; set; }
        public string Status { get; set; } = "Draft";

        public string? Note { get; set; }
        public List<PostManufacturingFormulaAdjustmentBatch> Batches { get; set; } = new();
    }
}
