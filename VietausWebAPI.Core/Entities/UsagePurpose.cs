using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class UsagePurpose
{
    public int PurposeId { get; set; }

    public string PurposeName { get; set; } = null!;

    public string? Note { get; set; }

    public virtual ICollection<NonCatalogHistory> NonCatalogHistories { get; set; } = new List<NonCatalogHistory>();

    public virtual ICollection<SparePartsWarehouseHistory> SparePartsWarehouseHistories { get; set; } = new List<SparePartsWarehouseHistory>();
}
