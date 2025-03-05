using System;
using System.Collections.Generic;

namespace VietausWebAPI.Infrastructure.Models;

public partial class RequestDetailMaterialDatum
{
    public string DetailId { get; set; } = null!;

    public string RequestId { get; set; } = null!;

    public string MaterialId { get; set; } = null!;

    public string MaterialName { get; set; } = null!;

    public int RequestedQuantity { get; set; }

    public string? Unit { get; set; }
}
