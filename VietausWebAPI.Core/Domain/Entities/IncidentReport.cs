using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class IncidentReport
{
    public string IncidentId { get; set; } = null!;

    public string? RelatedDocument { get; set; }

    public string? MachineId { get; set; }

    public DateTime? IncidentDate { get; set; }

    public string? PerformedBy { get; set; }

    public string? Receiver { get; set; }

    public DateTime? CopletionDate { get; set; }

    public string? Note { get; set; }

    public string? ApprovedId { get; set; }

    public virtual ICollection<IncidentMaterial> IncidentMaterials { get; set; } = new List<IncidentMaterial>();

    public virtual MachinesCommonDatum? Machine { get; set; }
}
