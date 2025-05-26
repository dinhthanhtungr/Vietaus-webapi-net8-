using System;
using System.Collections.Generic;
namespace VietausWebAPI.Core.Domain.Entities;


public partial class ProductionPlanPlpu
{
    public string? MachineId { get; set; }

    public int? Number { get; set; }

    public string? Color { get; set; }

    public string? ProducCode { get; set; }

    public string? Note1 { get; set; }

    public string? Note2 { get; set; }

    public string? Note3 { get; set; }

    public string? BatchNo { get; set; }

    public string? Note4 { get; set; }

    public DateTime? RequestDate { get; set; }

    public virtual MachinesCommonDatum? Machine { get; set; }
}
