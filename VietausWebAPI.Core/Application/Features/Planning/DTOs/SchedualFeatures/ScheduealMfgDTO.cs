using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Planning.DTOs.SchedualFeatures
{
    public class SchedualMfgDTO
    {
        public Guid Id { get; set; }

        public string? ExternalId { get; set; }
        public string? MachineId { get; set; }
        public int? Number { get; set; }

        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }
        public string? OrderName { get; set; }

        public int? Quantity { get; set; }

        public string? VerifyBatches { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? QCStatus { get; set; } = string.Empty;
    }
}
