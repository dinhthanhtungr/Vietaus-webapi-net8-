using System;
using System.Collections.Generic;

namespace VietausWebAPI.Infrastructure.Models;

public partial class ProductCodeHistory
{
    public string MachineId { get; set; } = null!;

    public string ProductionCode { get; set; } = null!;

    public string BatchNo { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal TotalTime { get; set; }

    public decimal? ProducingTime { get; set; }

    public decimal? WaitingTime { get; set; }

    public int EnergyTotal { get; set; }

    public int ProducingEnergy { get; set; }

    public int WaitingEnergy { get; set; }

    public virtual MachinesCommonDatum Machine { get; set; } = null!;
}
