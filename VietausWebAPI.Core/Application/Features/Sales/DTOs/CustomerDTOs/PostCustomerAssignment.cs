using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class PostCustomerAssignment
    {

        public Guid EmployeeId { get; set; }

        public Guid? GroupId { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

        public Guid? UpdatedBy { get; set; } 

        public Guid? CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
