using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.PostDTO
{
    public class RequestStatusPostDTO
    {
        public string requestStatus { get; set; } = null!;
        public string RequestId { get; set; } = null!;
        public DateTime ApprovalDate { get; set; }
        public string? Note { get; set; }
    }
}
