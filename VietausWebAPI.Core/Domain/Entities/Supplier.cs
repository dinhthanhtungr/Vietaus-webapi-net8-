using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Enums;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class Supplier
{
    public Guid SupplierId { get; set; }

    public string? ExternalId { get; set; }

    public string? SupplierName { get; set; }

    public string? RegistrationNumber { get; set; }

    public string? TaxNumber { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public string? Note { get; set; }

    public DateTime? IssueDate { get; set; }

    public string? IssuedPlace { get; set; }

    public string? FaxNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }
    public Guid? CompanyId { get; set; }
    public bool? IsActive { get; set; }
    public SupplierPriority Priority { get; set; } = SupplierPriority.Medium;

    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }
    public virtual Employee? UpdatedByNavigation { get; set; }


    public virtual ICollection<MaterialsSupplier> MaterialsSuppliers { get; set; } = new List<MaterialsSupplier>();

    public virtual ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<SupplierAddress> SupplierAddresses { get; set; } = new List<SupplierAddress>();

    public virtual ICollection<SupplierContact> SupplierContacts { get; set; } = new List<SupplierContact>();
}
