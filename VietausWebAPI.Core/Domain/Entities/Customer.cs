using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class Customer
{
    public Guid CustomerId { get; set; }

    public string? ExternalId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerGroup { get; set; }

    public string? ApplicationName { get; set; }

    public string? RegistrationNumber { get; set; }

    public string? TaxNumber { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Guid? CompanyId { get; set; }

    public DateTime? IssueDate { get; set; }

    public string? IssuedPlace { get; set; }

    public string? FaxNumber { get; set; }

    public string? Product { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Company? Company { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<CustomerAssignment> CustomerAssignments { get; set; } = new List<CustomerAssignment>();

    public virtual ICollection<DetailCustomerTransfer> DetailCustomerTransfers { get; set; } = new List<DetailCustomerTransfer>();

    public virtual ICollection<MerchandiseOrder> MerchandiseOrders { get; set; } = new List<MerchandiseOrder>();

    public virtual ICollection<MfgProductionOrder> MfgProductionOrders { get; set; } = new List<MfgProductionOrder>();

    public virtual ICollection<SampleRequest> SampleRequests { get; set; } = new List<SampleRequest>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}
