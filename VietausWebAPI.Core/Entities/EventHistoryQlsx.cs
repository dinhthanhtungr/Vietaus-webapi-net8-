using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class EventHistoryQlsx
{
    public int Id { get; set; }

    public string? MachineId { get; set; }

    public string? EventId { get; set; }

    public DateOnly? EventDate { get; set; }

    public virtual QlsxMachineEvent? Event { get; set; }
}
