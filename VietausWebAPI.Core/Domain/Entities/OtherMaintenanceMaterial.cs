using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class OtherMaintenanceMaterial
{
    public int Id { get; set; }

    public string? MachineId { get; set; }

    public string? MaterialName { get; set; }

    public int? QuantityUsed { get; set; }

    public string? Note { get; set; }

    public virtual MachinesCommonDatum? Machine { get; set; }
}
