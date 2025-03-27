using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.QueryObject
{
    public class InventoryReceiptsQuery : PaginationQuery
    {
        public string? RequestId { get; set; }
        //public DateTime? RequestDate { get; set; }
        //public DateTime? RequestDateFrom { get; set; }
        //public DateTime? RequestDateTo { get; set; }
        public DateTime? ReceiptDate { get; set; }
        //public string? EmployeeId { get; set; }
        public string? MaterialName { get; set; }
        public string? SortBy { get; set; }
        public string? RequestStatus { get; set; }
        //public string? Department { get; set; }
        public string? EmployeeName { get; set; }
        public bool? Static { get; set; }
        public bool SortAscending { get; set; }

        // Các trường cho phân trang
        public int PageNumber { get; set; } = 1; // Mặc định là trang 1
        public int PageSize { get; set; } = 5; // Mặc định 5 bản ghi mỗi trang
    }
}
