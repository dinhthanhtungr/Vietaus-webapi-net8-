using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseRequest
{
    public class PostWarehouseRequest
    {
        public string RequestCode { get; set; } = string.Empty;
        public string ReqStatus { get; set; } = string.Empty; // "Pending", "Approved", "Rejected", "Completed"
        public string RequestName { get; set; } = string.Empty;
        public WareHouseRequestType ReqType { get; set; }

        public Guid CompanyId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public IEnumerable<PostWarehouseRequestDetail> WarehouseRequestDetails { get; set; } = new List<PostWarehouseRequestDetail>();
    }
}
