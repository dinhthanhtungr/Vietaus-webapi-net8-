using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class CustomerAssignment
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid GroupId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid UpdatedBy { get; set; }

    public Guid CompanyId { get; set; }

    public bool IsActive { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employee UpdatedByNavigation { get; set; } = null!;
}
