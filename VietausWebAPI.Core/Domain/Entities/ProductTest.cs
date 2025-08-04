using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class ProductTest
{
    public Guid Id { get; set; }

    public string? ExternalId { get; set; }

    public string? ProductExternalId { get; set; }

    public string? ProductCustomerExternalId { get; set; }

    public DateTime? ManufacturingDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? ProductPackage { get; set; }

    public int? ProductWeight { get; set; }

    public Guid? ProductId { get; set; }

    public string? ProductName { get; set; }
}
