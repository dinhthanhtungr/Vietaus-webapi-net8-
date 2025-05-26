using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.DTO.PostDTO
{
    public class ApproveReceiptDTO
    {
        public string RequestId { get; set; }
        public string status { get; set; }
        public List<InventoryReceiptsMaterialDTO> Items { get; set; } = new List<InventoryReceiptsMaterialDTO>();
    }

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
        public string? SupplierId { get; set; }
    }
}
