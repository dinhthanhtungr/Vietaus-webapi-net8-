using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class MerchandiseOrderLog
    {
        public Guid LogId { get; set; } // PK

        public Guid MerchandiseOrderId { get; set; }

        public string Status { get; set; } = "New"; 
        public string? Note { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }

        public Employee CreatedByNavigation { get; set; } = null!;
        public MerchandiseOrder MerchandiseOrder { get; set; } = null!;
    }
}
