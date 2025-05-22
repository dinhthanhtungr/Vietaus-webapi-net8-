using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class InformationShift
{
    public string ShiftId { get; set; } = null!;

    public string? ShiftName { get; set; }

    public virtual ICollection<AssignTask> AssignTasks { get; set; } = new List<AssignTask>();
}
