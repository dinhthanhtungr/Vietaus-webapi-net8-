using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class TempEmployeesImport
{
    public int? EmployeeId { get; set; }

    public string? FullName { get; set; }

    public string? Gender { get; set; }

    public string? DateOfBirth { get; set; }

    public string? Identifier { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? PartId { get; set; }

    public string? LevelId { get; set; }

    public string? DateHired { get; set; }

    public string? Status { get; set; }

    public string? EndDate { get; set; }
}
