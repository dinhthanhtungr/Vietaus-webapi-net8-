using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class vw_RequestDetailsProgress
{
    public int? DetailId { get; set; }

    public int? RequestId { get; set; }

    public string? RequestCode { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductName { get; set; }

    public string? LotNumber { get; set; }

    public decimal? WeightKg { get; set; }

    public int? BagNumber { get; set; }

    public decimal? DoneKg { get; set; }

    public decimal? RemainingKg { get; set; }

    public string? StockStatus { get; set; }
}
