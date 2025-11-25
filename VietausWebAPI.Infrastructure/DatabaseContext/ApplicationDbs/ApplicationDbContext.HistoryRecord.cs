using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HistoryRecordSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<PlanGetbackHistory> PlanGetbackHistories { get; set; } = default!;
        public virtual DbSet<MachineHistory> MachineHistories { get; set; } = default!;
        public virtual DbSet<EventHistory> EventHistories { get; set; } = default!;
        public virtual DbSet<AssignTask> AssignTasks { get; set; } = default!;
        public virtual DbSet<Shift> Shifts { get; set; } = default!;
        public virtual DbSet<ShiftsEvent> ShiftsEvents { get; set; } = default!;

    }
}
