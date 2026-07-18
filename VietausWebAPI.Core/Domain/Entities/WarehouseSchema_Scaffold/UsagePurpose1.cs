using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class UsagePurpose1
{
    public int purposeId { get; set; }

    public string purposeCode { get; set; } = null!;

    public string purposeName { get; set; } = null!;

    public string? purposeNote { get; set; }

    public bool isActive { get; set; }

    public bool isReceivable { get; set; }

    public bool isPickable { get; set; }

    public virtual ICollection<WarehouseVoucherDetail> WarehouseVoucherDetails { get; set; } = new List<WarehouseVoucherDetail>();
}
