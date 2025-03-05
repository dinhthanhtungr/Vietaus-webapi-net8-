using System;
using System.Collections.Generic;

namespace VietausWebAPI.Infrastructure.Models;

public partial class EmployeesCommonDatum
{
    public string EmployeeId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Identifier { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string PartId { get; set; } = null!;

    public string LevelId { get; set; } = null!;

    public DateTime DateHired { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ApprovalHistoryMaterialDatum> ApprovalHistoryMaterialData { get; set; } = new List<ApprovalHistoryMaterialDatum>();

    public virtual ApprovalLevelsCommonDatum Level { get; set; } = null!;

    public virtual PartsCommonDatum Part { get; set; } = null!;

    public virtual ICollection<SupplyRequestsMaterialDatum> SupplyRequestsMaterialData { get; set; } = new List<SupplyRequestsMaterialDatum>();
}
