using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class TempStockIssueLedger
{
    public long ledgerId { get; set; }

    public Guid companyId { get; set; }

    public string sourceKind { get; set; } = null!;

    public string sourceKey { get; set; } = null!;

    public string? requestCode { get; set; }

    public int? requestDetailId { get; set; }

    public string vaCode { get; set; } = null!;

    public string productCode { get; set; } = null!;

    public string? lotNo { get; set; }

    public decimal qtyKg { get; set; }

    public DateTime createdAt { get; set; }

    public Guid? createdBy { get; set; }

    public string? UnitName { get; set; }
}
