using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class IncidentMaterial
{
    public int Id { get; set; }

    public string? IncidentId { get; set; }

    public string? MaterialName { get; set; }

    public int? QuantityUsed { get; set; }

    public string? Note { get; set; }

    public virtual IncidentReport? Incident { get; set; }
}
