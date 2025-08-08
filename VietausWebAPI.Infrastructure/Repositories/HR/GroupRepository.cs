using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
using VietausWebAPI.Core.Application.Features.HR.Querys.Groups;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.HR
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task CreateNewGroupAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
        }

        public async Task<bool> DeleteGroupAsync(Guid groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return false;
            }
            _context.Groups.Remove(group);
            return true;
        }

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            return await _context.Groups
                .Where(e => e.ExternalId.StartsWith(prefix))
                .OrderByDescending(e => e.ExternalId)
                .Select(e => e.ExternalId)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<Group>> GetAllGroupsAsync(GroupQuery? query)
        {
            var queryAble = _context.Groups
                .Include(x => x.MemberInGroups)
                    .ThenInclude(m => m.ProfileNavigation)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.keyword))
            {
                string keywordLower = query.keyword.ToLower();
                queryAble = queryAble.Where(x =>
                    x.Name != null && EF.Functions.Collate(x.Name, "Latin1_General_CI_AI").ToLower().Contains(keywordLower) ||
                    x.ExternalId != null && EF.Functions.Collate(x.ExternalId, "Latin1_General_CI_AI").ToLower().Contains(keywordLower));
            }

            if (!string.IsNullOrWhiteSpace(query.groupType))
            {
                queryAble = queryAble.Where(x => x.GroupType == query.groupType);
            }

            query.PageSize = 15;
            queryAble = queryAble.OrderByDescending(x => x.CreatedDate);

            return await QueryableExtensions.GetPagedAsync(queryAble, query);
        }

        public async Task AddMembers(IEnumerable<MemberInGroup> members)
        {
            await _context.MemberInGroups.AddRangeAsync(members);
        }

        public async Task<IEnumerable<MemberInGroup>> AllMembers(Guid Id)
        {
            var queryAble = _context.MemberInGroups
                .Include(x => x.ProfileNavigation)
                .AsNoTracking()
                .AsQueryable();

            queryAble = queryAble.Where(x => x.GroupId == Id);

            return await queryAble.ToListAsync();
        }

        public Task setChangeLeader(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
