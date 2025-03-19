using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.PostDTO
{
    public class RequestDTO
    {
        public string RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string EmployeeId { get; set; } 
        public string RequestStatus { get; set; }
        public List<RequestDetailMaterialDatumPostDTO> RequestDetails { get; set; }
    }
}
