using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.QueryObject;

namespace VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails.Query
{
    public class CreatePOQuery : PaginationQuery
    {
        public DateTime? RequestDate { get; set; }
        public string name { get; set; } = string.Empty;
        public string? materialGroupName { get; set; }
        public string? requestStatus { get; set; } = "ĐANG MUA";
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; }
    }
}
