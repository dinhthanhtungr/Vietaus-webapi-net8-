using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.InventoryReceipts
{
    public class FieldUpdateDTO
    {
        public string FieldName { get; set; } = string.Empty;
        public int? ReceiptQty { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
