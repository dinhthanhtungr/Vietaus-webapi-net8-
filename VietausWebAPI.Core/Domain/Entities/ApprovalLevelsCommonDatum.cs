using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class ApprovalLevelsCommonDatum
{
    public string LevelId { get; set; } = null!;

    public string LevelName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<EmployeesCommonDatum> EmployeesCommonData { get; set; } = new List<EmployeesCommonDatum>();
}
