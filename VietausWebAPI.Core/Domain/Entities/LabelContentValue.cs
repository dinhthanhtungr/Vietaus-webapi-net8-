using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class LabelContentValue
{
    public int Id { get; set; }

    public string BatchNo { get; set; } = null!;

    public string KeyName { get; set; } = null!;

    public string? KeyValue { get; set; }
}
