using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class IncidentReport
{
    public string IncidentId { get; set; } = null!;

    public string? RelatedDocument { get; set; }

    public string? MachineId { get; set; }

    public DateTime? IncidentDate { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? ApprovedId { get; set; }

    public virtual ICollection<IncidentMaterial> IncidentMaterials { get; set; } = new List<IncidentMaterial>();

    public virtual MachinesCommonDatum? Machine { get; set; }
}
