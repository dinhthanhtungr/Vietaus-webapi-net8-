using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class RequestDetailMaterialDatum
{
    public int DetailId { get; set; }

    public string RequestId { get; set; } = null!;

    public string MaterialGroupId { get; set; } = null!;

    public string MaterialName { get; set; } = null!;

    public int RequestedQuantity { get; set; }

    public string? Unit { get; set; }

    public virtual MaterialsMaterialGroupsDatum MaterialGroup { get; set; } = null!;

    public virtual SupplyRequestsMaterialDatum Request { get; set; } = null!;
}
