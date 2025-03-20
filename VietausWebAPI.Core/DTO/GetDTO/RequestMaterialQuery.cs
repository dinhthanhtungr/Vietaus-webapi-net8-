using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.GetDTO
{
    public class RequestMaterialQuery
    {
        public string? RequestId { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? EmployeeId { get; set; }
        public string? RequestStatus { get; set; }
    }
}
