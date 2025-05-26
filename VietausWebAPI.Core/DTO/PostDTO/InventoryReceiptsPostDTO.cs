using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.PostDTO
{
    public class InventoryReceiptsPostDTO 
    {
        public List<SendData> Items { get; set; }
    }
    public class SendData 
    {

        public string RequestId { get; set; } = null!;

        public DateTime ReceiptDate { get; set; }

        public Guid MaterialId { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string? Note { get; set; }

        public bool? Status { get; set; }
        public string? SupplierId { get; set; }
    }
}
