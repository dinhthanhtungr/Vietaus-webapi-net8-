using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class ListProducedForQc
{
    public string? MachineId { get; set; }

    public string? BatchNo { get; set; }

    public string? QcpassId { get; set; }

    public string? Note { get; set; }

    public DateTime? StartDate { get; set; }
}
