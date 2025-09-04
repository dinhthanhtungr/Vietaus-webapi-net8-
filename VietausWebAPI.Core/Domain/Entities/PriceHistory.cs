using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PriceHistory
{
    public Guid PriceHistoryId { get; set; }

    public Guid MaterialId { get; set; }

    public Guid SupplierId { get; set; }

    public decimal? OldPrice { get; set; }

    public decimal? NewPrice { get; set; }

    public string? Currency { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

}
