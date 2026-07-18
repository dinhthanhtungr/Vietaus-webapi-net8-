using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class WarehouseTempStock
{
    public int tempId { get; set; }

    public Guid companyId { get; set; }

    public string vaCode { get; set; } = null!;

    public string code { get; set; } = null!;

    public string? lotKey { get; set; }

    public decimal? qtyRequest { get; set; }

    public string? reserveStatus { get; set; }

    public Guid createdBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal? QtyUsed { get; set; }

    public string? UnitName { get; set; }
}
