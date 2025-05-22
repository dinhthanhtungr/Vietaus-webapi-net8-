using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class SystemGroup
{
    public string SystemGroupId { get; set; } = null!;

    public string? SystemGroupName { get; set; }

    public virtual ICollection<SparePartsWarehouse> SparePartsWarehouses { get; set; } = new List<SparePartsWarehouse>();
}
