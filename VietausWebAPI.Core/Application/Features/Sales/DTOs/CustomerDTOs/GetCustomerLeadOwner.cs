using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class GetCustomerLeadOwner
    {
        public Guid EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }

        public Guid GroupId { get; set; }
        public string? GroupName { get; set; }

        public DateTime ExpiresAt { get; set; }
        public int DaysLeft { get; set; }   // tiện hiển thị
    }
}
