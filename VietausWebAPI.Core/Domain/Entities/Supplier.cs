using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class Supplier
{
    public Guid SupplierId { get; set; }

    public string? ExternalId { get; set; }

    public string? Name { get; set; }

    public string? Group { get; set; }

    public string? Application { get; set; }

    public string? RegNo { get; set; }

    public string? TaxNo { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public string? LogoUrl { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? CompanyId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<FormulaMaterial> FormulaMaterials { get; set; } = new List<FormulaMaterial>();

    public virtual ICollection<MaterialsSupplier> MaterialsSuppliers { get; set; } = new List<MaterialsSupplier>();

    public virtual ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<SupplierAddress> SupplierAddresses { get; set; } = new List<SupplierAddress>();

    public virtual ICollection<SupplierContact> SupplierContacts { get; set; } = new List<SupplierContact>();
}
