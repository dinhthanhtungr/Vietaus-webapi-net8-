using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulaAdjustments
{
    public class PatchManufacturingFormulaAdjustment
    {
        public Guid? ManufacturingFormulaId { get; set; }
        public string? Status { get; set; }

        public string? Note { get; set; }

        public List<PostManufacturingFormulaAdjustmentBatch>? Batches { get; set; }
    }
}
