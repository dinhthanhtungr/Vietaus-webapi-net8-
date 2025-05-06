using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class NewMakingMaterial
{
    public int Id { get; set; }

    public string? NewMakingId { get; set; }

    public string? MaterialName { get; set; }

    public int? QuantityUsed { get; set; }

    public string? Note { get; set; }

    public virtual NewMakingHistory? NewMaking { get; set; }
}
