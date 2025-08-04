using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class Customer1
{
    public Guid CustomerId { get; set; }

    public string? ExternalId { get; set; }

    public string? CustomerName { get; set; }

    public Guid? EmployeeId { get; set; }

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

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Company? Company { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<MerchandiseOrder> MerchandiseOrders { get; set; } = new List<MerchandiseOrder>();

    public virtual ICollection<SampleRequest> SampleRequests { get; set; } = new List<SampleRequest>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}
