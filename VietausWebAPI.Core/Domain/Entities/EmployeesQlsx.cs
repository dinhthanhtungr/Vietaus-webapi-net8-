using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class EmployeesQlsx
{
    public string? EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string? Position { get; set; }
}
