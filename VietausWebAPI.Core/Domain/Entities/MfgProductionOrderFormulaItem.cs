using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MfgProductionOrderFormulaItem
{
    public Guid MfgProductionOrderId { get; set; }

    public string? MfgProductionOrderExternalId { get; set; }

    public Guid? FormulaItemId { get; set; }

    public string? FormulaItemExternalId { get; set; }

    public decimal? FormulaItemQuantity { get; set; }

    public string? FormulaItemName { get; set; }

    public string? ItemType { get; set; }

    public decimal? QuantityPerBatch { get; set; }

    public string? LotNo { get; set; }

    public DateTime? CreatedDate { get; set; }
}
