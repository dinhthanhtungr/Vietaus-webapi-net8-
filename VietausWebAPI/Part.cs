using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class Part
{
    public Guid PartId { get; set; }

    public string? ExternalId { get; set; }

    public string? PartName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
