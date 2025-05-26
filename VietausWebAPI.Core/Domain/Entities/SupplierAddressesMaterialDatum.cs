using System;
using System.Collections.Generic;
namespace VietausWebAPI.Core.Domain.Entities;

public partial class SupplierAddressesMaterialDatum
{
    public Guid AddressId { get; set; }

    public string? AddressLine { get; set; }

    public Guid? SupplierId { get; set; }

    public virtual SuppliersMaterialDatum? Supplier { get; set; }
}
