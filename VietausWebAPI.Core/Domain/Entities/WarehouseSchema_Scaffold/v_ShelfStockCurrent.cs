using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class v_ShelfStockCurrent
{
    public int? SlotId { get; set; }

    public string? SlotCode { get; set; }

    public Guid? companyId { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductName { get; set; }

    public string? LotNo { get; set; }

    public string? lotKey { get; set; }

    public decimal? qtyKg { get; set; }

    public int? Bags { get; set; }

    public int? stockType { get; set; }

    public decimal? currentWeightKg { get; set; }

    public decimal? maxWeightKg { get; set; }

    public decimal? FreeCapacityKg { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? updatedBy { get; set; }
}
