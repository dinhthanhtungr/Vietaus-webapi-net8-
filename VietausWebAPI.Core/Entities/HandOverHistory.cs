using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class HandOverHistory
{
    public DateTime? RecordTime { get; set; }

    public string? ShiftId { get; set; }

    public string? Note { get; set; }

    public virtual InformationShift? Shift { get; set; }
}
