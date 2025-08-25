using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class MerchandiseOrderDetail
{
    public Guid MerchandiseOrderDetailId { get; set; }

    public Guid MerchandiseOrderId { get; set; }

    public Guid ProductId { get; set; }

    public string? FormulaExternalId { get; set; }

    public double? ExpectedQuantity { get; set; }

    public string? BagType { get; set; }

    public string? Status { get; set; }

    public string? Comment { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public decimal? Price { get; set; }

    public virtual MerchandiseOrder MerchandiseOrder { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
