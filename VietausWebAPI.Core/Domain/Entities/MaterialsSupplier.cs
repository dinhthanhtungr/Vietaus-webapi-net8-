using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MaterialsSupplier
{
    public Guid MaterialsSuppliersId { get; set; }

    public Guid SupplierId { get; set; }

    public Guid MaterialId { get; set; }

    public int? MinDeliveryDays { get; set; }

    public decimal? CurrentPrice { get; set; }

    public string? Currency { get; set; }

    public decimal? LastPrice { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool? IsPreferred { get; set; }

    public virtual Material1 Material { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Employee? UpdatedByNavigation { get; set; }
}
