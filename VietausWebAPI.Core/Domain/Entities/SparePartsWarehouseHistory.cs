using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class SparePartsWarehouseHistory
{
    public int HistoryId { get; set; }

    public string SparePartId { get; set; } = null!;

    public DateTime? TransactionDate { get; set; }

    public string TransactionType { get; set; } = null!;

    public int Quantity { get; set; }

    public string? RelatedDocument { get; set; }

    public string? PerformedBy { get; set; }

    public int PurposeId { get; set; }

    public string? Department { get; set; }

    public string? MachineId { get; set; }

    public string? WorkOrderId { get; set; }

    public bool? IsPlanned { get; set; }

    public string? Note { get; set; }

    public virtual UsagePurpose Purpose { get; set; } = null!;

    public virtual SparePartsWarehouse SparePart { get; set; } = null!;
}
