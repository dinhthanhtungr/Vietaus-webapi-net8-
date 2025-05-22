using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class TempEndOfShiftReport
{
    public DateTime? Date { get; set; }

    public string? ShiftId { get; set; }

    public string? MachineId { get; set; }

    public string? Operator { get; set; }

    public string? ProductionCode { get; set; }

    public string? BatchNo { get; set; }

    public decimal? BtpKg { get; set; }

    public decimal? TpKg { get; set; }

    public decimal? TpWaitingForQcKg { get; set; }

    public decimal? ProducingErrKg { get; set; }

    public decimal? UnfinishedProductKg { get; set; }

    public string? Note { get; set; }
}
