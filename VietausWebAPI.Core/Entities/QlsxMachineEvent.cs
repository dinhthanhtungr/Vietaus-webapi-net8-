using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class QlsxMachineEvent
{
    public string EventId { get; set; } = null!;

    public string? EventName { get; set; }

    public virtual ICollection<EventHistoryQlsx> EventHistoryQlsxes { get; set; } = new List<EventHistoryQlsx>();
}
