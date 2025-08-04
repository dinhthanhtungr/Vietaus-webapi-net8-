using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class LabelElement
{
    public int ElementId { get; set; }

    public int TemplateId { get; set; }

    public string LabelType { get; set; } = null!;

    public int X { get; set; }

    public int Y { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public int? FontSize { get; set; }

    public string? Alignment { get; set; }

    public bool? Bold { get; set; }

    public bool? Italic { get; set; }

    public string ValueType { get; set; } = null!;

    public string? PrefixText { get; set; }

    public string? RenderType { get; set; }

    public string? FontName { get; set; }
}
