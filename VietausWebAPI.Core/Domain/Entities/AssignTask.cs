using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class AssignTask
{
    public int Id { get; set; }

    public DateTime? RecordDate { get; set; }

    public string? ShiftId { get; set; }

    public string? MachineId { get; set; }

    public string? Operator { get; set; }

    public string? Note { get; set; }

    public virtual InformationShift? Shift { get; set; }
}
