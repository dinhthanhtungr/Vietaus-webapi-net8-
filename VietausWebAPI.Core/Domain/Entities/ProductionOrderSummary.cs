using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class ProductionOrderSummary
{
    public string? ProductionCode { get; set; }

    public string BatchNo { get; set; } = null!;

    public double? BtpKg { get; set; }

    public double? TpKg { get; set; }

    public double? TperrKg { get; set; }

    public string? Status { get; set; }

    public DateOnly? CompletionDate { get; set; }

    public string? Note { get; set; }
}
