using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class WarehouseShelfLedger
{
    public long ledgerId { get; set; }

    public long? voucherId { get; set; }

    public long? voucherDetailId { get; set; }

    public int slotId { get; set; }

    public Guid companyId { get; set; }

    public string? productCode { get; set; }

    public string? lotNumber { get; set; }

    public decimal deltaKg { get; set; }

    public decimal beforeKg { get; set; }

    public decimal afterKg { get; set; }

    public string? reason { get; set; }

    public Guid? createdBy { get; set; }

    public DateTime createdAt { get; set; }

    public int? purposeId { get; set; }

    public string? requestCode { get; set; }

    public string? appSource { get; set; }

    public int? stockType { get; set; }

    /// <summary>
    /// Đơn vị tính tại thời điểm phát sinh ledger
    /// </summary>
    public string UnitName { get; set; } = null!;

    /// <summary>
    /// Hạn sử dụng tại thời điểm phát sinh log kho. Nullable để không phá dữ liệu cũ.
    /// </summary>
    public DateOnly? ExpiryDate { get; set; }

    public virtual WarehouseShelf slot { get; set; } = null!;
}
