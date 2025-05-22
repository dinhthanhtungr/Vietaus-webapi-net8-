using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MachinesCommonDatum
{
    public string MachineId { get; set; } = null!;

    public string MachineName { get; set; } = null!;

    public string GroupId { get; set; } = null!;

    public string PartId { get; set; } = null!;

    public string? Description { get; set; }

    public virtual GroupsCommonDatum Group { get; set; } = null!;

    public virtual ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();

    public virtual ICollection<MaintenanceHistory> MaintenanceHistories { get; set; } = new List<MaintenanceHistory>();

    public virtual ICollection<NewMakingHistory> NewMakingHistories { get; set; } = new List<NewMakingHistory>();

    public virtual ICollection<OtherMaintenanceHistory> OtherMaintenanceHistories { get; set; } = new List<OtherMaintenanceHistory>();

    public virtual ICollection<OtherMaintenanceMaterial> OtherMaintenanceMaterials { get; set; } = new List<OtherMaintenanceMaterial>();

    public virtual ICollection<ParameterStandardMd> ParameterStandardMds { get; set; } = new List<ParameterStandardMd>();

    public virtual PartsCommonDatum Part { get; set; } = null!;
}
