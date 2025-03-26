using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.PostDTO
{
    public class ApprovalHistoryMaterialPostDTO
    {

        public string RequestId { get; set; } = null!;

        public string EmployeeId { get; set; } = null!;

        public DateTime ApprovalDate { get; set; }

        public string? Note { get; set; }
        public string requestStatus { get; set; } = null!;
    }
}
