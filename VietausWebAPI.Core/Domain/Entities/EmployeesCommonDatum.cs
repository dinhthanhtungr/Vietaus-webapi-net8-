using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

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

    public string? LevelId { get; set; }

    public DateTime? DateHired { get; set; }

    public string? Status { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<ApprovalHistoryMaterialDatum> ApprovalHistoryMaterialData { get; set; } = new List<ApprovalHistoryMaterialDatum>();

    public virtual ApprovalLevelsCommonDatum? Level { get; set; }

    public virtual ICollection<MaterialsMaterialDatum> MaterialsMaterialData { get; set; } = new List<MaterialsMaterialDatum>();

    public virtual PartsCommonDatum Part { get; set; } = null!;

    public virtual ICollection<PriceHistoryMaterialDatum> PriceHistoryMaterialData { get; set; } = new List<PriceHistoryMaterialDatum>();

    public virtual ICollection<PurchaseOrdersMaterialDatum> PurchaseOrdersMaterialData { get; set; } = new List<PurchaseOrdersMaterialDatum>();

    public virtual ICollection<SupplyRequestsMaterialDatum> SupplyRequestsMaterialData { get; set; } = new List<SupplyRequestsMaterialDatum>();
}
