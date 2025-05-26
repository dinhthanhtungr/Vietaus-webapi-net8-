using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails
{
    public class MaterialRequestDetailGetDTO
    {
        public string RequestId { get; set; } = null!;

        public int? RequestedQuantity { get; set; }

        public string? MaterialId { get; set; }

        public string? Note { get; set; }
    }
}
