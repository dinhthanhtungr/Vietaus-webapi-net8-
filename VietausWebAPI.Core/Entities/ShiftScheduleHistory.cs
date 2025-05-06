using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class ShiftScheduleHistory
{
    public DateOnly? Date { get; set; }

    public string? ProductionShift { get; set; }

    public string? ShiftId { get; set; }

    public string? Note { get; set; }

    public virtual InformationShift? Shift { get; set; }
}
