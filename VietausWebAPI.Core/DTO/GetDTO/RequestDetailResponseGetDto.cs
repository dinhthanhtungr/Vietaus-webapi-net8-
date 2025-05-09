using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.GetDTO
{
    public class RequestDetailResponseGetDto
    {
        public string MaterialGroupName { get; set; } = null!;

        public string MaterialName { get; set; } = null!;

        public int RequestedQuantity { get; set; }

        public string? Unit { get; set; }
    }
}
