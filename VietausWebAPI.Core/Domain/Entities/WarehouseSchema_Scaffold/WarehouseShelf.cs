using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class WarehouseShelf
{
    public int ShelfStockId { get; set; }

    public string ShelfStockCode { get; set; } = null!;

    public Guid companyId { get; set; }

    public decimal currentWeightKg { get; set; }

    public decimal maxWeightKg { get; set; }

    public bool isActive { get; set; }

    public DateTime? lastUpdated { get; set; }

    public virtual ICollection<WarehouseShelfLedger> WarehouseShelfLedgers { get; set; } = new List<WarehouseShelfLedger>();

    public virtual ICollection<WarehouseShelfStock> WarehouseShelfStocks { get; set; } = new List<WarehouseShelfStock>();

    public virtual ICollection<WarehouseVoucherDetail> WarehouseVoucherDetails { get; set; } = new List<WarehouseVoucherDetail>();
}
