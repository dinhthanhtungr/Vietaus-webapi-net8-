using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails
{
    public class POMaterialRequestDetailPostDTO
    {
        public string externalId { get; set; } = null!;
        public Guid materialId { get; set; }
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public string MaterialGroupName { get; set; } = null!;
        public int RequestedQuantity { get; set; }

    }
}
