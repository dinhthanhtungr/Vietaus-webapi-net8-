using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Models.PageModels
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; } = 1; // Mặc định là trang 1
        public int PageSize { get; set; } = 5; // Mặc định 5 bản ghi mỗi trang  
    }
}
