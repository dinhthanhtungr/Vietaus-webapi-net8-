using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class LabelRule
{
    public int RuleId { get; set; }

    public string Keyword { get; set; } = null!;

    public string LabelType { get; set; } = null!;
}
