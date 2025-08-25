using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class SupplierContact
{
    public Guid ContactId { get; set; }

    public Guid SupplierId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual Supplier Supplier { get; set; } = null!;
}
