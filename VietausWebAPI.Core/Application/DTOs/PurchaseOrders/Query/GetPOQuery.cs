using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.DTOs.PurchaseOrders.Query
{
    public class GetPOQuery : PaginationQuery
    {
        public DateTime? RequestDate { get; set; }
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Pocode { get; set; } = string.Empty;
        public Guid? SupplierId { get; set; } = null;
        public List<string> StatusFilter { get; set; } = new List<string>();
        public string? SortBy { get; set; }

        public bool SortAscending { get; set; }

    }
}
