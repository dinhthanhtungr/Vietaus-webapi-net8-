using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs
{

    public class EmpLiteDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }   // tên NV
        public string Code { get; set; }       // mã NV (ExternalId/EmployeeCode)
    }

    public class CustomerLiteDto
    {
        public Guid Id { get; set; }
        public string ExternalId { get; set; } // mã KH
        public string Name { get; set; }       // tên KH
    }
    public class TransferCustomerDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public EmpLiteDto FromEmployee { get; set; }
        public EmpLiteDto ToEmployee { get; set; }
        public string? Note { get; set; } // ghi chú
        public List<CustomerLiteDto> Customers { get; set; }

        // Thêm:
        public GroupLiteDto? FromGroup { get; set; }
        public GroupLiteDto? ToGroup { get; set; }
    }

    public class GroupLiteDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; } // nếu có
    }
}
