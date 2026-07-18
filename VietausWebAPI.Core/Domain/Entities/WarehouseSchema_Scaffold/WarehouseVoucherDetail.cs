using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class WarehouseVoucherDetail
{
    public long voucherDetailId { get; set; }

    public long voucherId { get; set; }

    public int lineNo { get; set; }

    public string productCode { get; set; } = null!;

    public string productName { get; set; } = null!;

    public string? lotNumber { get; set; }

    public decimal qtyKg { get; set; }

    public int? bags { get; set; }

    public int? slotId { get; set; }

    public int? purposeId { get; set; }

    public bool isIncrease { get; set; }

    public string? note { get; set; }

    public bool isApplied { get; set; }

    public int voucherType { get; set; }

    public DateTime? expirydate { get; set; }

    public string UnitName { get; set; } = null!;

    public virtual UsagePurpose1? purpose { get; set; }

    public virtual WarehouseShelf? slot { get; set; }

    public virtual WarehouseVoucher voucher { get; set; } = null!;
}
