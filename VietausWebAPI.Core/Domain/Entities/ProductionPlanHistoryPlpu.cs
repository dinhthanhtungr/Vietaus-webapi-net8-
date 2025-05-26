using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;


public partial class ProductionPlanHistoryPlpu
{
    public string? MachineId { get; set; }

    public string? Color { get; set; }

    public string? ProductionCode { get; set; }

    public string? Note1 { get; set; }

    public string? Note2 { get; set; }

    public string? Note3 { get; set; }

    public string? BatchNo { get; set; }

    public string? Note4 { get; set; }

    public DateOnly? RequestDate { get; set; }
}
