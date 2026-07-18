using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class WarehouseRequest
{
    public string RequestCode { get; set; } = null!;

    public int requestId { get; set; }

    public string requestName { get; set; } = null!;

    public int reqType { get; set; }

    public Guid companyId { get; set; }

    public Guid createdBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public string codeFromRequest { get; set; } = null!;

    public int reqStatus { get; set; }

    public Guid? updatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<WarehouseRequestDetail> WarehouseRequestDetails { get; set; } = new List<WarehouseRequestDetail>();

    public virtual ICollection<WarehouseVoucher> WarehouseVouchers { get; set; } = new List<WarehouseVoucher>();
}
