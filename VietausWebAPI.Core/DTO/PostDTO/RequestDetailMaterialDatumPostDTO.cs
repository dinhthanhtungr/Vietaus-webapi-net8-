using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.PostDTO
{
    public class RequestDetailMaterialDatumPostDTO
    {
        public string RequestId { get; set; } = null!;

        public string MaterialGroupId { get; set; } = null!;

        public string MaterialName { get; set; } = null!;

        public int RequestedQuantity { get; set; }

        public string? Unit { get; set; }

    }
}
