using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class ProductionMixingHistoryQlsx
{
    public string FinalMixingId { get; set; } = null!;

    public string? ProductionFinalMixing { get; set; }

    public string? BatchNoFinalMixing { get; set; }

    public string? MixedBy { get; set; }

    public string? MixingShift { get; set; }

    public DateTime? MixingDate { get; set; }

    public string? MixerId { get; set; }

    public virtual ICollection<MixingDetailQlsx> MixingDetailQlsxes { get; set; } = new List<MixingDetailQlsx>();
}
