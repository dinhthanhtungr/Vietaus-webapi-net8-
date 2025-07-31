using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.GetDTO
{
    public class ProgressTimeLineDTO
    {
        public string RequestId { get; set; } = null!;

        public DateTime RequestDate { get; set; }
        public string? Note { get; set; }
        public string RequestStatus { get; set; } = null!;
        public string fullName { get; set; } = null!;
        public string partName { get; set; } = null!;
    }
}
