using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<IEnumerable<MemberInGroup>> AllMembers(Guid Id, string? keywork = null)
        {
            var queryAble = _context.MemberInGroups
                .Include(x => x.ProfileNavigation)
                .AsNoTracking()
                .AsQueryable();

            queryAble = queryAble.Where(x => x.GroupId == Id && x.IsActive == true);

            if (!string.IsNullOrWhiteSpace(keywork))
            {
                string keywordLower = keywork.ToLower();
                queryAble = queryAble.Where(x =>
                    x.ProfileNavigation.FullName != null && EF.Functions.Collate(x.ProfileNavigation.FullName, "Latin1_General_CI_AI").ToLower().Contains(keywordLower) ||
                    x.ProfileNavigation.ExternalId != null && EF.Functions.Collate(x.ProfileNavigation.Email, "Latin1_General_CI_AI").ToLower().Contains(keywordLower));
            }
            return await queryAble.ToListAsync();
        }

        public async Task<int> changeLeaderStatus(GroupMemberQuery query)
        {
            // Gỡ quyền admin các thành viên trong group
            await _context.MemberInGroups
                .Where(m => m.GroupId == query.GroupId && m.MemberId != query.MemberId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(m => m.IsAdmin, false)
                );

            // Cấp quyền admin cho thành viên được chọn
            int affected = await _context.MemberInGroups
                .Where(m => m.GroupId == query.GroupId && m.MemberId == query.MemberId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(m => m.IsAdmin, true)
                );

            return affected; // tổng số dòng bị ảnh hưởng
        }

        public async Task<Group> GetGroupByIdAsync(Guid groupId)
        {
            var group = await _context.Groups
                .Where(e => e.GroupId == groupId)
                .FirstOrDefaultAsync();

            if (group == null)
                throw new Exception("Group not found."); // hoặc return default / log...

            return group;
        }

        public async Task<int> DeleteMemberInGroupAsync(GroupMemberQuery query)
        {
            return await _context.MemberInGroups
                .Where(m => m.GroupId == query.GroupId && m.MemberId == query.MemberId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(m => m.IsActive, false)
                );
        }
    }
}
