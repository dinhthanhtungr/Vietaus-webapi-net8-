using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class v_outbound_due_line
{
    public Guid? company_id { get; set; }

    public Guid? merchandise_order_id { get; set; }

    public Guid? merchandise_order_detail_id { get; set; }

    public string? dhg_code { get; set; }

    public Guid? customer_id { get; set; }

    public string? customer_name { get; set; }

    public Guid? product_id { get; set; }

    public string? product_code { get; set; }

    public string? product_name { get; set; }

    public DateTime? due_ts { get; set; }

    public decimal? expected_qty { get; set; }

    public decimal? delivered_qty { get; set; }

    public decimal? remain_qty { get; set; }

    public int? pgh_count { get; set; }

    public string? pgh_codes { get; set; }

    public DateTime? last_pgh_ts { get; set; }

    public int? wh_request_count { get; set; }

    public string? wh_request_codes { get; set; }

    public string? first_wh_request_code { get; set; }

    public int? mfg_count { get; set; }

    public string? mfg_codes { get; set; }

    public DateTime? max_mfg_expected_ts { get; set; }

    public string? ready_status { get; set; }
}
