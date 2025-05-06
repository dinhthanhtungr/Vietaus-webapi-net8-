using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class PassDetailHistoryLabqc
{
    public DateTime? Qcdate { get; set; }

    public string? MachineId { get; set; }

    public string? BatchNo { get; set; }

    public string? Appearance { get; set; }

    public string? Physical { get; set; }

    public string? SizeMoisture { get; set; }

    public string? BlackSpot { get; set; }

    public string? ChipPressing { get; set; }

    public string? Dispersion { get; set; }

    public string? TempSmell { get; set; }

    public string? InspectionNo { get; set; }

    public string? Note { get; set; }

    public string? QcpassId { get; set; }

    public string? EmployeeId { get; set; }

    public string? StatusQc { get; set; }
}
