using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class RequestDetailMaterialDatum
{
    public int DetailId { get; set; }

    public string RequestId { get; set; } = null!;

    public int? RequestedQuantity { get; set; }

    public Guid MaterialId { get; set; }

    public string? Note { get; set; }

    public virtual MaterialsMaterialDatum Material { get; set; } = null!;

    public virtual SupplyRequestsMaterialDatum Request { get; set; } = null!;
}
