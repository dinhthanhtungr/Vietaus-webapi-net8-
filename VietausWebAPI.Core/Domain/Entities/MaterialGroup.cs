using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class MaterialGroup
{
    public string MaterialGroupId { get; set; } = null!;

    public string? MaterialGroupName { get; set; }
}
