using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrdersPlanRepository;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace VietausWebAPI.Infrastructure.Repositories.Labs
{
    public class MfgProductionOrdersPlanRepository : IMfgProductionOrdersPlanRepository
    {
        private readonly ApplicationDbContext _context;
        public MfgProductionOrdersPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<MfgProductionOrdersPlan> GetMfgProductionOrdersPlanByIdAsync(Guid id)
        //{
        //    var mfgProductionOrdersPlan = await _context.MfgProductionOrdersPlans
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(x => x.Id == id);

        //    if (mfgProductionOrdersPlan == null)
        //    {
        //        throw new InvalidOperationException($"MfgProductionOrdersPlan with ID {id} not found.");
        //    }
        //    return mfgProductionOrdersPlan;
        //}

        public async Task<PagedResult<MfgProductionOrdersPlan>> GetPagedAsync(MfgPOLQuery query)
        {
            var queryAble = _context.MfgProductionOrdersPlans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.keyword))
            {
                string keyword = query.keyword.ToLower();
                queryAble = queryAble.Where(x =>
                    x.ExternalId != null && EF.Functions.Collate(x.ExternalId, "Latin1_General_CI_AI").ToLower().Contains(keyword) ||
                    x.Product_ExternalId != null && EF.Functions.Collate(x.Product_ExternalId, "Latin1_General_CI_AI").ToLower().Contains(keyword));
            }

            query.PageSize = 15;
            queryAble = queryAble.OrderByDescending(x => x.CreatedDate);
            return await QueryableExtensions.GetPagedAsync(queryAble, query);
        }

        public async Task UpdateProductNameInPlansAsync(Guid productId, string newProductName)
        {
            var item = await _context.MfgProductionOrdersPlans
                .FirstOrDefaultAsync(x => x.Product_Id == productId);

            if (item != null)
            {
                item.Product_Name = newProductName;
                await _context.SaveChangesAsync();
            }


        }
    }
}
