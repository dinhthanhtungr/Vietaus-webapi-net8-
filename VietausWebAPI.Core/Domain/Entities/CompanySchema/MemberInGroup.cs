using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Core.Domain.Entities.CompanySchema;

public partial class MemberInGroup
{
    public Guid MemberId { get; set; }

    public bool? IsAdmin { get; set; }

    public Guid? Profile { get; set; }

    public Guid GroupId { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual Group Group { get; set; } = null!;

    public virtual Employee? ProfileNavigation { get; set; }
}
