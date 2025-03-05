using System;
using System.Collections.Generic;

namespace VietausWebAPI.Infrastructure.Models;

public partial class SupplyRequestsMaterialDatum
{
    public string RequestId { get; set; } = null!;

    public DateTime RequestDate { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string RequestStatus { get; set; } = null!;

    public virtual ICollection<ApprovalHistoryMaterialDatum> ApprovalHistoryMaterialData { get; set; } = new List<ApprovalHistoryMaterialDatum>();

    public virtual EmployeesCommonDatum Employee { get; set; } = null!;

    public virtual ICollection<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; } = new List<InventoryReceiptsMaterialDatum>();
}
