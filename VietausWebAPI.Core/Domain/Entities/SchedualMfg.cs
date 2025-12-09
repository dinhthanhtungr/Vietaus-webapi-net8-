using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class SchedualMfg
{
    public int Idpk { get; set; }

    public Guid? ProductId { get; set; }
    public Guid? MfgProductionOrderId { get; set; }       

    public string? MachineId { get; set; }

    public int? Number { get; set; }
    public string? Note { get; set; }
    public string? QcNote { get; set; }
    public string? requirement { get; set; }
    public double? ProductRecycleRate { get; set; }

    public DateTime? ExpectedCompletionDate { get; set; }
    public DateTime? DeliveryPlanDate { get; set; }
    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public DateTime? PlanDate { get; set; }

    public string? Qcstatus { get; set; }
    public int? Area { get; set; }
    public string? BTPStatus { get; set; }
    public int? StepOfProduct { get; set; }

    public virtual MfgProductionOrder? MfgProductionOrder { get; set; }
    public virtual Product? Product { get; set; }
}
