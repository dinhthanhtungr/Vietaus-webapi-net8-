using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class ApprovalHistoryMaterialDatum
{
    public int Id { get; set; }

    public string RequestId { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public DateTime ApprovalDate { get; set; }

    public string? Note { get; set; }

    public virtual EmployeesCommonDatum Employee { get; set; } = null!;

    public virtual SupplyRequestsMaterialDatum Request { get; set; } = null!;
}
