using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class RequestDetailMaterialDatum
{
    public int DetailId { get; set; }

    public string RequestId { get; set; } = null!;

    public string MaterialName { get; set; } = null!;

    public int RequestedQuantity { get; set; }

    public string? Unit { get; set; }

    public Guid? MaterialGroupId { get; set; }

    public virtual MaterialGroupsMaterialDatum? MaterialGroup { get; set; }

    public virtual SupplyRequestsMaterialDatum Request { get; set; } = null!;
}
