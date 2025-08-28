using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class Unit
{
    public Guid UnitId { get; set; }

    public string? ExternalId { get; set; }

    public string? Name { get; set; }

    public string? Symbol { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

}
