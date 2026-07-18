using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class WarehouseStockFifoLayer
{
    public long layerId { get; set; }

    public Guid companyId { get; set; }

    public int? shelfStockId { get; set; }

    public int slotId { get; set; }

    public string productCode { get; set; } = null!;

    public string lotNumber { get; set; } = null!;

    public DateOnly? expiryDate { get; set; }

    public int? stockType { get; set; }

    public long inLedgerId { get; set; }

    public DateTime inAt { get; set; }

    public decimal qtyInKg { get; set; }

    public decimal qtyUsedKg { get; set; }

    public decimal? remainingKg { get; set; }

    public string UnitName { get; set; } = null!;
}
