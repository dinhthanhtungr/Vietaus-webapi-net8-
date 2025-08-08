using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MemberInGroup
{
    public Guid MemberId { get; set; }

    public bool? IsAdmin { get; set; }

    public Guid? Profile { get; set; }

    public Guid GroupId { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Employee? ProfileNavigation { get; set; } 
}
