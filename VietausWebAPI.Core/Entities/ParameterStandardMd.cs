using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class ParameterStandardMd
{
    public int Id { get; set; }

    public string MachineId { get; set; } = null!;

    public string? ProductionCode { get; set; }

    public int? Set1Standard { get; set; }

    public int? Set2Standard { get; set; }

    public int? Set3Standard { get; set; }

    public int? Set4Standard { get; set; }

    public int? Set5Standard { get; set; }

    public int? Set6Standard { get; set; }

    public int? Set7Standard { get; set; }

    public int? Set8Standard { get; set; }

    public int? Set9Standard { get; set; }

    public int? Set10Standard { get; set; }

    public int? Set11Standard { get; set; }

    public int? Set12Standard { get; set; }

    public int? Set13Standard { get; set; }

    public int? ScrewSpeedStandard { get; set; }

    public int? ScrewCurrentStandard { get; set; }

    public int? FeederSpeedStandard { get; set; }

    public string EmployeeId { get; set; } = null!;

    public virtual EmployeesCommonDatum Employee { get; set; } = null!;

    public virtual MachinesCommonDatum Machine { get; set; } = null!;
}
