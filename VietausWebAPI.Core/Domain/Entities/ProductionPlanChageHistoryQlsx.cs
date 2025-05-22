using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class ProductionPlanChageHistoryQlsx
{
    public string? ProductionCode { get; set; }

    public string? BatchNo { get; set; }

    public string? FromMachineId { get; set; }

    public string? ToMachineId { get; set; }

    public DateTime? ChageDate { get; set; }

    public string? Note { get; set; }

    public string? ShiftId { get; set; }
}
