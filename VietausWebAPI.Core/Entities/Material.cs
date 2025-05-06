using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class Material
{
    public string MaterialId { get; set; } = null!;

    public string? MaterialName { get; set; }

    public virtual ICollection<SparePartsWarehouse> SparePartsWarehouses { get; set; } = new List<SparePartsWarehouse>();
}
