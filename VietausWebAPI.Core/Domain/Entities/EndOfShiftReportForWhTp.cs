using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class EndOfShiftReportForWhTp
{
    public DateTime? Date { get; set; }

    public string? ShiftId { get; set; }

    public string? MachineId { get; set; }

    public string? Operator { get; set; }

    public string? ProductionCode { get; set; }

    public string? BatchNo { get; set; }

    public decimal? NlKg { get; set; }

    public decimal? TpKg { get; set; }
}
