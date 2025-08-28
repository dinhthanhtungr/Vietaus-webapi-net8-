using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.HR
{
    public class MemberInGroupRepository : IMemberInGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public MemberInGroupRepository (ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public IQueryable<MemberInGroup> Query()
        {
            return _context.MemberInGroups
                .Include(m => m.Group)
                .Include(m => m.ProfileNavigation)                // thêm
                    .ThenInclude(e => e.MemberInGroups)           // thêm
                        .ThenInclude(m => m.Group)
                        .AsNoTracking();               // thêm
        }
    }
}
