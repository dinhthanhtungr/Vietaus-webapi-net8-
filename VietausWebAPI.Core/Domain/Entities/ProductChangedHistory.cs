using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class ProductChangedHistory
{
    public int ProductChangedHistoryId { get; set; }

    public Guid ProductId { get; set; }

    public Guid ChangedBy { get; set; }

    public DateTime ChangedDate { get; set; }

    public string? FieldChanged { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? ChangeNote { get; set; }

    public string? ChangeType { get; set; }

    public virtual Employee ChangedByNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
