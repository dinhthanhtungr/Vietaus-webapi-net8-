using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs
{
    public class TransferCustomersRequest
    {
        public Guid FromEmployeeId { get; set; }
        public Guid ToEmployeeId { get; set; }
        public Guid FromGroupId { get; set; }
        public Guid ToGroupId { get; set; }
        public string? Note { get; set; } // ghi chú
        public Guid CompanyId { get; set; }
        public Guid CreatedBy { get; set; }      // có thể lấy từ claims
        public List<Guid> CustomerIds { get; set; } = new();
    }

}
