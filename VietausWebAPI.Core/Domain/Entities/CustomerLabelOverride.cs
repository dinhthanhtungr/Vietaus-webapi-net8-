using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class CustomerLabelOverride
{
    public int Id { get; set; }

    public string CustomerId { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public string LabelType { get; set; } = null!;
}
