using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class ApprovalHistory
{
    public Guid ApprovalId { get; set; }

    public Guid? RequestId { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public Guid? EmployeeId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? Note { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual SupplyRequest? Request { get; set; }
}
