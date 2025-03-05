using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietausWebAPI.Infrastructure.Models;

public partial class InventoryReceiptsMaterialDatum
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReceiptId { get; set; }

    public string MaterialGroupId { get; set; } = null!;

    public string RequestId { get; set; } = null!;

    public DateTime ReceiptDate { get; set; }

    public string MaterialName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public int ReceivedQuantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Note { get; set; }
    public virtual MaterialsMaterialGroupsDatum MaterialGroup { get; set; } = null!;

    public virtual SupplyRequestsMaterialDatum Request { get; set; } = null!;
}
