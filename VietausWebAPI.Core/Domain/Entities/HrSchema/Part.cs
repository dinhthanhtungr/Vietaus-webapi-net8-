using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Entities.AuditSchema;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;
using VietausWebAPI.Core.Domain.Entities.MROSchema;
using VietausWebAPI.Core.Domain.Entities.Notifications;

namespace VietausWebAPI.Core.Domain.Entities.HrSchema;

public partial class Part
{
    public Guid PartId { get; set; }

    public string ExternalId { get; set; } = string.Empty;

    public string PartName { get; set; } = string.Empty;

    public string? Description { get; set; }


    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    //public virtual ICollection<EquipmentMRO> Equipments { get; set; } = new List<EquipmentMRO>();
    public virtual ICollection<EventLog> EventLogs { get; set; } = new List<EventLog>();
    public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; } = new List<NotificationRecipient>();
}
