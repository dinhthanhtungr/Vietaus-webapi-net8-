using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.SupplyRequestSchema;

namespace VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<SupplyRequest> SupplyRequests { get; set; } = default!;
        public virtual DbSet<SupplyRequestDetail> SupplyRequestDetails { get; set; } = default!;
    }
}
