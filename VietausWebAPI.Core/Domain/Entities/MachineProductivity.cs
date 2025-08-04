using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MachineProductivity
{
    public int Id { get; set; }

    public string? MachineId { get; set; }

    public string? ProductionCode { get; set; }

    public int? Quantity { get; set; }
}
