using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class NonCatalogHistory
{
    public int HistoryId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string TransactionType { get; set; } = null!;

    public string MaterialName { get; set; } = null!;

    public string? Unit { get; set; }

    public int Quantity { get; set; }

    public string? RelatedDocument { get; set; }

    public int PurposeId { get; set; }

    public string? PerformedBy { get; set; }

    public string? Note { get; set; }

    public virtual UsagePurpose Purpose { get; set; } = null!;
}
