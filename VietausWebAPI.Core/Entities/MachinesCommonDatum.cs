using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class MachinesCommonDatum
{
    public string MachineId { get; set; } = null!;

    public string MachineName { get; set; } = null!;

    public string GroupId { get; set; } = null!;

    public string PartId { get; set; } = null!;

    public string? Description { get; set; }

    public virtual GroupsCommonDatum Group { get; set; } = null!;

    public virtual PartsCommonDatum Part { get; set; } = null!;
}
