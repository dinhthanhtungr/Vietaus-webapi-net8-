using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class DepartmentTempStock
{
    public long TempStockId { get; set; }

    public Guid companyId { get; set; }

    public Guid? PartID { get; set; }

    public int? RequestId { get; set; }

    public string? RequestCode { get; set; }

    public long? VoucherId { get; set; }

    public long VoucherDetailId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string? ProductName { get; set; }

    public string? LotNumber { get; set; }

    public int? ItemStockType { get; set; }

    public decimal IssuedQtyKg { get; set; }

    public decimal UsedQtyKg { get; set; }

    public decimal ReturnedQtyKg { get; set; }

    public decimal? RemainingQtyKg { get; set; }

    public decimal? AvgUnitCost { get; set; }

    public decimal? AmountCost { get; set; }

    public string Status { get; set; } = null!;

    public string CostStatus { get; set; } = null!;

    public string? UncostedReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public string? Note { get; set; }

    public string? UnitName { get; set; }

    public DateOnly? ExpiryDate { get; set; }
}
