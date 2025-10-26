//using System;
//using System.Collections.Generic;

// namespace VietausWebAPI.Core.Domain.Entities;

//public partial class InventoryReceiptsMaterialDatum
//{
//    public int ReceiptId { get; set; }

//    public string RequestId { get; set; } = null!;

//    public DateTime? ReceiptDate { get; set; }

//    public decimal? UnitPrice { get; set; }

//    public string? Note { get; set; }

//    public Guid MaterialId { get; set; }

//    public int? DetailId { get; set; }

//    public int? ReceiptQty { get; set; }

//    public decimal? TotalPrice { get; set; }

//    public int ExportedQty { get; set; }

//    public virtual RequestDetailMaterialDatum? Detail { get; set; }

//    public virtual MaterialsMaterialDatum Material { get; set; } = null!;

//    public virtual SupplyRequestsMaterialDatum Request { get; set; } = null!;
//}
