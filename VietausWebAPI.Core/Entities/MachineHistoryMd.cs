using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class MachineHistoryMd
{
    public string MachineId { get; set; } = null!;

    public DateTime Time { get; set; }

    public decimal? ProducingTimeOfDay { get; set; }

    public decimal? WaitingTimeOfDay { get; set; }

    public decimal? MachineCleaningTimeOfDay { get; set; }

    public decimal? EnergyTotalOfDay { get; set; }

    public decimal? ProducingEnergyOfDay { get; set; }

    public decimal? MachineCleaningEnergyOfDay { get; set; }

    public decimal? WaitingEnergyOfDay { get; set; }

    public virtual MachinesCommonDatum Machine { get; set; } = null!;
}
