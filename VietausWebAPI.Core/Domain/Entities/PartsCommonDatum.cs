using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PartsCommonDatum
{
    public string PartId { get; set; } = null!;

    public string PartName { get; set; } = null!;

    public virtual ICollection<EmployeesCommonDatum> EmployeesCommonData { get; set; } = new List<EmployeesCommonDatum>();

    public virtual ICollection<MachinesCommonDatum> MachinesCommonData { get; set; } = new List<MachinesCommonDatum>();
}
