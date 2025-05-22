using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.InventoryReceipts;

namespace VietausWebAPI.Core.Application.DTOs.Approval
{
    public class ApprovalHistoryAndInventoryRequestDTO : ApprovalRequestDTO
    {
        public List<InventoryReceiptsMaterialDTO> InventoryReceipts { get; set; } = new List<InventoryReceiptsMaterialDTO>();
    }
}
