using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MaintenanceHistory
{
    public string MaintenanceId { get; set; } = null!;

    public string? RelatedDocument { get; set; }

    public string? MachineId { get; set; }

    public DateOnly? MaintenanceDate { get; set; }

    public string? PerformedBy { get; set; }

    public string? Receiver { get; set; }

    public DateOnly? CompletionDate { get; set; }

    public string? Note { get; set; }

    public string? ApprovedId { get; set; }

    public virtual MachinesCommonDatum? Machine { get; set; }

    public virtual ICollection<MaintenanceMaterial> MaintenanceMaterials { get; set; } = new List<MaintenanceMaterial>();
}
