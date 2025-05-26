using System;
using System.Collections.Generic;
namespace VietausWebAPI.Core.Domain.Entities;


public partial class OtherMaintenanceHistory
{
    public string OtherMaintenanceId { get; set; } = null!;

    public string? RelatedDocument { get; set; }

    public string? MachineId { get; set; }

    public DateTime? OtherMaintenanceDate { get; set; }

    public string? PerformedBy { get; set; }

    public string? Receiver { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string? Note { get; set; }

    public string? ApprovedId { get; set; }

    public virtual MachinesCommonDatum? Machine { get; set; }
}
