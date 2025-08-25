using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class MerchandiseOrderSchedule
{
    public Guid MerchandiseOrderScheduleId { get; set; }

    public Guid MerchandiseOrderId { get; set; }

    public DateTime DeliveryDate { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual MerchandiseOrder MerchandiseOrder { get; set; } = null!;
}
