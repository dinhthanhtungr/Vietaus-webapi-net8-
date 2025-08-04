using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class EmployeesDiAn
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string? Position { get; set; }
}
