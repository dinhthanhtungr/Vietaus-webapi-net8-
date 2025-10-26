//using System;
//using System.Collections.Generic;

//namespace VietausWebAPI.Core.Domain.Entities;

//public partial class SupplyRequestsMaterialDatum
//{
//    public string RequestId { get; set; } = null!;

//    public DateTime RequestDate { get; set; }

//    public string EmployeeId { get; set; } = null!;

//    public string RequestStatus { get; set; } = null!;

//    public string? Note { get; set; }

//    public string? NoteCancel { get; set; }

//    public virtual ICollection<ApprovalHistoryMaterialDatum> ApprovalHistoryMaterialData { get; set; } = new List<ApprovalHistoryMaterialDatum>();

//    public virtual EmployeesCommonDatum Employee { get; set; } = null!;

//    public virtual ICollection<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; } = new List<InventoryReceiptsMaterialDatum>();

//    public virtual ICollection<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; } = new List<RequestDetailMaterialDatum>();
//}
