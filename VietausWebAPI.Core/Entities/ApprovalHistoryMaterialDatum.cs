using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class ApprovalHistoryMaterialDatum
{
    public string Id { get; set; } = null!;

    public string RequestId { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public DateTime ApprovalDate { get; set; }

    public string? Note { get; set; }

    public virtual EmployeesCommonDatum Employee { get; set; } = null!;

    public virtual SupplyRequestsMaterialDatum Request { get; set; } = null!;
}
