using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.GetDTO
{
    public class RequestDetailResponseGetDto
    {
        public int DetailId { get; set; }
        public string RequestId { get; set; }
        public string MaterialGroupId { get; set; }
        public string MaterialName { get; set; }
        public int RequestedQuantity { get; set; }
        public string Unit { get; set; }
    }
}
