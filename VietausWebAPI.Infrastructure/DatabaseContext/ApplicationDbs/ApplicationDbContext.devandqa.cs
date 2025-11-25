using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<ProductStandard> ProductStandards { get; set; } = default!;
        public virtual DbSet<ProductTest> ProductTests { get; set; } = default!;
        public virtual DbSet<QcPassDetailHistory> QcPassDetailHistories { get; set; } = default!;
        public virtual DbSet<QcPassHistory> QcPassHistories { get; set; } = default!;
        public virtual DbSet<ProductInspection> ProductInspections { get; set; } = default!;
    }
}
