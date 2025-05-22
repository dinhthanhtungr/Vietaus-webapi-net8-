using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class GroupsCommonDatum
{
    public string GroupId { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<MachinesCommonDatum> MachinesCommonData { get; set; } = new List<MachinesCommonDatum>();
}
