using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class MaterialsSuppliersMaterialDatum
{
    public Guid MaterialsSuppliersId { get; set; }

    public Guid? MaterialId { get; set; }

    public Guid? SupplierId { get; set; }

    public int? MinDeliveryDays { get; set; }

    public decimal? CurrentPrice { get; set; }

    public string? Currency { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsPreferred { get; set; }

    public Guid? PriceHistoryId { get; set; }

    public virtual MaterialsMaterialDatum? Material { get; set; }

    public virtual PriceHistoryMaterialDatum? PriceHistory { get; set; }

    public virtual SuppliersMaterialDatum? Supplier { get; set; }
}
