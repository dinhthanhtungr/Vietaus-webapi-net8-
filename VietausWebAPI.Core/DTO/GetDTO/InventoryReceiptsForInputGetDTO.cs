using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.GetDTO
{
    public class InventoryReceiptsForInputGetDTO
    {
        public int ReceiptId { get; set; }
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
        public DateTime RequestDate { get; set; }
        public string? RequestStatus { get; set; }
    }
}
