using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

public partial class PicklistDraft
{
    public long draftId { get; set; }

    public Guid companyId { get; set; }

    public string requestCode { get; set; } = null!;

    public short kind { get; set; }

    public int? reqType { get; set; }

    public string status { get; set; } = null!;

    public DateTime createdAt { get; set; }

    public Guid? createdBy { get; set; }

    public DateTime? updatedAt { get; set; }

    public Guid? updatedBy { get; set; }

    public DateTime? frozenAt { get; set; }

    public DateTime? receivedAt { get; set; }

    public Guid? receivedBy { get; set; }

    /// <summary>
    /// Số đợt xuất cho cùng một phiếu CT. NULL để tương thích draft cũ.
    /// </summary>
    public int? roundNo { get; set; }

    /// <summary>
    /// Thời điểm kho xác nhận xuất đợt này. Nullable để không ảnh hưởng dữ liệu cũ.
    /// </summary>
    public DateTime? exportedAt { get; set; }

    /// <summary>
    /// Người kho xác nhận xuất đợt này. Nullable để không ảnh hưởng dữ liệu cũ.
    /// </summary>
    public Guid? exportedBy { get; set; }

    public virtual ICollection<PicklistDraftLine> PicklistDraftLines { get; set; } = new List<PicklistDraftLine>();
}
