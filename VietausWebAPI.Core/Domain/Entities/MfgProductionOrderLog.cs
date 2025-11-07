using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class MfgProductionOrderLog
    {
        public Guid LogId { get; set; } // PK

        public Guid MfgProductionOrderId { get; set; }

        public string Status { get; set; } = "New"; 

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }

        public Employee CreatedByNavigation { get; set; } = null!;
        public MfgProductionOrder MfgProductionOrder { get; set; } = null!;
    }
}
