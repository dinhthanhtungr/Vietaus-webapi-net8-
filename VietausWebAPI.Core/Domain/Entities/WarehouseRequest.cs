using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class WarehouseRequest
    {
        public int RequestId { get; set; } //PK (RequestCode)

        public string RequestCode { get; set; } = string.Empty; 
        public WarehouseRequestStatus ReqStatus { get; set; }  // "Pending", "Approved", "Rejected", "Completed"
        public string RequestName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public WareHouseRequestType ReqType { get; set; }
        public string codeFromRequest { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        public Company Company { get; set; } = null!;
        public Employee CreatedByNavigation { get; set; } = null!;
        public Employee UpdatedByNavigation { get; set; } = null!;
        public List<DeliveryOrderPO> DeliveryOrderPOs { get; set; } = new List<DeliveryOrderPO>();
        public List<WarehouseRequestDetail> WarehouseRequestDetails { get; set; } = new List<WarehouseRequestDetail>();
    }
}
