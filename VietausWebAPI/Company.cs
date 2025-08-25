using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class Company
{
    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Code { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<CustomerAssignment> CustomerAssignments { get; set; } = new List<CustomerAssignment>();

    public virtual ICollection<CustomerTransferLog> CustomerTransferLogs { get; set; } = new List<CustomerTransferLog>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Formula> Formulas { get; set; } = new List<Formula>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    public virtual ICollection<MerchandiseOrder> MerchandiseOrders { get; set; } = new List<MerchandiseOrder>();

    public virtual ICollection<ProductStandard> ProductStandards { get; set; } = new List<ProductStandard>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<SampleRequest> SampleRequests { get; set; } = new List<SampleRequest>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<SupplyRequest> SupplyRequests { get; set; } = new List<SupplyRequest>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}
