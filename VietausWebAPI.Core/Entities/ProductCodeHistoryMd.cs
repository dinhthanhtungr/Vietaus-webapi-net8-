using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class ProductCodeHistoryMd
{
    public string MachineId { get; set; } = null!;

    public string ProductionCode { get; set; } = null!;

    public string BatchNo { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int TotalTime { get; set; }

    public int? ProducingTime { get; set; }

    public int? WaitingTime { get; set; }

    public int EnergyTotal { get; set; }

    public int ProducingEnergy { get; set; }

    public int WaitingEnergy { get; set; }
}
