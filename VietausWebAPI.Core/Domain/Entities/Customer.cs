using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class Customer
{
    public string ExternalId { get; set; } = null!;

    public string CustomerName { get; set; } = null!;
}
