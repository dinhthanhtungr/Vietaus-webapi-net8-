using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class HistoryPrintLabelForProMixingQlsx
{
    public int Id { get; set; }

    public string? BatchNoFinalMixing { get; set; }

    public int? NumberOfCopies { get; set; }

    public DateTime? DatePrint { get; set; }

    public string? MixingShift { get; set; }

    public int? LogNumberOfMixing { get; set; }
}
