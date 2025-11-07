using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Entities.MROSchema;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class Part
{
    public Guid PartId { get; set; }

    public string ExternalId { get; set; } = string.Empty;

    public string PartName { get; set; } = string.Empty;

    public string? Description { get; set; }


    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public virtual ICollection<EquipmentMRO> Equipments { get; set; } = new List<EquipmentMRO>();
    public virtual ICollection<EventLog> EventLogs { get; set; } = new List<EventLog>();
}
