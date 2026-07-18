using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class PicklistDraftLine
{
    public long lineId { get; set; }

    public long draftId { get; set; }

    public int lineNo { get; set; }

    public int shelfStockId { get; set; }

    public string? slotCode { get; set; }

    public string productCode { get; set; } = null!;

    public string? productName { get; set; }

    public string? lotNumber { get; set; }

    public decimal qtyKg { get; set; }

    public int? bags { get; set; }

    public decimal? stdBagKg { get; set; }

    public string? lotKey { get; set; }

    public int? rawLineNo { get; set; }

    public string UnitName { get; set; } = null!;

    public DateOnly? ExpiryDate { get; set; }

    public virtual PicklistDraft draft { get; set; } = null!;
}
