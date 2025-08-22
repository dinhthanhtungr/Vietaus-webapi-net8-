using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class PriceHistory1
{
    public Guid PriceHistoryId { get; set; }

    public Guid? ProductId { get; set; }

    public decimal? Price { get; set; }

    public string? Currency { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Employee? UpdatedByNavigation { get; set; }
}
