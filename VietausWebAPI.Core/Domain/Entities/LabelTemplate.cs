using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class LabelTemplate
{
    public int TemplateId { get; set; }

    public int WidthMm { get; set; }

    public int HeightMm { get; set; }

    public string LabelType { get; set; } = null!;
}
