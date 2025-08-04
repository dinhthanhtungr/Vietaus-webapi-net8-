using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class SampleRequest
{
    public Guid SampleRequestId { get; set; }

    public string? ExternalId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid ManagerBy { get; set; }

    public Guid ProductId { get; set; }

    public DateTime? RealDeliveryDate { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }

    public DateTime? RealPriceQuoteDate { get; set; }

    public DateTime? ExpectedPriceQuoteDate { get; set; }

    public string? AdditionalComment { get; set; }

    public string? RequestType { get; set; }

    public double? ExpectedQuantity { get; set; }

    public decimal? ExpectedPrice { get; set; }

    public double? SampleQuantity { get; set; }

    public string? OtherComment { get; set; }

    public string? InfoType { get; set; }

    public Guid? FormulaId { get; set; }

    public string? Comment { get; set; }

    public string? Image { get; set; }

    public int? Branch { get; set; }

    public string? Status { get; set; }

    public string? Package { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Guid? CompanyId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Customer1 Customer { get; set; } = null!;

    public virtual Formula? Formula { get; set; }

    public virtual Employee ManagerByNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Employee? UpdatedByNavigation { get; set; }
}
