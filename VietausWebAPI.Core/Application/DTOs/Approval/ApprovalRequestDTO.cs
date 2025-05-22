using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.Approval
{
    public class ApprovalRequestDTO
    {
        public string requestStatus { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;
        public string employeeId { get; set; } = string.Empty;
        public DateTime approvalDate { get; set; } = DateTime.UtcNow;
        public string note { get; set; } = string.Empty;
    }
}
