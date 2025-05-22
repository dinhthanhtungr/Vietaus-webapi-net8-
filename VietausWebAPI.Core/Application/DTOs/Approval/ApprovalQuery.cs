using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.QueryObject;

namespace VietausWebAPI.Core.Application.DTOs.Approval
{
    public class ApprovalQuery : PaginationQuery
    {
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        public string KeyWord { get; set; } = string.Empty;
        public string? PartId { get; set; }
        public List<string> StatusFilter { get; set; } = new List<string>();
        public string? requestStatus { get; set; }
        public string? sortBy { get; set; } 
        public bool SortAscending { get; set; }
    }
}
