using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Queries
{
    public class WarehouseVoucherReadQuery : PaginationQuery
    {
        public string? Keyword { get; set; } // tìm theo VoucherCode, RequestCode, RequestName
        public WareHouseRequestType? ReqType { get; set; } // lọc nhập/xuất
        public int? VoucherType { get; set; } // nếu cần
        public string? Status { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public Guid? CompanyId { get; set; }
    }
}
