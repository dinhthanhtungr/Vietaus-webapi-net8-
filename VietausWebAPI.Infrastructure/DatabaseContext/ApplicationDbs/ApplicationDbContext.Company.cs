using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<MemberInGroup> MemberInGroups { get; set; }

    }
}
