using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class v_ShelfHistory_Canonical
{
    public Guid? CompanyId { get; set; }

    public int? FactoryId { get; set; }

    public int? SlotId { get; set; }

    public string? SlotCode { get; set; }

    public string? SlotFrom { get; set; }

    public string? SlotTo { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductName { get; set; }

    public string? LotNumber { get; set; }

    public string? Operation { get; set; }

    public decimal? InKg { get; set; }

    public decimal? OutKg { get; set; }

    public decimal? DeltaKg { get; set; }

    public decimal? BeforeKg { get; set; }

    public decimal? AfterKg { get; set; }

    public string? Note { get; set; }

    public DateTime? AssignedTime { get; set; }

    public Guid? AssignedBy { get; set; }

    public string? AssignedByName { get; set; }

    public int? PurposeID { get; set; }

    public string? purposeCode { get; set; }

    public string? purposeName { get; set; }

    public string? RequestCode { get; set; }

    public string? AppSource { get; set; }

    public long? VoucherId { get; set; }

    public int? stockType { get; set; }

    public string? UnitName { get; set; }

    public DateOnly? ExpiryDate { get; set; }
}
