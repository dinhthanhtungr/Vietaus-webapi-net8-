using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class MerchandiseOrderDetail
{
    public Guid MerchandiseOrderDetailId { get; set; }

    public Guid MerchandiseOrderId { get; set; }

    public Guid ProductId { get; set; }
    public string? ProductExternalIdSnapshot { get; set; }
    public string? ProductNameSnapshot { get; set; }
    public string? ProductionType { get; set; }

    public Guid FormulaId { get; set; }
    public string? FormulaExternalIdSnapshot { get; set; }

    public decimal ExpectedQuantity { get; set; }
    public decimal? RealQuantity { get; set; }

    public string? BagType { get; set; }
    public string? PackageWeight { get; set; }

    public string? Status { get; set; }

    public string? Comment { get; set; }

    public DateTime? DeliveryRequestDate { get; set; }
    public DateTime? DeliveryActualDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }

    public decimal BaseCostSnapshot { get; set; }
    public decimal RecommendedUnitPrice { get; set; }
    public decimal UnitPriceAgreed { get; set; }
    public decimal TotalPriceAgreed { get; set; }

    public bool? IsActive { get; set; }

    public virtual IEnumerable<DeliveryOrderDetail> DeliveryOrderDetails { get; set; } = new List<DeliveryOrderDetail>();
    public virtual MerchandiseOrder MerchandiseOrder { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Formula Formula { get; set; } = null!;
}
