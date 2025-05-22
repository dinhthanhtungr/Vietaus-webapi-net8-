using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class NewMakingHistory
{
    public string NewMakingId { get; set; } = null!;

    public string? RelatedDocument { get; set; }

    public string? MachineId { get; set; }

    public DateTime? NewMaintenanceDate { get; set; }

    public string? PerformedBy { get; set; }

    public string? Receiver { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string? Note { get; set; }

    public string? ApprovedId { get; set; }

    public virtual MachinesCommonDatum? Machine { get; set; }

    public virtual ICollection<NewMakingMaterial> NewMakingMaterials { get; set; } = new List<NewMakingMaterial>();
}
