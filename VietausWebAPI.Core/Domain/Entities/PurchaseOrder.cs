using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PurchaseOrder
{
    public Guid Poid { get; set; }

    public string? Pocode { get; set; }

    public Guid? SupplierId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? EmployeeId { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual EmployeesCommonDatum? Employee { get; set; }

    public virtual SuppliersMaterialDatum? Supplier { get; set; }
}
