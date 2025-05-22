using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class QcresultLabqc
{
    public DateTime? Qcdate { get; set; }

    public string? QcpassId { get; set; }

    public string? StatusQc { get; set; }
}
