using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MixingDetailQlsx
{
    public int MixingDetailId { get; set; }

    public string? FinalMixingId { get; set; }

    public string? SourceBatchNo { get; set; }

    public string? SourceCode { get; set; }

    public decimal? Dentity { get; set; }

    public string? ShiftSource { get; set; }

    public int? LogNumberSource { get; set; }

    public DateTime? MixingDetailDate { get; set; }

    public virtual ProductionMixingHistoryQlsx? FinalMixing { get; set; }
}
