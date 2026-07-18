using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class WarehouseVoucher
{
    public long voucherId { get; set; }

    public string voucherCode { get; set; } = null!;

    public int voucherType { get; set; }

    public int? requestId { get; set; }

    public Guid companyId { get; set; }

    public Guid createdBy { get; set; }

    public DateTime createdDate { get; set; }

    public string? status { get; set; }

    public virtual ICollection<WarehouseVoucherDetail> WarehouseVoucherDetails { get; set; } = new List<WarehouseVoucherDetail>();

    public virtual WarehouseRequest? request { get; set; }
}
