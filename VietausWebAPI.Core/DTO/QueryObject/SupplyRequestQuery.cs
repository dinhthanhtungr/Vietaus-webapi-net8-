using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.DTO.QueryObject
{
    public class SupplyRequestQuery : PaginationQuery
    {
        public string? materialName { get; set; }
        public Guid? materialGroupId { get; set; }
        public Guid? supplierId { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        public string KeyWord { get; set; } = string.Empty;
        public string? PartId { get; set; }
        public List<string> StatusFilter { get; set; } = new List<string>();
        public string? requestStatus { get; set; }
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; }
    }
}
