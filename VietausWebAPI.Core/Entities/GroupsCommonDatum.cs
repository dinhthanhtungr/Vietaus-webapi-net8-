using System;
using System.Collections.Generic;

namespace VietausWebAPI.Infrastructure.Models;

public partial class GroupsCommonDatum
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<MachinesCommonDatum> MachinesCommonData { get; set; } = new List<MachinesCommonDatum>();
}
