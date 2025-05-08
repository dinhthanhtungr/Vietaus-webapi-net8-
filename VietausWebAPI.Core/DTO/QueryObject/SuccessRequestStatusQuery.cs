using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.QueryObject
{
    public class SuccessRequestStatusQuery
    {
        public string RequestId { get; set; } = null!;
        public string Note { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
