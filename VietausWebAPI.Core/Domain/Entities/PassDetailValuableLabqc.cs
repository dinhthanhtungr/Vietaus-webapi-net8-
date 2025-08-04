using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PassDetailValuableLabqc
{
    public DateTime? Qcdate { get; set; }

    public string? MachineId { get; set; }

    public string? BatchNo { get; set; }

    public int? Qcround { get; set; }

    public string? Note { get; set; }
}
