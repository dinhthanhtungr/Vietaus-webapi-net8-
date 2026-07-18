using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class UsagePurpose
{
    public Guid usage_purpose_id { get; set; }

    public string purpose_code { get; set; } = null!;

    public string purpose_name { get; set; } = null!;

    public string? description { get; set; }

    public bool is_active { get; set; }

    public int sort_no { get; set; }

    public DateTime created_at { get; set; }
}
