using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class HistoryPrintLabelQlsx
{
    public int Id { get; set; }

    public string? BatchNo { get; set; }

    public int? NumberOfCopies { get; set; }

    public DateTime? DatePrint { get; set; }

    public string? Shift { get; set; }

    public int? LogNumber { get; set; }
}
