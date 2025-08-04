using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MemberInGroup
{
    public Guid MemberId { get; set; }

    public bool? IsAdmin { get; set; }

    public Guid? Profile { get; set; }

    public virtual Employee? ProfileNavigation { get; set; }
}
