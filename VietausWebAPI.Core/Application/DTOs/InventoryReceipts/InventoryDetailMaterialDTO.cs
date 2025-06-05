using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.InventoryReceipts
{
    public class InventoryDetailMaterialDTO
    {
        public string RequestId { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public Guid MaterialId { get; set; }
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public string? GroupName { get; set; }
        public int ReceiptQty { get; set; }
        public int? DetailId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
        public bool? Status { get; set; }
    }
}
