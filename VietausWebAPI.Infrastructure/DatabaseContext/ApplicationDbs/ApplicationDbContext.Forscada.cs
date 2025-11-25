using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.ForscadaSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<InfoBatForPlc> InfoBatForPlcs { get; set; } = default!;
        public virtual DbSet<InfoProForPlc> InfoProForPlcs { get; set; } = default!;
        public virtual DbSet<OperatorForRecordToPlc> OperatorForRecordToPlcs { get; set; } = default!;
        public virtual DbSet<ShiftLeaderForRecordToPlc> ShiftLeaderForRecordToPlcs { get; set; } = default!;
        public virtual DbSet<ProductionStatus> ProductionStatuses { get; set; } = default!;
        public virtual DbSet<ProductionPlanPlpu> ProductionPlanPlpus { get; set; } = default!;

    }
}
