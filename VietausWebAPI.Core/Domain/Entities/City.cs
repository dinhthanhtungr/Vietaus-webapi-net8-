using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class City
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
}
