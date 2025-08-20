using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class Contact
{
    public Guid ContactId { get; set; }

    public Guid CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
    public bool? IsPrimary { get; set; }

    public virtual Customer1 Customer { get; set; } = null!;
}
