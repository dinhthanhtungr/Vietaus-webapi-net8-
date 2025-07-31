using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class QCDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? BatchExternalId { get; set; }

        [ForeignKey("ProductInspection")]
        public Guid? BatchId { get; set; }

        public string MachineExternalId { get; set; } = default!;

        // Navigation
        public virtual ProductInspection ProductInspection { get; set; } = null!;
    }
}
