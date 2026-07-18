using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class DepartmentTempStockLog
{
    public long LogId { get; set; }

    public Guid companyId { get; set; }

    public long? TempStockId { get; set; }

    public Guid? PartID { get; set; }

    public int? RequestId { get; set; }

    public string? RequestCode { get; set; }

    public long? VoucherId { get; set; }

    public long? VoucherDetailId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string? LotNumber { get; set; }

    public string ActionType { get; set; } = null!;

    public decimal DeltaKg { get; set; }

    public decimal QtyAfter { get; set; }

    public decimal? AvgUnitCost { get; set; }

    public decimal? AmountCost { get; set; }

    public string? CostStatus { get; set; }

    public string? UncostedReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public string? Note { get; set; }

    public string? UnitName { get; set; }

    public DateOnly? ExpiryDate { get; set; }
}
