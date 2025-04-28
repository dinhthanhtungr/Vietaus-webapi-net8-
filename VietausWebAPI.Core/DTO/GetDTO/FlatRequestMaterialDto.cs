using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.GetDTO
{
    public class FlatRequestMaterialDto
    {
        public string RequestId { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public string EmployeeId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string RequestStatus { get; set; } = null!;

        public string MaterialGroupId { get; set; } = null!;
        public string MaterialName { get; set; } = null!;
        public int RequestQuantity { get; set; }
        public string Unit { get; set; } = null!;
    }
}
