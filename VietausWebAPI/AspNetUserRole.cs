using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class AspNetUserRole
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public bool IsActive { get; set; }

    public virtual AspNetRole Role { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
