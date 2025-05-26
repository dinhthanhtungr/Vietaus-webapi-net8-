using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.InventoryReceipts
{
    public class InventoryReceiptsMaterialDTO
    {
        public string RequestId { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public Guid MaterialId { get; set; }
        public int ReceivedQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
        public bool? Status { get; set; }
    }
}
