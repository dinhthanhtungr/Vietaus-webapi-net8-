using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.InventoryReceipts;

namespace VietausWebAPI.Core.Application.DTOs.Approval
{
    public class ApprovalHistoryAndInventoryRequestDTO 
    {
        public string RequestId { get; set; }
        public string status { get; set; }
        public List<InventoryReceiptsMaterialDTO> Items { get; set; } = new List<InventoryReceiptsMaterialDTO>();
    }
}
