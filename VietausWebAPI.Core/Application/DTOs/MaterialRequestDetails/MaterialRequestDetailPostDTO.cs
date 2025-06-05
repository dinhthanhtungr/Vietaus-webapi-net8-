using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails
{
    public class MaterialRequestDetailPostDTO
    {
        public string RequestId { get; set; } = null!;

        public int? RequestedQuantity { get; set; }
        public int? DetailId { get; set; }
        public Guid MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public string? Unit { get; set; }

        public string? Note { get; set; }
    }
}
