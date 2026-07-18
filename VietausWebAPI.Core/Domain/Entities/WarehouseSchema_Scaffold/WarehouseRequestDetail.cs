using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class WarehouseRequestDetail
{
    public int detailId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? LotNumber { get; set; }

    public decimal weightKg { get; set; }

    public int BagNumber { get; set; }

    public string? StockStatus { get; set; }

    public int requestId { get; set; }

    public bool isActive { get; set; }

    /// <summary>
    /// Loại hàng theo từng dòng: 1=FinishedGood, 2=DefectiveFinishedGood, 3=RawMaterial, 4=DefectiveRawMaterial, 5=Other. Nullable để không ảnh hưởng dữ liệu/luồng cũ.
    /// </summary>
    public int? ItemStockType { get; set; }

    public string UnitName { get; set; } = null!;

    /// <summary>
    /// Hạn sử dụng được ghi nhận trên dòng phiếu yêu cầu nhập/xuất kho.
    /// </summary>
    public DateOnly? ExpiryDate { get; set; }

    public virtual WarehouseRequest request { get; set; } = null!;
}
