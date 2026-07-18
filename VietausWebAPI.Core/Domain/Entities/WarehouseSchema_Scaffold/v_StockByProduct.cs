using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class v_StockByProduct
{
    public Guid? companyId { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductName { get; set; }

    public string? lotKey { get; set; }

    public decimal? TotalQtyKg { get; set; }

    public long? TotalBags { get; set; }
}
