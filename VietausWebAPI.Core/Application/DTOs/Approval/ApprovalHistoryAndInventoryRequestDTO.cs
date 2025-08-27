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
        public string requestStatus { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;
        public string employeeId { get; set; } = string.Empty;
        public DateTime approvalDate { get; set; } = DateTime.Now;
        public string note { get; set; } = string.Empty;
        public List<InventoryReceiptsMaterialDTO> Items { get; set; } = new List<InventoryReceiptsMaterialDTO>();
    }
}
