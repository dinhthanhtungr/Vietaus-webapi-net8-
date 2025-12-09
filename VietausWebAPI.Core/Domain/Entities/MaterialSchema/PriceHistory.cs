using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Core.Domain.Entities.MaterialSchema;

public partial class PriceHistory
{
    public Guid PriceHistoryId { get; set; }

    public Guid MaterialsSuppliersId { get; set; }

    public decimal? OldPrice { get; set; }

    public string? Currency { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual MaterialsSupplier MaterialsSuppliers { get; set; } = null!;

}
