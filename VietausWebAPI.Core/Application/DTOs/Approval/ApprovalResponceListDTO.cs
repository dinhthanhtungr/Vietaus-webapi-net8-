using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.Approval
{
    public class ApprovalResponceListDTO
    {
        public string requestStatus { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;
        public string fullName { get; set; } = string.Empty;
        public string partName { get; set; } = string.Empty;
        public DateTime approvalDate { get; set; } = DateTime.UtcNow;
        public string note { get; set; } = string.Empty;
    }
}
