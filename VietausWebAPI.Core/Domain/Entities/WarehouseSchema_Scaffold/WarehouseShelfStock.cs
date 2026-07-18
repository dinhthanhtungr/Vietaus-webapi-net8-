using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class WarehouseShelfStock
{
    public int SlotId { get; set; }

    public Guid companyId { get; set; }

    public string ShelfStockCode { get; set; } = null!;

    public string code { get; set; } = null!;

    public string? LotNo { get; set; }

    public string? lotKey { get; set; }

    public decimal qtyKg { get; set; }

    public int? Bags { get; set; }

    public Guid? updatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public int stockType { get; set; }

    public int ShelfStockId { get; set; }

    /// <summary>
    /// Đơn vị tính
    /// </summary>
    public string UnitName { get; set; } = null!;

    public DateOnly? ExpiryDate { get; set; }

    public virtual WarehouseShelf Slot { get; set; } = null!;
}
