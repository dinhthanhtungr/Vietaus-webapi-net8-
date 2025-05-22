using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MaintenanceMaterial
{
    public int Id { get; set; }

    public string MaintenanceId { get; set; } = null!;

    public string MaterialName { get; set; } = null!;

    public int QuantityUsed { get; set; }

    public string? Note { get; set; }

    public virtual MaintenanceHistory Maintenance { get; set; } = null!;
}
