using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.PostDTO
{
    public class InventoryReceiptsUpdatePriceDTO
    {
        public int ReceiptId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool? Status { get; set; }
    }
}
