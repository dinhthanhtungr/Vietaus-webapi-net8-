using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Planning.Queries.SchedualFeatures;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Planning.Schedueal
{
    public class ScheduealRepository : IScheduealRepository
    {
        //private readonly ApplicationDbContext _context;

        //public ScheduealRepository(ApplicationDbContext context)
        //{
        //    _context = context ?? throw new ArgumentNullException(nameof(context));
        //}

        //public async Task<PagedResult<SchedualMfg>> GetSchedualPageAsync(SchedualQuery scheduealQuery)
        //{
        //    var queryAble = _context.SchedualMfgs.AsQueryable();
        //    if (!string.IsNullOrWhiteSpace(scheduealQuery.keyword))
        //    {
        //        string keyword = scheduealQuery.keyword.ToLower();
        //        queryAble = queryAble.Where(x =>
        //            x.ExternalId != null && EF.Functions.Collate(x.ExternalId, "Latin1_General_CI_AI").ToLower().Contains(keyword));
        //    }

        //    scheduealQuery.PageSize = 15;
        //    queryAble = queryAble.OrderByDescending(x => x.CreatedDate);
        //    return await QueryableExtensions.GetPagedAsync(queryAble, scheduealQuery);
        //}

        //public async Task<SchedualMfg> GetScheduealByIdAsync(string externalId)
        //{
        //    var queryable = await _context.SchedualMfgs
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(x => x.ExternalId == externalId);
        //    if (queryable == null)
        //    {
        //        throw new InvalidOperationException($"Schedueal with ID {externalId} not found.");
        //    }
        //    return queryable;
        //}
    }
}
