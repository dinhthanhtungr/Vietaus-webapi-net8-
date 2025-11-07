using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class Branch
{
    public Guid BranchId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public Guid CompanyId { get; set; }

    public virtual ICollection<SampleRequest> SampleRequests { get; } = new List<SampleRequest>();

    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Employee? UpdatedByNavigation { get; set; }
}
