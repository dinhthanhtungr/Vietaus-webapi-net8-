using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class RequestDetail
{
    public int DetailId { get; set; }

    public Guid? RequestId { get; set; }

    public int? RequestedQuantity { get; set; }

    public Guid? MaterialId { get; set; }

    public string? RequestStatus { get; set; }

    public string? Note { get; set; }

    public virtual Material1? Material { get; set; }

    public virtual SupplyRequest? Request { get; set; }
}
