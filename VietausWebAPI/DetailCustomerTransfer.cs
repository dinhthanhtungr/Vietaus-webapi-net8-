using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class DetailCustomerTransfer
{
    public int Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid LogId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual CustomerTransferLog Log { get; set; } = null!;
}
