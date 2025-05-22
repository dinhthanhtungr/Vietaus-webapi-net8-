using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.InventoryReceipts
{
    public class InventoryReceiptsMaterialDTO
    {
        public string MaterialGroupId { get; set; } = null!;
        public string RequestId { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public string MaterialName { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public int ReceivedQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
        public bool? Status { get; set; }
        public string? SupplierId { get; set; }
    }
}
